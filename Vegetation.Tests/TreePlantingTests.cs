using System.Numerics;
using Xunit;

namespace Vegetation.Tests;

/// <summary>
/// Where trees stand by <see cref="TreePlanting"/>, a forest spread over its disc, a tree's variant,
/// turn and size from its seed, the variants' own seeds, and the forests and variants refused.
/// </summary>
public sealed class TreePlantingTests
{
    [Fact]
    public void AForestIsSpreadOverItsDiscTheSameForTheSameSeed()
    {
        var middle = new Vector3(10f, 2f, -5f);
        var forest = TreePlanting.Forest(4, middle, 12f, 15);
        var places = forest.Select(tree => tree.Place).ToArray();

        // An even spread puts each tree in a fifteenth of the disc, and none stands nearer another
        // than seven tenths of that spacing.
        var apart = 0.7f * MathF.Sqrt(MathF.PI * 12f * 12f / 15f);
        Assert.InRange(places.Length, 10, 15);
        Assert.All(places, place => Assert.True((place - middle).Length() <= 12.001f && place.Y == middle.Y, $"a tree stands at {place}"));
        for (var i = 0; i < places.Length; i++)
            for (var j = i + 1; j < places.Length; j++)
                Assert.True((places[i] - places[j]).Length() >= apart - 1e-3f, $"two trees stand {(places[i] - places[j]).Length()} apart");

        Assert.Equal(forest, TreePlanting.Forest(4, middle, 12f, 15));
        Assert.NotEqual(places, TreePlanting.Forest(5, middle, 12f, 15).Select(tree => tree.Place));
        Assert.Equal(forest.Count, forest.Select(tree => tree.Seed).Distinct().Count());
        Assert.Empty(TreePlanting.Forest(4, middle, 12f, 0));
    }

    /// <summary>
    /// A tree's seed picks its variant by what is left over its variants, and turns it anywhere in a
    /// whole turn and sizes it a tenth either way, the same seed the same stance.
    /// </summary>
    [Fact]
    public void ATreesSeedGivesItsVariantTurnAndSize()
    {
        var stances = Enumerable.Range(0, 1000).Select(seed => TreePlanting.Stance((uint)seed)).ToArray();
        for (var seed = 0; seed < stances.Length; seed++)
        {
            Assert.Equal(seed % TreePlanting.Variants, stances[seed].Variant);
            Assert.InRange(stances[seed].Turn, 0f, MathF.Tau);
            Assert.InRange(stances[seed].Size, 0.9f, 1.1f);
            Assert.Equal(stances[seed], TreePlanting.Stance((uint)seed));
        }

        // The turns and the sizes spread over their ranges rather than gathering, the same variant's
        // trees standing apart in both.
        Assert.True(stances.Count(stance => stance.Turn < MathF.PI) is > 400 and < 600, "the turns gather on one half");
        Assert.True(stances.Count(stance => stance.Size < 1f) is > 400 and < 600, "the sizes gather on one side");
        Assert.NotEqual(stances[3], stances[3 + TreePlanting.Variants]);
        Assert.Equal(TreePlanting.Stance(uint.MaxValue), TreePlanting.Stance(uint.MaxValue));
    }

    [Fact]
    public void EachVariantGrowsFromASeedOfItsOwn()
    {
        var seeds = Enumerable.Range(0, TreePlanting.Variants).Select(TreePlanting.VariantSeed).ToArray();
        Assert.Equal(TreePlanting.Variants, seeds.Distinct().Count());
        Assert.Equal(0x7E57_0000u, seeds[0]);
        Assert.Throws<ArgumentOutOfRangeException>(() => TreePlanting.VariantSeed(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => TreePlanting.VariantSeed(TreePlanting.Variants));
    }

    [Fact]
    public void AForestOfNoRadiusOrBelowNoTreesIsRefused()
    {
        foreach (var radius in new[] { 0f, -1f, float.NaN, float.PositiveInfinity })
            Assert.Equal("radius", Assert.Throws<ArgumentOutOfRangeException>(() => TreePlanting.Forest(1, Vector3.Zero, radius, 4)).ParamName);
        Assert.Equal("count", Assert.Throws<ArgumentOutOfRangeException>(() => TreePlanting.Forest(1, Vector3.Zero, 4f, -1)).ParamName);
    }
}
