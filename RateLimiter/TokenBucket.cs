using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter
{
  public class TokenBucket
  {
    private BlockingCollection<Token> _tokens;
    private System.Timers.Timer _timer;
    private int _maxTokens;

    public TokenBucket(int maxNumberOfTokens, int refillRateInMilliseconds)
    {
      _maxTokens = maxNumberOfTokens;
      _timer = new System.Timers.Timer(refillRateInMilliseconds);
      _tokens = new BlockingCollection<Token>(maxNumberOfTokens);
      Init(maxNumberOfTokens);
    }

    private void Init(int maxNumberOfTokens)
    {
      foreach (var _ in Enumerable.Range(0, maxNumberOfTokens))
      {
        _tokens.Add(new Token());
      }
      _timer.AutoReset = true;
      _timer.Enabled = true;
      _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
      foreach (var _ in Enumerable.Range(0, _maxTokens - _tokens.Count))
      {
        _tokens.Add(new Token());
      }
    }

    public void UseToken()
    {
      if (!_tokens.TryTake(out Token _))
      {
        throw new Exception("No tokens are available");
      }
    }
  }

  public record Token;
}
