using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace CloudStorage.Services
{
    public class KeyVaultProvider
    {
        public SecretClient SecretClient { get; }

        //public KeyVaultProvider()
        //{
        //    var keyVaultUrl = new Uri("https://webprogramming.vault.azure.net/");

        //    var credential = new AzureCliCredential();

        //    SecretClient = new SecretClient(keyVaultUrl, credential);
        //}

        public KeyVaultProvider()
        {
            var options = new DefaultAzureCredentialOptions
            {
                VisualStudioTenantId = "f6c0e524-fbeb-44d7-851f-48fcaa6c6044"
            };

            SecretClient = new SecretClient(new Uri("https://webprogramming.vault.azure.net/"), new DefaultAzureCredential(options));
        }
    }
}
