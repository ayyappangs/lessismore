using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmploeeDb>(opt => opt.UseInMemoryDatabase("Employees"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/Employees", async (EmploeeDb emploeeDb) =>
{
    var employees = await emploeeDb.Employees.ToListAsync();
    return  Results.Ok(employees);
});

app.MapPost("/AddEmployee", async (Employee employee, EmploeeDb emploeeDb) =>
{
    emploeeDb.Employees.Add(employee);
    await emploeeDb.SaveChangesAsync();

    return Results.Ok(employee);
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

class EmploeeDb : DbContext
{
    public EmploeeDb(DbContextOptions<EmploeeDb> options)
    : base(options)
    {
       
    }

    public DbSet<Employee> Employees => Set<Employee>();

}

class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Title { get; set; }
}