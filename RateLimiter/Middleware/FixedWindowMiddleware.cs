using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using RateLimiter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter.Middleware
{
  public class FixedWindowMiddleware 
  {
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _distributedCache;

    public FixedWindowMiddleware(RequestDelegate next, IDistributedCache cache)
    {
      _next = next;
      _distributedCache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      if (!context.HasRateLimitAttribute(out var decorator))
      {
        await _next(context);
        return;
      }

      var consumptionData = await _distributedCache.GetCustomerConsumptionDataFromContextAsync(context);
      if (consumptionData is not null)
      {
        if (consumptionData.HasConsumedAllRequests(decorator!.TimeWindowInSeconds, decorator!.MaxRequests))
        {
          context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
          return;
        }

        consumptionData.IncreaseRequests(decorator!.MaxRequests);
      }

      await _distributedCache.SetCacheValueAsync(context.GetKey(), consumptionData);

      await _next(context);
    }
  }
}
