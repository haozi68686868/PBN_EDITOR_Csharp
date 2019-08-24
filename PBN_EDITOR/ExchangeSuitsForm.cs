using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBN_EDITOR
{
    public partial class ExchangeSuitsForm : Form
    {
        public CheckBox[] checkBoxSuit = new CheckBox[4];
        public ExchangeSuitsForm()
        {
            InitializeComponent();
            checkBoxSuit[0] = checkBoxSpade;
            checkBoxSuit[1] = checkBoxHeart;
            checkBoxSuit[2] = checkBoxDiamond;
            checkBoxSuit[3] = checkBoxClub;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxSuit_CheckedChanged(object sender, EventArgs e)
        {
            int cnt = 0;
            foreach(CheckBox cb in checkBoxSuit)
            {
                if (cb.Checked) cnt++;
            }
            if (cnt < 3) return;
            CheckBox c = (CheckBox)sender;
            c.Checked = false;
        }
    }
}
