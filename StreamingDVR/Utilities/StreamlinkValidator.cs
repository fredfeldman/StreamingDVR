using System.Diagnostics;

namespace StreamingDVR.Utilities
{
    public static class StreamlinkValidator
    {
        /// <summary>
        /// Gets the full path to the Streamlink executable if found
        /// </summary>
        /// <returns>Full path to streamlink.exe or null if not found</returns>
        public static string? GetStreamlinkPath()
        {
            // First, check if streamlink is in PATH and can be executed
            if (IsStreamlinkAvailable())
            {
                // Try to get the path using 'where' command on Windows
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "where",
                            Arguments = "streamlink",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit(3000);

                    if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                    {
                        // Return the first path found
                        var firstLine = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(firstLine) && File.Exists(firstLine))
                        {
                            return firstLine.Trim();
                        }
                    }
                }
                catch
                {
                    // Fall through to check common locations
                }
            }

            // Check common installation locations
            var possiblePaths = new List<string>();

            // Python Scripts folder in user profile
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Roaming", "Python", "Python310", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Roaming", "Python", "Python311", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Roaming", "Python", "Python312", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Local", "Programs", "Python", "Python310", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Local", "Programs", "Python", "Python311", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(userProfile, "AppData", "Local", "Programs", "Python", "Python312", "Scripts", "streamlink.exe"));

            // Program Files
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            possiblePaths.Add(Path.Combine(programFiles, "Streamlink", "bin", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(programFilesX86, "Streamlink", "bin", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(programFiles, "Python310", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(programFiles, "Python311", "Scripts", "streamlink.exe"));
            possiblePaths.Add(Path.Combine(programFiles, "Python312", "Scripts", "streamlink.exe"));

            // Check each possible path
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            // Check PATH environment variable manually
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrEmpty(pathEnv))
            {
                var paths = pathEnv.Split(';');
                foreach (var dir in paths)
                {
                    if (!string.IsNullOrWhiteSpace(dir))
                    {
                        var streamlinkPath = Path.Combine(dir.Trim(), "streamlink.exe");
                        if (File.Exists(streamlinkPath))
                        {
                            return streamlinkPath;
                        }
                    }
                }
            }

            return null;
        }

        public static bool IsStreamlinkAvailable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "streamlink",
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit(3000);

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static string? GetStreamlinkVersion()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "streamlink",
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(3000);

                return output.Trim();
            }
            catch
            {
                return null;
            }
        }

        public static void ShowStreamlinkInstallationInstructions()
        {
            var streamlinkPath = GetStreamlinkPath();
            string message;

            if (!string.IsNullOrEmpty(streamlinkPath))
            {
                // Streamlink is installed but maybe not working correctly
                message = $"Streamlink appears to be installed but is not functioning correctly.\n\n" +
                         $"Detected Location:\n{streamlinkPath}\n\n" +
                         "Try the following:\n\n" +
                         "1. Reinstall Streamlink:\n" +
                         "   pip install --upgrade --force-reinstall streamlink\n\n" +
                         "2. Check your PATH environment variable\n\n" +
                         "3. Restart this application\n\n" +
                         "Would you like to open the Streamlink installation page?";
            }
            else
            {
                // Streamlink is not installed
                message = "Streamlink is not installed or not available in your system PATH.\n\n" +
                         "Installation Options:\n\n" +
                         "1. Using pip (Recommended):\n" +
                         "   pip install streamlink\n\n" +
                         "2. Using Chocolatey:\n" +
                         "   choco install streamlink\n\n" +
                         "3. Using winget:\n" +
                         "   winget install streamlink.streamlink\n\n" +
                         "4. Windows Installer:\n" +
                         "   • Download from https://streamlink.github.io/install.html\n" +
                         "   • Run the installer\n" +
                         "   • Restart this application\n\n" +
                         "Benefits of Streamlink:\n" +
                         "• Better stream compatibility\n" +
                         "• Support for authentication\n" +
                         "• Automatic quality selection\n" +
                         "• Plugin system for various platforms\n\n" +
                         "Would you like to open the Streamlink installation page?";
            }

            var result = MessageBox.Show(message, "Streamlink Not Found", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://streamlink.github.io/install.html",
                        UseShellExecute = true
                    });
                }
                catch
                {
                    // Silently fail if browser won't open
                }
            }
        }

        /// <summary>
        /// Gets information about the Streamlink installation including path and version
        /// </summary>
        /// <returns>Formatted string with installation details or null if not found</returns>
        public static string? GetStreamlinkInfo()
        {
            var path = GetStreamlinkPath();
            var version = GetStreamlinkVersion();
            var isAvailable = IsStreamlinkAvailable();

            if (!isAvailable && path == null)
            {
                return "Streamlink is not installed or not available.";
            }

            var info = new System.Text.StringBuilder();
            info.AppendLine("Streamlink Installation Info:");
            info.AppendLine();

            if (isAvailable)
            {
                info.AppendLine("✓ Status: Available and functional");
            }
            else
            {
                info.AppendLine("✗ Status: Found but not functional");
            }

            if (!string.IsNullOrEmpty(version))
            {
                info.AppendLine($"✓ Version: {version}");
            }

            if (!string.IsNullOrEmpty(path))
            {
                info.AppendLine($"✓ Path: {path}");
            }
            else if (isAvailable)
            {
                info.AppendLine("ℹ Path: Available in system PATH");
            }

            return info.ToString();
        }
    }
}
