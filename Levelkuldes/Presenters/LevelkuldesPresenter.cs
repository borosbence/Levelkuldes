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

        private BackgroundWorker bw;
        private string fileExtension;
        private int colIndex;

        public LevelkuldesPresenter(IMainView mainV, IMessageView messageV, IAddressView addressV)
        {
            mainView = mainV;
            messageView = messageV;
            addressView = addressV;
            model = new EmailMessage();
            mainView.StatusText = Resources.ImportalasSzukseges;
        }

        public void LoadMessage(string fajlUtvonal)
        {
            messageView.uzenetFajl = fajlUtvonal;
            mainView.StatusText = Resources.LevelBetoltve;
            model.HTMLBody = File.ReadAllText(fajlUtvonal);
        }

        public void LoadAddresses(string fajlUtvonal, string fajlNev)
        {
            model.ToAddresses = new List<string>();
            addressView.cimzettFajl = fajlNev;
            mainView.StatusText = Resources.CimzettekBetoltve;

            fileExtension = Path.GetExtension(fajlNev);
            using (var sr = new StreamReader(fajlUtvonal))
            {
                addressView.fejlecek = null;
                if (fileExtension == ".csv")
                {
                    addressView.fejlecek = sr.ReadLine().Split(';');
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

            mainView.StatusText = Resources.LevelekKuldese;
            addressView.eredmenyKimenet = "";
            colIndex = addressView.cimzettOszlop;

            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_Completed);
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
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

            double counter = 0;
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
                counter++;
                double percentage = (counter / model.ToAddresses.Count) * 100;
                // itt lehet int típus a counter változó
                // int percentage = counter * 100 / model.ToAddresses.Count;
                bw.ReportProgress(Convert.ToInt32(percentage), eredmeny);
                // Kiüríti a címzettek listáját, hogy ne ismételje a leveleket
                mail.To.Clear();
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainView.ShowProgress(e.ProgressPercentage, e.UserState.ToString());
            mainView.StatusText = Resources.LevelekKuldese + " " + e.ProgressPercentage + "%";
        }

        void bw_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            mainView.ShowProgress(100);
            mainView.StatusText = Resources.LevelekElkuldve;
        }
    }
}
