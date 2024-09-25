using System.IO;
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
using System;

namespace MailServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string baseDirectory = @"C:\MailServer";
        private string emailDirectory = @"C:\MailServer\Emails";

        public MainWindow()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }

            var directories = Directory.GetDirectories(baseDirectory);
            AccountList.Items.Clear();
            foreach (var dir in directories)
            {
                AccountList.Items.Add(System.IO.Path.GetFileName(dir));
            }
        }

        private void AccountList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountList.SelectedItem != null)
            {
                string accountName = AccountList.SelectedItem.ToString();
                string accountPath = System.IO.Path.Combine(baseDirectory, accountName);

                var emailFiles = Directory.GetFiles(accountPath);
                EmailList.Items.Clear();
                foreach (var file in emailFiles)
                {
                    EmailList.Items.Add(System.IO.Path.GetFileName(file));
                }
            }
        }

        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmailList.SelectedItem != null)
            {
                string accountName = AccountList.SelectedItem.ToString();
                string fileName = EmailList.SelectedItem.ToString();
                string filePath = System.IO.Path.Combine(baseDirectory, accountName, fileName);

                EmailContent.Text = File.ReadAllText(filePath);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
        }
    }
}