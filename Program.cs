using Microsoft.EntityFrameworkCore;
using PolicyNotesServiceApi.Controllers;
using PolicyNotesServiceApi.Data;
using PolicyNotesServiceApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PolicyNotesDbContext>(options =>
    options.UseInMemoryDatabase("PolicyNotesDb"));

builder.Services.AddScoped<IPolicyNotesService, PolicyNotesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPolicyNotesEndpoints();

app.Run();


public partial class Program { }