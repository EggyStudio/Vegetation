using System.Numerics;

namespace Vegetation;

/// <summary>A clump of grass, two cards crossed about its root, from <see cref="GrassScatter.Clumps"/>.</summary>
/// <param name="Root">The middle of its foot on the ground, at y 0, from its patch's middle, in meters.</param>
/// <param name="Turn">How far its first card turns about the up axis from X, in radians from zero up to half a turn, the second card a quarter turn more.</param>
/// <param name="Height">How tall it stands, in meters.</param>
/// <param name="Swing">Its phase in the wind's swing along the wind, from zero up to one.</param>
/// <param name="Flutter">Its phase in the flutter across the wind, from zero up to one.</param>
public readonly record struct GrassClump(Vector3 Root, float Turn, float Height, float Swing, float Flutter);
