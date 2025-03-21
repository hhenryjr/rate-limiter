using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RateLimiter.Extensions
{
  public static class DistributedCacheExtension
  {
    public async static Task<ConsumptionData?> GetCustomerConsumptionDataFromContextAsync(this IDistributedCache cache, HttpContext context, CancellationToken cancellation = default)
    {
      var result = await cache.GetStringAsync(context.GetKey(), cancellation);
      if (result is null)
        return null;

      return JsonConvert.DeserializeObject<ConsumptionData>(result);
    }

    public async static Task SetCacheValueAsync(this IDistributedCache cache, string key, ConsumptionData? customerRequests, CancellationToken cancellation = default)
    {
      customerRequests ??= new ConsumptionData(DateTime.UtcNow, 1);

      await cache.SetStringAsync(key, JsonConvert.SerializeObject(customerRequests), cancellation);
    }
  }
}
