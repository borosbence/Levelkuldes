using Levelkuldes.Properties;
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

        public string Felado => FeladoTextBox.Text;
        public string errorFelado { set => errorPFelado.SetError(FeladoTextBox, value); }
        public string Targy => TargyTextBox.Text;
        public string errorTargy { set => errorPTargy.SetError(TargyTextBox, value); }
        public string uzenetFajl { set => webBrowser1.Navigate(value); }
        public void ShowError()
        {
            MessageBox.Show(Resources.HibaUzenet, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
