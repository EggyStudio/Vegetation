# Vegetation

Vegetation is a library both engines, 3DEngine and BevyCSharp, take as a git submodule, for plant
species, each one's skeleton grown from a seed, the tree and grass meshes built through Modeling,
their texture sets and their placement. It references no engine, and each engine implements the
interfaces it declares where it binds the module.

## What it holds

| Type | Gives |
|---|---|
| `TreeSpecies` | What a kind of tree is, by Weber and Penn's controls of 1995, its crown's outline, its size and how much one varies from the next, its trunk and its levels of branches, its leaves, and its bark's and leaves' colors in linear sRGB, with presets for a quaking aspen, a black tupelo, a weeping willow, a California black oak, a bush and a fern, and `Check`, which refuses a control outside what it may be |
| `TreeLevel`, `TreeShape` and `TreeLeafShape` | The controls of a level's stems, how they leave the stem before, bend, fork and taper, the crown's outline among the paper's eight or its pruning envelope, and the outline of a leaf an engine generates |
| `TreeSkeleton` and `TreeNode` | A tree's stems as points, each with the point it grew from, its place and its radius, and `TreeSkeleton.Grow`, which grows one from a species and a seed by space colonization round boxes it does not grow into, a seed growing the same points every time |
| `Pcg` | The random numbers a tree grows from, O'Neill's PCG, the same for a seed on every runtime, which an engine draws a tree's leaves and its planting from as well |
| `TreeTextures`, `TreeTextureSet` and `TreeTextureKind` | The texture collection the module carries in its assembly, bark, leaf and grass sets found by name, each set's files by their paths with the mean color and surface its manifest measured and a leaf atlas's boxes, and a game's own folders of sets added beside them |

## Taking it into an engine

An engine adds the repository as a git submodule under `modules/` and references its project, here
from a project one folder below the engine's root.

```bash
git submodule add https://github.com/EggyStudio/Vegetation.git modules/Vegetation
```

```xml
<ProjectReference Include="..\modules\Vegetation\Vegetation\Vegetation.csproj" />
```

A clone of the engine fetches the module with `git clone --recurse-submodules`, or with
`git submodule update --init` in a clone made without it, and a workflow checks it out with
`submodules: true` on `actions/checkout`. A change the engine uses is committed and pushed here
before the engine's commit that moves its pointer, so the engine's history never names a commit
this repository does not have.

## Building and testing

With the .NET 10 SDK:

```bash
dotnet build Vegetation.slnx
dotnet test Vegetation.Tests
```

## The texture collection

`Vegetation/Textures` holds the bark, leaf and grass sets the module carries in its assembly, which
`TreeTextures` finds by name, each three KTX2 files in BC7 and BC5 with their mipmaps and zstd over
each level, and a bark a fourth, its depth in BC4. `build/textures.py` makes them from the sources
sorted under `.vegetation/`, which the repository leaves out, or under the folder `--sources` names,
by the recipe the collection's `textures.json` gives each set, the crop, the size, the maps packed
into each file and any change of color, so a set is made again from its source byte for byte. It
needs Python 3.14, whose own modules have zstd, and Pillow:

```bash
build/textures.py                              # import every set whose source is here
build/textures.py fern-leaves                  # import one
build/textures.py --sources DIR fern-leaves    # import one from the sources under DIR
```

A set the manifest marks waiting is imported into `waiting/`, which the repository and the build
leave out, and `TreeTextures.Add` reads it there for a look on the machine that imported it. A set
is at most 1024 square, a species' bark and leaves at most 3 MB together and the collection at most
32 MB, which `TreeTexturesTests` holds.

## License

The Mozilla Public License 2.0, in [LICENSE](LICENSE).
