using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PBN_EDITOR
{
    public partial class Form1 : Form
    {
        public TextBox[,] TBArray = new TextBox[4, 4];
        public PBNFile pbnF;
        public string path="";
        public Form1()
        {
            InitializeComponent();
            TBArray[0, 0] = textBoxNS;
            TBArray[0, 1] = textBoxNH;
            TBArray[0, 2] = textBoxND;
            TBArray[0, 3] = textBoxNC;
            TBArray[1, 0] = textBoxES;
            TBArray[1, 1] = textBoxEH;
            TBArray[1, 2] = textBoxED;
            TBArray[1, 3] = textBoxEC;
            TBArray[2, 0] = textBoxSS;
            TBArray[2, 1] = textBoxSH;
            TBArray[2, 2] = textBoxSD;
            TBArray[2, 3] = textBoxSC;
            TBArray[3, 0] = textBoxWS;
            TBArray[3, 1] = textBoxWH;
            TBArray[3, 2] = textBoxWD;
            TBArray[3, 3] = textBoxWC;
            PBNFile.form1 = this;
            pbnF = new PBNFile();
            Global_Random.init();
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool Save()
        {
            pbnF.saveTemp();
            string errorMessage;
            foreach (Board b in pbnF.boardList)
            {
                if (!b.check(out errorMessage))
                {
                    MessageBox.Show(String.Format("第{0}副牌有误： ", b.num) + errorMessage);
                    return false;
                }
            }
            if (path == "")
            {
                SaveFileDialog sDlg = new SaveFileDialog();
                sDlg.Filter = "牌例记录文件(*.pbn)|*.pbn";
                if (sDlg.ShowDialog() == DialogResult.OK)
                {
                    pbnF.Save(sDlg.FileName);
                    path = sDlg.FileName;
                    return true;
                }
            }
            else
            {
                pbnF.Save(path);
                return true;
            }
            return false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.saveTemp();
            string errorMessage;
            foreach (Board b in pbnF.boardList)
            {
                if (!b.check(out errorMessage))
                {
                    MessageBox.Show(String.Format("第{0}副牌有误： ", b.num) + errorMessage);
                    return;
                }
            }
            SaveFileDialog sDlg = new SaveFileDialog();
            sDlg.Filter = "牌例记录文件(*.pbn)|*.pbn";
            if (sDlg.ShowDialog() == DialogResult.OK)
            {
                pbnF.Save(sDlg.FileName);
                path = sDlg.FileName;
            }
        }

        private void exitEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "牌例记录文件(*.pbn)|*.pbn";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                pbnF.Load(openDlg.FileName);
                path = openDlg.FileName;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (pbnF.index == (pbnF.boardList.Count - 1)) return;
            pbnF.saveTemp();    
            pbnF.index++;
            pbnF.Show();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (pbnF.index == 0) return;
            pbnF.saveTemp();
            pbnF.index--;
            pbnF.Show();
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            if (pbnF.index == 0) return;
            pbnF.saveTemp();
            pbnF.index=0;
            pbnF.Show();
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            if (pbnF.index == (pbnF.boardList.Count - 1)) return;
            pbnF.saveTemp();
            pbnF.index = pbnF.boardList.Count - 1;
            pbnF.Show();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i=0;i<4;i++)
                for (int j = 0; j < 4; j++)
                {
                    TBArray[i, j].Clear();
                }
        }

        private void newDealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.newDeal();
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存当前文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pbnF.saveTemp();
                if (path == "")
                {
                    SaveFileDialog sDlg = new SaveFileDialog();
                    sDlg.Filter = "牌例记录文件(*.pbn)|*.pbn";
                    if (sDlg.ShowDialog() == DialogResult.OK)
                    {
                        pbnF.Save(sDlg.FileName);
                    }
                }
                else
                {
                    pbnF.Save(path);
                }
            }
            pbnF = new PBNFile();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 'x' && e.KeyChar != 'X') return;
            TextBox textB = (TextBox)sender;
            string temp;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(TBArray[i,j].Equals(textB))
                    {
                        temp = TBArray[0, j].Text + TBArray[1, j].Text + TBArray[2, j].Text + TBArray[3, j].Text;
                        for (int k = 2; k <= 9; k++)
                        {
                            if (temp.IndexOf(Convert.ToChar(48+k)) == -1)
                            {
                                TBArray[i, j].AppendText(Convert.ToString(k));
                                break;
                            }
                        }
                        e.Handled = true;
                        return;
                    }

                }
            }
        }

        private void insertIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "牌例记录文件(*.pbn)|*.pbn";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                if (pbnF.Insert(openDlg.FileName)&&MessageBox.Show("是否重新对牌组编号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pbnF.ResetBoradNum(pbnF.boardList[0].num);
                }
            }
        }

        private void deleteDealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.DeleteDeal(pbnF.index);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutbox = new AboutBox1();
            aboutbox.Show();
        }

        private void setBoradNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox dlg = new InputBox();
            dlg.text.Text = "设置第一幅牌的编号";
            while (dlg.ShowDialog() == DialogResult.OK)
            {
                short x;
                try
                {
                    x = Convert.ToInt16(dlg.Input.Text);
                }
                catch
                {
                    MessageBox.Show("请输入一个整数", "PBN编辑器");
                    continue;
                }
                    pbnF.ResetBoradNum(x);
                    break;
            }
        }

        private void exchangeEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.saveTemp();
            pbnF.EXG_EW(pbnF.index);
        }

        private void exchangeNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.saveTemp();
            pbnF.EXG_NS(pbnF.index);
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.saveTemp();
            pbnF.Rotate(pbnF.index);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!pbnF.textChanged) return;
            DialogResult result = MessageBox.Show("是否保存当前文件？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    if(!Save())
                        e.Cancel=true;
                    break;
                default:
                    break;
            }
        }


        private void textBox_TextChanged(object sender, EventArgs e)
        {
            pbnF.textChanged = true;
        }

        private void TempRandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.RandomDeal(pbnF.index);
        }

        private void randomRestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage;
            pbnF.saveTemp();
            if (!pbnF.boardList[pbnF.index].check(out errorMessage, false))
            {
                MessageBox.Show(String.Format("本副牌输入有误： ") + errorMessage);
                return;
            }
            pbnF.Random_Rest(pbnF.index);
        }

        private void shuffleAllDealsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pbnF.Shuffle_All_Deals();
        }

        private void exchangeSuitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExchangeSuitsForm exSuit_form = new ExchangeSuitsForm();
            if (exSuit_form.ShowDialog() == DialogResult.OK)
            {
                int cnt = 0;
                int suit1 = -1, suit2 = -1;
                CheckBox[] list = exSuit_form.checkBoxSuit;
                for (int i=0;i<list.Length;i++)
                {
                    if (list[i].Checked)
                    {
                        cnt++;
                        if (cnt == 1) suit1 = i;
                        else
                            suit2 = i;
                    }
                    
                }
                if (cnt != 2) return;
                pbnF.ExChange_Suit(pbnF.index, suit1, suit2);
            }
        }
    }
}
