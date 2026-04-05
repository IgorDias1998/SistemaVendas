using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Infrastructure.Integration.Cep;
using SistemaVendas.Infrastructure.Persistence;
using SistemaVendas.Infrastructure.Repositories;
using SistemaVendas.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SistemaVendas API",
        Version = "v1",
        Description = "API de estudo para produtos, clientes, pedidos, deliveries e rotas."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SistemaVendas";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SistemaVendasClient";
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "SistemaVendas-Secret-Key-Dev-1234567890";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

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

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IRotaRepository, RotaRepository>();
builder.Services.AddScoped<IRotaService, RotaService>();
builder.Services.AddScoped<ILogMudancaRotaRepository, LogMudancaRotaRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddHttpClient<ICepService, ViaCepService>();
builder.Services.AddScoped<IProdutoImportService, ProdutoImportService>();

builder.Services.AddScoped<IValidator<ProdutoCriarDto>, ProdutoValidator>();
builder.Services.AddScoped<IValidator<ClienteCreateDto>, ClienteCreateValidator>();
builder.Services.AddScoped<IValidator<AuthLoginDto>, AuthLoginValidator>();
builder.Services.AddScoped<IValidator<UsuarioCriarDto>, UsuarioCriarValidator>();
builder.Services.AddScoped<IValidator<PedidoCriarDto>, PedidoCriarValidator>();
builder.Services.AddScoped<IValidator<RotaCriarDto>, RotaCriarValidator>();
builder.Services.AddScoped<IValidator<RotaReordenarParadasDto>, RotaReordenarParadasValidator>();
builder.Services.AddScoped<IValidator<DeliveryAtualizarStatusDto>, DeliveryAtualizarStatusValidator>();

var app = builder.Build();

app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Erro de validacao"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Erro de validacao"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Operacao invalida"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso nao encontrado"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Nao autorizado"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno")
        };

        var errors = exception is ValidationException validationException
            ? validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).Distinct().ToArray())
            : null;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            title,
            detail = exception?.Message,
            status = statusCode,
            errors
        });
    });
});

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
