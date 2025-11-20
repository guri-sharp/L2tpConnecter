using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace L2TPConnecter
{
    public static class PowerShell
    {
        public static async Task Run(string script, Action<string> onOutput, Action<string> onError)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script.Replace("\"", "`\"")}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (s, e) =>
            {
                if (!string .IsNullOrEmpty(e.Data))
                    onOutput?.Invoke(e.Data);
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    onError?.Invoke(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await Task.Run(() => process.WaitForExit());
        }

    }
}
