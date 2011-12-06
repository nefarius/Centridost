using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NetSupport
{
    public partial class ServerAddress : Form
    {
        public ServerAddress()
        {
            InitializeComponent();
        }

        private void ServerAddress_Shown(object sender, EventArgs e)
        {
            SplashScreen.CloseForm();
            textBoxHost.Focus();
            textBoxHost.SelectAll();
        }

        private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !int.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void textBoxPort_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
