using System.Numerics;

namespace Vegetation;

/// <summary>A square of a stretch of grass that an engine draws as one mesh, from <see cref="GrassScatter.Patches"/>.</summary>
/// <param name="Column">Which patch of its row it is, counted along X from the stretch's corner.</param>
/// <param name="Row">Which row it is in, counted along Z from the stretch's corner.</param>
/// <param name="Middle">Where its middle stands from the stretch's middle, along X in X and along Z in Y, in meters.</param>
/// <param name="Size">Its width along X in X and its depth along Z in Y, in meters.</param>
/// <param name="Seed">The seed its clumps are scattered by, as <see cref="GrassScatter.Clumps"/> takes it.</param>
public readonly record struct GrassPatch(int Column, int Row, Vector2 Middle, Vector2 Size, uint Seed);
