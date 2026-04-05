using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Infrastructure.Integration.Cep;
using SistemaVendas.Infrastructure.Persistence;
using SistemaVendas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// registra o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IProdutoService, ProdutoService>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<IPessoaService, PessoaService>();

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

builder.Services.AddScoped<IVendaService, VendaService>();

builder.Services.AddScoped<IVendaRepository, VendaRepository>();

builder.Services.AddHttpClient<ICepService, ViaCepService>();

builder.Services.AddScoped<IProdutoImportService, ProdutoImportService>();

builder.Services.AddScoped<IValidator<ProdutoCriarDto>, ProdutoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
