using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace EdinoeOkno_program
{
    public class MailSend
    {
        //   MMail.SendMail("Почта", "Тема сообщения", "Текс сообщения");
        public static void Send(string addressee, string Subject, string messageText)
        {
            try
            {

                SmtpClient mySmtpClient = new SmtpClient("smtp.mail.ru"); //Сервер исходящей почты (SMTP-сервер)
                // set smtp-client with basicAuthentication
                mySmtpClient.UseDefaultCredentials = true;
                mySmtpClient.EnableSsl = true; // использование шифрования по SSL

                System.Net.NetworkCredential basicAuthenticationInfo = new
                   System.Net.NetworkCredential("edinoeokno@internet.ru", "Y23xgi7F4zFZLMvbBxVz"); //авторизация от кого отправляются письма
                mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                MailAddress from = new MailAddress("edinoeokno@internet.ru", "МФЦ \"Единое Окно\"");// от кого с именем отправителя 
                MailAddress to = new MailAddress(addressee, "Студенту"); // кому
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                // add ReplyTo
                MailAddress replyTo = new MailAddress("edinoeokno@internet.ru"); // этот емаил будет подставлятся при нажатии кнопки "ответить на сообщение"
                myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                myMail.Subject = Subject;    // тема сообшения
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = messageText;  // текст сообщения в формате text or html (если html, то нужно сделать myMail.IsBodyHtml = true)
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = false;

                mySmtpClient.Send(myMail);
            }

            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message);
                //throw new ApplicationException
                  //("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw ex;
            }

            MessageBox.Show("Отправлено");
        }

    }
}