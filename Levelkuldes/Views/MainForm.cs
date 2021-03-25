using Levelkuldes.Presenters;
using Levelkuldes.ViewInterfaces;
using System;
using System.Windows.Forms;

namespace Levelkuldes.Views
{
    public partial class MainForm : Form, IMainView
    {
        private MessageView messageView;
        private AddressView addressView;
        private LevelkuldesPresenter presenter;

        public MainForm()
        {
            InitializeComponent();

            messageView = new MessageView();
            messageView.Dock = DockStyle.Fill;
            addressView = new AddressView();
            addressView.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(messageView);
            tabPage2.Controls.Add(addressView);

            presenter = new LevelkuldesPresenter(this, messageView, addressView);
        }

        private void kilepesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void levelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Levél megnyitása...";
            openFileDialog1.Filter = "Weblap (*.html;*.htm) |*.html;*.htm";
            var dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                presenter.LoadMessage(openFileDialog1.FileName);
            }
        }

        private void cimzettekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Címzettek megnyitása...";
            openFileDialog1.Filter = "Egyszerű szöveg (*.txt)|*.txt|CSV (pontosvesszővel tagolt) (*.csv)|*.csv";
            var dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                presenter.LoadAddresses(openFileDialog1.FileName, openFileDialog1.SafeFileName);
            }
        }

        private void nevjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Készítette: Boros Bence", "Névjegy", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void LevelKuldesButton_Click(object sender, EventArgs e)
        {
            AllapotProgressBar.Value = 0;
            presenter.SendMail();
        }

        public string StatusText { set => AllapotLabel.Text = value; }

        public void ShowProgress(int progressPercentage, string userState = null)
        {
            AllapotProgressBar.Value = progressPercentage;
            if (!string.IsNullOrWhiteSpace(userState))
            {
                addressView.eredmenyKimenet += userState;
            }
        }
    }
}
