namespace CaracalGateway.Infrastructure
{
    public interface IKeyValueStorage
    {
        Task AddEntranceAsync(string key, object value, int? lifetime = null);
        Task<T> GetValueAsync<T>(string key);
        Task RemoveEntranceAsync(string key);
    }
}
