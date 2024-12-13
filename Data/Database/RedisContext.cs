﻿using StackExchange.Redis;

namespace BackendLaboratory.Data.Database
{
    public class RedisContext
    {
        private readonly IDatabase _database;

        public RedisContext(string connectionString)
        {
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _database = connection.GetDatabase();
        }

        public async Task<bool> AddExpiredToken(string tokenId)
        {
            var key = $"ExpiredToken:{tokenId}";
            return await _database.StringSetAsync(key, "expired", TimeSpan.FromMinutes(30));
        }

        public async Task<bool> IsTokenExpired(string tokenId)
        {
            var key = $"ExpiredToken:{tokenId}";
            return await _database.KeyExistsAsync(key);
        }
    }
}
