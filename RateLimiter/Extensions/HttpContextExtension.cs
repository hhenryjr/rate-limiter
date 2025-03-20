using Microsoft.AspNetCore.Http;
using System.Net.Http;
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

namespace RateLimiter.Extensions
{
  public static class HttpContextExtension
  {
    public static string GetCustomerKey(this HttpContext context)
        => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

    public static bool HasRateLimitAttribute(this HttpContext context, out RateLimitAttribute? rateLimitAttribute)
    {
      rateLimitAttribute = context.GetEndpoint()?.Metadata.GetMetadata<RateLimitAttribute>();
      return rateLimitAttribute is not null;
    }
  }
}
