# Azure files extension for Azure functions in .NET

- [NuGet Package](https://www.nuget.org/packages/AzureFunctions.Extension.AzureFiles)
- [Release Notes](https://github.com/laveeshb/azure-function-extensions-net/releases)

This extension helps developers manage data in [Azure Files](https://azure.microsoft.com/en-us/services/storage/files/) via bindings.

This extension provides the following bindings:

| Binding   | Description | Example |
|------------|------------------|-|
| Get file         | Gets the contents of a file | [GetFile](samples/Extension.AzureFiles.Sample.v2/GetFile.cs) |

The bindings support the following authentication methods:
* Storage account connection string
