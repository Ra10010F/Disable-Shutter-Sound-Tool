using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disable_Shutter_Sound_Tool
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Console.Title = "Log";

            if (IsAdbAvailable())
            {
                //Console.WriteLine("ADB commands are available.");
            }
            else
            {
                //Console.WriteLine("Please install ADB development environment");
                //Console.WriteLine("ADB is not found in the PATH.");
            }

            bool isAdbAvailable = IsAdbAvailable();
            string adbMessage = IsAdbAvailable()
            ? "ADBコマンドを使用できます。"
            : "ADBコマンドを使用できません。";


            //Form1を実行
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Mainform(adbMessage, isAdbAvailable));
        }
        public static bool IsAdbAvailable()
        {
            // PATH環境変数を取得
            string pathEnv = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(pathEnv))
            {
                //Console.WriteLine("PATH environment variable is empty.");
                return false;
            }

            // PATH内にadbが存在するかチェック
            string[] paths = pathEnv.Split(';'); // Windowsは';'で分割
            bool adbInPath = paths.Any(path => System.IO.File.Exists(System.IO.Path.Combine(path, "adb.exe")));

            if (!adbInPath)
            {
                return false;
            }

            // adbコマンドの実行を試す
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "adb",
                        Arguments = "version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // 正常に実行されたか確認
                if (process.ExitCode == 0 && output.Contains("Android Debug Bridge"))
                {
                    return true;
                }
                else
                {
                    //Console.WriteLine("ADB command failed. Error: " + error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception while executing adb: " + ex.Message);
                return false;
            }
        }
    }
}
