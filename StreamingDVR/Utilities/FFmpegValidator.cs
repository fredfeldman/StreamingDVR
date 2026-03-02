using System.Diagnostics;

namespace StreamingDVR.Utilities
{
    public static class FFmpegValidator
    {
        public static bool IsFFmpegAvailable()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = "-version",
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

        public static string? GetFFmpegVersion()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = "-version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadLine();
                process.WaitForExit(3000);

                return output;
            }
            catch
            {
                return null;
            }
        }

        public static void ShowFFmpegInstallationInstructions()
        {
            var message = "FFmpeg is not installed or not available in your system PATH.\n\n" +
                         "Installation Options:\n\n" +
                         "1. Using Chocolatey:\n" +
                         "   choco install ffmpeg\n\n" +
                         "2. Using winget:\n" +
                         "   winget install FFmpeg\n\n" +
                         "3. Manual Installation:\n" +
                         "   • Download from https://ffmpeg.org/download.html\n" +
                         "   • Extract to a folder (e.g., C:\\ffmpeg)\n" +
                         "   • Add the bin folder to your system PATH\n" +
                         "   • Restart this application\n\n" +
                         "Would you like to open the FFmpeg download page?";

            var result = MessageBox.Show(message, "FFmpeg Not Found", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://ffmpeg.org/download.html",
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
