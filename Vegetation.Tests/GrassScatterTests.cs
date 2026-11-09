using System.Numerics;
using Xunit;

namespace Vegetation.Tests;

/// <summary>
/// Grass scattered by <see cref="GrassScatter"/>, a stretch cut into its patches, a patch's clumps a
/// cell each and on the ground, their sameness by seed, and the stretches and settings refused.
/// </summary>
public sealed class GrassScatterTests
{
    /// <summary>
    /// A stretch of 20 by 12 meters in patches of eight stands as six patches about its middle, the
    /// last of each row and column cut to what is left, each with a seed of its own.
    /// </summary>
    [Fact]
    public void AStretchIsCutIntoPatchesFromItsCorner()
    {
        var patches = GrassScatter.Patches(new Vector2(20f, 12f), new GrassSettings());
        Assert.Equal([(0, 0), (1, 0), (2, 0), (0, 1), (1, 1), (2, 1)], patches.Select(patch => (patch.Column, patch.Row)));
        Assert.Equal([-6f, 2f, 8f, -6f, 2f, 8f], patches.Select(patch => patch.Middle.X));
        Assert.Equal([-2f, -2f, -2f, 4f, 4f, 4f], patches.Select(patch => patch.Middle.Y));
        Assert.Equal([8f, 8f, 4f, 8f, 8f, 4f], patches.Select(patch => patch.Size.X));
        Assert.Equal([8f, 8f, 8f, 4f, 4f, 4f], patches.Select(patch => patch.Size.Y));
        Assert.Equal(patches.Count, patches.Select(patch => patch.Seed).Distinct().Count());
    }

    /// <summary>
    /// Four clumps a square meter over 8 by 6 meters fall a clump a cell of a 16 by 12 grid, each
    /// root on the ground inside its cell, each clump within its height's variation, its turn within
    /// half a turn and its phases from zero up to one.
    /// </summary>
    [Fact]
    public void APatchsClumpsStandACellEachOnTheGround()
    {
        var settings = new GrassSettings { Density = 4f, Height = 0.6f, Variation = 0.3f };
        var clumps = GrassScatter.Clumps(settings, new Vector2(8f, 6f), 7);
        Assert.Equal(16 * 12, clumps.Count);
        for (var at = 0; at < clumps.Count; at++)
        {
            var (clump, column, row) = (clumps[at], at % 16, at / 16);
            Assert.True(clump.Root.Y == 0f, $"clump {at} stands at {clump.Root.Y}");
            Assert.InRange(clump.Root.X, column * 0.5f - 4f, (column + 1) * 0.5f - 4f);
            Assert.InRange(clump.Root.Z, row * 0.5f - 3f, (row + 1) * 0.5f - 3f);
            Assert.InRange(clump.Height, 0.6f * 0.7f - 1e-5f, 0.6f * 1.3f + 1e-5f);
            Assert.InRange(clump.Turn, 0f, MathF.PI);
            Assert.InRange(clump.Swing, 0f, 1f);
            Assert.InRange(clump.Flutter, 0f, 1f);
        }
    }

    /// <summary>The same seed scatters the same clumps and the same patches, and another seed others.</summary>
    [Fact]
    public void TheSameSeedScattersTheSameClumpsAndAnotherAnother()
    {
        var settings = new GrassSettings();
        var once = GrassScatter.Clumps(settings, new Vector2(4f, 4f), 11);
        Assert.Equal(once, GrassScatter.Clumps(settings, new Vector2(4f, 4f), 11));
        Assert.NotEqual(once, GrassScatter.Clumps(settings, new Vector2(4f, 4f), 12));

        var patches = GrassScatter.Patches(new Vector2(20f, 12f), settings with { Seed = 3 });
        Assert.Equal(patches, GrassScatter.Patches(new Vector2(20f, 12f), settings with { Seed = 3 }));
        Assert.NotEqual(patches.Select(patch => patch.Seed), GrassScatter.Patches(new Vector2(20f, 12f), settings with { Seed = 4 }).Select(patch => patch.Seed));
    }

    /// <summary>
    /// A stretch of no size or of a size that is no number is refused, as are a set that is not grass
    /// or not in the collection and a setting outside what it may be, each as the argument it came in.
    /// </summary>
    [Fact]
    public void AStretchOrASettingOutsideWhatItMayBeIsRefused()
    {
        var settings = new GrassSettings();
        foreach (var size in new[] { new Vector2(0f, 4f), new Vector2(4f, -1f), new Vector2(float.NaN, 4f), new Vector2(4f, float.PositiveInfinity) })
        {
            Assert.Equal("size", Assert.Throws<ArgumentException>(() => GrassScatter.Patches(size, settings)).ParamName);
            Assert.Equal("size", Assert.Throws<ArgumentException>(() => GrassScatter.Clumps(settings, size, 0)).ParamName);
        }

        foreach (var refused in new[] { settings with { Set = "silver-birch-leaves" }, settings with { Set = "no-such-grass" }, settings with { Density = 0f }, settings with { Height = -1f }, settings with { PatchSize = 0f }, settings with { Variation = 1f }, settings with { Variation = float.NaN } })
            Assert.Equal("settings", Assert.Throws<ArgumentException>(() => GrassScatter.Patches(new Vector2(4f, 4f), refused)).ParamName);
        Assert.Equal("settings", Assert.Throws<ArgumentException>(() => GrassScatter.Clumps(settings with { Density = 0f }, new Vector2(4f, 4f), 0)).ParamName);
    }
}
