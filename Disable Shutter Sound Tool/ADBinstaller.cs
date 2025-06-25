using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;

namespace ADB
{
    public class ADBinstaller
    {
        private readonly string workDir;
        private readonly string adbZipUrl = "https://dl.google.com/android/repository/platform-tools-latest-windows.zip";
        private readonly string usbZipUrl = "https://dl.google.com/android/repository/latest_usb_driver_windows.zip";
        private readonly string installPath;

        public ADBinstaller()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            workDir = Path.Combine(desktop, "ADB-Installer-Updater-Windows");
            installPath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "ADB");
        }

        public void Run()
        {
            Console.Title = "ADBインストーラー";
            if (!IsAdministrator())
            {
                throw new InvalidOperationException("この操作は管理者権限が必要です。");
            }

            if (Directory.Exists(workDir)) Directory.Delete(workDir, true);
            Directory.CreateDirectory(workDir);

            // ADBのダウンロード
            string adbZip = Path.Combine(workDir, "adb.zip");
            DownloadWithProgress(adbZipUrl, adbZip, "ADB");
            ZipFile.ExtractToDirectory(adbZip, workDir);
            Directory.Move(Path.Combine(workDir, "platform-tools"), Path.Combine(workDir, "ADB"));
            File.Delete(adbZip);

            // USBドライバのダウンロード
            string usbZip = Path.Combine(workDir, "usb.zip");
            DownloadWithProgress(usbZipUrl, usbZip, "USB Driver");
            ZipFile.ExtractToDirectory(usbZip, workDir);
            Directory.Move(Path.Combine(workDir, "usb_driver"), Path.Combine(workDir, "Driver"));
            File.Delete(usbZip);

            //adb.exe強制終了
            try
            {
                Process[] adbProcesses = Process.GetProcessesByName("adb");

                foreach (Process process in adbProcesses)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit();
                        Console.WriteLine($"adb.exe (PID: {process.Id}) を強制終了しました。");
                    }
                    catch (Exception ex) { }
                }
                if (adbProcesses.Length == 0)
                {
                    Console.WriteLine("実行中の adb.exe は見つかりませんでした。");
                }
            }
            catch (Exception ex) { }

            // ADBをコピー
            if (Directory.Exists(installPath)) Directory.Delete(installPath, true);
            DirectoryCopy(Path.Combine(workDir, "ADB"), installPath, true);

            // 環境変数PATHに追加
            AddPathEnvironmentVariable(installPath);

            // ドライバをインストール
            string infFile = Path.Combine(workDir, "Driver", "android_winusb.inf");
            if (File.Exists(infFile))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "pnputil.exe",
                    Arguments = $"/add-driver \"{infFile}\" /install",
                    Verb = "runas",
                    UseShellExecute = true
                })?.WaitForExit();
            }

            Directory.Delete(workDir, true);
        }

        private static void DownloadWithProgress(string url, string destination, string label)
        {
            using (var client = new HttpClient())
            using (var response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();
                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var canReportProgress = totalBytes != -1;

                using (var contentStream = response.Content.ReadAsStreamAsync().Result)
                using (var fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var buffer = new byte[8192];
                    long totalRead = 0;
                    int read;
                    var lastProgress = -1;

                    while ((read = contentStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, read);
                        totalRead += read;

                        if (canReportProgress)
                        {
                            int progress = (int)((totalRead * 100) / totalBytes);
                            if (progress != lastProgress)
                            {
                                Console.Write($"\r{label} ダウンロード中: {progress}%   ");
                                lastProgress = progress;
                            }
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("ダウンロード完了。");
        }

        private static void DirectoryCopy(string sourceDir, string destDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destDir, subDir.Name);
                    DirectoryCopy(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private static void AddPathEnvironmentVariable(string installPath)
        {
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine) ?? "";
            if (!currentPath.Split(';').Contains(installPath, StringComparer.OrdinalIgnoreCase))
            {
                string newPath = currentPath.TrimEnd(';') + ";" + installPath;
                Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);
                Console.WriteLine("システム PATH に追加しました。");
            }
            else
            {
                Console.WriteLine("PATH に既に含まれています。");
            }
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
