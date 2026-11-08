namespace Vegetation;

public sealed partial record TreeSpecies
{
    // The name a refused control is given under, the species being the argument of whatever took it.
    private const string Argument = "species";

    /// <summary>Throws where a control of the species is outside what it may be, which the message names.</summary>
    /// <exception cref="ArgumentOutOfRangeException">A control is outside what it may be.</exception>
    /// <remarks>
    /// <see cref="TreeSkeleton.Grow(TreeSpecies, uint)"/> checks the species it grows, and an engine
    /// that keeps species by name checks one as it is kept, so a wrong control is heard of where it
    /// was set rather than when a tree first grows from it.
    /// </remarks>
    public void Check()
    {
        Finite(Scale, nameof(Scale), above: 0f);
        if (!(ScaleVariation >= 0f && ScaleVariation <= Scale))
        {
            throw new ArgumentOutOfRangeException(Argument, ScaleVariation, "A species' scale variation is from zero to its scale.");
        }

        if (!Enum.IsDefined(Shape)) throw new ArgumentOutOfRangeException(Argument, Shape, "A species' shape is one of TreeShape's.");
        if (!(BaseSize >= 0f && BaseSize < 1f)) throw new ArgumentOutOfRangeException(Argument, BaseSize, "A species' base size is from zero up to one.");
        if (BaseSplits < 0) throw new ArgumentOutOfRangeException(Argument, BaseSplits, "A species' base splits are none or more.");
        Finite(Ratio, nameof(Ratio), above: 0f);
        Finite(Flare, nameof(Flare), above: 0f, orEqual: true);
        Finite(AttractionUp, nameof(AttractionUp));
        if (!(PruneRatio is >= 0f and <= 1f)) throw new ArgumentOutOfRangeException(Argument, PruneRatio, "A species' prune ratio is from zero to one.");
        Finite(PruneWidth, nameof(PruneWidth), above: 0f);
        if (!(PruneWidthPeak is > 0f and <= 1f)) throw new ArgumentOutOfRangeException(Argument, PruneWidthPeak, "A species' prune width peak is above zero and up to one.");
        Finite(PrunePowerLow, nameof(PrunePowerLow), above: 0f, orEqual: true);
        Finite(PrunePowerHigh, nameof(PrunePowerHigh), above: 0f, orEqual: true);

        var levels = Levels ?? throw new ArgumentOutOfRangeException(Argument, "A species has its levels.");
        if (levels.Count is < 1 or > 4) throw new ArgumentOutOfRangeException(Argument, levels.Count, "A species has from one level, its trunk, to four.");

        for (var i = 0; i < levels.Count; i++)
        {
            var level = levels[i] ?? throw new ArgumentOutOfRangeException(Argument, $"Level {i} of the species is missing.");
            foreach (var (value, name) in (ReadOnlySpan<(float, string)>)[(level.DownAngle, nameof(level.DownAngle)), (level.DownAngleVariation, nameof(level.DownAngleVariation)), (level.Rotate, nameof(level.Rotate)), (level.Curve, nameof(level.Curve)), (level.CurveBack, nameof(level.CurveBack)), (level.CurveVariation, nameof(level.CurveVariation))])
            {
                Finite(value, $"level {i}'s {name}");
            }

            foreach (var (value, name) in (ReadOnlySpan<(float, string)>)[(level.RotateVariation, nameof(level.RotateVariation)), (level.Splits, nameof(level.Splits)), (level.SplitAngle, nameof(level.SplitAngle)), (level.SplitAngleVariation, nameof(level.SplitAngleVariation)), (level.Length, nameof(level.Length)), (level.LengthVariation, nameof(level.LengthVariation))])
            {
                Finite(value, $"level {i}'s {name}", above: 0f, orEqual: true);
            }

            if (level.SplitAngle > 180f) throw new ArgumentOutOfRangeException(Argument, level.SplitAngle, $"Level {i}'s split angle is up to 180 degrees.");
            if (level.Branches < 0) throw new ArgumentOutOfRangeException(Argument, level.Branches, $"Level {i}'s branches are none or more.");
            if (!(level.Taper is >= 0f and <= 3f)) throw new ArgumentOutOfRangeException(Argument, level.Taper, $"Level {i}'s taper is from zero to three.");
        }

        if (!(levels[0].LengthVariation < levels[0].Length)) throw new ArgumentOutOfRangeException(Argument, levels[0].LengthVariation, "The trunk's length is above zero and its variation below it.");

        if (!Enum.IsDefined(LeafShape)) throw new ArgumentOutOfRangeException(Argument, LeafShape, "A species' leaf shape is one of TreeLeafShape's.");
        Finite(LeafScale, nameof(LeafScale), above: 0f);
        Finite(LeafScaleX, nameof(LeafScaleX), above: 0f);
        if (!(LeafBend is >= 0f and <= 1f)) throw new ArgumentOutOfRangeException(Argument, LeafBend, "A species' leaf bend is from zero to one.");
        var leaf = LeafLevel ?? throw new ArgumentOutOfRangeException(Argument, "A species has its leaf level.");
        foreach (var (value, name) in (ReadOnlySpan<(float, string)>)[(leaf.DownAngle, nameof(leaf.DownAngle)), (leaf.DownAngleVariation, nameof(leaf.DownAngleVariation)), (leaf.Rotate, nameof(leaf.Rotate)), (leaf.RotateVariation, nameof(leaf.RotateVariation))])
        {
            Finite(value, $"leaf level's {name}");
        }
    }

    private static void Finite(float value, string name, float? above = null, bool orEqual = false)
    {
        var fits = float.IsFinite(value) && (above is not { } least || value > least || (orEqual && value == least));
        if (!fits)
        {
            var range = above is { } bound ? (orEqual ? $" and {bound} or more" : $" and above {bound}") : "";
            throw new ArgumentOutOfRangeException(Argument, value, $"A species' {name} is a finite number{range}.");
        }
    }
}
