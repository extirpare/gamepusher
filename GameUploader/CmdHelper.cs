using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public static class CmdHelper
{
    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
    }

    public static string GetCmdOutput(string cmd)
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = "/C " + cmd; // "/C Carries out the command specified by string and then terminates"

        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;

        var outputStr = new StringBuilder();
        var errorStr = new StringBuilder();

        const int timeoutMs = 1500;

        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
        {
            p.OutputDataReceived += (sender, args) => { if (args.Data == null) outputWaitHandle.Set(); else outputStr.AppendLine(args.Data); };
            p.ErrorDataReceived += (sender, args) => { if (args.Data == null) errorWaitHandle.Set(); else errorStr.AppendLine(args.Data); };

            p.Start();

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            if (p.WaitForExit(timeoutMs) && outputWaitHandle.WaitOne(timeoutMs) && errorWaitHandle.WaitOne(timeoutMs))
            {
                if (p.ExitCode == 0)
                    return outputStr.ToString();
                else
                    throw new Exception("Error exit code returned from '" + cmd + "': " + p.ExitCode + " | " + errorStr + " | " + outputStr.ToString());
            }
            else
                throw new Exception("Error while executing '" + cmd + "': " + errorStr.ToString());
        }
    }

    public static Process RunCmd(string cmd, Action onExitAction = null)
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = false;
        p.StartInfo.RedirectStandardOutput = false;
        p.StartInfo.CreateNoWindow = false;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

        p.StartInfo.FileName = "cmd.exe";
        //see https://stackoverflow.com/questions/355988/how-do-i-deal-with-quote-characters-when-using-cmd-exe
        p.StartInfo.Arguments = $"/S /K \"{cmd}\"";

        if (onExitAction != null)
		{
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler((sender, e) => { onExitAction(); });
        }

        p.Start();
        return p;
    }
}
