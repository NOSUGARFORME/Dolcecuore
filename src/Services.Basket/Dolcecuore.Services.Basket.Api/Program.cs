using System;
using Dolcecuore.Application;
using Dolcecuore.Services.Basket;
using Dolcecuore.Services.Basket.ConfigurationOptions;
using Dolcecuore.Services.Discount.Grpc.Protos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddApplicationServices();
builder.Services.AddBasketModule(appSettings);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dolcecuore.Services.Basket.Api", Version = "v1" });
});

var app = builder.Build();

Policy.Handle<Exception>().WaitAndRetry(new[]
    {
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(20),
        TimeSpan.FromSeconds(30),
    })
    .Execute(() =>
    {
        app.MigrateBasketEventDb();
    });

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dolcecuore.Services.Basket.Api v1"));
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

