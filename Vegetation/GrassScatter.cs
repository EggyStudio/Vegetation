using System.Numerics;

namespace Vegetation;

/// <summary>
/// Where grass stands over a stretch of ground, the stretch cut into square patches and each patch's
/// clumps scattered over it from a seed, which an engine draws as a mesh a patch, two cards crossed
/// a clump.
/// </summary>
/// <remarks>
/// <para>
/// The patches are square from the stretch's corner, the last of a row and a column cut to what is
/// left, and each takes a seed of its own from <see cref="GrassSettings.Seed"/> and where it
/// stands, so a stretch planted twice with the same settings stands the same.
/// </para>
/// <para>
/// A patch's clumps fall on a grid of cells a clump each, each placed anywhere in its cell, so the
/// grass covers its patch evenly without the rows of a grid. A clump's numbers are drawn from
/// <see cref="Pcg"/> in a fixed order, its place, its turn, its height and its two phases, so a seed
/// means the same clumps on every runtime.
/// </para>
/// </remarks>
public static class GrassScatter
{
    /// <summary>Cuts a stretch of ground into the patches its grass is drawn in.</summary>
    /// <param name="size">The stretch's width along X in X and its depth along Z in Y, in meters.</param>
    /// <param name="settings">The set, the density, the height, the patch size and the seed.</param>
    /// <returns>The patches a row at a time from the stretch's corner, each row along X and the rows along Z.</returns>
    /// <exception cref="ArgumentException">The set is not a grass set the collection has, or a size or a setting is not a positive number.</exception>
    public static IReadOnlyList<GrassPatch> Patches(Vector2 size, GrassSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        Check(size, settings);
        if (TreeTextures.Find(settings.Set) is not { Kind: TreeTextureKind.Grass })
            throw new ArgumentException($"The texture collection has no grass set {settings.Set}.", nameof(settings));

        var (across, along) = ((int)MathF.Ceiling(size.X / settings.PatchSize), (int)MathF.Ceiling(size.Y / settings.PatchSize));
        var patches = new GrassPatch[across * along];
        for (var row = 0; row < along; row++)
        {
            for (var column = 0; column < across; column++)
            {
                var x0 = column * settings.PatchSize;
                var z0 = row * settings.PatchSize;
                var (width, depth) = (MathF.Min(settings.PatchSize, size.X - x0), MathF.Min(settings.PatchSize, size.Y - z0));
                var middle = new Vector2(x0 + width / 2f - size.X / 2f, z0 + depth / 2f - size.Y / 2f);
                patches[row * across + column] = new GrassPatch(column, row, middle, new Vector2(width, depth), PatchSeed(settings.Seed, column, row));
            }
        }

        return patches;
    }

    /// <summary>Scatters a patch's clumps over it, a cell each of a grid as fine as the density asks.</summary>
    /// <param name="settings">The density, the height and how far a clump's height strays.</param>
    /// <param name="size">The patch's width along X in X and its depth along Z in Y, in meters, as <see cref="GrassPatch.Size"/> gives it.</param>
    /// <param name="seed">The patch's seed, as <see cref="GrassPatch.Seed"/> gives it.</param>
    /// <returns>The clumps a row of cells at a time, each row along X and the rows along Z, their roots about the patch's middle.</returns>
    /// <exception cref="ArgumentException">A size or a setting is not a positive number.</exception>
    public static IReadOnlyList<GrassClump> Clumps(GrassSettings settings, Vector2 size, uint seed)
    {
        ArgumentNullException.ThrowIfNull(settings);
        Check(size, settings);
        var (width, depth) = (size.X, size.Y);
        var cell = 1f / MathF.Sqrt(settings.Density);
        var (columns, rows) = (Math.Max(1, (int)MathF.Round(width / cell)), Math.Max(1, (int)MathF.Round(depth / cell)));
        var clumps = new GrassClump[columns * rows];
        var random = new Pcg(seed);
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                var root = new Vector3(
                    (column + random.Unit()) * width / columns - width / 2f,
                    0f,
                    (row + random.Unit()) * depth / rows - depth / 2f);
                var turn = random.Unit() * MathF.PI;
                var tall = settings.Height * (1f + settings.Variation * (2f * random.Unit() - 1f));
                var (swing, flutter) = (random.Unit(), random.Unit());
                clumps[row * columns + column] = new GrassClump(root, turn, tall, swing, flutter);
            }
        }

        return clumps;
    }

    /// <summary>A patch's own seed from the stretch's and where the patch stands in it.</summary>
    private static uint PatchSeed(uint seed, int column, int row) => unchecked(seed * 0x9E3779B9u ^ (uint)column * 0x85EBCA6Bu ^ (uint)row * 0xC2B2AE35u);

    // A size and the settings' numbers, refused as the argument they came in.
    private static void Check(Vector2 size, GrassSettings settings)
    {
        if (!(size.X > 0f && size.Y > 0f) || !float.IsFinite(size.X) || !float.IsFinite(size.Y))
            throw new ArgumentException($"Grass is planted over a stretch of some size, not {size.X} by {size.Y}.", nameof(size));
        if (!(settings.Density > 0f && settings.Height > 0f && settings.PatchSize > 0f) || !(settings.Variation >= 0f && settings.Variation < 1f))
            throw new ArgumentException("Grass needs a density, a height and a patch size above nothing and a variation from nothing up to less than the whole.", nameof(settings));
    }
}
