using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disable_Shutter_Sound_Tool
{
    public partial class DeviceDetailForm : Form
    {
        private static string selectedDeviceId; // デバイスIDを保持するための静的フィールド

        public DeviceDetailForm(string model, string deviceId, string status, Form owner)
        {
            selectedDeviceId = deviceId; // 静的フィールドに保存
            this.Owner = owner; // 所有者フォームを設定

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Text = $"[モデル名: {model}, デバイスID: {deviceId}]の詳細";

            // ラベルにデバイス情報を設定
            labelDeviceDetails.Text = $"モデル名: {model}  デバイスID: {deviceId}";
            // フォームのサイズ変更時にフォントサイズを調整
            this.Resize += (sender, e) => AdjustLabelFontSize();
            AdjustLabelFontSize(); // 初期サイズに合わせて調整

            // 初期ラベル設定
            labelShutterKey.Text = "状態を確認中...";

            // ステータスが "device" か確認
            if (status != "device")
            {
                labelShutterKey.Text = "USBデバッグが許可されていないか接続が不正です";
                buttonE.Enabled = false;
                buttonD.Enabled = false;
                return;
            }
            GetShutterSoundStatus();
        }
        private static string GetCurrentlyConnectedDeviceId(Mainform mainForm)
        {
            // デバイスID取得のロジックを実装
            // 例：
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = "devices";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // 出力を行ごとに分割して解析
                string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 1) // ヘッダー行を除いて少なくとも1デバイスある場合
                {
                    string deviceLine = lines[1]; // 最初のデバイス行
                    string[] parts = deviceLine.Split('\t');
                    if (parts.Length >= 1)
                    {
                        return parts[0].Trim(); // デバイスIDを返す
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error getting device ID: {ex.Message}");
            }

            return null;
        }

        private void buttonE_Click(object sender, EventArgs e)
        {
            if (!CheckDeviceConnection())
            {
                return;
            }
            SetShutterSound(1);
        }

        private void buttonD_Click(object sender, EventArgs e)
        {
            if (!CheckDeviceConnection())
            {
                return;
            }
            SetShutterSound(0);
        }

        private void AdjustLabelFontSize()
        {
            if (string.IsNullOrEmpty(labelDeviceDetails.Text)) return;

            // 初期フォントを基準にフォントサイズを変更
            float minFontSize = 8f;  // 最小フォントサイズ
            float maxFontSize = 48f; // 最大フォントサイズ

            // フォームの幅を基準に計算
            using (Graphics g = labelDeviceDetails.CreateGraphics())
            {
                float newFontSize = maxFontSize;

                while (newFontSize > minFontSize)
                {
                    Font testFont = new Font(labelDeviceDetails.Font.FontFamily, newFontSize, labelDeviceDetails.Font.Style);
                    SizeF textSize = g.MeasureString(labelDeviceDetails.Text, testFont);

                    // テキストがラベルの幅に収まるか確認
                    if (textSize.Width <= this.ClientSize.Width - 22) // マージンを考慮
                    {
                        break;
                    }

                    newFontSize -= 0.5f; // 徐々に小さくする
                }

                // 新しいフォントサイズを設定
                labelDeviceDetails.Font = new Font(labelDeviceDetails.Font.FontFamily, newFontSize, labelDeviceDetails.Font.Style);
            }
        }

        private void GetShutterSoundStatus()
        {
            try
            {
                // adb コマンドでシャッター音のキーを取得
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = $"shell settings get system csc_pref_camera_forced_shuttersound_key";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                // 出力に応じてラベルのテキストを設定
                if (string.IsNullOrEmpty(output) || output.Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    labelShutterKey.Text = "このデバイスは対応していません";
                    //Console.WriteLine("This device does not support changing the shutter sound");
                    //Console.WriteLine("Get Key:" + output);
                    buttonE.Enabled = false;
                    buttonD.Enabled = false;
                }
                else if (output == "1")
                {
                    labelShutterKey.Text = "シャッター音は有効になっています";
                    //Console.WriteLine("Shutter sound is Enabled");
                    //Console.WriteLine("Get Key:" + output);
                    buttonE.Enabled = false;
                    buttonD.Enabled = true;
                }
                else if (output == "0")
                {
                    labelShutterKey.Text = "シャッター音は無効になっています";
                    //Console.WriteLine("Shutter sound is Disabled");
                    //Console.WriteLine("Get Key:" + output);
                    buttonE.Enabled = true;
                    buttonD.Enabled = false;
                }
                else
                {
                    labelShutterKey.Text = "不明な値: " + output;
                }
            }
            catch (Exception ex)
            {
                // エラーが発生した場合の処理
                //Console.WriteLine($"エラー: {ex.Message}");
                labelShutterKey.Text = "エラーが発生しました";
            }
        }

        private void SetShutterSound(int value)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "adb";
                process.StartInfo.Arguments = $"shell settings put system csc_pref_camera_forced_shuttersound_key {value}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                // 結果のメッセージを表示
                string message = value == 1 ? "シャッター音を有効にしました。" : "シャッター音を無効にしました。";
                MessageBox.Show(message, "操作完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // シャッター音の状態を再確認してラベルを更新
                GetShutterSoundStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckDeviceConnection()
        {
            if (this.Owner is Mainform mainForm)
            {
                bool isConnected = mainForm.IsDeviceConnected(selectedDeviceId);
                if (isConnected)
                {
                    return true;
                }
                else
                {
                    string currentDeviceId = GetCurrentlyConnectedDeviceId(mainForm);
                    if (!string.IsNullOrEmpty(currentDeviceId) && currentDeviceId != selectedDeviceId)
                    {
                        labelShutterKey.Text = "違うデバイスが接続されました。";
                        buttonE.Enabled = false;
                        buttonD.Enabled = false;
                        MessageBox.Show("違うデバイスが接続されています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        labelShutterKey.Text = "デバイスが切断されました。";
                        buttonE.Enabled = false;
                        buttonD.Enabled = false;
                        MessageBox.Show("デバイスが接続されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
            }
            return false;
        }

    }
}
