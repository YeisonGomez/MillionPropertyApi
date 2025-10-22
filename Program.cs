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

var connectionString = builder.Configuration.GetConnectionString("MongoDB");
var databaseName = builder.Configuration["DatabaseName"];

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(connectionString);
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAll");

// Mapear GraphQL
app.MapGraphQL();

app.Run();
