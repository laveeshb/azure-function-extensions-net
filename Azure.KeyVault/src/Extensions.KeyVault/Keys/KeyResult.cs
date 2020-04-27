
namespace Azure.Functions.Extensions.KeyVault
{
    using Microsoft.Azure.KeyVault.WebKey;

    public class KeyResult
    {
        public JsonWebKey Key { get; set; }
    }
}
