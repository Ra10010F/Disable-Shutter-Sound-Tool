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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private void buttonFetchDevices_Click(object sender, EventArgs e)
        {
            // 閉じるべきフォームを一時的にリストに保存
            List<DeviceDetailForm> formsToClose = new List<DeviceDetailForm>();

            foreach (var form in deviceForms.Values)
            {
                formsToClose.Add(form);  // フォームをリストに追加
            }

            // リストのフォームを一括で閉じる
            foreach (var form in formsToClose)
            {
                form.Close();
            }

            // 開いているフォームを辞書から削除
            deviceForms.Clear();

            buttonDetails.Enabled = false;

            // デバイス情報を再取得するための処理（例: ADB コマンドを再実行）
            FetchDevices();
        }

        private void FetchDevices()
        {
            try
            {
                // adbコマンドを実行するプロセスを設定
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "devices";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                // adbコマンドを実行
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();


                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                bool devicesFound = false; // デバイスが見つかったかどうかを示すフラグ

                labelConnectedDevice.Text = ""; // 初期化
                labelConnectedDeviceStatus.Visible = false;

                if (lines.Length > 2) // 複数デバイスが接続されている場合
                {
                    buttonDetails.Enabled = false;
                    labelConnectedDevice.Visible = true;
                    labelConnectedDeviceStatus.Visible = true;
                    labelConnectedDevice.Text = "複数のデバイスが接続されています。";
                    labelConnectedDeviceStatus.Text = "このツールは複数のデバイスの接続には対応しておりません。";
                    //Console.Clear();
                    //Console.WriteLine("---Error---");
                    //Console.WriteLine("Multiple devices are connected.");
                    MessageBox.Show("複数のデバイスが接続されています。\nこのツールは複数のデバイスの接続には対応しておりません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (lines.Length == 2)
                {
                    buttonDetails.Enabled = true;

                    foreach (var line in lines)
                    {
                        // 行が "List of devices attached" のヘッダー行の場合はスキップ
                        if (line.StartsWith("List of devices attached")) continue;

                        // 出力を解析してコンソールに表示
                        //Console.Clear();
                        //Console.WriteLine("Connected Devices:");

                        // 行をタブで分割して解析
                        var parts = line.Split('\t');
                        if (parts.Length > 1)
                        {
                            string deviceId = parts[0]; // デバイスID
                            string status = parts[1];  // ステータス
                            // モデル名を取得
                            string modelName = GetDeviceModel(deviceId);

                            // デバイス情報を保存
                            selectedDeviceId = deviceId;
                            selectedStatus = status;
                            selectedModel = modelName;

                            // デバイスが見つかったフラグを設定
                            devicesFound = true;

                            // コンソール出力
                            //Console.WriteLine($"Model: {modelName}, Device ID: {deviceId}, Status: {status}");

                            // labelに接続されているデバイスの情報を表示
                            labelConnectedDevice.Visible = true;
                            labelConnectedDeviceStatus.Visible = true;
                            labelConnectedDevice.Text = $"モデル名: {modelName}, デバイスID: {deviceId}";
                            labelConnectedDeviceStatus.Text = $"ステータス: {status}";
                        }
                    }
                }
                else
                {
                    //Console.WriteLine("Not detected");
                    labelConnectedDevice.Text = "デバイスが接続されていません。";
                }
            }
            catch (Exception ex)
            {
                //Console.Clear();
                //Console.WriteLine("An error has occurred: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;

            System.Diagnostics.Process.Start("https://github.com/reindex-ot/15-Seconds-Online-ADB-Installer-and-Updater-jp");
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
                // エラーが発生した場合の処理
                //Console.WriteLine($"Failed to get model name: {ex.Message}");
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
    }
}






