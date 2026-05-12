# 🎮 BedrockLauncher

**Launcher universal para Minecraft Bedrock**  
Compatible con Windows 10/11 LTSC y sistemas customs (TomexOS, etc.)

**Autor:** AlbertDevX  
**Versión:** 1.0.0  
**Fecha:** Mayo 2026

---

## 🔍 ¿Qué problema resuelve?

En sistemas Windows modificados (LTSC, TomexOS, customs), la ventana de autenticación de Xbox se abre y cierra automáticamente en menos de 1 segundo, impidiendo iniciar sesión en Minecraft.

**Causa:** Estos sistemas eliminan componentes críticos como:
- Microsoft Edge WebView2 Runtime
- Xbox Identity Provider
- Gaming Services

## ✅ Solución

Este launcher utiliza **Device Code Flow**, un método de autenticación OAuth 2.0 que:
- ❌ No requiere WebView2
- ❌ No muestra ventanas emergentes
- ✅ Funciona en cualquier sistema Windows
- ✅ Solo necesita escribir un código de 8 letras

---

## 🚀 Cómo usar

### 1. Compilar (en Windows con .NET 8 SDK)

```bash
dotnet restore
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

O ejecutar `build.bat`

### 2. Ejecutar

1. Abre `BedrockLauncher.exe`
2. Haz clic en **"🚀 Iniciar Sesión"**
3. El launcher mostrará un código (ej: `ABC12345`)
4. Abre tu navegador y ve a `https://microsoft.com/devicelogin`
5. Ingresa el código mostrado
6. Inicia sesión con tu cuenta Microsoft
7. ¡Listo! El launcher detectará la autorización automáticamente

### 3. Lanzar Minecraft

Después de autenticarte:
1. Selecciona la versión (por defecto: `latest`)
2. Haz clic en **"▶️ Lanzar Minecraft Bedrock"**

---

## 🛠️ Requisitos

### Para compilar:
- .NET 8 SDK
- Windows (para compilación WPF)

### Para ejecutar:
- Windows 10/11 (cualquier edición)
- .NET 8 Runtime (incluido en el ejecutable standalone)
- Minecraft Bedrock Edition instalado

---

## 📁 Estructura del proyecto

```
BedrockLauncher/
├── App.xaml              # Estilos globales
├── App.xaml.cs           # Código de aplicación
├── MainWindow.xaml       # Interfaz de usuario
├── MainWindow.xaml.cs    # Lógica de autenticación y lanzamiento
├── BedrockLauncher.csproj # Configuración del proyecto
└── build.bat             # Script de compilación
```

---

## 🔐 Seguridad

- Los tokens se almacenan temporalmente en memoria
- No se guardan credenciales en disco
- El código de autenticación expira en 15 minutos
- Conexión directa a servidores de Microsoft (sin intermediarios)

---

## ⚠️ Notas importantes

1. **Uso personal:** Este launcher es para uso personal. Para distribución masiva, registra tu aplicación en Azure Portal.

2. **Minecraft Bedrock:** Asegúrate de tener Minecraft Bedrock Edition instalado legalmente en tu sistema.

3. **Sistemas customs:** Si experimentas problemas, verifica que:
   - Tienes conexión a internet
   - Los servidores de Microsoft no están bloqueados por firewall
   - El archivo hosts no tiene redirecciones maliciosas

---

## 📞 Soporte

- **GitHub:** [AlbertDevX](https://github.com/AlbertDevX)
- **Discord:** [Servidor del autor]

---

## 📄 Licencia

Copyright © 2026 AlbertDevX. Todos los derechos reservados.

---

## 🎯 Comparativa

| Método | ¿Funciona en LTSC? | Requiere WebView2 | Dependencias |
|--------|-------------------|-------------------|--------------|
| Launcher oficial | ❌ No | ✅ Sí | Múltiples |
| WebView2 integrado | ❌ No | ✅ Sí | WebView2 Runtime |
| **BedrockLauncher** | ✅ **Sí** | ❌ **No** | **Ninguna** |

---

*Disfruta de Minecraft sin límites.*
