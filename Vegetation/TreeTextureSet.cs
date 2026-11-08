using System.Numerics;

namespace Vegetation;

/// <summary>
/// A set of the vegetation's texture collection, its files by their paths in the collection, found
/// by name in <see cref="TreeTextures"/>.
/// </summary>
/// <param name="Name">Its name, which a species gives as its <see cref="TreeSpecies.BarkSet"/> or <see cref="TreeSpecies.LeafSet"/>.</param>
/// <param name="Kind">Whether it is a bark, an atlas of leaves or a card of grass.</param>
/// <param name="Albedo">The path of the color in RGB and the opacity in A, BC7 in sRGB.</param>
/// <param name="Normal">The path of the tangent-space normal's X and Y, BC5, a two-channel normal map.</param>
/// <param name="Surface">The path of occlusion in R, roughness in G, metallic in B and translucency in A, BC7.</param>
/// <remarks>
/// <para>
/// Each file is a KTX2 with its mipmaps, compressed in the formats a desktop GPU samples as they
/// are, a byte a texel, and each level supercompressed with zstd. The surface's first three
/// channels are where glTF's occlusion and metallic-roughness textures place them and its fourth is
/// the share of light let through, so an engine binds the one image as a material's occlusion, its
/// metallic-roughness and its diffuse transmission texture.
/// </para>
/// <para>
/// A path is the set's name and the file's, as <c>silver-birch-bark/albedo.ktx2</c>, below
/// <c>added/</c> and the folder's number for a set a game added with
/// <see cref="TreeTextures.Add"/>. <see cref="TreeTextures.Open"/> opens a file by its path, which
/// an engine reads under a folder of its own.
/// </para>
/// </remarks>
public sealed record TreeTextureSet(string Name, TreeTextureKind Kind, string Albedo, string Normal, string Surface)
{
    /// <summary>The albedo's width and height in texels, the normal and the surface being half of each, or nothing where the set's manifest does not say.</summary>
    public (int Width, int Height)? Size { get; init; }

    /// <summary>
    /// Where each leaf of a leaf set's atlas is, as the share of the atlas's width and height from its
    /// left and top and the share it spans, its stalk at the bottom, or the whole atlas as one leaf
    /// where the manifest gives none.
    /// </summary>
    public IReadOnlyList<(float U, float V, float Width, float Height)> LeafBoxes { get; init; } = [(0f, 0f, 1f, 1f)];

    /// <summary>
    /// The mean color of the albedo's opaque texels in linear sRGB, the summer color a fall tint is
    /// worked out against, or white where the manifest does not say.
    /// </summary>
    public Vector3 MeanColor { get; init; } = Vector3.One;

    /// <summary>
    /// The mean of the surface's occlusion, roughness, metallic and translucency over the albedo's
    /// opaque texels, which a tree's far card, a picture of a whole branch, is drawn with, or a
    /// surface fully rough and lit through nowhere where the manifest does not say.
    /// </summary>
    public (float Occlusion, float Roughness, float Metallic, float Translucency) MeanSurface { get; init; } = (1f, 1f, 0f, 0f);

    /// <summary>
    /// The path of a bark's depth for parallax occlusion, one channel in BC4, white the deepest, a
    /// depth below the surface rather than a height, or nothing for a set without one.
    /// </summary>
    public string? Depth { get; init; }
}
