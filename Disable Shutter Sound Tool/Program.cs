using ADB;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace Disable_Shutter_Sound_Tool
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 管理者としてADBインストール処理を実行
            if (args.Length > 0 && args[0] == "install-adb")
            {
                AllocConsole();

                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    var installer = new ADBinstaller();
                    installer.Run();
                    Console.WriteLine("すべて完了しました");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("エラー: " + ex.Message);
                    Console.ResetColor();
                    Console.WriteLine("何かキーを押すと終了します...");
                    Console.ReadKey();
                    FreeConsole();
                    return;
                }

                Console.ResetColor();
                Console.WriteLine("\nインストール完了。アプリを通常権限で再起動します...");
                System.Threading.Thread.Sleep(1500);
                FreeConsole();

                RestartAsNormalUser();
                return;
            }

            // 通常のGUI起動処理
            string adbMessage = IsAdbAvailable()
                ? "ADBコマンドを使用できます。"
                : "ADBコマンドを使用できません。";

            Application.Run(new Mainform(adbMessage, IsAdbAvailable()));
        }

        public static bool IsAdbAvailable()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "version";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return output.ToLower().Contains("android debug bridge");
            }
            catch
            {
                return false;
            }
        }

        public static bool IsRunAsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void RestartAsNormalUser()
        {
            try
            {
                string exePath = Application.ExecutablePath;

                ProcessStartInfo startInfo = new ProcessStartInfo("explorer.exe", $"\"{exePath}\"")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("通常起動に失敗しました: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
