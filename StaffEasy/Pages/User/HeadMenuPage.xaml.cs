using StaffEasy.AppFiles;
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

namespace StaffEasy.Pages.User
{
    /// <summary>
    /// Логика взаимодействия для HeadMenuPage.xaml
    /// </summary>
    public partial class HeadMenuPage : Page
    {
        public HeadMenuPage()
        {
            InitializeComponent();

            SecondFrame.secfrmobj = SecondMainFrame;
            SecondFrame.secfrmobj.Navigate(new Pages.WellcomePage());
            
        }

        

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.frmobj.Navigate(new Pages.AuthPage());
        }

        private void ButtonStaff_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.secfrmobj.Navigate(new Pages.User.PageStaff());
        }

        private void VacanciesButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.secfrmobj.Navigate(new Pages.User.VacancyPage());
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.secfrmobj.Navigate(new Pages.User.ArchivePage());
        }
    }
}
