using MillionPropertyApi.Shared.Services;
using MillionPropertyApi.Shared.Extensions;
using MillionPropertyApi.Modules.Owners;
using MillionPropertyApi.Modules.Owners.GraphQL;
using MillionPropertyApi.Modules.Owners.DTOs;
using MillionPropertyApi.Modules.Properties;
using MillionPropertyApi.Modules.Properties.GraphQL;
using MillionPropertyApi.Modules.Properties.DTOs;
using MillionPropertyApi.Modules.PropertyImages;
using MillionPropertyApi.Modules.PropertyImages.GraphQL;
using MillionPropertyApi.Modules.PropertyImages.DTOs;
using MillionPropertyApi.Modules.PropertyTraces;
using MillionPropertyApi.Modules.PropertyTraces.GraphQL;
using MillionPropertyApi.Modules.PropertyTraces.DTOs;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Leer configuración de MongoDB desde variables de entorno primero, luego appsettings
var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("MongoDB") 
    ?? throw new InvalidOperationException("MongoDB connection string not configured");

var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
    ?? builder.Configuration["DatabaseName"] 
    ?? "MillionPropertyDB";

// Logs de diagnóstico (sin exponer passwords)
var isFromEnvVar = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") != null;
var connectionType = connectionString.StartsWith("mongodb+srv://") ? "MongoDB Atlas" : "MongoDB Local";
var maskedConnection = connectionString.Contains("@") 
    ? $"{connectionString.Split('@')[0].Split(':')[0]}:***@{connectionString.Split('@')[1]}" 
    : "mongodb://localhost:***";

Console.WriteLine("=== MongoDB Configuration ===");
Console.WriteLine($"Source: {(isFromEnvVar ? "Environment Variable ✅" : "appsettings.json ⚠️")}");
Console.WriteLine($"Type: {connectionType}");
Console.WriteLine($"Connection: {maskedConnection}");
Console.WriteLine($"Database: {databaseName}");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine("============================");

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = MongoClientSettings.FromConnectionString(connectionString);
    
    // Configuración SSL para MongoDB Atlas
    settings.SslSettings = new MongoDB.Driver.SslSettings
    {
        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13
    };
    
    // Timeouts razonables
    settings.ServerSelectionTimeout = TimeSpan.FromSeconds(10);
    settings.ConnectTimeout = TimeSpan.FromSeconds(10);
    
    return new MongoClient(settings);
});

builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseName);
});

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddModule<OwnersModule>();
builder.Services.AddModule<PropertiesModule>();
builder.Services.AddModule<PropertyImagesModule>();
builder.Services.AddModule<PropertyTracesModule>();

// Configurar GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<PropertyQuery>()
        .AddTypeExtension<OwnerQuery>()
        .AddTypeExtension<PropertyImageQuery>()
        .AddTypeExtension<PropertyTraceQuery>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<PropertyMutation>()
        .AddTypeExtension<OwnerMutation>()
        .AddTypeExtension<PropertyImageMutation>()
        .AddTypeExtension<PropertyTraceMutation>()
    .AddType<PropertyDto>()
    .AddType<PaginatedPropertiesDto>()
    .AddType<OwnerDto>()
    .AddType<PropertyImageDto>()
    .AddType<PropertyTraceDto>();

var app = builder.Build();

// Verificar conexión a MongoDB al iniciar
try
{
    Console.WriteLine("🔍 Probando conexión a MongoDB...");
    var client = app.Services.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase(databaseName);
    
    // Hacer un ping real a la base de datos
    await database.RunCommandAsync<MongoDB.Bson.BsonDocument>(new MongoDB.Bson.BsonDocument("ping", 1));
    
    Console.WriteLine("✅ Conexión a MongoDB EXITOSA");
    Console.WriteLine($"📊 Database: {databaseName}");
    
    // Verificar colecciones existentes
    var collections = await database.ListCollectionNamesAsync();
    var collectionsList = await collections.ToListAsync();
    Console.WriteLine($"📂 Colecciones encontradas: {string.Join(", ", collectionsList)}");
}
catch (Exception ex)
{
    Console.WriteLine("❌ ERROR: No se pudo conectar a MongoDB");
    Console.WriteLine($"❌ Tipo: {ex.GetType().Name}");
    Console.WriteLine($"❌ Mensaje: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"❌ Inner Exception: {ex.InnerException.Message}");
    }
    Console.WriteLine("⚠️ El servidor continuará, pero las queries fallarán");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Solo usar HTTPS redirection en desarrollo, no en producción (Render maneja SSL)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Usar CORS
app.UseCors("AllowAll");

// Mapear GraphQL
app.MapGraphQL();

app.Run();
