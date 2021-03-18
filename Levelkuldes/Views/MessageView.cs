﻿using Levelkuldes.Properties;
using Levelkuldes.ViewInterfaces;
using System.Windows.Forms;

namespace Levelkuldes.Views
{
    public partial class MessageView : UserControl, IMessageView
    {
        public MessageView()
        {
            InitializeComponent();
        }

        public string felado => FeladoTextBox.Text;
        public string errorFelado { set => errorPFelado.SetError(FeladoTextBox, value); }
        public string targy => TargyTextBox.Text;
        public string errorTargy { set => errorPTargy.SetError(TargyTextBox, value); }
        public string uzenetHTML { set => webBrowser1.Navigate(value); }
        public void ShowError()
        {
            MessageBox.Show(Resources.HibaUzenet, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
