using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Infrastructure.Integration.Cep;
using SistemaVendas.Infrastructure.Persistence;
using SistemaVendas.Infrastructure.Repositories;
using SistemaVendas.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SistemaVendas";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SistemaVendasClient";
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "SistemaVendas-Secret-Key-Dev-1234567890";

// registra o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IProdutoService, ProdutoService>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddScoped<IPessoaService, PessoaService>();

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

builder.Services.AddScoped<IVendaService, VendaService>();

builder.Services.AddScoped<IVendaRepository, VendaRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();

builder.Services.AddScoped<ITokenService, JwtTokenService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
