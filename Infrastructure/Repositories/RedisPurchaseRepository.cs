using Domain.Entities;
using Domain.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure
{

    public class RedisPurchaseRepository : IPurchaseRepository
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;

        public RedisPurchaseRepository()
        {
            // Initialize the Redis connection and database
            _redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
            _database = _redisConnection.GetDatabase();
        }

        public async Task<Purchase> GetPurchaseByIdAsync(string purchaseId)
        {
            var purchaseJson = await _database.StringGetAsync(purchaseId);
            return JsonSerializer.Deserialize<Purchase>(purchaseJson);
        }

        public async Task DeletePurchaseByIdAsync(string purchaseId)
        {
            await _database.KeyDeleteAsync(purchaseId);
        }

        public async Task<Purchase> AddPurchaseAsync(Purchase purchase)
        {
            var id = await GenerateUniqueIdAsync("purchase_");
            purchase.PurchaseId = $"purchase_{id}";

            var purchaseJson = JsonSerializer.Serialize<Purchase>(purchase);
            await _database.StringSetAsync(purchase.PurchaseId, purchaseJson);

            return await GetPurchaseByIdAsync(purchase.PurchaseId);
        }

        public async Task<Purchase> UpdatePurchaseAsync(Purchase purchase)
        {
            var purchaseJson = JsonSerializer.Serialize<Purchase>(purchase);
            await _database.StringSetAsync(purchase.PurchaseId, purchaseJson);

            return await GetPurchaseByIdAsync(purchase.PurchaseId);
        }

        private async Task<long> GenerateUniqueIdAsync(string key)
        {
            return await _database.StringIncrementAsync($"{key}id");
        }
    }



}



