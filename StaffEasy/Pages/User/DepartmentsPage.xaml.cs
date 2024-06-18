using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StaffEasy.AppFiles.DataBase;

namespace StaffEasy.Pages.User
{
    public partial class DepartmentsPage : Page
    {
        private HRDatabaseEntities _context;

        public DepartmentsPage()
        {
            InitializeComponent();
            _context = new HRDatabaseEntities();
            LoadDepartments();
            UpdatePlaceholderVisibility();
        }

        private void txtDepartmentName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            txtDepartmentNamePlaceholder.Visibility = string.IsNullOrEmpty(txtDepartmentName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadDepartments()
        {
            var departments = _context.Departments.ToList();
            DGDepartments.ItemsSource = departments;
        }

        private void ButtonAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDepartmentName.Text))
            {
                var newDepartment = new Departments
                {
                    DepartmentName = txtDepartmentName.Text
                };
                _context.Departments.Add(newDepartment);
                _context.SaveChanges();
                LoadDepartments();
                txtDepartmentName.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название отдела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonEditDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DGDepartments.SelectedItem is Departments selectedDepartment)
            {
                if (!string.IsNullOrEmpty(txtDepartmentName.Text))
                {
                    selectedDepartment.DepartmentName = txtDepartmentName.Text;
                    _context.SaveChanges();
                    LoadDepartments();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите название отдела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отдел для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DGDepartments_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DGDepartments.SelectedItem is Departments selectedDepartment)
            {
                txtDepartmentName.Text = selectedDepartment.DepartmentName;
            }
        }

        private void ButtonDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DGDepartments.SelectedItem is Departments selectedDepartment)
            {
                _context.Departments.Remove(selectedDepartment);
                _context.SaveChanges();
                LoadDepartments();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите отдел для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
