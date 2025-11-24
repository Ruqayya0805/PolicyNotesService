using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Controllers;
using PolicyNotesService.Data;
using PolicyNotesService.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PolicyNotesDbContext>(options =>
    options.UseInMemoryDatabase("PolicyNotesDb"));

builder.Services.AddScoped<IPolicyNotesService, PolicyNotesService.Services.PolicyNotesService>();


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