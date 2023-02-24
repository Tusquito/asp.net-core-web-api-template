using Foundatio.Caching;

namespace Backend.Libs.Caching;

public sealed class RedisGenericKeyValueStorageAsync<TKey, TObject> : IKeyValueStorageAsync<TKey, TObject>
    where TKey : notnull
    {
        private readonly ICacheClient _cacheClient;
        private readonly string _dataPrefix;
        private readonly string _keySetKey;

        public RedisGenericKeyValueStorageAsync(ICacheClient multiplexer) : this(typeof(TObject).Name.ToLower(), multiplexer)
        {
        }

        private RedisGenericKeyValueStorageAsync(string basePrefix, ICacheClient multiplexer)
        {
            _dataPrefix = "data:" + basePrefix + ':';
            _keySetKey = "keys:" + basePrefix;
            _cacheClient = multiplexer;
        }
        
        private Task<CacheValue<ICollection<string>>> KeySet => _cacheClient.GetListAsync<string>(_keySetKey);

        public async Task<IEnumerable<TObject>> GetAllAsync()
        {
            return (await _cacheClient.GetAllAsync<TObject>(await GetAllKeysAsync().ConfigureAwait(false)).ConfigureAwait(false)).Select(s => s.Value.Value);
        }

        public async Task ClearAllAsync()
        {
            CacheValue<ICollection<string>> keys = await KeySet;
            await _cacheClient.ListRemoveAsync(_keySetKey, keys);
            await _cacheClient.RemoveAllAsync(keys.Value);
        }


        public async Task<TObject?> GetByIdAsync(TKey id) => (await _cacheClient.GetAsync<TObject>(ToKey(id)).ConfigureAwait(false)).Value;

        public async Task<IEnumerable<TObject>> GetByIdsAsync(IEnumerable<TKey> ids)
        {
            return (await _cacheClient.GetAllAsync<TObject>(ids.Select(ToKey))).Select(s => s.Value.Value);
        }

        public async Task RegisterAsync(TKey id, TObject obj, TimeSpan? lifeTime = null, bool keepTtl = false)
        {
            if (keepTtl)
            {
                lifeTime = await _cacheClient.GetExpirationAsync(ToKey(id));
            }
            await RegisterKeyAsync(new[] { id }, lifeTime).ConfigureAwait(false);
            await _cacheClient.SetAsync(ToKey(id), obj, lifeTime).ConfigureAwait(false);
        }


        public async Task RegisterAsync(IEnumerable<(TKey, TObject)> objs, TimeSpan? lifeTime = null, bool keepTtl = false)
        {
            await RegisterKeyAsync(objs.Select(s => s.Item1), lifeTime).ConfigureAwait(false);
            await _cacheClient.SetAllAsync(objs.ToDictionary(s => ToKey(s.Item1), o => o), lifeTime).ConfigureAwait(false);
        }

        public async Task<TObject?> RemoveAsync(TKey id)
        {
            await RemoveKeyAsync(new[] { id }).ConfigureAwait(false);
            await _cacheClient.RemoveAsync(ToKey(id)).ConfigureAwait(false);
            return default;
        }

        public async Task<IEnumerable<TObject>?> RemoveAsync(IEnumerable<TKey> ids)
        {
            await RemoveKeyAsync(ids);
            await _cacheClient.RemoveAllAsync(ids.Select(ToKey));
            return null;
        }

        private async Task<ICollection<string>> GetAllKeysAsync() => (await KeySet).Value;

        private Task RegisterKeyAsync(IEnumerable<TKey> keys, TimeSpan? lifeTime) => _cacheClient.ListAddAsync(_keySetKey, keys.Select(ToKey), lifeTime);

        private Task RemoveKeyAsync(IEnumerable<TKey> keys) => _cacheClient.ListRemoveAsync(_keySetKey, keys.Select(ToKey));

        private string ToKey(TKey id) => _dataPrefix + id;
}