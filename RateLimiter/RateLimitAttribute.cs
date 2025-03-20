using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class RateLimitAttribute : Attribute
  {
    public int TimeWindowInSeconds { get; set; }
    public int MaxRequests { get; set; }
  }
}
