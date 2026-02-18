using System.Diagnostics;

namespace CloudStorage.Services
{
    public class SecretProvider
    {
        public SecretProvider(KeyVaultProvider keyVaultProvider)
        {
            this.keyVaultProvider = keyVaultProvider;
        }

        private readonly KeyVaultProvider keyVaultProvider;

        public async Task<string> LoadSecretAsync(string secretName)
        {
            try
            {
                var secret = await this.keyVaultProvider.SecretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
