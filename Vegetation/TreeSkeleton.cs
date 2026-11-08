using System.Numerics;

namespace Vegetation;

/// <summary>
/// A grown tree's stems as points, each with the point it grew from, its place and its radius,
/// which <see cref="Grow(TreeSpecies, uint)"/> grows from a species and a seed and an engine builds
/// into a mesh.
/// </summary>
/// <remarks>
/// <para>
/// The growth is after Runions, Lane and Prusinkiewicz (2007), with buds as Palubicki and others
/// (2009) give them. Attractors are scattered through the crown's <see cref="TreeSpecies.Shape"/>,
/// the trunk grows up through them, and every bud that perceives one ahead of it grows toward it
/// an internode at a time, taking the attractors it comes near, until none is left that a bud
/// perceives. The paper's angles, bends, forks and lengths are the buds' tendencies, so a species
/// reads as the paper's does, and the attractors decide which buds grow, so the crown fills its
/// outline and its inside stays open.
/// </para>
/// <para>
/// A seed grows the same skeleton every time, point for point, since the growth draws its random
/// numbers from a generator of its own and does everything in a fixed order. Another machine's
/// floating point may round a sine differently and grow a slightly different tree, which keeping
/// the skeleton rather than the seed avoids.
/// </para>
/// <para>
/// The first point is the trunk's foot and has no parent, and every other point comes after the
/// point it grew from, so the points read in order go from the trunk out to the twigs and a
/// stem is followed by following parents. A point with more than one child is a fork or a place a
/// branch leaves, the thickest child carrying the stem on.
/// </para>
/// <para>
/// The skeleton is plain data, a parent, a place and a radius a point and nothing worked out from
/// them, so a level may keep it beside the species and the seed and draw the same tree on a machine
/// whose floating point would grow it a little differently.
/// </para>
/// </remarks>
public sealed record TreeSkeleton
{
    /// <summary>A skeleton of the points given.</summary>
    /// <param name="nodes">The points, the trunk's foot first and each after its parent.</param>
    /// <exception cref="ArgumentException">
    /// There are no points, the first has a parent, another's parent does not come before it, or a
    /// place or a radius is not a finite number, a radius above zero.
    /// </exception>
    public TreeSkeleton(IReadOnlyList<TreeNode> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        if (nodes.Count == 0) throw new ArgumentException("A tree's skeleton has a point at the trunk's foot at least.", nameof(nodes));

        for (var i = 0; i < nodes.Count; i++)
        {
            var (parent, place, radius) = nodes[i];
            if (i == 0 ? parent != -1 : parent < 0 || parent >= i)
            {
                throw new ArgumentException(i == 0
                    ? $"The trunk's foot, the first point, has no parent, and this one's is {parent}."
                    : $"Point {i} grew from point {parent}, which does not come before it.", nameof(nodes));
            }

            if (!float.IsFinite(place.X) || !float.IsFinite(place.Y) || !float.IsFinite(place.Z) || !(radius > 0f) || !float.IsFinite(radius))
            {
                throw new ArgumentException($"Point {i} is at {place} with a radius of {radius}, and a place is finite and a radius finite and above zero.", nameof(nodes));
            }
        }

        Nodes = nodes;
    }

    /// <summary>The points, the trunk's foot first and each after the point it grew from.</summary>
    public IReadOnlyList<TreeNode> Nodes { get; }

    /// <summary>Grows a tree of a species from a seed.</summary>
    /// <param name="species">The species.</param>
    /// <param name="seed">The seed, any number, each growing another tree of the species.</param>
    /// <returns>The tree's skeleton, standing up the Y axis from the origin.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A control of the species is outside what it may be, which the message names.</exception>
    /// <remarks>
    /// It grows on the thread that calls it and touches nothing outside itself, so it runs on a
    /// worker while a level loads. Its cost grows with the crown's points.
    /// </remarks>
    public static TreeSkeleton Grow(TreeSpecies species, uint seed) => Grow(species, seed, []);

    /// <summary>Grows a tree of a species from a seed, round boxes it does not grow into and that shade it.</summary>
    /// <param name="species">The species.</param>
    /// <param name="seed">The seed, any number, each growing another tree of the species.</param>
    /// <param name="blockers">
    /// Boxes in the tree's space, the trunk's foot at the origin, from their lowest corner to their
    /// highest, a wall, a building or a neighbor's crown.
    /// </param>
    /// <returns>The tree's skeleton, standing up the Y axis from the origin.</returns>
    /// <exception cref="ArgumentOutOfRangeException">A control of the species is outside what it may be, which the message names.</exception>
    /// <remarks>
    /// No stem grows into a box, and a box shades the space under it, so a tree beside a wall or
    /// under a neighbor's crown grows its branches away from it toward the light, as a tree at a
    /// forest's edge leans out of it. The same species, seed and boxes grow the same tree.
    /// </remarks>
    public static TreeSkeleton Grow(TreeSpecies species, uint seed, IReadOnlyList<(Vector3 Low, Vector3 High)> blockers)
    {
        ArgumentNullException.ThrowIfNull(species);
        ArgumentNullException.ThrowIfNull(blockers);
        species.Check();
        foreach (var (low, high) in blockers)
        {
            if (!(low.X <= high.X && low.Y <= high.Y && low.Z <= high.Z)) throw new ArgumentOutOfRangeException(nameof(blockers), $"A box runs from its lowest corner to its highest, and one runs from {low} to {high}.");
        }

        return new TreeGrowth(species, seed, blockers).Grow();
    }

    /// <summary>Whether another skeleton has the same points in the same order, compared one by one.</summary>
    /// <param name="other">The other skeleton.</param>
    public bool Equals(TreeSkeleton? other) => other is not null && Nodes.SequenceEqual(other.Nodes);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Nodes.Count, Nodes[0], Nodes[^1]);

    /// <summary>The smallest box holding every point, not counting the stems' thickness.</summary>
    public (Vector3 Low, Vector3 High) Bounds
    {
        get
        {
            var low = Nodes[0].Place;
            var high = low;
            foreach (var node in Nodes)
            {
                var place = node.Place;
                low = new Vector3(MathF.Min(low.X, place.X), MathF.Min(low.Y, place.Y), MathF.Min(low.Z, place.Z));
                high = new Vector3(MathF.Max(high.X, place.X), MathF.Max(high.Y, place.Y), MathF.Max(high.Z, place.Z));
            }

            return (low, high);
        }
    }
}
