# BedrockLauncher
Launcher para Minecraft Bedrock Universal Compatible con Sistemas Operativos Customs y Windows como Windows 10 LTSC

## 🎮 Launcher universal para Minecraft en sistemas
**Autor:** AlbertDevX  
**Fecha:** Mayo 2026  
**Versión del documento:** 1.0

---

## 📌 Resumen ejecutivo

Este proyecto presenta una solución definitiva al problema de autenticación de Xbox en sistemas **Customs** como tambien **Windows 10/11 LTSC modificados**, donde la ventana de inicio de sesión de Microsoft se abre y se cierra automáticamente sin permitir al usuario identificarse. O diferentes tipos de cosas como correccion de inicio de sesiones corruptas, o etc...

La solución propuesta es la creación de un **launcher autónomo en C#** que utiliza **Device Code Flow**, un método de autenticación que no requiere ventanas emergentes ni el componente `WebView2` (roto en estos sistemas).

**Autor:** AlbertDevX  
**Contacto:** [GitHub / Discord del autor]

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

Diferentes tipos de distribuciones eliminan o deshabilitan componentes críticos como:
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

### 2️⃣ Lenguaje y tecnologías

| Componente | Elección | Motivo |
|------------|----------|--------|
| **Lenguaje** | C# | Nativo de Windows, sin dependencias externas |
| **Framework** | .NET 8 | LTS, soporte prolongado, alto rendimiento |
| **UI** | WPF | Diseños profesionales, separación lógica/visual |
| **Autenticación** | `CmlLib.Core.Auth.Microsoft` | Librería probada para Minecraft |
| **Compilación** | Self-contained single-file | Un solo .exe, no requiere runtime instalado |
