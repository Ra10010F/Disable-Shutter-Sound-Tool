using ADB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace Disable_Shutter_Sound_Tool
{
    public partial class Mainform : Form
    {
        private string selectedDeviceId;
        private string selectedStatus;
        private string selectedModel;

        private Dictionary<string, DeviceDetailForm> deviceForms = new Dictionary<string, DeviceDetailForm>();

        public Mainform(string adbMessage, bool isAdbAvailable)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.FormClosing += Mainform_FormClosing;
            this.Shown += Mainform_Shown;

            label1.Text = adbMessage;

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // PictureBox に画像を設定
            pictureBox1.Image = isAdbAvailable
                ? Properties.Resources.check // ADB 使用可能
                : Properties.Resources.batsu; // ADB 使用不可

            buttonFetchDevices.Enabled = isAdbAvailable;

            // 詳細ボタンのクリックイベントを設定
            buttonDetails.Click += buttonDetails_Click;
        }

        private void Mainform_Load(object sender, EventArgs e) { }

        private void Mainform_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ShowTutorialForm)
            {
                using (var tutorialForm = new TutorialForm())
                {
                    tutorialForm.StartPosition = FormStartPosition.CenterParent;
                    tutorialForm.ShowDialog(this); // Mainform を親にする
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // ADB の使用可否を再確認
            bool isAdbAvailable = Program.IsAdbAvailable();
            string adbMessage = isAdbAvailable
                ? "ADBコマンドを使用できます。"
                : "ADBコマンドを使用できません。";

            // フォームの要素を更新
            label1.Text = adbMessage; // ラベルの更新
            pictureBox1.Image = isAdbAvailable
                ? Properties.Resources.check
                : Properties.Resources.batsu; // 状態に応じた画像の切り替え

            linkLabel1.Visible = isAdbAvailable
                ? false
                : true;

            // ボタンの有効/無効を更新
            buttonFetchDevices.Enabled = isAdbAvailable;
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            // DeviceDetailForm を開く
            var detailForm = new DeviceDetailForm(selectedModel, selectedDeviceId, selectedStatus, this)
            {
                Owner = this  // Mainform をオーナーに設定
            };

            // 選択されたデバイスを取得
            string selectedDevice = labelConnectedDevice.Text;
            if (string.IsNullOrEmpty(selectedDevice)) return;

            // 保存されたデバイス情報を使用して詳細フォームを開く
            string model = selectedModel;
            string deviceId = selectedDeviceId;
            string status = selectedStatus;


            // すでにそのデバイスIDに対応するフォームが開いていないか確認
            if (!deviceForms.ContainsKey(deviceId) || deviceForms[deviceId].IsDisposed)
            {
                //Console.WriteLine("---Open Detail---");
                //Console.WriteLine($"Device ID: {deviceId}");

                // 新しいフォームを作成して表示
                var newDetailForm = new DeviceDetailForm(model, deviceId, status, this);
                newDetailForm.FormClosed += (s, args) =>
                {
                    // フォームが閉じられたら辞書から削除
                    deviceForms.Remove(deviceId);
                };
                newDetailForm.Show();
                deviceForms[deviceId] = newDetailForm; // 新しいフォームを辞書に追加
            }
            else
            {
                // すでに開いている場合、そのフォームを最前面に表示
                deviceForms[deviceId].BringToFront();
            }
        }

        private async void buttonFetchDevices_Click(object sender, EventArgs e)
        {
            List<DeviceDetailForm> formsToClose = deviceForms.Values.ToList();
            foreach (var form in formsToClose)
            {
                form.Close();
            }
            deviceForms.Clear();

            buttonDetails.Enabled = false;

            await FetchDevices(); // 非同期呼び出しでフリーズ防止
        }


        private async Task FetchDevices()
        {
            try
            {
                string output = await Task.Run(() =>
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "adb";
                    process.StartInfo.Arguments = "devices";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return result;
                });

                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                Invoke((MethodInvoker)(() =>
                {
                    labelConnectedDevice.Text = "";
                    labelConnectedDeviceStatus.Visible = false;
                }));

                if (lines.Length > 2)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        buttonDetails.Enabled = false;
                        labelConnectedDevice.Visible = true;
                        labelConnectedDeviceStatus.Visible = true;
                        labelConnectedDevice.Text = "複数のデバイスが接続されています。";
                        labelConnectedDeviceStatus.Text = "このツールは複数のデバイスの接続には対応しておりません。";
                        MessageBox.Show("複数のデバイスが接続されています。\nこのツールは複数のデバイスの接続には対応しておりません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
                else if (lines.Length == 2)
                {
                    Invoke((MethodInvoker)(() => buttonDetails.Enabled = true));

                    foreach (var line in lines)
                    {
                        if (line.StartsWith("List of devices attached")) continue;

                        var parts = line.Split('\t');
                        if (parts.Length > 1)
                        {
                            string deviceId = parts[0];
                            string status = parts[1];
                            string modelName = await Task.Run(() => GetDeviceModel(deviceId));

                            selectedDeviceId = deviceId;
                            selectedStatus = status;
                            selectedModel = modelName;

                            var statusMap = new Dictionary<string, string>()
                            {
                                { "device", "準備完了" },
                                { "unauthorized", "未認証" },
                                { "offline", "切断" }
                            };

                            string displayStatus = statusMap.ContainsKey(status) ? statusMap[status] : status;

                            Invoke((MethodInvoker)(() =>
                            {
                                labelConnectedDevice.Visible = true;
                                labelConnectedDeviceStatus.Visible = true;
                                labelConnectedDevice.Text = $"モデル名: {modelName}  デバイスID: {deviceId}";
                                labelConnectedDeviceStatus.Text = $"ステータス: {displayStatus}";

                                // ステータスが device のときのみ詳細ボタンを有効にする
                                buttonDetails.Enabled = (status == "device");
                            }));
                        }
                    }
                }
                else
                {
                    Invoke((MethodInvoker)(() => labelConnectedDevice.Text = "デバイスが接続されていません。"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FetchDevices Error: {ex.Message}");
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!Program.IsRunAsAdministrator())
                {
                    var exeName = Application.ExecutablePath;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName, "install-adb")
                    {
                        UseShellExecute = true,
                        Verb = "runas"
                    };

                    try
                    {
                        Process.Start(startInfo); 
                        Application.Exit();
                        return;
                    }
                    catch (Win32Exception)
                    {
                        MessageBox.Show("管理者としての実行が必要です。", "アクセス拒否", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    
                }

                Program.AllocConsole();

                Console.ForegroundColor = ConsoleColor.Green;
                var installer = new ADBinstaller();
                installer.Run();
                Console.WriteLine("すべて完了しました");
                Console.ResetColor();
                Console.WriteLine("何かキーを押すと閉じます...");
                Console.ReadKey();
                Program.FreeConsole();
            }
            catch (Exception ex)
            {
                Program.AllocConsole();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("エラー: " + ex.Message);
                Console.ResetColor();
                Console.ReadKey();
                Program.FreeConsole();
            }
        }



        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 情報ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.ShowDialog();
        }

        private string GetDeviceModel(string deviceId)
        {
            try
            {
                // adbコマンドでモデル名を取得
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = $"-s {deviceId} shell getprop ro.product.model";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                // コマンドを実行
                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                // モデル名を返す
                return string.IsNullOrEmpty(output) ? "Unknown Model" : output;
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        // Mainform にデバイス接続を再確認するメソッドを追加
        public bool IsDeviceConnected(string deviceId)
        {
            try
            {
                // adb コマンドで接続されているデバイス情報を取得
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "devices";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    // 各デバイス行が "device" で終わっているかを確認
                    var parts = line.Split('\t');
                    if (parts.Length > 1 && parts[0] == deviceId && parts[1] == "device")
                    {
                        return true;  // 同じデバイスIDが接続されていれば true を返す
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return false;  // デバイスが接続されていなければ false を返す
        }

        private void チュートリアルを再表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var tutorialForm = new TutorialForm())
            {
                tutorialForm.StartPosition = FormStartPosition.CenterParent;
                tutorialForm.ShowDialog(this); // Mainform を親にする

                Properties.Settings.Default.ShowTutorialForm = true;
                Properties.Settings.Default.Save();  // 設定を保存
            }
        }

        public static class ConsoleManager
        {
            [DllImport("kernel32.dll")]
            public static extern bool AllocConsole();

            [DllImport("kernel32.dll")]
            public static extern bool FreeConsole();
        }
        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "kill-server";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("adb kill-server エラー: " + ex.Message);
            }
        }
    }
}