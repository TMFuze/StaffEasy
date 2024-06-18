using StaffEasy.AppFiles;
using StaffEasy.Pages.Lawyer;
using StaffEasy.Pages.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace StaffEasy.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxbLog.Text) || string.IsNullOrWhiteSpace(PsbPass.Password))
                {

                    MessageBox.Show("Заполните все строки!",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                    return;
                }
                else
                {
                    var userObj = ConnectionDB.entobj.User
            .FirstOrDefault(x => x.Login.Equals(TxbLog.Text, StringComparison.OrdinalIgnoreCase)
                              && x.Password == PsbPass.Password);

                    if (userObj == null)
                    {
                        MessageBox.Show("Такой пользователь не найден",
                                    "Уведомление",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                    }
                    else
                    {






                        switch (userObj.IdRole)
                        {
                            case 1:
                                MessageBox.Show("Здравствуйте кадровик " + userObj.Name + "!",
                                   "Уведомление",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);

                                // Создайте объект UserMenu и перейдите на эту страницу
                                
                                FrameApp.frmobj.Navigate(new HeadMenuPage());
                                break;
                            case 2:
                                MessageBox.Show("Здравствуйте юрист " + userObj.Name + "!",
                                  "Уведомление",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                                // Создайте объект UserMenu и перейдите на эту страницу

                                FrameApp.frmobj.Navigate(new LawyerMainPage());
                                break;
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критический сбой в работе приложения: " + ex.Message.ToString(),
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

            }
        }
    }
}
