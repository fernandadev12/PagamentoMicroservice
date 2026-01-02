using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Repositories;
using PaymentService.Infra.Data;
using PaymentService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuração do EF Core com SQL Server
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// 🔹 MediatR (CQRS)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(PaymentService.Application.Payments.GetPaymentsHandler).Assembly));

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
var app = builder.Build();

// 🔹 Middleware de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 HTTPS redirection
app.UseHttpsRedirection();

// 🔹 Authorization (se precisar)
app.UseAuthorization();

// 🔹 Map Controllers
app.MapControllers();

app.Run();