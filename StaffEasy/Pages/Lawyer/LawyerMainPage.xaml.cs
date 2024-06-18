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

namespace StaffEasy.Pages.Lawyer
{
    /// <summary>
    /// Логика взаимодействия для LawyerMainPage.xaml
    /// </summary>
    public partial class LawyerMainPage : Page
    {
        public LawyerMainPage()
        {
            InitializeComponent();

            ThirdFrame.thirdFrm = ThirdMainFrame;
            ThirdFrame.thirdFrm.Navigate(new Pages.WellcomePage());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            ThirdFrame.thirdFrm.Navigate(new AuthPage());
        }

        private void ButtonStaff_Click(object sender, RoutedEventArgs e)
        {
            ThirdFrame.thirdFrm.Navigate(new LawyerStaffPage());
        }
    }
}
