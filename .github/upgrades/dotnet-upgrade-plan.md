# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade DataVisualizer.Contract\DataVisualizer.Contract.csproj
4. Upgrade DataVisualizer.TestClient\DataVisualizer.TestClient.csproj
5. Upgrade DataVisualizer.WinformViewer\DataVisualizer.WinformViewer.csproj
6. Run unit tests to validate upgrade in the projects listed below:


## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|


### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                                   |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Newtonsoft.Json                     |   13.0.3        |  13.0.4     | Recommended update for Winform project        |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.


#### DataVisualizer.Contract modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - None

Feature upgrades:
  - None

Other changes:
  - None


#### DataVisualizer.TestClient modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - None

Feature upgrades:
  - None

Other changes:
  - None


#### DataVisualizer.WinformViewer modifications

Project properties changes:
  - Target framework should be changed from `net9.0-windows` to `net10.0-windows`

NuGet packages changes:
  - Newtonsoft.Json should be updated from `13.0.3` to `13.0.4` (*recommended for .NET 10.0*)

Feature upgrades:
  - None

Other changes:
  - None
