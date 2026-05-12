using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CmlLib.Core.Auth.Microsoft;

namespace BedrockLauncher
{
    public partial class MainWindow : Window
    {
        private MCodeLogin? _loginHandler;
        private MCredentialResponse? _credential;
        private readonly HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            Log("BedrockLauncher v1.0 - AlbertDevX");
            Log("Iniciando aplicación...");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginButton.IsEnabled = false;
                InstructionText.Text = "Solicitando código de autenticación a Microsoft...";
                
                await Task.Run(async () =>
                {
                    _loginHandler = JELoginMicrosoft.GetHandlerForDeviceCode();
                });

                if (_loginHandler?.DeviceCodeInfo != null)
                {
                    CodeDisplay.Visibility = Visibility.Visible;
                    StatusDisplay.Visibility = Visibility.Visible;
                    
                    UserCodeText.Text = _loginHandler.DeviceCodeInfo.UserCode;
                    UrlTextBox.Text = _loginHandler.DeviceCodeInfo.VerificationUri;
                    
                    InstructionText.Text = $"1. Abre {_loginHandler.DeviceCodeInfo.VerificationUri}\n2. Ingresa el código: {_loginHandler.DeviceCodeInfo.UserCode}\n3. Inicia sesión con tu cuenta Microsoft";
                    
                    Log($"Código generado: {_loginHandler.DeviceCodeInfo.UserCode}");
                    Log("Esperando autorización del usuario...");
                    
                    await WaitForAuthorizationAsync();
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ResetLoginUI();
            }
        }

        private async Task WaitForAuthorizationAsync()
        {
            try
            {
                _credential = await _loginHandler!.LoginFromDeviceCodeAsync();
                
                Dispatcher.Invoke(() =>
                {
                    OnLoginSuccess();
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Log($"Error en autenticación: {ex.Message}");
                    MessageBox.Show($"La autenticación falló o expiró: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetLoginUI();
                });
            }
        }

        private void OnLoginSuccess()
        {
            if (_credential == null) return;

            CodeDisplay.Visibility = Visibility.Collapsed;
            StatusDisplay.Visibility = Visibility.Collapsed;
            UserInfoBorder.Visibility = Visibility.Visible;
            LaunchSection.Visibility = Visibility.Visible;
            
            UserNameText.Text = $"Usuario: {_credential.Profile.Name}";
            XuidText.Text = $"XUID: {_credential.Profile.Id}";
            
            InstructionText.Text = "✅ Autenticación completada exitosamente";
            LoginButton.Content = "🔄 Renovar Sesión";
            LoginButton.IsEnabled = true;
            
            Log($"Inicio de sesión exitoso: {_credential.Profile.Name}");
            Log($"Token obtenido correctamente");
        }

        private void ResetLoginUI()
        {
            CodeDisplay.Visibility = Visibility.Collapsed;
            StatusDisplay.Visibility = Visibility.Collapsed;
            UserInfoBorder.Visibility = Visibility.Collapsed;
            LaunchSection.Visibility = Visibility.Collapsed;
            InstructionText.Text = "Haz clic en 'Iniciar Sesión' para comenzar";
            LoginButton.Content = "🚀 Iniciar Sesión";
            LoginButton.IsEnabled = true;
        }

        private void CopyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UserCodeText.Text))
            {
                Clipboard.SetText(UserCodeText.Text);
                Log("Código copiado al portapapeles");
                MessageBox.Show("Código copiado al portapapeles", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (_credential == null)
            {
                MessageBox.Show("Debes iniciar sesión primero", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string version = VersionTextBox.Text.Trim();
                if (string.IsNullOrEmpty(version))
                    version = "latest";

                Log($"Preparando para lanzar Minecraft Bedrock versión {version}...");
                LaunchButton.IsEnabled = false;
                LaunchButton.Content = "⏳ Iniciando...";

                await Task.Run(async () =>
                {
                    var launcher = new CmlLib.Core.MinecraftLauncher();
                    
                    var initOption = new CmlLib.Core.LaunchOption
                    {
                        MaximumRam = 4096,
                        GameArgs = "--version BedrockLauncher"
                    };

                    var process = await launcher.LaunchAsync(_credential, initOption);
                    
                    Dispatcher.Invoke(() =>
                    {
                        Log($"Proceso iniciado con PID: {process.Id}");
                        LaunchButton.IsEnabled = true;
                        LaunchButton.Content = "▶️ Lanzar Minecraft Bedrock";
                        
                        process.EnableRaisingEvents = true;
                        process.Exited += (s, args) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Log("Minecraft ha finalizado");
                                LaunchButton.IsEnabled = true;
                                LaunchButton.Content = "▶️ Lanzar Minecraft Bedrock";
                            });
                        };
                    });
                });
            }
            catch (Exception ex)
            {
                Log($"Error al lanzar: {ex.Message}");
                MessageBox.Show($"Error al lanzar Minecraft: {ex.Message}\n\nNota: Asegúrate de tener Minecraft Bedrock instalado en tu sistema.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LaunchButton.IsEnabled = true;
                LaunchButton.Content = "▶️ Lanzar Minecraft Bedrock";
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _credential = null;
            _loginHandler = null;
            
            ResetLoginUI();
            UserInfoBorder.Visibility = Visibility.Collapsed;
            LaunchSection.Visibility = Visibility.Collapsed;
            
            Log("Sesión cerrada");
        }

        private void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            LogTextBox.AppendText($"[{timestamp}] {message}\n");
            LogTextBox.ScrollToEnd();
        }
    }
}
