using System;
using System.Drawing;
using System.Windows.Forms;

namespace Disable_Shutter_Sound_Tool
{
    public partial class TutorialForm : Form
    {
        private int currentIndex = 0;

        public TutorialForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterParent;

            this.Load += TutorialForm_Load;
            this.FormClosing += TutorialForm_FormClosing;
        }

        private void TutorialForm_Load(object sender, EventArgs e)
        {
            ShowCurrentItem();
        }

        private void ShowCurrentItem()
        {
            if (pictureBox1.Image != null) pictureBox1.Image.Dispose();

            switch (currentIndex)
            {
                case 0:
                    pictureBox1.BackgroundImage = Properties.Resources._1;
                    break;

                case 1:
                    pictureBox1.BackgroundImage = Properties.Resources._2;
                    break;

                case 2:
                    pictureBox1.BackgroundImage = Properties.Resources._3;
                    break;
                case 3:
                    pictureBox1.BackgroundImage = Properties.Resources._4;
                    break;
                case 4:
                    pictureBox1.BackgroundImage = Properties.Resources._5;
                    break;
                case 5:
                    pictureBox1.BackgroundImage = Properties.Resources._6;
                        break;
                case 6:
                    pictureBox1.BackgroundImage = Properties.Resources._7;
                        break;
                case 7:
                    pictureBox1.BackgroundImage = Properties.Resources._8;
                        break;
                default:
                    break;
            }
            // ボタンの状態更新
            btnBack.Enabled = currentIndex != 0;
            btnNext.Text = (currentIndex == 7) ? "閉じる" : "次へ";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex == 7)
            {
                this.Close();
            }
            else
            {
                currentIndex++;
                ShowCurrentItem();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowCurrentItem();
            }
        }
        private void TutorialForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // チェックが入っていたら今後は表示しないように設定
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.ShowTutorialForm = false;
                Properties.Settings.Default.Save();  // 設定を保存
            }
        }
    }
}
