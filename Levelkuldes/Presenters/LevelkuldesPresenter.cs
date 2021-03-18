using Levelkuldes.Models;
using Levelkuldes.Properties;
using Levelkuldes.ViewInterfaces;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Levelkuldes.Presenters
{
    public class LevelkuldesPresenter
    {
        private IAddressView addressView;
        private IMessageView messageView;
        private EmailMessage model;

        public LevelkuldesPresenter(IMessageView messageView, IAddressView addressView)
        {
            this.messageView = messageView;
            this.addressView = addressView;
            model = new EmailMessage();
        }

        public void LoadMessage(string fajlUtvonal)
        {
            messageView.uzenetHTML = fajlUtvonal;
            model.HTMLBody = File.ReadAllText(fajlUtvonal);
        }

        public void LoadAddresses(string fajlUtvonal, string fajlNev)
        {
            model.ToAddresses = null;
            addressView.cimzettFajl = fajlNev;
            var fileExtension = Path.GetExtension(fajlNev);
            using (var sr = new StreamReader(fajlUtvonal))
            {
                while (!sr.EndOfStream)
                {
                    if (fileExtension == ".txt")
                    {
                        model.ToAddresses.Add(sr.ReadLine());
                    }
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
            
            model.From = messageView.Felado;
            model.Subject = messageView.Targy;

            addressView.eredmenyKimenet = "";

            // https://myaccount.google.com/lesssecureapps
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("*****@gmail.com", "*****");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("*****@gmail.com", model.From);
            mail.Subject = model.Subject;
            mail.Body = model.HTMLBody;
            mail.IsBodyHtml = true;

            foreach (var address in model.ToAddresses)
            {
                mail.To.Add(address);
                try
                {
                    smtpClient.Send(mail);
                    addressView.eredmenyKimenet += $"Sikeres üzenetküldés ide: {address}" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    addressView.eredmenyKimenet += "***************************************" + Environment.NewLine +
                                                   "Hiba: " + ex.Message + Environment.NewLine +
                                                   "***************************************" + Environment.NewLine;
                }
                mail.To.Clear();
            }
        }


    }
}
