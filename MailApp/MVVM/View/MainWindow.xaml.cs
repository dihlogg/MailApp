using MailApp.MVVM.Model;
using MailApp.MVVM.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Email> emails;
        public MainWindow()
        {
            InitializeComponent();
            LoadEmails();
        }
        private void LoadEmails()
        {
            // Giả sử đây là danh sách email từ server (bạn có thể thay đổi việc lấy email từ server thật)
            emails = new List<Email>
            {
                new Email { Subject = "Chào bạn", Sender = "alice@gmail.com", ReceivedDate = DateTime.Now, Body = "Đây là nội dung email chào mừng."},
                new Email { Subject = "Thông báo", Sender = "bob@yahoo.com", ReceivedDate = DateTime.Now.AddDays(-1), Body = "Thông báo từ công ty."},
                new Email { Subject = "Ưu đãi mới", Sender = "sale@store.com", ReceivedDate = DateTime.Now.AddDays(-2), Body = "Mua ngay với giá ưu đãi."}
            };

            InboxListBox.ItemsSource = emails;
        }

        private void InboxListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (InboxListBox.SelectedItem is Email selectedEmail)
            {
                // Hiển thị chi tiết email khi được chọn
                EmailSubjectTextBlock.Text = selectedEmail.Subject;
                EmailSenderTextBlock.Text = selectedEmail.Sender;
                EmailDateTextBlock.Text = selectedEmail.ReceivedDate.ToString();
                EmailBodyTextBox.Text = selectedEmail.Body;
            }
        }
        private void OpenSendEmailWindow(object sender, RoutedEventArgs e)
        {
            SendEmailWindow sendEmailWindow = new SendEmailWindow();
            sendEmailWindow.Show();
        }
    }
}