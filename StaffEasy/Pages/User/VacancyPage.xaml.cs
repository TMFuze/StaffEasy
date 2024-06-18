using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StaffEasy.AppFiles;
using StaffEasy.AppFiles.DataBase;

namespace StaffEasy.Pages.User
{
    public partial class VacancyPage : Page
    {
        public VacancyPage()
        {
            InitializeComponent();
            Loaded += VacancyPage_Loaded;
        }

        private void VacancyPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromDatabase();
            RefreshDepartmentsComboBox();
            ApplyFilter();
        }

        private void LoadDataFromDatabase()
        {
            if (DGVacancies == null) return;

            var query = from vacancy in ConnectionDB.entobj.Vacancies
                        join dept in ConnectionDB.entobj.Departments
                        on vacancy.DepartmentID equals dept.DepartmentID
                        select new
                        {
                            VacancyID = vacancy.VacancyID,
                            Title = vacancy.Title,
                            Department = dept.DepartmentName,
                            Description = vacancy.Description,
                            Salary = vacancy.Salary
                        };

            DGVacancies.ItemsSource = query.ToList();
        }

        private void RefreshDepartmentsComboBox()
        {
            if (cmbFilterDepartment == null) return;

            var departments = (from dept in ConnectionDB.entobj.Departments
                               select dept.DepartmentName).Distinct().ToList();

            cmbFilterDepartment.Items.Clear();
            cmbFilterDepartment.Items.Add(new ComboBoxItem { Content = "Все отделы", IsSelected = true });

            foreach (var department in departments)
            {
                cmbFilterDepartment.Items.Add(new ComboBoxItem { Content = department });
            }
        }

        private void ApplyFilter()
        {
            if (DGVacancies == null || cmbFilterDepartment == null || txtSearch == null || cmbSortBy == null) return;

            // Получаем все записи из базы данных
            var query = from vacancy in ConnectionDB.entobj.Vacancies
                        join dept in ConnectionDB.entobj.Departments
                        on vacancy.DepartmentID equals dept.DepartmentID
                        select new
                        {
                            VacancyID = vacancy.VacancyID,
                            Title = vacancy.Title,
                            Department = dept.DepartmentName,
                            Description = vacancy.Description,
                            Salary = vacancy.Salary
                        };

            var vacancies = query.ToList();

            // Применяем фильтрацию по отделу, если выбран конкретный отдел
            if (cmbFilterDepartment.SelectedItem != null &&
                cmbFilterDepartment.SelectedItem is ComboBoxItem selectedDepartmentItem &&
                selectedDepartmentItem.Content.ToString() != "Все отделы")
            {
                string selectedDepartment = selectedDepartmentItem.Content.ToString();
                vacancies = vacancies.Where(v => v.Department == selectedDepartment).ToList();
            }

            // Применяем фильтрацию по тексту поиска
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Поиск...")
            {
                string searchQuery = txtSearch.Text.ToLower();
                vacancies = vacancies.Where(v => v.Title.ToLower().Contains(searchQuery) ||
                                                 v.Description.ToLower().Contains(searchQuery) ||
                                                 v.Department.ToLower().Contains(searchQuery)).ToList();
            }

            // Применяем сортировку
            if (cmbSortBy.SelectedItem != null && cmbSortBy.SelectedItem is ComboBoxItem selectedSortItem)
            {
                switch (selectedSortItem.Content.ToString())
                {
                    case "От А до Я":
                        vacancies = vacancies.OrderBy(v => v.Title).ToList();
                        break;
                    case "От Я до А":
                        vacancies = vacancies.OrderByDescending(v => v.Title).ToList();
                        break;
                }
            }

            DGVacancies.ItemsSource = vacancies;
        }


        private void cmbFilterDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void cmbSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            cmbFilterDepartment.SelectedIndex = 0;
            cmbSortBy.SelectedIndex = 0;
            LoadDataFromDatabase();
        }

        private void DGVacancies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = DGVacancies.SelectedItem;
            if (selectedRow != null)
            {
                // Логика открытия карточки вакансии
                // Например, можно открыть новое окно для редактирования вакансии
            }
        }

        private void ButtonAddVacancy_Click(object sender, RoutedEventArgs e)
        {
            var vacancyWindow = new VacancyDetailsWindow();
            if (vacancyWindow.ShowDialog() == true)
            {
                ApplyFilter();
            }
        }

        private void ButtonEditVacancy_Click(object sender, RoutedEventArgs e)
        {
            if (DGVacancies.SelectedItem != null)
            {
                dynamic selectedVacancy = DGVacancies.SelectedItem;
                int vacancyID = selectedVacancy.VacancyID;
                var vacancyWindow = new VacancyDetailsWindow(vacancyID);
                if (vacancyWindow.ShowDialog() == true)
                {
                    ApplyFilter();
                }
            }
            else
            {
                MessageBox.Show("Выберите вакансию для редактирования.");
            }
        }

        private void btnDeleteVacancy_Click(object sender, RoutedEventArgs e)
        {

            // Проверяем, выбрана ли вакансия для удаления
            if (DGVacancies.SelectedItem != null)
            {
                // Получаем выбранную вакансию
                var selectedVacancy = (dynamic)DGVacancies.SelectedItem;

                // Получаем идентификатор выбранной вакансии
                int vacancyID = selectedVacancy.VacancyID;

                // Удаляем вакансию из базы данных
                var vacancyToRemove = ConnectionDB.entobj.Vacancies.Find(vacancyID);
                if (vacancyToRemove != null)
                {
                    ConnectionDB.entobj.Vacancies.Remove(vacancyToRemove);
                    ConnectionDB.entobj.SaveChanges();

                    // Обновляем отображение таблицы после удаления
                    LoadDataFromDatabase();
                }
            }
            else
            {
                // Если вакансия не выбрана, выведите сообщение об ошибке
                MessageBox.Show("Выберите вакансию для удаления.");
            }
        }

        private void JobSite1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://arzamas.hh.ru/employer?hhtmFrom=main",
                UseShellExecute = true
            });
        }

        private void JobSite2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://ir-center.ru/sznregion/jobs/jobform.asp?Region=52&Okato=40868&rn=%C0%F0%E7%E0%EC%E0%F1",
                UseShellExecute = true
            });
        }

        private void JobSite3_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://arzamas.superjob.ru/",
                UseShellExecute = true
            });
        }

        private void ManageDepartmentsButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.secfrmobj.Navigate(new Pages.User.DepartmentsPage());
        }
    }
}
