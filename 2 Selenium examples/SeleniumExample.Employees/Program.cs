using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeleniumExample.Users;
using SeleniumExample.Users.Context;
using SeleniumExample.Users.Dtos;
using SeleniumExample.Users.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettings = builder.Configuration.Get<AppSettings>();
builder.Services.AddDatabase(appSettings?.Database.ConnectionString);
builder.Services.AddMassTransit(appSettings?.MassTransit);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<EmployeesDbMigrator>();
    await migrator.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/employees/{id}", async (Guid id, IEmployeesDbContext dbContext) =>
    {
        var employee = await dbContext.Employees.SingleAsync(x => x.Id == id);
        return new GetEmployeeDto(employee.Id, employee.Phone, employee.UserName, employee.Email, employee.Position);
    })
    .WithName("GetEmployee")
    .WithOpenApi();

app.Run();