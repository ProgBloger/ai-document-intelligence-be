using System.Collections.Concurrent;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class SecretProvider
{
    private readonly SecretClient _client;
    private readonly ConcurrentDictionary<string, string> _cache = new();

    public SecretProvider(string keyVaultUrl)
    {
        _client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    }

    public async Task<string> GetSecretAsync(string name)
    {
        if (_cache.TryGetValue(name, out var cached))
        {
            return cached;
        }

        var secret = await _client.GetSecretAsync(name);
        _cache[name] = secret.Value.Value;

        return secret.Value.Value;
    }
}