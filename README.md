# AGT Exporter / RiceTools

Editors, packers and converters for Drift City / SkidRush client files.

## RicePack (AGT Exporter GUI)

This is the application to use when inspecting the game client.

Supported formats:
- `.agt`
- `.tdf` files stored inside AGT archives
- `.lof` (read-only)
- `.ntx`

### Build

1. Open `RiceTools.sln` in Visual Studio 2022.
2. Restore NuGet packages when prompted.
3. Make sure `RicePack` is selected as the Startup Project.
4. Build `Release | Any CPU`.
5. Run `RicePack\bin\Release\AGTExporter.exe`.

The project targets .NET Framework 4.8.

If the application fails during startup, it now shows the error and writes `AGTExporter-crash.log` next to the executable.

### Drift City client research workflow

For server/client protocol research:

1. Open the relevant client `.agt` archive with **Open AGT**.
2. Locate `ItemClient.tdf` (or another `.tdf`) in the tree.
3. Selecting a TDF displays its rows/columns in the grid.
4. Use **Export All** to extract archive files when needed.

For vehicle-key research, the important data is the client-side mapping between item rows/indexes and IDs such as:

- `pc_0000c` (Kicker)
- `pc_0068s` (Nevera)
- `pc_0070s` (Metro)

That mapping lets the server use the real client TableIndex instead of guessing from server-side XML order.

## RiceConvert

Command-line converter for:
- `.hit` → `.hit.obj`
- `.chpath` → `.chpath.obj`

Drag a supported file onto `RiceConvert.exe` to convert it. After editing, drag it onto RiceConvert again to convert back.
