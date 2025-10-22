# 🚀 Guía de Deployment - MillionProperty API

## 📋 Pre-requisitos

1. Cuenta en MongoDB Atlas (gratuita)
2. Cuenta en Render o Fly.io (gratuitas)
3. Repositorio de GitHub con el código

---

## 🗄️ PASO 1: Configurar MongoDB Atlas (Obligatorio)

### 1. Crear Cluster

1. Ve a [MongoDB Atlas](https://cloud.mongodb.com)
2. Click en **Build a Database**
3. Selecciona **FREE** (M0 Sandbox)
4. Elige región cercana (ej: `us-east-1` o `São Paulo`)
5. Click **Create**

### 2. Configurar Acceso

#### **Network Access:**
1. Ve a **Network Access** en el menú lateral
2. Click **Add IP Address**
3. Click **Allow Access from Anywhere** (0.0.0.0/0)
4. Confirma

#### **Database Access:**
1. Ve a **Database Access**
2. Click **Add New Database User**
3. Crea usuario y contraseña (¡guárdalos!)
4. Rol: **Read and write to any database**
5. Click **Add User**

### 3. Obtener Connection String

1. Ve a **Database** → **Connect**
2. Selecciona **Connect your application**
3. Driver: **C# / .NET** versión **2.13 or later**
4. Copia el connection string:

```
mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/?retryWrites=true&w=majority
```

5. Reemplaza `<usuario>` y `<password>` con tus credenciales

**Ejemplo:**
```
mongodb+srv://yeison:MiPassword123@cluster0.abc123.mongodb.net/?retryWrites=true&w=majority
```

### 4. Importar Data Dummy

Usa MongoDB Compass o mongoimport:

```bash
# Opción 1: MongoDB Compass
# 1. Conecta a tu cluster de Atlas
# 2. Crea database "MillionPropertyDB"
# 3. Para cada colección, importa el JSON correspondiente

# Opción 2: mongoimport (Terminal)
mongoimport --uri "mongodb+srv://usuario:password@cluster0.xxxxx.mongodb.net/MillionPropertyDB" \
  --collection owners --file backup/owners.json --jsonArray

mongoimport --uri "mongodb+srv://usuario:password@cluster0.xxxxx.mongodb.net/MillionPropertyDB" \
  --collection properties --file backup/properties.json --jsonArray

mongoimport --uri "mongodb+srv://usuario:password@cluster0.xxxxx.mongodb.net/MillionPropertyDB" \
  --collection propertyImages --file backup/propertyimages.json --jsonArray

mongoimport --uri "mongodb+srv://usuario:password@cluster0.xxxxx.mongodb.net/MillionPropertyDB" \
  --collection propertyTraces --file backup/propertytraces.json --jsonArray
```

---

## 🎨 PASO 2: Deployment en Render (Opción 1 - Recomendada)

### ✅ **Ventajas:**
- 100% Gratuito (750 horas/mes)
- Soporta .NET 9
- SSL automático
- Deploy desde GitHub
- Sin límites de requests

### **Pasos:**

1. **Subir cambios a GitHub:**

```bash
git add .
git commit -m "feat: Configurar deployment para Render"
git push origin main
```

2. **Crear cuenta en Render:**
   - Ve a [render.com](https://render.com)
   - Regístrate con GitHub

3. **Crear Web Service:**
   - Click **New +** → **Web Service**
   - Conecta tu repositorio `MillionPropertyApi`
   - Configuración:
     - **Name:** `millionproperty-api`
     - **Region:** `Oregon (US West)` o el más cercano
     - **Branch:** `main`
     - **Root Directory:** (dejar vacío)
     - **Runtime:** `Docker`
     - **Instance Type:** `Free`

4. **Configurar Variables de Entorno:**

   En **Environment**, agrega:

   | Variable | Valor |
   |----------|-------|
   | `MONGODB_CONNECTION_STRING` | `mongodb+srv://usuario:password@cluster0...` |
   | `MONGODB_DATABASE_NAME` | `MillionPropertyDB` |
   | `ASPNETCORE_ENVIRONMENT` | `Production` |

5. **Deploy:**
   - Click **Create Web Service**
   - Espera 5-10 minutos mientras se construye

6. **Obtener URL:**
   - Render te dará una URL: `https://millionproperty-api.onrender.com`
   - GraphQL Playground: `https://millionproperty-api.onrender.com/graphql`

### ⚠️ **Nota sobre Render Free:**
- El servicio se "duerme" después de 15 minutos sin uso
- La primera request después de dormir tarda ~30 segundos
- Solución: Usa un servicio de "ping" como [UptimeRobot](https://uptimerobot.com)

---

## 🪰 PASO 2: Deployment en Fly.io (Opción 2 - Alternativa)

### ✅ **Ventajas:**
- 100% Gratuito (3 máquinas pequeñas)
- NO se duerme (siempre activo)
- Más rápido que Render
- SSL automático

### **Pasos:**

1. **Instalar Fly CLI:**

```bash
# macOS
brew install flyctl

# Windows
powershell -Command "iwr https://fly.io/install.ps1 -useb | iex"

# Linux
curl -L https://fly.io/install.sh | sh
```

2. **Login:**

```bash
fly auth login
```

3. **Crear app:**

```bash
cd /Users/yeisongomez/WorkCode/MillionProject/MillionPropertyApi
fly launch
```

Responde:
- **App name:** `millionproperty-api` (o el que quieras)
- **Region:** Elige el más cercano
- **Would you like to set up a PostgreSQL database?** NO
- **Would you like to deploy now?** NO

4. **Configurar Variables de Entorno:**

```bash
fly secrets set MONGODB_CONNECTION_STRING="mongodb+srv://usuario:password@cluster0..."
fly secrets set MONGODB_DATABASE_NAME="MillionPropertyDB"
fly secrets set ASPNETCORE_ENVIRONMENT="Production"
```

5. **Deploy:**

```bash
fly deploy
```

6. **Obtener URL:**

```bash
fly status
```

Tu URL será: `https://millionproperty-api.fly.dev/graphql`

---

## 🐳 PASO 2: Deployment en Azure (Opción 3 - Si tienes créditos)

### ✅ **Ventajas:**
- Plataforma nativa de .NET
- $200 USD gratis por 30 días (estudiantes)
- Mejor rendimiento

### **Pasos:**

1. **Instalar Azure CLI:**

```bash
# macOS
brew install azure-cli

# Windows
winget install Microsoft.AzureCLI
```

2. **Login:**

```bash
az login
```

3. **Crear App Service:**

```bash
az webapp up \
  --name millionproperty-api \
  --runtime "DOTNET|9.0" \
  --sku F1 \
  --location eastus
```

4. **Configurar Variables:**

```bash
az webapp config appsettings set \
  --name millionproperty-api \
  --settings \
    MONGODB_CONNECTION_STRING="mongodb+srv://..." \
    MONGODB_DATABASE_NAME="MillionPropertyDB"
```

5. **URL:** `https://millionproperty-api.azurewebsites.net/graphql`

---

## ✅ Verificar que Funciona

### Opción 1: GraphQL Playground

Abre en tu navegador:
```
https://tu-url/graphql
```

Ejecuta esta query:
```graphql
query {
  properties(page: 1, pageSize: 5) {
    items {
      name
      price
      firstImage
    }
    totalCount
  }
}
```

### Opción 2: Postman

1. Importa `PostmanCollection.json`
2. Actualiza la variable `base_url` con tu URL de producción
3. Prueba cualquier query

---

## 🔧 Troubleshooting

### Error: "MongoDB connection failed"

**Solución:**
- Verifica que la IP `0.0.0.0/0` esté permitida en Atlas
- Revisa que el usuario y contraseña sean correctos
- Asegúrate de haber reemplazado `<usuario>` y `<password>` en el connection string
- Verifica que la variable de entorno esté configurada correctamente

### Error: "Application failed to start" (Render)

**Solución:**
- Ve a **Logs** en Render
- Verifica que las variables de entorno estén configuradas
- Asegúrate de que el `Dockerfile` esté en el repositorio

### Error: "Port binding failed"

**Solución:**
- Verifica que `Program.cs` no tenga puertos hardcodeados
- Render y Fly.io asignan el puerto automáticamente vía variable `$PORT`

### Render se "duerme"

**Solución:**
- Usa [UptimeRobot](https://uptimerobot.com) para hacer ping cada 5 minutos
- O cambia a Fly.io que NO se duerme

---

## 🔄 Redeploy Automático

### Render:
- Automático cuando haces `git push` a `main`

### Fly.io:
```bash
fly deploy
```

### Azure:
```bash
az webapp up --name millionproperty-api
```

---

## 🌐 Dominio Custom (Opcional)

### Render:
1. Settings → Custom Domains
2. Agrega tu dominio
3. Configura CNAME en tu DNS

### Fly.io:
```bash
fly certs add tudominio.com
```

---

## 💰 Comparación de Opciones

| Plataforma | Costo | Sleep | Build Time | Recomendado |
|------------|-------|-------|------------|-------------|
| **Render** | Gratis | Sí (15 min) | 5-8 min | ⭐⭐⭐⭐ |
| **Fly.io** | Gratis | No | 3-5 min | ⭐⭐⭐⭐⭐ |
| **Azure** | $200 crédito | No | 2-4 min | ⭐⭐⭐ |
| **Railway** | $5/mes | No | 3-5 min | ❌ (Pago) |

---

## 🎯 Mi Recomendación

**Para producción:** Fly.io (no se duerme, siempre rápido)  
**Para demos/portfolio:** Render (más fácil de configurar)

---

## 📝 Checklist de Deployment

- [ ] MongoDB Atlas configurado
- [ ] Network Access permite 0.0.0.0/0
- [ ] Usuario de base de datos creado
- [ ] Data dummy importada
- [ ] Connection string copiado
- [ ] Cambios subidos a GitHub
- [ ] Plataforma elegida (Render/Fly.io)
- [ ] Variables de entorno configuradas
- [ ] Deployment exitoso
- [ ] GraphQL Playground funciona
- [ ] Queries de prueba pasan

---

**¡Listo para producción! 🚀**
