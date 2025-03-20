using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
  public class ConsumptionData
  {
    public DateTime LastResponse { get; private set; }
    public int NumberOfRequests { get; private set; }
    public ConsumptionData(DateTime lastResponse, int numberOfRequests)
    {
      LastResponse = lastResponse;
      NumberOfRequests = numberOfRequests;
    }

    public bool HasConsumedAllRequests(int timeWindowInSeconds, int maxRequests)
        => DateTime.UtcNow < LastResponse.AddSeconds(timeWindowInSeconds) && NumberOfRequests == maxRequests;

    public void IncreaseRequests(int maxRequests)
    {
      LastResponse = DateTime.UtcNow;

      if (NumberOfRequests == maxRequests)
        NumberOfRequests = 1;

      else
        NumberOfRequests++;
    }
  }
}
