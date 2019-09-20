# FHIR Find Duplicate IDs

FHIR Find Duplicate IDs is a .NET core console application that detects duplicate IDs in a FHIR package / folder validating the package itself by detecting issues with duplicate FHIR resources.
Note: it does not support subfolders.

## Usage

Run

```bash
cd publish
dotnet FHIR.Utils.FindDuplicateIDs.dll <path-for-fhir-resources>
```

## Installation 

Build and run 

```bash
dotnet publish -c Release
```

## Usage

```bash
cd bin/Release/netcoreapp2.2
dotnet FHIR.Utils.FindDuplicateIDs.dll <path-for-fhir-resources>
```


