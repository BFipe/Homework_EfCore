using Homework_EfCore.Database;
using Homework_EfCore.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEfCoreDataManagement(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1");
});

app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<ErrorThrowerMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();