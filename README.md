# 🏢 MillionProperty API

API GraphQL para gestión de propiedades inmobiliarias desarrollada con .NET 9, MongoDB y arquitectura modular.

---

## 🚀 Inicio Rápido

### 1️⃣ **Clonar el Proyecto**

```bash
git clone <repository-url>
cd MillionPropertyApi
```

### 2️⃣ **Configurar MongoDB Local**

**Desarrollo Local:**
El proyecto usa `appsettings.Development.json` automáticamente. Si usas MongoDB local, asegúrate de que esté corriendo en `mongodb://localhost:27017`.

**Producción (Render, etc.):**
Configura las variables de entorno:
- `MONGODB_CONNECTION_STRING`: Connection string de MongoDB Atlas
- `MONGODB_DATABASE_NAME`: `MillionPropertyDB`

### 3️⃣ **Importar Data Dummy (Muy Importante! 🎯)**

Usa **MongoDB Compass** para importar la data de prueba:

1. Abre **MongoDB Compass**
2. Conéctate a `mongodb://localhost:27017` (o tu MongoDB Atlas)
3. Crea la base de datos `MillionPropertyDB`
4. Para cada colección, importa los archivos JSON:

| Colección | Archivo | Registros |
|-----------|---------|-----------|
| `owners` | `backup/owners.json` | 8 owners |
| `properties` | `backup/properties.json` | 20 propiedades |
| `propertyImages` | `backup/propertyimages.json` | 53 imágenes |
| `propertyTraces` | `backup/propertytraces.json` | 21 traces |

**Pasos en Compass:**
- Click en la colección → **ADD DATA** → **Import JSON or CSV file**
- Selecciona el archivo correspondiente → **Import**

### 4️⃣ **Ejecutar el Proyecto**

```bash
dotnet restore
dotnet run
```

El API estará disponible en:
- **HTTP:** `http://localhost:5189/graphql`
- **HTTPS:** `https://localhost:7287/graphql`

**Nota:** Por defecto, el proyecto usa HTTP en el puerto `5189`. Si quieres usar HTTPS, ejecuta:
```bash
dotnet run --launch-profile https
```

### 5️⃣ **Importar Postman Collection**

1. Abre **Postman**
2. Click en **Import**
3. Selecciona el archivo `PostmanCollection.json`
4. ¡Listo! Ahora puedes probar todos los endpoints

**Variables importantes:**
- `base_url`: `http://localhost:5189` (o `https://localhost:7287` si usas HTTPS)
- `owner_id`: Copia un ID después de crear o consultar un Owner
- `property_id`: Copia un ID después de crear o consultar una Property

---

## 📁 Arquitectura Modular

El proyecto usa una **arquitectura modular** donde cada funcionalidad está separada en módulos independientes:

```
MillionPropertyApi/
├── Modules/
│   ├── Properties/          # Gestión de propiedades
│   │   ├── Models/          # Property.cs
│   │   ├── DTOs/            # PropertyDto, PropertyFilterDto, etc.
│   │   ├── Services/        # Property.cs, PropertyImage.cs
│   │   ├── Interfaces/      # IPropertyService, IPropertyImageService
│   │   ├── GraphQL/         # PropertyQuery, PropertyMutation
│   │   └── PropertiesModule.cs
│   │
│   ├── Owners/              # Gestión de propietarios
│   │   ├── Models/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   ├── Interfaces/
│   │   ├── GraphQL/
│   │   └── OwnersModule.cs
│   │
│   ├── PropertyImages/      # Gestión de imágenes
│   │   └── ...
│   │
│   └── PropertyTraces/      # Historial de transacciones
│       └── ...
│
├── Shared/                  # Servicios compartidos
│   ├── Services/
│   │   ├── IDatabaseService.cs
│   │   └── DatabaseService.cs
│   ├── Interfaces/
│   │   └── IModule.cs
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs
│
└── Program.cs               # Configuración del API
```

### **Ventajas de esta arquitectura:**

✅ **Separación de responsabilidades:** Cada módulo maneja su propia lógica  
✅ **Escalabilidad:** Fácil agregar nuevos módulos sin afectar los existentes  
✅ **Mantenibilidad:** Código organizado y fácil de encontrar  
✅ **Reutilización:** Servicios compartidos en la carpeta `Shared/`

---

## 🔍 Servicios Disponibles

### **Properties Module**

#### **PropertyService** (`Property.cs`)
Gestiona las operaciones CRUD de propiedades.

**Métodos:**
- `GetAllAsync()` - Lista propiedades con filtros y paginación
- `GetByIdAsync()` - Obtiene una propiedad por ID
- `GetByOwnerIdAsync()` - Lista propiedades de un dueño
- `CreateAsync()` - Crea una nueva propiedad

#### **PropertyImageService** (`PropertyImage.cs`)
Gestiona las imágenes de las propiedades.

**Métodos:**
- `GetFirstImageAsync()` - Obtiene la primera imagen de una propiedad
- `GetAllImagesAsync()` - Obtiene todas las imágenes de una propiedad

---

### **Owners Module**

#### **OwnerService**
Gestiona los propietarios.

**Métodos:**
- `GetAllAsync()` - Lista todos los propietarios
- `GetByIdAsync()` - Obtiene un propietario por ID
- `CreateAsync()` - Crea un nuevo propietario

---

### **PropertyImages Module**

#### **PropertyImageService**
Gestiona el catálogo completo de imágenes.

**Métodos:**
- `GetAllAsync()` - Lista todas las imágenes
- `GetByIdAsync()` - Obtiene una imagen por ID
- `GetByPropertyIdAsync()` - Lista imágenes de una propiedad específica
- `CreateAsync()` - Crea una nueva imagen

---

### **PropertyTraces Module**

#### **PropertyTraceService**
Gestiona el historial de transacciones/ventas de las propiedades.

**Métodos:**
- `GetAllAsync()` - Lista todos los traces
- `GetByIdAsync()` - Obtiene un trace por ID
- `GetByPropertyIdAsync()` - Lista traces de una propiedad específica
- `CreateAsync()` - Crea un nuevo trace

---

## 🎯 Filtros y Búsquedas en Properties

### **GetAllAsync() - Explicación Simple**

El método `GetAllAsync()` permite buscar y filtrar propiedades de manera muy flexible:

```csharp
public async Task<(IEnumerable<Property> properties, int totalCount)> GetAllAsync(PropertyFilterDto? filter = null)
```

#### **Parámetros de Filtrado:**

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `query` | string | Busca en **nombre** O **dirección** (case insensitive) |
| `minPrice` | decimal? | Precio mínimo |
| `maxPrice` | decimal? | Precio máximo |
| `page` | int? | Número de página (default: 1) |
| `pageSize` | int? | Registros por página (default: 10) |

#### **Ejemplos de Uso:**

**1. Buscar por nombre o dirección:**
```graphql
query {
  properties(query: "Casa") {
    items {
      name
      address
      price
    }
  }
}
```
☝️ Esto busca "Casa" tanto en el **nombre** como en la **dirección**

**2. Buscar por rango de precio:**
```graphql
query {
  properties(minPrice: 400000000, maxPrice: 800000000) {
    items {
      name
      price
    }
  }
}
```
☝️ Encuentra propiedades entre $400M y $800M

**3. Combinar filtros:**
```graphql
query {
  properties(
    query: "Bogotá"
    minPrice: 300000000
    maxPrice: 600000000
    page: 1
    pageSize: 5
  ) {
    items {
      name
      address
      price
    }
    totalCount
    totalPages
  }
}
```
☝️ Busca "Bogotá" con precio entre $300M-$600M, mostrando 5 resultados por página


## 🗂️ Estructura de Datos

### **Property (Propiedad)**
```json
{
  "idProperty": "65b1c2d3e4f5a6b7c8d9e0f1",
  "name": "Apartamento Moderno en Chapinero",
  "address": "Carrera 7 #63-44, Bogotá",
  "price": 450000000,
  "codeInternal": "PROP-001",
  "year": 2020,
  "idOwner": "65a1b2c3d4e5f6a7b8c9d0e1"
}
```

### **Owner (Propietario)**
```json
{
  "idOwner": "65a1b2c3d4e5f6a7b8c9d0e1",
  "name": "Juan Carlos Rodríguez",
  "address": "Calle 72 #10-34, Bogotá",
  "photo": "https://images.unsplash.com/...",
  "birthday": "1978-03-15T00:00:00Z"
}
```

### **PropertyImage (Imagen)**
```json
{
  "idPropertyImage": "65c1d2e3f4a5b6c7d8e9f001",
  "idProperty": "65b1c2d3e4f5a6b7c8d9e0f1",
  "file": "https://images.unsplash.com/...",
  "enabled": true
}
```

### **PropertyTrace (Historial)**
```json
{
  "idPropertyTrace": "65d1e2f3a4b5c6d7e8f9a001",
  "dateSale": "2019-03-15T00:00:00Z",
  "name": "Venta a Carlos Méndez",
  "value": 380000000,
  "tax": 7600000,
  "idProperty": "65b1c2d3e4f5a6b7c8d9e0f1"
}
```

---

## 🔧 Tecnologías Utilizadas

- **.NET 9** - Framework principal
- **MongoDB** - Base de datos NoSQL
- **HotChocolate** - GraphQL para .NET
- **MongoDB.Driver** - Driver oficial de MongoDB

---

## 🎨 Datos de Prueba

El backup incluye:

- **8 Owners** con fotos de Unsplash
- **20 Properties** en Bogotá, Medellín, Chía, La Calera
- **53 PropertyImages** (URLs reales de Unsplash)
  - 1 propiedad sin imágenes
  - Algunas con 1-4 imágenes
- **21 PropertyTraces** (historial de ventas desde 2016-2023)
  - 12 propiedades con historial
  - 8 propiedades sin historial

---

## 🐛 Troubleshooting

### **Error: MongoDB Connection**
```
No se puede conectar a MongoDB
```
**Solución:**
- Verifica que MongoDB esté corriendo: `brew services list` (macOS)
- Revisa la connection string en `appsettings.json`

### **Error: Database not found**
```
Database MillionPropertyDB not found
```
**Solución:**
- Asegúrate de haber importado el backup (Paso 3)
- Verifica el nombre de la base de datos en `appsettings.json`

### **Error: Port already in use**
```
Port 7257 is already in use
```
**Solución:**
- Cambia el puerto en `Properties/launchSettings.json`
- O detén la aplicación que está usando el puerto

---


**¡Listo para usar! 🚀**

