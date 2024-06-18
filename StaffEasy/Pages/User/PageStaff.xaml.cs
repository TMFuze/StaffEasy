using StaffEasy.AppFiles;
using StaffEasy.AppFiles.DataBase;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace StaffEasy.Pages.User
{
    public partial class PageStaff : Page
    {
        public ObservableCollection<EmployeeDTO> Employees { get; set; }
        public ObservableCollection<Departments> Departments { get; set; }
        public ObservableCollection<Vacancies> Positions { get; set; }

        public PageStaff()
        {
            InitializeComponent();
            Loaded += PageStaff_Loaded; // Добавляем обработчик события Loaded страницы
        }

        private void PageStaff_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid(); // Обновляем данные в DataGrid при загрузке страницы
            LoadDepartments(); // Заполняем ComboBox отделов
            LoadPositions(); // Заполняем ComboBox должностей
            ApplyFilterAndSort(); // Применяем фильтрацию и сортировку после загрузки страницы
        }

        private void RefreshDataGrid()
        {
            var query = from emp in ConnectionDB.entobj.Employees
                        join dept in ConnectionDB.entobj.Departments
                        on emp.DepartmentID equals dept.DepartmentID
                        join vacancy in ConnectionDB.entobj.Vacancies
                        on emp.Position equals vacancy.VacancyID
                        select new EmployeeDTO
                        {
                            EmployeeID = emp.EmployeeID,
                            LastName = emp.LastName,
                            FirstName = emp.FirstName,
                            Position = vacancy.Title, // Title - поле с названием должности из таблицы Vacancies
                            Department = dept.DepartmentName,
                            StartDate = emp.StartDate
                        };

            Employees = new ObservableCollection<EmployeeDTO>(query.ToList());
            DGStaff.ItemsSource = Employees;
        }

        private void LoadDepartments()
        {
            var departments = ConnectionDB.entobj.Departments.ToList();
            Departments = new ObservableCollection<Departments>(departments);
            cmbDepartment.ItemsSource = Departments;
            sortByDepartment.Items.Clear();
            sortByDepartment.Items.Add(new ComboBoxItem { IsEnabled = false, Content = "Фильтр по отделам...", IsSelected = true });
            foreach (var department in departments)
            {
                sortByDepartment.Items.Add(new ComboBoxItem { Content = department.DepartmentName });
            }
        }

        private void LoadPositions()
        {
            var positions = ConnectionDB.entobj.Vacancies.ToList();
            Positions = new ObservableCollection<Vacancies>(positions);
            cmbPosition.ItemsSource = Positions;
        }

        private void ApplyFilterAndSort()
        {
            if (txtSearch != null && sortByDepartment != null && cmbSortBy != null)
            {
                var query = from emp in ConnectionDB.entobj.Employees
                            join dept in ConnectionDB.entobj.Departments
                            on emp.DepartmentID equals dept.DepartmentID
                            join vacancy in ConnectionDB.entobj.Vacancies
                            on emp.Position equals vacancy.VacancyID
                            select new EmployeeDTO
                            {
                                EmployeeID = emp.EmployeeID,
                                LastName = emp.LastName,
                                FirstName = emp.FirstName,
                                Position = vacancy.Title, // Title - поле с названием должности из таблицы Vacancies
                                Department = dept.DepartmentName,
                                StartDate = emp.StartDate
                            };

                // Применяем фильтр по тексту поиска
                if (txtSearch.Text != "Поиск..." && !string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    string searchQuery = txtSearch.Text.ToLower();
                    query = query.Where(item => item.LastName.ToLower().Contains(searchQuery) ||
                                                item.FirstName.ToLower().Contains(searchQuery) ||
                                                item.Position.ToLower().Contains(searchQuery) ||
                                                item.Department.ToLower().Contains(searchQuery));
                }

                // Применяем фильтр по отделу
                if (sortByDepartment.SelectedItem is ComboBoxItem selectedDepartmentItem &&
                    selectedDepartmentItem.Content.ToString() != "Фильтр по отделам...")
                {
                    string selectedDepartment = selectedDepartmentItem.Content.ToString();
                    query = query.Where(item => item.Department == selectedDepartment);
                }

                // Применяем сортировку
                if (cmbSortBy.SelectedItem is ComboBoxItem selectedSortItem)
                {
                    switch (selectedSortItem.Content.ToString())
                    {
                        case "От А до Я":
                            query = query.OrderBy(item => item.LastName);
                            break;
                        case "От Я до А":
                            query = query.OrderByDescending(item => item.LastName);
                            break;
                        default:
                            break;
                    }
                }

                DGStaff.ItemsSource = query.ToList();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void cmbSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void sortByDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterAndSort();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            // Сбросить значение текстового поля поиска
            txtSearch.Text = "";

            // Сбросить выбранный элемент в комбо-боксе для фильтрации по отделам
            sortByDepartment.SelectedIndex = 0;

            cmbSortBy.SelectedIndex = 0;

            // Вызвать метод RefreshDataGrid() для обновления данных без фильтров или сортировок
            RefreshDataGrid();
        }

        private void DGStaff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DGStaff.SelectedItem is EmployeeDTO selectedEmployee)
            {
                // Открываем новую страницу с карточкой сотрудника, передавая его ID
                SecondFrame.secfrmobj.Navigate(new EmployeeCardPage(selectedEmployee.EmployeeID));
            }
        }

        private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeForm.Visibility = Visibility.Visible;
        }

        private void CancelAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeForm.Visibility = Visibility.Collapsed;
        }

        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            string lastName = txtLastName.Text;
            string firstName = txtFirstName.Text;
            var selectedPosition = cmbPosition.SelectedItem as Vacancies;
            var selectedDepartment = cmbDepartment.SelectedItem as Departments;
            DateTime? startDate = dpStartDate.SelectedDate;

            if (!string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(firstName) && selectedPosition != null && selectedDepartment != null && startDate.HasValue)
            {
                // Добавляем нового сотрудника в базу данных
                var newEmployee = new Employees
                {
                    LastName = lastName,
                    FirstName = firstName,
                    Position = selectedPosition.VacancyID,
                    DepartmentID = selectedDepartment.DepartmentID,
                    StartDate = startDate.Value
                };
                ConnectionDB.entobj.Employees.Add(newEmployee);
                ConnectionDB.entobj.SaveChanges();

                // Обновляем список сотрудников
                RefreshDataGrid();

                // Сбрасываем поля формы
                txtLastName.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                cmbPosition.SelectedIndex = -1;
                cmbDepartment.SelectedIndex = -1;
                dpStartDate.SelectedDate = null;

                AddEmployeeForm.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime? StartDate { get; set; }
    }
}