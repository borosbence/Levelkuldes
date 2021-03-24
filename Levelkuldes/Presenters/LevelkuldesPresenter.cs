using Levelkuldes.Models;
using Levelkuldes.Properties;
using Levelkuldes.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Levelkuldes.Presenters
{
    public class LevelkuldesPresenter
    {
        private IMainView mainView;
        private IAddressView addressView;
        private IMessageView messageView;
        private EmailMessage model;

        private BackgroundWorker _bw;
        private string fileExtension;
        private int colIndex;

        public LevelkuldesPresenter(IMainView mainV, IMessageView messageV, IAddressView addressV)
        {
            mainView = mainV;
            messageView = messageV;
            addressView = addressV;
            model = new EmailMessage();
            mainView.Allapot = Resources.AdatokImportalasa;
        }

        public void LoadMessage(string fajlUtvonal)
        {
            messageView.uzenetHTML = fajlUtvonal;
            mainView.Allapot = Resources.LevelBetoltve;
            model.HTMLBody = File.ReadAllText(fajlUtvonal);
        }

        public void LoadAddresses(string fajlUtvonal, string fajlNev)
        {
            model.ToAddresses = new List<string>();
            addressView.cimzettFajl = fajlNev;
            mainView.Allapot = Resources.CimzettekBetoltve;

            fileExtension = Path.GetExtension(fajlNev);
            using (var sr = new StreamReader(fajlUtvonal))
            {
                addressView.Fejlecek = null;
                if (fileExtension == ".csv")
                {
                    addressView.Fejlecek = sr.ReadLine().Split(';');
                }
                while (!sr.EndOfStream)
                {
                    model.ToAddresses.Add(sr.ReadLine());
                }
            }
            addressView.BeolvasottElemek = model.ToAddresses.Count;
        }

        private bool Validate()
        {
            messageView.errorFelado = null;
            messageView.errorTargy = null;
            addressView.errorCimzettFajl = null;
            bool valid = true;

            if (string.IsNullOrWhiteSpace(messageView.Felado))
            {
                messageView.errorFelado = Resources.KotelezoMezo;
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(messageView.Targy))
            {
                messageView.errorTargy = Resources.KotelezoMezo;
                valid = false;
            }
            if (!valid)
            {
                messageView.ShowError();
            }
            if (string.IsNullOrWhiteSpace(addressView.cimzettFajl) || addressView.cimzettFajl == "üres")
            {
                addressView.errorCimzettFajl = Resources.NincsFajl;
                valid = false;
            }

            return valid;
        }

        public void SendMail()
        {
            if (!Validate())
            {
                return;
            }

            mainView.Allapot = Resources.LevelkuldesFolyamat;
            addressView.eredmenyKimenet = "";
            colIndex = addressView.cimzettOszlop;

            _bw = new BackgroundWorker();
            _bw.WorkerReportsProgress = true;
            _bw.DoWork += new DoWorkEventHandler(_bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(_bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bw_Completed);

            _bw.RunWorkerAsync();
        }

        void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            model.From = messageView.Felado;
            model.Subject = messageView.Targy;

            // https://myaccount.google.com/lesssecureapps
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("****@gmail.com", "****");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("****@gmail.com", model.From);
            mail.Subject = model.Subject;
            mail.Body = model.HTMLBody;
            mail.IsBodyHtml = true;

            int counter = 0;
            string eredmeny = null;
            foreach (var row in model.ToAddresses)
            {
                string mailAddress = row;
                if (fileExtension == ".csv")
                {
                    var lineArray = row.Split(';');
                    mailAddress = lineArray[colIndex];
                }
                try
                {
                    mail.To.Add(mailAddress);
                    // System.Threading.Thread.Sleep(1000);
                    smtpClient.Send(mail);
                    eredmeny = $"Sikeres üzenetküldés ide: {mailAddress}" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    eredmeny = "***************************************" + Environment.NewLine +
                                "Hiba: " + ex.Message + Environment.NewLine +
                                "***************************************" + Environment.NewLine;
                }
                mail.To.Clear();
                counter++;
                int percentage = counter * 100 / model.ToAddresses.Count;
                _bw.ReportProgress(percentage, eredmeny);
            }
        }

        void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainView.ShowProgress(e.ProgressPercentage, e.UserState.ToString());
        }

        void _bw_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            mainView.ShowProgress(100);
            mainView.Allapot = Resources.LevelekElkuldve;
        }
    }
}
