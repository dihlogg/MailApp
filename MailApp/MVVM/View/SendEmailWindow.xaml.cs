using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MailApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for SendEmailWindow.xaml
    /// </summary>
    public partial class SendEmailWindow : Window
    {
        public SendEmailWindow()
        {
            InitializeComponent();
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin từ các trường nhập liệu
            string recipient = RecipientTextBox.Text;
            string subject = SubjectTextBox.Text;
            string body = BodyTextBox.Text;

            // Gọi phương thức để gửi email
            try
            {
                SendEmail(recipient, subject, body);
                MessageBox.Show("Email đã được gửi thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi gửi email: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmail(string recipient, string subject, string body)
        {
            // Cấu hình máy chủ SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.your-email-server.com") // thay đổi với máy chủ email thật
            {
                Port = 587, // cổng SMTP (có thể là 465 hoặc 587 tùy dịch vụ email)
                Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
                EnableSsl = true, // Sử dụng SSL nếu cần
            };

            // Tạo đối tượng MailMessage để gửi email
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("your-email@example.com"), // Thay đổi bằng email của bạn
                Subject = subject,
                Body = body,
                IsBodyHtml = false // Đặt thành true nếu nội dung email là HTML
            };

            mail.To.Add(recipient); // Thêm người nhận

            // Gửi email
            smtpClient.Send(mail);
        }
    }
}
