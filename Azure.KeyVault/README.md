# Azure key vault extension for Azure functions in .NET

- [Homepage](.)
- [NuGet Package](https://www.nuget.org/packages/AzureFunctions.Extension.KeyVault)
- [Release Notes](https://github.com/laveeshb/azure-function-extensions-net/releases)
- [License](LICENSE)

This extension helps developers get secrets, keys and certificates from [Azure key vault](https://azure.microsoft.com/en-us/services/key-vault/) via bindings.

This extension provides the following bindings:

| Binding   | Description | Example |
|------------|------------------|-|
| Get secret         | Gets the latest version of a secret | [GetSecret](samples/Extensions.KeyVault.Sample.v2/Secrets/GetSecret.cs) |
| Get secret with version | Gets the specified version of a secret | [GetSecretVersion](samples/Extensions.KeyVault.Sample.v2/Secrets/GetSecretVersion.cs) |
| Get certificate public key | Gets the public key of the latest version of a certificate | [GetCertificatePublicKey](samples/Extensions.KeyVault.Sample.v2/Certificates/GetCertificatePublicKey.cs) |
| Get certificate public key with version  | Gets the public key of the specified version of a certificate | [GetCertificateVersionPublicKey](samples/Extensions.KeyVault.Sample.v2/Certificates/GetCertificateVersionPublicKey.cs) |
| Get certificate private key | Gets the private key of the latest version of a certificate | [GetCertificatePrivateKey](samples/Extensions.KeyVault.Sample.v2/Certificates/GetCertificatePrivateKey.cs) |
| Get certificate private key with version  | Gets the private key of the specified version of a certificate | [GetCertificateVersionPrivateKey](samples/Extensions.KeyVault.Sample.v2/Certificates/GetCertificateVersionPrivateKey.cs) |
| Get key         | Gets the latest version of a key | [GetKey](samples/Extensions.KeyVault.Sample.v2/Keys/GetKey.cs) |
| Get key with version | Gets the specified version of a key | [GetKeyVersion](samples/Extensions.KeyVault.Sample.v2/Keys/GetKeyVersion.cs) |

The bindings support the following authentication methods:
* Azure AD application with client secret
