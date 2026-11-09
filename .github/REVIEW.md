# Review

Reviewed up to `f18a993`, item 6, the texture sets with their recipe, carried in the module's
assembly and opened by their paths, pushed at 01:01 on 2026-11-09, its run green at 01:01 on the
three systems in under a minute each; it keeps the form and references no engine. Nothing is pushed
after it. Before it `c2149be`, item 5, the species and the skeleton by the BevyCSharp session, with
the module's DEVLOG.md, pushed at 22:36 on 2026-11-08, its run green at 22:37 on the three systems
in under a minute each; it keeps the form and references no engine. Before it `63fc628`, item 2,
pushed at 12:31 on 2026-10-31 with `41b4925`, item 1, after the owner's first commit, its run green
at 12:35 on the three systems in about three minutes, the first of this repository. Both commits
keep the form, the three-mark subject, one sentence, the owner's identity as author and committer,
no trailer (N 7.2 and N 7.6 kept at the first commit); the library references no engine (N 2.1). The
documents are the engines' with the engine's name replaced and the engine-only lines dropped,
COMMITS.md the game's with N 7.1's pointer rule in place of the legacy branch line, .gitignore with
the dump folder, .claude and .ref; AGENTS.md's devlog row carries no link until DEVLOG.md exists and
its table of areas gains the rows item 2 adds. The skeleton sets ImportDirectoryBuildTargets false,
since a submodule under modules/ would inherit an engine's Directory.Build.targets, makes a missing
summary an error, loads the assembly by name in its one test, and build/version.sh prints 0.1.0; the
strict build 0 warnings and 0 errors, the test passing. After it `7ec59cf`, the norm alone, the
seven repositories named, pushed at 14:51 on 2026-11-03, with no run since the build passes over the
review's files; it keeps the form. Before it `472e7fa`, the owner's first commit alone.

## Now

The order the owner set on 2026-10-31 for the six modules, this one holding the species, the
skeleton grown from a seed, the tree and grass meshes through the modeling, the texture sets and the
placement. The documents first, then the rest as the other projects have it, then the engines taking
the module. Items 1 and 2 are the game's session's, being free while the game waits on the engine's
package; item 3 is the session's that owns the code today; item 4 is each engine's session's. Among
the six this module comes fourth, after Levels and once BevyCSharp's item 83 is done, on Modeling.

1. **The documents, one commit.** AGENTS.md on the game's shape (one paragraph saying what the
    module is and that no engine is referenced, N 2.1; the documents table with STYLE, COMMITS,
    REVIEW, NORM and DEVLOG; building and testing; Where things are with a row a folder; the
    conventions, the commit form among them), README.md (what the module is, how an engine takes it
    as a submodule, `dotnet build` and `dotnet test`), `.github/STYLE.md`, `COMMITS.md` and
    `CODE_OF_CONDUCT.md` as the engines' with the engine's name replaced, `.gitignore` of the
    engines' shape with `/.github/dump/` in it, `.gitattributes`. The game's AGENTS.md and the
    engines' documents are the models; nothing is written that the module does not have yet. This
    commit is the first test of the commit form here, N 7.2 and N 7.6, read by the reviewing session
    before it is pushed. Done at `41b4925`.

2. **The skeleton, one commit after item 1.** `Vegetation.slnx`, `Vegetation/Vegetation.csproj` as a
    class library on net10.0 with `Nullable` and `ImplicitUsings` in `Directory.Build.props`,
    `Vegetation.Tests/` on xunit with one test that the library loads, `build/version.txt` at `0.1`
    and `build/version.sh` as the engines', and `.github/workflows/build.yml` on a push, `test (os)`
    on ubuntu-24.04, windows-latest and macos-15 with `dotnet build Vegetation.slnx -warnaserror`
    and `dotnet test Vegetation.Tests --no-build`, a time limit on every job with its reason,
    `permissions` read and the engines' concurrency group. Green on the three systems before item 3.
    Done at `63fc628`.

3. **The code's move.** The source is BevyCSharp's Vegetation, Trees, TreeSpecies, TreeSkeleton,
TreeTextureSet and the grass of its item 83, moved after 83's phases are done, each engine binding
the leaves' material, the wind, the impostors and the light through as its renderer has them. One
commit a part, the tests coming with the code, the strict build and the suite green, a Shared line
in the engines' REVIEW.md when a part lands. Items written by the reviewing session when item 2 is
green and the owning session is free.

4. **The engines take the module.** Each engine adds this repository as a git submodule under
`modules/` and references its project, its workflows checking submodules out, its own folder for
this code becoming the binding of the module's interfaces (N 2.1) and the rest deleted; a change
used by an engine is committed here first and pushed before the engine's commit that moves the
pointer. One commit an engine, after item 3's first part.

5. **The species and the skeleton.** Item 3's first part, by the BevyCSharp session. TreeSpecies
    with its presets, TreeLevel, TreeShape, TreeLeafShape, TreeNode, TreeSkeleton, TreeGrowth with
    Space, Pcg, the species check and the growth entry move to the namespace Vegetation; the growth
    is TreeSkeleton.Grow of a species, a seed and blockers; places and boxes are System.Numerics
    vectors and the bark, leaf and fall colors are linear sRGB vectors as Sky's Colorimetry has
    them, converted at each use in the engines; ten of the bridge's thirteen tree tests come with
    the code, the three that build meshes staying. One module commit, told to the reviewer first and
    pushed before the bridge's commit that takes the module as a submodule, as Sky is taken, with
    the types out of its public listing and its tree guide and cheat sheet following; a Shared line
    in the bridge's REVIEW.md. Done at `c2149be`, pushed at 22:36 on 2026-11-08; the growth is the
    bridge's to the bit, 26 skeletons giving the same 72,137 lines of points, parents and radii, ten
    tests moved and 16 running, Pcg and the species check public while the bridge's leaves, grass
    and planting draw from them.

6. **The texture sets.** Item 3's second part. TreeTextureKind, TreeTextureSet, TreeTextures and the
    Textures folder with its recipe, kept as Modeling keeps its collection (Modeling's Decision 8),
    the loading staying in the engines through the module's paths. A module commit pushed before the
    bridge's. After 5. Done at `f18a993`, pushed at 01:01 on 2026-11-09; the 13 sets and their
    manifest under Vegetation/Textures, embedded in the module's assembly, TreeTextures.Files
    listing every file by its path and Open opening one, an added set under added/<n>/, the recipe
    taking --sources since the sources stay in the bridge's .vegetation and rebuilding all 43 files
    byte for byte, MeanColor a linear sRGB vector, ten tests moved or new, the loading staying in
    the bridge as TreeTextureAssets.

7. **The grass's scatter.** Item 3's third part. Grass and GrassSettings, the scatter alone, the
    grass's material, wind and spawning staying in the engines. A module commit pushed before the
    bridge's. After 6.

8. **The planting's placement.** Item 3's fourth part. Where the trees stand in a level, the
    placement alone, the spawn, the colliders and the impostors staying in the engines. A module
    commit pushed before the bridge's. After 7.

9. **The meshes.** Item 3's fifth part, blocked. Build, Cards, Leaves, Volume, TreeLooks and
    TreeDetail are built on the edit mesh, which the Modeling module does not hold until its item 3
    moves it, given to the 3DEngine session by Modeling's Decision 5; until then they stay in the
    bridge as its binding, with the wind, the materials, the impostors, the card pictures, the
    memory and the Tree component, which stay there for good. After Modeling's item 3.

## Verdicts

None yet.

## Decisions

1. **The repository's birth.** The owner made this repository on 2026-10-31 at 12:03 among six,
Modeling, Levels, Landscape, RadianceCascades, Tools beside it, after the ideas recorded as
3DEngine's Decision 78 and BevyCSharp's Decision 71, to hold the species, the skeleton grown from a
seed, the tree and grass meshes through the modeling, the texture sets and the placement as one
library both engines use rather than one implementation an engine. The owner set the order the same
day, the documents first, then the rest as the other projects have it, then the engines taking the
module, and said the modules may be tried at once beside the engine's global illumination work
(3DEngine Decision 79).

2. **A git submodule, not a package.** The owner chose on 2026-10-31 that the engines take the
module as a git submodule built as a project of their own, since a package's upload, wait, test and
pack is slower to iterate than a commit. A change is committed here first; the reviewing session
pushes this repository before the engine's commit that moves its pointer; the workflows check
submodules out. The game keeps using the engine through the engine's package.

3. **The module knows no engine.** N 2.1. The module references no engine, package or bridge; what
it needs of a renderer, a world, a scheduler or a file system it takes through interfaces it
declares, which each engine implements where it binds the module. A type of an engine's in this
repository is a fault.

4. **The repository's terms are the owner's.** The repository was made by the owner with its terms
   file in place, which stands as the owner set it and is spoken of nowhere else.

5. **Who writes here.** The game's session writes items 1 and 2 for the six modules, being free
while the game waits on the engine's package; the session that owns the code today moves it (item
3), BevyCSharp's for Vegetation and Levels, 3DEngine's for Modeling and the cascades, the reviewing
session's scripts going into Tools; each engine's session binds the module (item 4). Every session
reads REVIEW.md's head, its items and the Replies before a batch, as N 7.7 says, and the reviewing
session pushes.

6. **The move in five parts.** On 2026-11-08 at 21:51 the review took the BevyCSharp session's split
    of item 3 as items 5 to 9, the species and skeleton first, then the texture sets, the grass's
    scatter and the planting's placement, the meshes last and blocked on Modeling's edit mesh; every
    part a module commit pushed before the engine's commit that moves its pointer, with its tests,
    the strict build, the suite and the drive. The module's DEVLOG.md, written with items 1 and 2
    and left untracked, goes in with item 5's commit and takes a Done line as each part lands, by
    hand, since the modules have no generator.

## Replies

Item 5, the species and the skeleton, in the commit carrying this line. TreeSpecies with its
presets, TreeLevel, TreeShape, TreeLeafShape, TreeNode, TreeSkeleton, TreeGrowth with its space and
Pcg move from BevyCSharp, the namespace Vegetation. The growth's entry is TreeSkeleton.Grow of a
species, a seed and blockers, the bridge's Trees.Grow as it was, and the species' check is
TreeSpecies.Check, public, since the bridge keeps species by name and checks one as it is kept. Pcg
is public as well, since the bridge's leaves, grass and planting draw from it until their parts
move, the leaves' with the meshes once Modeling holds the edit mesh. Places and boxes are
System.Numerics' Vector3, and the bark's, the leaves' and the fall's colors are linear sRGB vectors
as Sky's Colorimetry has them, the presets written in sRGB as before and decoded by sRGB's own
curve. A way at unit length, which System.Numerics gives as NaN for a vector of no length, is an
extension of the module's that turns such a way along Z as the bridge's Vec3 did. The
documentation's references to the bridge's meshes, texture sets and leaf material are said in words.

The growth is the bridge's to the bit, the six presets at four seeds each and two trees grown round
boxes giving the same 72,137 lines of points, parents and radii as the bridge's head. Ten of the
bridge's thirteen tree tests come with the code, the refusal of a skeleton out of order without the
mesh it built, which stays in the bridge, and the refusal test checks the public check as well. The
strict build is clean and the tests pass, 16 where there was 1 (dotnet test Vegetation.Tests). The
workflow's comment gives the build's 8 seconds and the test's 19 from an empty package folder, where
it gave 3 and 2. The README's table names what the module holds in four rows. DEVLOG.md, written
with items 1 and 2, goes in with this commit, and its Done line for this item with the next.
BevyCSharp's commit that takes the module follows once this one is pushed.

Item 6, the texture sets, in the commit carrying this line. TreeTextureKind, TreeTextureSet and
TreeTextures move from BevyCSharp, the namespace Vegetation, with the thirteen sets and their
manifest under Vegetation/Textures and the recipe, build/textures.py, as Modeling keeps its
collection. The library embeds the sets and the manifest in its assembly, the sets marked waiting
apart, so TreeTextures lists and finds them with no engine, and an engine reads each file by its
path in the collection, which Files lists and Open opens, the set's name and the file's, as
silver-birch-bark/albedo.ktx2, a game's folder added with Add lying below added/ and its number. The
loading stays in the engines, the bridge reading every file under a folder of its asset root and
loading a set's images with their samplers. A set's mean color is linear sRGB in a Vector3 as a
species' colors are, decoded by the same curve, and its mean surface is public, since the bridge's
far cards are drawn with it.

The recipe takes --sources, the folder its sources are read under, .vegetation/ at the repository's
root where none is given, since the sources stay on the machine that imports them, beside the bridge
here, and the manifest names each source within that folder. Run from the module with the bridge's
sources, it makes all 43 files of the thirteen sets and the tests' sample the same to the byte as
the collection and the sample hold. Its docstring gives the formats in their own terms where it
named Bevy's material and loader. Ten tests come with the code, eight of the bridge's and two new,
one opening every carried file against its folder's bytes and one reading each set's mean color as
its manifest's through sRGB's curve, and the leaf boxes' test checks the sets' boxes against the
manifest as well. The tests that run the recipe stay in the bridge, where a test is skipped on a
machine without Python 3.14 and Pillow, as do those that load a set on the GPU. The strict build is
clean and the tests pass, 26 where there were 16. The README names the three types and gives the
collection's section, which BUILDING.md held in the bridge, and the .gitignore leaves out the
waiting sets and the sources. DEVLOG.md takes item 5's Done line. AGENTS.md's row for build names
the version alone, and naming textures.py there, as Modeling's row names its own, waits on the
owner's word (N 7.4).

Item 7, the grass's scatter, in the commit carrying this line. GrassSettings moves from BevyCSharp
whole, the namespace Vegetation, and the scatter that was the bridge's Grass.Plant and Grass.Patch
is GrassScatter. Its Patches cuts a stretch into the square patches its grass is drawn in, each a
GrassPatch with its column, its row, its middle, its size and its seed, and its Clumps scatters a
patch's clumps a cell each, each a GrassClump with its root, its turn, its height and the two phases
it sways by. A clump's numbers are drawn from Pcg in the order the bridge drew them, so the bridge's
patches and meshes are the same to the bit, 400 patches' meshes over four settings, five sizes, five
seeds and four widths of picture hashed whole and 161 patches' places over ten stretches the same as
the bridge's before the move, with its three refusals' arguments and messages. The two cards a clump
is drawn as, their width from the set's picture, the sway streams, the material, the wind and the
patches' entities stay in the engines, the bridge's Grass.Plant planting what the scatter gives. The
refusals are the bridge's, a stretch of no size as the size argument and a setting out of range or a
set that is not grass as the settings argument, and Clumps refuses what Patches does but the set,
which it does not read.

Four tests come with the code, the bridge's test of a seed scattering the same patch and three new,
the patches a stretch is cut into, a patch's clumps a cell each on the ground and the refusals,
which the bridge's tests checked through what it planted. The strict build is clean and the tests
pass, 30 where there were 26. The README names the four types in a row, and DEVLOG.md takes item 6's
Done line.
