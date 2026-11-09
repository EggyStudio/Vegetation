using System.Numerics;

namespace Vegetation;

/// <summary>
/// Where trees stand and how each stands, a forest's places spread over a disc from a seed and a
/// tree's variant, turn and size from its own seed, so a level that keeps its trees by species and
/// seed stands the same trees in every engine that takes the module.
/// </summary>
/// <remarks>
/// <para>
/// A species grows <see cref="Variants"/> skeletons, each from a seed of the variant's own
/// (<see cref="VariantSeed"/>) apart from any seed a game gives, and a tree is one of them turned
/// about the up axis and sized a tenth either way by its own seed. A forest of a species is then a
/// few skeletons grown once and many trees, and the same species grows the same eight in every game.
/// </para>
/// <para>
/// The numbers are drawn from <see cref="Pcg"/> in a fixed order, so a seed means the same places
/// and the same stance on every runtime.
/// </para>
/// </remarks>
public static class TreePlanting
{
    /// <summary>How many variants a species grows, which the trees planted from it share, each turned and sized by its own seed.</summary>
    public const int Variants = 8;

    /// <summary>The seed a species' variant grows from, as <see cref="TreeSkeleton.Grow(TreeSpecies, uint)"/> takes it.</summary>
    /// <param name="variant">The variant, from zero up to <see cref="Variants"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">The variant is not one of a species' variants.</exception>
    public static uint VariantSeed(int variant)
    {
        if (variant is < 0 or >= Variants) throw new ArgumentOutOfRangeException(nameof(variant), variant, $"A species' variants are from zero up to {Variants}.");
        return (uint)(0x7E57_0000 + variant);
    }

    /// <summary>How a tree planted with a seed stands, which variant of its species it is, its turn and its size.</summary>
    /// <param name="seed">The tree's own seed.</param>
    public static TreeStance Stance(uint seed)
    {
        // A turn and a size of a tenth either way, from a generator of the seed's own, apart from
        // the variant the seed picks.
        var random = new Pcg((ulong)seed * 0x9E3779B97F4A7C15ul + 1);
        var turn = random.Unit() * MathF.Tau;
        var size = 0.9f + 0.2f * random.Unit();
        return new TreeStance((int)(seed % Variants), turn, size);
    }

    /// <summary>Spreads a forest's trees over a disc on level ground, none nearer another than an even spread allows.</summary>
    /// <param name="seed">The seed the trees' places and their own seeds are drawn from.</param>
    /// <param name="middle">The disc's middle, at the height the trees stand at.</param>
    /// <param name="radius">The disc's radius in meters.</param>
    /// <param name="count">How many trees, fewer where they do not fit apart.</param>
    /// <returns>Where each tree's trunk's foot stands and the tree's own seed, in the order they were drawn.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The radius is not a finite number above zero, or the count is below zero.</exception>
    /// <remarks>
    /// Each tree is drawn at random in the disc and kept where it stands seven tenths of an even
    /// spread's spacing from every tree before it, Poisson disk sampling by trial, so the forest is
    /// neither a grid nor a heap, and the seed spreads the same forest each time.
    /// </remarks>
    public static IReadOnlyList<(Vector3 Place, uint Seed)> Forest(uint seed, Vector3 middle, float radius, int count)
    {
        if (!(radius > 0f) || !float.IsFinite(radius)) throw new ArgumentOutOfRangeException(nameof(radius), radius, "A forest's radius is a finite number above zero.");
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var random = new Pcg((ulong)seed);
        var apart = 0.7f * MathF.Sqrt(MathF.PI * radius * radius / MathF.Max(count, 1));
        var places = new List<Vector3>();
        for (var tries = 0; places.Count < count && tries < count * 30; tries++)
        {
            var reach = radius * MathF.Sqrt(random.Unit());
            var angle = random.Unit() * MathF.Tau;
            var place = middle + new Vector3(reach * MathF.Cos(angle), 0f, reach * MathF.Sin(angle));
            if (places.All(other => Apart(other, place) >= apart * apart)) places.Add(place);
        }

        return [.. places.Select(place => (place, random.Next()))];
    }

    // The square of the distance between two places, its three products summed in one order, so a
    // forest's places do not hang on how a vector library sums them.
    private static float Apart(Vector3 one, Vector3 other)
    {
        var way = one - other;
        return way.X * way.X + way.Y * way.Y + way.Z * way.Z;
    }
}
