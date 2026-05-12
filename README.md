# BedrockLauncher
Launcher para Minecraft Bedrock Universal Compatible con Sistemas Operativos Customs y Windows como Windows 10 LTSC

-# # 🎮 Launcher universal para Minecraft en sistemas
**Autor:** AlbertDevX  
**Fecha:** Mayo 2026  
**Versión del documento:** 1.0

---

## 📌 Resumen ejecutivo

Este documento presenta una solución definitiva al problema de autenticación de Xbox en sistemas **TomexOS** y **Windows 10/11 LTSC modificados**, donde la ventana de inicio de sesión de Microsoft se abre y se cierra automáticamente sin permitir al usuario identificarse.

La solución propuesta es la creación de un **launcher autónomo en C#** que utiliza **Device Code Flow**, un método de autenticación que no requiere ventanas emergentes ni el componente `WebView2` (roto en estos sistemas).

**Autor:** AlbertDevX  
**Contacto:** [GitHub / Discord / X (Twitter) del autor]

---

## 🔍 Diagnóstico del problema

| Prueba realizada | Resultado obtenido | Conclusión |
|------------------|-------------------|------------|
| `nslookup account.live.com` | ✅ Resolución correcta | DNS funcional |
| `ping account.live.com` | ✅ Respuesta exitosa | Conectividad de red OK |
| `curl -v https://account.live.com` | ✅ HTTP 301 (redirección) | Servidores de Microsoft accesibles |
| Archivo `hosts` | ✅ Sin bloqueos | No hay redirecciones locales maliciosas |
| WebView2 | ❌ Ausente o corrupto | Componente roto en TomexOS |

### Causa raíz

**TomexOS** (y otras distribuciones modificadas de Windows LTSC) eliminan o deshabilitan componentes críticos como:
- `Microsoft Edge WebView2 Runtime`
- `Xbox Identity Provider`
- `Gaming Services`

Estos componentes son necesarios para que las aplicaciones modernas de Microsoft muestren ventanas de autenticación. Al faltar, la ventana de login intenta abrirse y falla silenciosamente.

---

## ✅ Solución propuesta: Launcher con Device Code Flow

En lugar de intentar reparar el sistema (proceso complejo y no siempre exitoso), se construye un **launcher autónomo** que utiliza **Device Code Flow**:

### ¿Qué es Device Code Flow?

Es un estándar de autenticación OAuth 2.0 diseñado para dispositivos con capacidades de entrada limitadas (televisores, consolas, impresoras) o entornos donde no se puede abrir un navegador integrado.

**Flujo de trabajo:**
1. El launcher solicita un código a Microsoft
2. Muestra al usuario: *"Ve a https://microsoft.com/devicelogin y escribe el código: ABC12345"*
3. El usuario inicia sesión en su navegador (PC, móvil, cualquier dispositivo)
4. El launcher detecta automáticamente la autorización y obtiene el token

**Ventajas:**
- ✅ No requiere WebView2 ni ventanas emergentes
- ✅ Funciona en cualquier sistema operativo
- ✅ Seguro (el código expira rápidamente)
- ✅ No hay que copiar URLs largas ni códigos complejos

---

## 🛠️ Implementación técnica

### 1️⃣ Registrar aplicación en Azure (gratuito, 2 minutos)

| Paso | Acción |
|------|--------|
| 1 | Ve a [Azure Portal - App registrations](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) |
| 2 | Inicia sesión con tu cuenta Microsoft |
| 3 | Haz clic en **"New registration"** |
| 4 | **Nombre:** `MiLauncherXbox` (o el que prefieras) |
| 5 | **Supported account types:** "Personal Microsoft accounts only" |
| 6 | **Redirect URI:** Déjalo vacío (no necesario para Device Code Flow) |
| 7 | Haz clic en **"Register"** |
| 8 | **Copia el "Application (client) ID"** → lo usarás en el código |
| 9 | Ve a **"Certificates & secrets"** → **"New client secret"** |
| 10 | Elige expiración (recomendado: 1 año) → **"Add"** |
| 11 | **Copia el "Value" del secreto** (solo se muestra una vez) |

### 2️⃣ Lenguaje y tecnologías

| Componente | Elección | Motivo |
|------------|----------|--------|
| **Lenguaje** | C# | Nativo de Windows, sin dependencias externas |
| **Framework** | .NET 8 | LTS, soporte prolongado, alto rendimiento |
| **UI** | WPF | Diseños profesionales, separación lógica/visual |
| **Autenticación** | `CmlLib.Core.Auth.Microsoft` | Librería probada para Minecraft |
| **Compilación** | Self-contained single-file | Un solo .exe, no requiere runtime instalado |
