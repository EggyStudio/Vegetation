namespace Vegetation;

/// <summary>How a tree stands where it is planted, from its seed by <see cref="TreePlanting.Stance"/>.</summary>
/// <param name="Variant">Which of its species' variants it is, from zero up to <see cref="TreePlanting.Variants"/>, grown from <see cref="TreePlanting.VariantSeed"/> of it.</param>
/// <param name="Turn">How far it turns about the up axis, in radians from zero up to a whole turn.</param>
/// <param name="Size">How large it stands against its variant, from nine tenths up to eleven tenths.</param>
public readonly record struct TreeStance(int Variant, float Turn, float Size);
