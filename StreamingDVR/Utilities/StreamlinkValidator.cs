using System.Diagnostics;

namespace StreamingDVR.Utilities
{
    public static class StreamlinkValidator
    {
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
            var message = "Streamlink is not installed or not available in your system PATH.\n\n" +
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
    }
}
