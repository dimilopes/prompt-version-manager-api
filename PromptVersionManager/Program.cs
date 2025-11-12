using PromptVersionManager.Data;
using PromptVersionManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar a string de conexão do SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=promptmanager.db;Version=3;";

// Registrar DatabaseConnection como serviço singleton
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

// Registrar repositório e serviço
builder.Services.AddScoped<IPromptRepository, PromptRepository>();
builder.Services.AddScoped<IPromptService, PromptService>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Prompt Version Manager API",
        Version = "v1",
        Description = "API para gerenciar e versionar prompts de IA",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Manus AI",
            Email = "support@manus.im"
        }
    });
});

// Adicionar CORS para permitir requisições de diferentes origens
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adicionar controladores
builder.Services.AddControllers();

var app = builder.Build();

// Inicializar o banco de dados
var dbConnection = app.Services.GetRequiredService<DatabaseConnection>();
dbConnection.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prompt Version Manager API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
