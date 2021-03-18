using Levelkuldes.Properties;
using Levelkuldes.ViewInterfaces;
using System.Windows.Forms;

namespace Levelkuldes.Views
{
    public partial class AddressView : UserControl, IAddressView
    {
        public AddressView()
        {
            InitializeComponent();
        }

        public string cimzettFajl { get => FajlLabel.Text; set => FajlLabel.Text = value; }
        public string errorCimzettFajl
        {
            set
            {
                if (value != null)
                {
                    errorPFajl.SetError(FajlLabel, value);
                    MessageBox.Show(Resources.HibaCimzettek, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public int BeolvasottElemek { set => OsszCimzettLabel.Text = value.ToString(); }
        public string eredmenyKimenet { get => textBox1.Text; set => textBox1.Text = value; }
    }
}
