using Microsoft.AspNetCore.Http;
using RateLimiter.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter.Middleware
{
  public class TokenBucketMiddleware 
  {
    private readonly RequestDelegate _next;

    public TokenBucketMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TokenBucket bucket)
    {
      if (!context.HasRateLimitAttribute(out var decorator))
      {
        await _next(context);
        return;
      }
      try
      {
        bucket.UseToken();
        await _next(context);
      }
      catch (Exception ex)
      {
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await _next(context);
      }
    }
  }
}
