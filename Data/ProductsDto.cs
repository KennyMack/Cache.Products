using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cache.Products.Data
{
    public class ProductsDto : IProductsDto
    {
        readonly IDistributedCache Cache;
        readonly string CacheKey = "Products";
        readonly DistributedCacheEntryOptions OptionsCache;
        readonly DbCacheContext Context;

        public ProductsDto(DbCacheContext context, IDistributedCache cache)
        {
            Context = context;
            Cache = cache;
            OptionsCache = new DistributedCacheEntryOptions();
            OptionsCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            OptionsCache.SetSlidingExpiration(TimeSpan.FromMinutes(1));
        }

        public IEnumerable<Models.Products> GetAll()
        {
            var KeyRedis = $"{CacheKey}-List";
            var ProductsCache = Cache.GetString(KeyRedis);
            if (ProductsCache != null)
                return JsonSerializer.Deserialize<IEnumerable<Models.Products>>(ProductsCache);

            var Products = Context.Products.ToList();
            ProductsCache = JsonSerializer.Serialize<IEnumerable<Models.Products>>(Products);
            Cache.SetString(KeyRedis, ProductsCache, OptionsCache);
            return Products;
        }

        public Models.Products GetById(Guid pId)
        {
            var KeyRedis = $"{CacheKey}-Id-${pId}";
            var ProductCache = Cache.GetString(KeyRedis);
            if (ProductCache != null)
                return JsonSerializer.Deserialize<Models.Products>(ProductCache);

            var Product = Context.Products.Find(pId);
            ProductCache = JsonSerializer.Serialize(Product);
            Cache.SetString(KeyRedis, ProductCache, OptionsCache);

            return Product;
        }

        public Models.Products New(Models.Products pProduct)
        {
            var KeyRedisList = $"{CacheKey}-List";
            var KeyRedis = $"{CacheKey}-Id-${pProduct.Id}";

            Context.Products.Add(pProduct);
            Context.SaveChanges();

            var ProductCache = JsonSerializer.Serialize(pProduct);
            Cache.SetString(KeyRedis, ProductCache, OptionsCache);
            Cache.Remove(KeyRedisList);

            return pProduct;
        }

        public Models.Products Update(Models.Products pProduct)
        {
            var KeyRedisList = $"{CacheKey}-List";
            var KeyRedis = $"{CacheKey}-Id-${pProduct.Id}";

            Context.Products.Update(pProduct);
            Context.SaveChanges();

            var ProductCache = JsonSerializer.Serialize(pProduct);
            Cache.SetString(KeyRedis, ProductCache, OptionsCache);
            Cache.Remove(KeyRedisList);

            return pProduct;
        }

        public Models.Products RemoveById(Guid pId)
        {
            var KeyRedis = $"{CacheKey}-Id-${pId}";
            var KeyRedisList = $"{CacheKey}-List";

            var Product = Context.Products.Find(pId);
            Context.Products.Remove(Product);
            Context.SaveChanges();

            Cache.Remove(KeyRedis);
            Cache.Remove(KeyRedisList);

            return Product;
        }
    }
}