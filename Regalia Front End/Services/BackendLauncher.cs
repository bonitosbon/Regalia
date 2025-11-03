using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Regalia_Front_End.Services
{
    public class BackendLauncher
    {
        private static string GetBackendUrl()
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["BackendUrl"];
                if (!string.IsNullOrEmpty(url))
                {
                    return url.TrimEnd('/');
                }
            }
            catch
            {
                // Fallback if config fails
            }
            // Default fallback
            return "http://localhost:7288";
        }
        
        private static string BackendUrl => GetBackendUrl();
        private static string BackendApiUrl => $"{BackendUrl}/api/auth/login";
        private static Process _backendProcess;
        
        // Try multiple possible paths to find the backend project
        private static string[] BackendProjectPaths = new[]
        {
            @"..\..\..\CondoSystem_Backend\CondoSystem_Backend\CondoSystem_Backend.csproj", // From bin/Debug
            @"..\..\..\..\CondoSystem_Backend\CondoSystem_Backend\CondoSystem_Backend.csproj", // Alternative
            @"CondoSystem_Backend\CondoSystem_Backend\CondoSystem_Backend.csproj", // Direct path
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                @"C# PRoject\CondoSystem_Backend\CondoSystem_Backend\CondoSystem_Backend.csproj"
            ) // Full path from Documents
        };

        /// <summary>
        /// Checks if backend is running and starts it if not.
        /// Returns true if backend is ready, false if it failed to start.
        /// </summary>
        public static async Task<bool> EnsureBackendRunningAsync()
        {
            // Check if backend is already running
            if (await IsBackendRunningAsync())
            {
                System.Diagnostics.Debug.WriteLine("Backend is already running.");
                return true;
            }

            // Check if dotnet is available
            if (!IsDotNetAvailable())
            {
                System.Diagnostics.Debug.WriteLine("ERROR: 'dotnet' command not found. Please ensure .NET SDK is installed and in PATH.");
                return false;
            }

            // Try to start the backend
            try
            {
                System.Diagnostics.Debug.WriteLine("Backend is not running. Starting backend...");
                
                if (!StartBackendProcess())
                {
                    System.Diagnostics.Debug.WriteLine("Failed to start backend process. Check the debug output above for details.");
                    return false;
                }

                // Check if process is still running after a short delay
                await Task.Delay(2000); // Wait 2 seconds for process to initialize
                
                if (_backendProcess == null || _backendProcess.HasExited)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Backend process exited immediately. Check the backend console window for errors.");
                    if (_backendProcess != null && !_backendProcess.HasExited)
                    {
                        System.Diagnostics.Debug.WriteLine($"Exit code: {_backendProcess.ExitCode}");
                    }
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"Backend process is running (PID: {_backendProcess.Id}). Waiting for API to be ready...");

                // Wait for backend to be ready (with timeout)
                int maxWaitSeconds = 60; // Increased timeout to 60 seconds
                int waitedSeconds = 0;
                
                while (waitedSeconds < maxWaitSeconds)
                {
                    await Task.Delay(2000); // Check every 2 seconds instead of 1
                    waitedSeconds += 2;

                    // Check if process is still running
                    if (_backendProcess.HasExited)
                    {
                        System.Diagnostics.Debug.WriteLine($"ERROR: Backend process exited after {waitedSeconds} seconds. Check the backend console window for compilation/runtime errors.");
                        return false;
                    }

                    // Check if API is responding
                    if (await IsBackendRunningAsync())
                    {
                        System.Diagnostics.Debug.WriteLine($"Backend started successfully after {waitedSeconds} seconds.");
                        return true;
                    }

                    // Show progress every 10 seconds
                    if (waitedSeconds % 10 == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Waiting for backend to start... ({waitedSeconds}/{maxWaitSeconds} seconds). Process is running...");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"WARNING: Backend process is running but API did not respond after {maxWaitSeconds} seconds.");
                System.Diagnostics.Debug.WriteLine($"The backend console window should show what's happening. You can still try to login - the API might become available shortly.");
                // Return true anyway - process is running, might just need more time
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting backend: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Checks if dotnet CLI is available.
        /// </summary>
        private static bool IsDotNetAvailable()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit(3000); // Wait max 3 seconds
                        if (process.ExitCode == 0)
                        {
                            string version = process.StandardOutput.ReadToEnd().Trim();
                            System.Diagnostics.Debug.WriteLine($"Found .NET SDK version: {version}");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking dotnet: {ex.Message}");
            }
            
            return false;
        }

        /// <summary>
        /// Checks if the backend is currently running by making a test request.
        /// </summary>
        private static async Task<bool> IsBackendRunningAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60); // Longer timeout for Render free tier wake-up
                    
                    // Try to connect to the backend (we'll just check if it's reachable)
                    var response = await client.GetAsync($"{BackendUrl}/swagger/index.html");
                    return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound;
                    // NotFound is acceptable - means server is running but path doesn't exist
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Starts the backend process using dotnet run.
        /// </summary>
        private static bool StartBackendProcess()
        {
            try
            {
                string backendPath = null;

                // Try each possible path
                foreach (var relativePath in BackendProjectPaths)
                {
                    string testPath;
                    
                    if (System.IO.Path.IsPathRooted(relativePath))
                    {
                        // Already a full path
                        testPath = relativePath;
                    }
                    else
                    {
                        // Relative path from application startup
                        testPath = System.IO.Path.GetFullPath(
                            System.IO.Path.Combine(Application.StartupPath, relativePath)
                        );
                    }

                    if (System.IO.File.Exists(testPath))
                    {
                        backendPath = testPath;
                        System.Diagnostics.Debug.WriteLine($"Found backend project at: {backendPath}");
                        break;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Backend project not found at: {testPath}");
                    }
                }

                if (backendPath == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Could not find backend project file.");
                    System.Diagnostics.Debug.WriteLine($"Searched from: {Application.StartupPath}");
                    System.Diagnostics.Debug.WriteLine("Tried paths:");
                    foreach (var path in BackendProjectPaths)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - {path}");
                    }
                    System.Diagnostics.Debug.WriteLine("Please ensure CondoSystem_Backend\\CondoSystem_Backend\\CondoSystem_Backend.csproj exists.");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"Starting backend from: {backendPath}");

                // Get the directory containing the .csproj file
                string backendDir = System.IO.Path.GetDirectoryName(backendPath);

                // Start dotnet run process
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "run",
                    WorkingDirectory = backendDir,
                    UseShellExecute = true, // Set to true so it opens in its own window
                    CreateNoWindow = false, // Show the console window so you can see backend logs
                    WindowStyle = ProcessWindowStyle.Normal
                };

                try
                {
                    _backendProcess = Process.Start(processStartInfo);

                    if (_backendProcess != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Backend process started with PID: {_backendProcess.Id}");
                        System.Diagnostics.Debug.WriteLine($"Working directory: {backendDir}");
                        System.Diagnostics.Debug.WriteLine($"Command: dotnet run");
                        System.Diagnostics.Debug.WriteLine("Check the backend console window for startup logs and any errors.");
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Process.Start returned null. Check if dotnet is properly installed.");
                        return false;
                    }
                }
                catch (System.ComponentModel.Win32Exception win32Ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: Failed to start process. Win32 Error: {win32Ex.Message}");
                    if (win32Ex.NativeErrorCode == 2)
                    {
                        System.Diagnostics.Debug.WriteLine("This usually means 'dotnet' command was not found. Ensure .NET SDK is installed and in PATH.");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error starting backend process: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stops the backend process if it was started by this launcher.
        /// </summary>
        public static void StopBackend()
        {
            try
            {
                if (_backendProcess != null && !_backendProcess.HasExited)
                {
                    _backendProcess.Kill();
                    _backendProcess.WaitForExit(5000); // Wait up to 5 seconds
                    _backendProcess.Dispose();
                    _backendProcess = null;
                    System.Diagnostics.Debug.WriteLine("Backend process stopped.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping backend: {ex.Message}");
            }
        }
    }
}

