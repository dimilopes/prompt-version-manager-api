using PromptVersionManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar a string de conexão do SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=promptmanager.db;Version=3;";

// Registrar DatabaseConnection como serviço singleton
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Inicializar o banco de dados
var dbConnection = app.Services.GetRequiredService<DatabaseConnection>();
dbConnection.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.Run();
