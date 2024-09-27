using MailApp.MVVM.Model;
using MailApp.MVVM.View;
using Microsoft.Win32;
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
        public MainWindow()
        {
            InitializeComponent();
            RegisterWindow registerWindow = new RegisterWindow();

            // Show the RegisterWindow
            registerWindow.Show();

            // Optionally, close the MainWindow if you don't want it visible
            this.Close();
        }
    }
}