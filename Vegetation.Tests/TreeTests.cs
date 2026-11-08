using System.Numerics;
using Xunit;
using Xunit.Abstractions;

namespace Vegetation.Tests;

/// <summary>
/// Trees grown by <see cref="TreeSkeleton.Grow(TreeSpecies, uint)"/>, their sameness by seed, the
/// presets' sizes against the paper's, their thickness by the pipe model, the boxes they grow round
/// and the controls and skeletons refused.
/// </summary>
public sealed class TreeTests(ITestOutputHelper output)
{
    private static readonly TreeSpecies Aspen = TreeSpecies.QuakingAspen;

    [Fact]
    public void ASeedGrowsTheSameSkeletonBitForBit()
    {
        var first = TreeSkeleton.Grow(Aspen, 7).Nodes;
        var again = TreeSkeleton.Grow(Aspen, 7).Nodes;
        Assert.Equal(first.Count, again.Count);
        for (var i = 0; i < first.Count; i++)
        {
            Assert.Equal(first[i].Parent, again[i].Parent);
            foreach (var (one, other) in new[] { (first[i].Place.X, again[i].Place.X), (first[i].Place.Y, again[i].Place.Y), (first[i].Place.Z, again[i].Place.Z), (first[i].Radius, again[i].Radius) })
            {
                Assert.Equal(BitConverter.SingleToInt32Bits(one), BitConverter.SingleToInt32Bits(other));
            }
        }

        Assert.Equal(TreeSkeleton.Grow(Aspen, 7), TreeSkeleton.Grow(Aspen with { Levels = [.. Aspen.Levels] }, 7));
        Assert.NotEqual(TreeSkeleton.Grow(Aspen, 7), TreeSkeleton.Grow(Aspen, 8));
    }

    // The presets, each with the heights the paper's scale and trunk length allow it, and its width
    // over its height as the paper's plates draw it, read off them at about 0.45 for the aspen,
    // 0.55 for the tupelo, 0.9 for the willow and 1.05 for the oak, held to three tenths either way.
    // A tree may stand a little over its trunk, as a crown's tips rise past it, and the bush's and the
    // fern's are the library's own.
    [Theory]
    [InlineData("quaking aspen", 10f, 16f, 0.45f)]
    [InlineData("black tupelo", 18f, 28f, 0.55f)]
    [InlineData("weeping willow", 8f, 16f, 0.9f)]
    [InlineData("California black oak", 0f, 20f, 1.05f)]
    [InlineData("bush", 1.1f, 1.9f, 1f)]
    [InlineData("fern", 0.4f, 1.1f, 2.5f)]
    public void APresetStandsAsTallAsThePapersAndAsWideAsItsPlate(string name, float lowest, float highest, float plate)
    {
        var species = name switch
        {
            "quaking aspen" => TreeSpecies.QuakingAspen,
            "black tupelo" => TreeSpecies.BlackTupelo,
            "weeping willow" => TreeSpecies.WeepingWillow,
            "California black oak" => TreeSpecies.CaliforniaBlackOak,
            "bush" => TreeSpecies.Bush,
            _ => TreeSpecies.Fern,
        };

        var ratios = new List<float>();
        for (var seed = 1u; seed <= 3; seed++)
        {
            var (low, high) = TreeSkeleton.Grow(species, seed).Bounds;
            var width = (high.X - low.X + high.Z - low.Z) / 2f;
            Assert.InRange(high.Y, lowest * 0.9f, highest * 1.15f);
            Assert.True(low.Y >= 0f, $"seed {seed} grew a stem into the ground, to {low.Y}");
            ratios.Add(width / high.Y);
            output.WriteLine($"| {name} | {seed} | {lowest:0.#} to {highest:0.#} m | {high.Y:0.0} m | {width:0.0} m | {plate:0.00} | {width / high.Y:0.00} |");
        }

        Assert.InRange(ratios.Average(), plate * 0.7f, plate * 1.3f);
    }

    [Fact]
    public void TheAspensTrunkIsBareBelowItsCrownAndAsThickAsItsRatio()
    {
        for (var seed = 1u; seed <= 3; seed++)
        {
            var skeleton = TreeSkeleton.Grow(Aspen, seed);
            var nodes = skeleton.Nodes;
            var (_, high) = skeleton.Bounds;

            // The trunk's foot is as thick for the trunk's height as the ratio and the flare make it.
            Assert.InRange(nodes[0].Radius / high.Y, 0.015f * 1.594f * 0.85f, 0.015f * 1.594f * 1.15f);

            // Below the crown's foot, four tenths of the scale up, the trunk stands alone, the bound
            // lowered for the crown's tips standing over the trunk and for the lowest branches,
            // which leave at up to 110 degrees and dip.
            var bare = 0.35f * high.Y - 1f;
            var children = new int[nodes.Count];
            for (var i = 1; i < nodes.Count; i++) children[nodes[i].Parent]++;
            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Place.Y < bare) Assert.True(children[i] <= 1, $"seed {seed} branches at {nodes[i].Place}, below the crown's foot at {bare}");
            }
        }
    }

    [Fact]
    public void AStemIsAsThickAsThePipeModelMakesIt()
    {
        var nodes = TreeSkeleton.Grow(Aspen with { Flare = 0f }, 3).Nodes;
        var unit = nodes.Min(node => node.Radius);
        var carried = new float[nodes.Count];
        for (var i = 1; i < nodes.Count; i++) carried[nodes[i].Parent] += nodes[i].Radius * nodes[i].Radius;

        for (var i = 0; i < nodes.Count; i++)
        {
            // Every level tapers fully, so a point's cross section is its own unit and those of the
            // points it carries.
            var expected = unit * unit + carried[i];
            Assert.True(MathF.Abs(nodes[i].Radius * nodes[i].Radius - expected) <= expected * 1e-3f, $"point {i} is {nodes[i].Radius} thick and its stems call for {MathF.Sqrt(expected)}");
            if (i > 0) Assert.True(nodes[i].Radius < nodes[nodes[i].Parent].Radius, $"point {i} is thicker than the point it grew from");
        }
    }

    [Fact]
    public void ATrunkAloneGrowsStraightToItsHeight()
    {
        var nodes = TreeSkeleton.Grow(new TreeSpecies { Scale = 8f, Levels = [new TreeLevel()] }, 1).Nodes;
        Assert.InRange(nodes.Count, 40, 41);
        Assert.All(nodes, node => Assert.True(node.Place.X == 0f && node.Place.Z == 0f, $"a trunk with no curve leans, to {node.Place}"));
        Assert.InRange(nodes[^1].Place.Y, 7.6f, 8.01f);
        Assert.Equal(8f * 0.015f, nodes[0].Radius, 5);

        // A flare of one doubles the trunk at the ground, less a hundredth, as the paper's does.
        var flared = TreeSkeleton.Grow(new TreeSpecies { Scale = 8f, Flare = 1f, Levels = [new TreeLevel()] }, 1).Nodes;
        Assert.Equal(8f * 0.015f * 1.99f, flared[0].Radius, 4);
        Assert.Equal(nodes[^1].Radius, flared[^1].Radius);
    }

    [Fact]
    public void ATrunkSplitsAtTheCrownsFootIntoItsBaseSplits()
    {
        var species = new TreeSpecies { Scale = 8f, BaseSize = 0.25f, BaseSplits = 2, Levels = [new TreeLevel { SplitAngle = 20f }] };
        var nodes = TreeSkeleton.Grow(species, 4).Nodes;
        var children = new int[nodes.Count];
        for (var i = 1; i < nodes.Count; i++) children[nodes[i].Parent]++;

        // One point carries three, two meters up, and the three trunks run on to the top apart.
        var fork = Assert.Single(Enumerable.Range(0, nodes.Count), i => children[i] > 1);
        Assert.Equal(3, children[fork]);
        Assert.InRange(nodes[fork].Place.Y, 1.8f, 2.4f);
        var tips = Enumerable.Range(0, nodes.Count).Where(i => children[i] == 0).Select(i => nodes[i].Place).ToArray();
        Assert.Equal(3, tips.Length);
        Assert.All(tips, tip => Assert.InRange(tip.Y, 6f, 8.01f));
        Assert.True(tips.Min(tip => tips.Where(other => other != tip).Min(other => (other - tip).Length())) > 0.3f, "the trunks run on together");
    }

    [Fact]
    public void AWillowKeepsInsideItsPruningEnvelope()
    {
        // The envelope is four tenths of the scale at its widest, and the scale twenty meters at most.
        var widest = 0.4f * 20f;
        foreach (var node in TreeSkeleton.Grow(TreeSpecies.WeepingWillow, 2).Nodes)
        {
            Assert.True(MathF.Sqrt(node.Place.X * node.Place.X + node.Place.Z * node.Place.Z) <= widest, $"a point stands at {node.Place}, outside the envelope");
        }
    }

    [Fact]
    public void ATreeGrowsRoundABoxAndAwayFromIt()
    {
        // A wall a meter from the trunk along X, past the aspen's crown in height and breadth.
        (Vector3 Low, Vector3 High) wall = (new Vector3(1f, 0f, -20f), new Vector3(3f, 40f, 20f));
        var open = TreeSkeleton.Grow(Aspen, 5).Nodes;
        var walled = TreeSkeleton.Grow(Aspen, 5, [wall]).Nodes;

        Assert.All(walled, node => Assert.False(node.Place.X >= wall.Low.X && node.Place.X <= wall.High.X, $"a point stands in the wall at {node.Place}"));
        // The wall takes the sky from the crown's side next to it, so the crown stands away from it by
        // more than it loses to it, half a meter and more where the open tree stands about the trunk.
        Assert.True(walled.Average(node => node.Place.X) < open.Average(node => node.Place.X) - 0.5f, "the crown does not lean away from the wall");
        Assert.Equal(walled, TreeSkeleton.Grow(Aspen, 5, [wall]).Nodes);
    }

    [Fact]
    public void ASpeciesOutsideItsRangeIsRefused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Scale = -1f }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { ScaleVariation = 14f }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { BaseSplits = -1 }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Shape = (TreeShape)99 }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { PruneRatio = 2f }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { PruneWidthPeak = 0f }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen, 1, [(Vector3.One, Vector3.Zero)]));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { BaseSize = 1f }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Ratio = float.NaN }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [] }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [.. Aspen.Levels, new(), new()] }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [new TreeLevel { Taper = 3.5f }] }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [new TreeLevel(), new TreeLevel { Branches = -1 }] }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [new TreeLevel { Curve = float.PositiveInfinity }] }, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreeSkeleton.Grow(Aspen with { Levels = [new TreeLevel { Length = 0f }] }, 1));

        // The check an engine keeps a species by refuses what the growth refuses and takes every preset.
        Assert.Throws<ArgumentOutOfRangeException>(() => (Aspen with { Scale = -1f }).Check());
        foreach (var preset in new[] { TreeSpecies.QuakingAspen, TreeSpecies.BlackTupelo, TreeSpecies.WeepingWillow, TreeSpecies.CaliforniaBlackOak, TreeSpecies.Bush, TreeSpecies.Fern }) preset.Check();
    }

    [Fact]
    public void ASkeletonOutOfOrderIsRefused()
    {
        Assert.Throws<ArgumentException>(() => new TreeSkeleton([]));
        Assert.Throws<ArgumentException>(() => new TreeSkeleton([new TreeNode(0, Vector3.Zero, 0.1f)]));
        Assert.Throws<ArgumentException>(() => new TreeSkeleton([new TreeNode(-1, Vector3.Zero, 0.1f), new TreeNode(1, Vector3.UnitY, 0.1f)]));
        Assert.Throws<ArgumentException>(() => new TreeSkeleton([new TreeNode(-1, Vector3.Zero, 0f)]));
    }
}
