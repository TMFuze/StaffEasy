// VacancyDetailsWindow.xaml.cs
using StaffEasy.AppFiles;
using StaffEasy.AppFiles.DataBase;
using System.Linq;
using System.Windows;

namespace StaffEasy
{
    public partial class VacancyDetailsWindow : Window
    {
        private int? vacancyID;

        public VacancyDetailsWindow(int? id = null)
        {
            InitializeComponent();
            vacancyID = id;
            LoadDepartments();
            if (vacancyID.HasValue)
            {
                LoadVacancyDetails();
            }
        }

        private void LoadDepartments()
        {
            var departments = ConnectionDB.entobj.Departments.ToList();
            cmbDepartment.ItemsSource = departments;
        }

        private void LoadVacancyDetails()
        {
            var vacancy = ConnectionDB.entobj.Vacancies.Find(vacancyID.Value);
            if (vacancy != null)
            {
                txtTitle.Text = vacancy.Title;
                cmbDepartment.SelectedValue = vacancy.DepartmentID;
                txtDescription.Text = vacancy.Description;
                txtSalary.Text = vacancy.Salary.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (vacancyID.HasValue)
            {
                // Update existing vacancy
                var vacancy = ConnectionDB.entobj.Vacancies.Find(vacancyID.Value);
                if (vacancy != null)
                {
                    vacancy.Title = txtTitle.Text;
                    vacancy.DepartmentID = (int)cmbDepartment.SelectedValue;
                    vacancy.Description = txtDescription.Text;
                    vacancy.Salary = decimal.Parse(txtSalary.Text);
                }
            }
            else
            {
                // Add new vacancy
                var newVacancy = new Vacancies
                {
                    Title = txtTitle.Text,
                    DepartmentID = (int)cmbDepartment.SelectedValue,
                    Description = txtDescription.Text,
                    Salary = decimal.Parse(txtSalary.Text)
                };
                ConnectionDB.entobj.Vacancies.Add(newVacancy);
            }

            ConnectionDB.entobj.SaveChanges();
            this.DialogResult = true;
            this.Close();
        }
    }
}
