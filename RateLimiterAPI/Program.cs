using Microsoft.AspNetCore.RateLimiting;
using RateLimiter;
using RateLimiter.Middleware;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton(new TokenBucket(maxNumberOfTokens: 5, refillRateInMilliseconds: 10000));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<TokenBucketMiddleware>()
  .UseMiddleware<FixedWindowMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
