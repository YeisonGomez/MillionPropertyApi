# 🚀 Guía de Deployment en Railway

## 📋 Pre-requisitos

1. Cuenta en [Railway.app](https://railway.app)
2. Cuenta en [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
3. Repositorio de GitHub con el código

---

## 🗄️ Configurar MongoDB Atlas

### 1. Crear Cluster (si no lo tienes)

1. Ve a [MongoDB Atlas](https://cloud.mongodb.com)
2. Click en **Build a Database**
3. Selecciona **FREE** (M0 Sandbox)
4. Elige región cercana (ej: `us-east-1`)
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
3. Crea usuario y contraseña (guárdalos!)
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

### 4. Importar Data Dummy

Usa MongoDB Compass o la CLI:

```bash
mongoimport --uri "mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/MillionPropertyDB" --collection owners --file backup/owners.json --jsonArray

mongoimport --uri "mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/MillionPropertyDB" --collection properties --file backup/properties.json --jsonArray

mongoimport --uri "mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/MillionPropertyDB" --collection propertyImages --file backup/propertyimages.json --jsonArray

mongoimport --uri "mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/MillionPropertyDB" --collection propertyTraces --file backup/propertytraces.json --jsonArray
```

---

## 🚂 Deployment en Railway

### 1. Crear Proyecto en Railway

1. Ve a [railway.app](https://railway.app)
2. Click **Start a New Project**
3. Selecciona **Deploy from GitHub repo**
4. Autoriza Railway para acceder a tu GitHub
5. Selecciona el repositorio `MillionPropertyApi`

### 2. Configurar Variables de Entorno

Una vez creado el proyecto:

1. Click en tu proyecto
2. Ve a la pestaña **Variables**
3. Click **New Variable** y agrega:

| Variable | Valor |
|----------|-------|
| `MONGODB_CONNECTION_STRING` | `mongodb+srv://<usuario>:<password>@cluster0.xxxxx.mongodb.net/?retryWrites=true&w=majority` |
| `MONGODB_DATABASE_NAME` | `MillionPropertyDB` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

4. Click **Deploy** (o espera el auto-deploy)

### 3. Verificar Deployment

1. Espera a que termine el build (2-5 minutos)
2. Railway generará una URL automática: `https://millionpropertyapi-production.up.railway.app`
3. Click en **Settings** → **Generate Domain** si no se generó automáticamente
4. Prueba tu GraphQL Playground: `https://tu-url.railway.app/graphql`

---

## ✅ Verificar que Funciona

### Opción 1: GraphQL Playground

Abre en tu navegador:
```
https://tu-url.railway.app/graphql
```

Ejecuta esta query:
```graphql
query {
  properties(page: 1, pageSize: 5) {
    items {
      name
      price
    }
    totalCount
  }
}
```

### Opción 2: Postman

1. Importa `PostmanCollection.json`
2. Actualiza la variable `base_url` con tu URL de Railway
3. Prueba cualquier query

---

## 🔧 Troubleshooting

### Error: "MongoDB connection failed"

**Solución:**
- Verifica que la IP `0.0.0.0/0` esté permitida en Atlas
- Revisa que el usuario y contraseña sean correctos
- Asegúrate de haber reemplazado `<usuario>` y `<password>` en el connection string

### Error: "Application failed to start"

**Solución:**
- Verifica los logs en Railway (tab **Deployments**)
- Asegúrate de que las variables de entorno estén configuradas
- Verifica que el archivo `nixpacks.toml` esté en el repositorio

### Error: "Port already in use"

**Solución:**
- Railway asigna el puerto automáticamente vía variable `PORT`
- No necesitas configurar nada, el proyecto ya está preparado

---

## 🔄 Redeploy Automático

Railway hace redeploy automático cuando:
- Haces `git push` a la rama `main`
- Cambias variables de entorno

Para forzar redeploy:
1. Ve a **Deployments**
2. Click en los 3 puntos del último deploy
3. Click **Redeploy**

---

## 📊 Monitoreo

Railway provee:
- **Logs en tiempo real**: Tab **Deployments** → Click en el deployment activo
- **Métricas**: CPU, RAM, Network
- **Domains**: Administra tus dominios custom

---

## 🌐 Dominio Custom (Opcional)

1. Ve a **Settings** → **Domains**
2. Click **Custom Domain**
3. Agrega tu dominio: `api.tudominio.com`
4. Configura un registro `CNAME` en tu DNS apuntando a Railway

---

## 💰 Costos

Railway ofrece:
- **$5 USD gratis/mes** para todos los usuarios
- Después de eso: **$0.000463 por GB-hora**

Para este proyecto pequeño, probablemente te mantengas en el tier gratuito.

---

## 🔐 Seguridad

✅ Las variables de entorno están encriptadas en Railway  
✅ No subas `.env` a GitHub  
✅ Usa diferentes credenciales para desarrollo y producción  
✅ Habilita autenticación en MongoDB Atlas  

---

## 📝 Checklist Final

- [ ] MongoDB Atlas configurado
- [ ] Network Access permite 0.0.0.0/0
- [ ] Usuario de base de datos creado
- [ ] Data dummy importada
- [ ] Connection string copiado
- [ ] Proyecto creado en Railway
- [ ] Variables de entorno configuradas
- [ ] Deployment exitoso
- [ ] GraphQL Playground funciona
- [ ] Queries de prueba pasan

---

**¡Listo para producción! 🚀**

