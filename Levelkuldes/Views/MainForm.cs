using Levelkuldes.Presenters;
using System;
using System.Windows.Forms;

namespace Levelkuldes.Views
{
    public partial class MainForm : Form
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
            presenter = new LevelkuldesPresenter(messageView, addressView);
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
            openFileDialog1.Filter = "Egyszerű szöveg (*.txt)|*.txt|" +
                "CSV (pontosvesszővel tagolt) (*.csv)|*.csv";
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
            presenter.SendMail();
        }
    }
}
