using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using StaffEasy.AppFiles;
using StaffEasy.AppFiles.DataBase;
using System.Collections.Generic;

namespace StaffEasy.Pages.User
{
    public partial class EmployeeCardPage : Page
    {
        private int employeeID;

        public ObservableCollection<EducationDisplay> EducationList { get; set; }
        public ObservableCollection<WorkExperienceDisplay> WorkExperienceList { get; set; }
        public ObservableCollection<MedicalRecordDisplay> MedicalRecordList { get; set; }

        public ObservableCollection<string> Positions { get; set; }
        public ObservableCollection<string> Departments { get; set; }

        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string StartDate { get; set; }

        public class EducationDisplay
        {
            public int EducationID { get; set; }
            public string EducationInstitution { get; set; }
            public string EducationLevel { get; set; }
            public string EducationSpecialty { get; set; }
            public string GraduationDate { get; set; }
        }

        public class WorkExperienceDisplay
        {
            public int WorkExperienceID { get; set; }
            public string Company { get; set; }
            public string JobTitle { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Department { get; set; }
        }

        public class MedicalRecordDisplay
        {
            public int MedicalRecordID { get; set; }
            public string RecordDate { get; set; }
            public string MedicalInfo { get; set; }
        }

        public EmployeeCardPage(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            Loaded += EmployeeCardPage_Loaded;

            EducationList = new ObservableCollection<EducationDisplay>();
            WorkExperienceList = new ObservableCollection<WorkExperienceDisplay>();
            MedicalRecordList = new ObservableCollection<MedicalRecordDisplay>();

            Positions = new ObservableCollection<string>();
            Departments = new ObservableCollection<string>();

            icEducation.ItemsSource = EducationList;
            icWorkExperience.ItemsSource = WorkExperienceList;
            icMedicalRecords.ItemsSource = MedicalRecordList;

            DataContext = this;
        }

        private void EmployeeCardPage_Loaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadEmployeeDetails();
        }

        private void LoadEmployeeDetails()
        {
            var employee = (from emp in ConnectionDB.entobj.Employees
                            join dept in ConnectionDB.entobj.Departments
                            on emp.DepartmentID equals dept.DepartmentID
                            join vacancy in ConnectionDB.entobj.Vacancies
                            on emp.Position equals vacancy.VacancyID
                            where emp.EmployeeID == employeeID
                            select new
                            {
                                emp,
                                dept.DepartmentName,
                                vacancy.Title
                            }).FirstOrDefault();

            if (employee != null)
            {
                EmployeeName = $"{employee.emp.FirstName} {employee.emp.LastName} {employee.emp.MiddleName}";
                Position = employee.Title;
                Department = employee.DepartmentName;
                StartDate = employee.emp.StartDate?.ToString("dd/MM/yyyy") ?? string.Empty;

                txtLastName.Text = employee.emp.LastName;
                txtFirstName.Text = employee.emp.FirstName;
                txtMiddleName.Text = employee.emp.MiddleName;
                txtStartDate.Text = employee.emp.StartDate.HasValue ? employee.emp.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                txtDateOfBirth.Text = employee.emp.DateOfBirth.ToString("dd/MM/yyyy") ?? string.Empty;
                txtPlaceOfBirth.Text = employee.emp.PlaceOfBirth;
                txtGender.Text = employee.emp.Gender;
                txtCitizenship.Text = employee.emp.Citizenship;
                txtRegistrationAddress.Text = employee.emp.RegistrationAddress;
                txtResidentialAddress.Text = employee.emp.ResidentialAddress;
                txtPhone.Text = employee.emp.Phone;
                txtEmail.Text = employee.emp.Email;

                txtPassportSeries.Text = employee.emp.PassportSeries;
                txtPassportNumber.Text = employee.emp.PassportNumber;
                txtPassportIssuedBy.Text = employee.emp.PassportIssuedBy;
                txtPassportIssueDate.Text = employee.emp.PassportIssueDate?.ToString("dd/MM/yyyy") ?? string.Empty;

                txtINN.Text = employee.emp.INN;
                txtSNILS.Text = employee.emp.SNILS;

                txtMilitaryID.Text = employee.emp.MilitaryID;
                txtMilitaryDocumentType.Text = employee.emp.MilitaryDocumentType;

                txtBirthCertificate.Text = employee.emp.BirthCertificate;

                // Заполнение списков образования, опыта работы и медицинских записей
                LoadEducationList(employee.emp.Education);
                LoadWorkExperienceList(employee.emp.WorkExperience);
                LoadMedicalRecordList(employee.emp.MedicalRecords);

                // Загрузка данных для ComboBox
                LoadComboBoxData();

                // Установка выбранных значений ComboBox
                cbPosition.SelectedItem = Position;
                cbDepartment.SelectedItem = Department;
            }
        }

        private void LoadEducationList(ICollection<Education> educationList)
        {
            EducationList.Clear();
            foreach (var education in educationList)
            {
                EducationList.Add(new EducationDisplay
                {
                    EducationID = education.EducationID,
                    EducationInstitution = education.EducationInstitution,
                    EducationLevel = education.EducationLevel,
                    EducationSpecialty = education.EducationSpecialty,
                    GraduationDate = education.GraduationDate?.ToString("dd/MM/yyyy") ?? string.Empty
                });
            }
        }

        private void LoadWorkExperienceList(ICollection<WorkExperience> workExperienceList)
        {
            WorkExperienceList.Clear();
            foreach (var work in workExperienceList)
            {
                WorkExperienceList.Add(new WorkExperienceDisplay
                {
                    WorkExperienceID = work.WorkExperienceID,
                    Company = work.Company,
                    JobTitle = work.JobTitle,
                    StartDate = work.StartDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    EndDate = work.EndDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    Department = work.Department
                });
            }
        }

        private void LoadMedicalRecordList(ICollection<MedicalRecords> medicalRecordList)
        {
            MedicalRecordList.Clear();
            foreach (var medical in medicalRecordList)
            {
                MedicalRecordList.Add(new MedicalRecordDisplay
                {
                    MedicalRecordID = medical.MedicalRecordID,
                    RecordDate = medical.RecordDate?.ToString("dd/MM/yyyy") ?? string.Empty,
                    MedicalInfo = medical.MedicalInfo
                });
            }
        }

        private void LoadComboBoxData()
        {
            // Заполнение списка должностей
            Positions.Clear();
            foreach (var pos in ConnectionDB.entobj.Vacancies.Select(v => v.Title).Distinct())
            {
                Positions.Add(pos);
            }

            // Заполнение списка отделов
            Departments.Clear();
            foreach (var dept in ConnectionDB.entobj.Departments.Select(d => d.DepartmentName).Distinct())
            {
                Departments.Add(dept);
            }

            cbPosition.ItemsSource = Positions;
            cbDepartment.ItemsSource = Departments;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            var employee = ConnectionDB.entobj.Employees.FirstOrDefault(emp => emp.EmployeeID == employeeID);

            if (employee != null)
            {
                // Получение данных из текстбоксов
                employee.LastName = txtLastName.Text;
                employee.FirstName = txtFirstName.Text;
                employee.MiddleName = txtMiddleName.Text;

                // Сохранение выбранных значений из ComboBox
                employee.Position = ConnectionDB.entobj.Vacancies.FirstOrDefault(v => v.Title == Position)?.VacancyID;
                employee.DepartmentID = ConnectionDB.entobj.Departments.FirstOrDefault(d => d.DepartmentName == Department)?.DepartmentID;

                // Сохранение основной информации
                employee.DateOfBirth = (DateTime)(DateTime.TryParse(txtDateOfBirth.Text, out DateTime dob) ? dob : (DateTime?)null);
                employee.StartDate = DateTime.TryParse(StartDate, out DateTime startDate) ? startDate : (DateTime?)null;
                employee.PlaceOfBirth = txtPlaceOfBirth.Text;
                employee.Gender = txtGender.Text;
                employee.Citizenship = txtCitizenship.Text;
                employee.RegistrationAddress = txtRegistrationAddress.Text;
                employee.ResidentialAddress = txtResidentialAddress.Text;
                employee.Phone = txtPhone.Text;
                employee.Email = txtEmail.Text;
                employee.PassportSeries = txtPassportSeries.Text;
                employee.PassportNumber = txtPassportNumber.Text;
                employee.PassportIssuedBy = txtPassportIssuedBy.Text;
                employee.PassportIssueDate = DateTime.TryParse(txtPassportIssueDate.Text, out DateTime pid) ? pid : (DateTime?)null;
                employee.INN = txtINN.Text;
                employee.SNILS = txtSNILS.Text;
                employee.MilitaryID = txtMilitaryID.Text;
                employee.MilitaryDocumentType = txtMilitaryDocumentType.Text;
                employee.BirthCertificate = txtBirthCertificate.Text;

                // Сохранение данных из списков образования, опыта работы и медицинских записей
                SaveEducationList(employee);
                SaveWorkExperienceList(employee);
                SaveMedicalRecordList(employee);

                try
                {
                    ConnectionDB.entobj.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveEducationList(Employees employee)
        {
            foreach (var education in EducationList)
            {
                var graduationDate = DateTime.TryParse(education.GraduationDate, out DateTime parsedGraduationDate) ? (DateTime?)parsedGraduationDate : null;
                var edu = new Education
                {
                    EducationID = education.EducationID,
                    EducationInstitution = education.EducationInstitution,
                    EducationLevel = education.EducationLevel,
                    EducationSpecialty = education.EducationSpecialty,
                    GraduationDate = graduationDate,
                    EmployeeID = employee.EmployeeID
                };

                if (education.EducationID == 0)
                {
                    ConnectionDB.entobj.Education.Add(edu);
                }
                else
                {
                    var existingEducation = ConnectionDB.entobj.Education.FirstOrDefault(e => e.EducationID == education.EducationID);
                    if (existingEducation != null)
                    {
                        existingEducation.EducationInstitution = edu.EducationInstitution;
                        existingEducation.EducationLevel = edu.EducationLevel;
                        existingEducation.EducationSpecialty = edu.EducationSpecialty;
                        existingEducation.GraduationDate = edu.GraduationDate;
                    }
                }
            }
        }

        private void SaveWorkExperienceList(Employees employee)
        {
            foreach (var workExperience in WorkExperienceList)
            {
                var startDate = DateTime.TryParse(workExperience.StartDate, out DateTime parsedStartDate) ? (DateTime?)parsedStartDate : null;
                var endDate = DateTime.TryParse(workExperience.EndDate, out DateTime parsedEndDate) ? (DateTime?)parsedEndDate : null;
                var work = new WorkExperience
                {
                    WorkExperienceID = workExperience.WorkExperienceID,
                    Company = workExperience.Company,
                    JobTitle = workExperience.JobTitle,
                    StartDate = startDate,
                    EndDate = endDate,
                    Department = workExperience.Department,
                    EmployeeID = employee.EmployeeID
                };

                if (workExperience.WorkExperienceID == 0)
                {
                    ConnectionDB.entobj.WorkExperience.Add(work);
                }
                else
                {
                    var existingWorkExperience = ConnectionDB.entobj.WorkExperience.FirstOrDefault(w => w.WorkExperienceID == workExperience.WorkExperienceID);
                    if (existingWorkExperience != null)
                    {
                        existingWorkExperience.Company = work.Company;
                        existingWorkExperience.JobTitle = work.JobTitle;
                        existingWorkExperience.StartDate = work.StartDate;
                        existingWorkExperience.EndDate = work.EndDate;
                        existingWorkExperience.Department = work.Department;
                    }
                }
            }
        }

        private void SaveMedicalRecordList(Employees employee)
        {
            foreach (var medical in MedicalRecordList)
            {
                var recordDate = DateTime.TryParse(medical.RecordDate, out DateTime parsedRecordDate) ? (DateTime?)parsedRecordDate : null;
                var medicalRecord = new MedicalRecords
                {
                    MedicalRecordID = medical.MedicalRecordID,
                    RecordDate = recordDate,
                    MedicalInfo = medical.MedicalInfo,
                    EmployeeID = employee.EmployeeID
                };

                if (medical.MedicalRecordID == 0)
                {
                    ConnectionDB.entobj.MedicalRecords.Add(medicalRecord);
                }
                else
                {
                    var existingMedicalRecord = ConnectionDB.entobj.MedicalRecords.FirstOrDefault(m => m.MedicalRecordID == medical.MedicalRecordID);
                    if (existingMedicalRecord != null)
                    {
                        existingMedicalRecord.RecordDate = medicalRecord.RecordDate;
                        existingMedicalRecord.MedicalInfo = medicalRecord.MedicalInfo;
                    }
                }
            }
        }

        private void AddEducation_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            EducationList.Add(new EducationDisplay());
        }

        private void AddExperience_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            WorkExperienceList.Add(new WorkExperienceDisplay());
        }

        private void AddMedicalRecord_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            MedicalRecordList.Add(new MedicalRecordDisplay());
        }

        private void DeleteEducation_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is Button button && button.DataContext is EducationDisplay education)
            {
                if (education.EducationID != 0)
                {
                    var existingEducation = ConnectionDB.entobj.Education.FirstOrDefault(edu => edu.EducationID == education.EducationID);
                    if (existingEducation != null)
                    {
                        ConnectionDB.entobj.Education.Remove(existingEducation);
                    }
                }
                EducationList.Remove(education);
            }
        }

        private void DeleteWorkExperience_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is Button button && button.DataContext is WorkExperienceDisplay workExperience)
            {
                if (workExperience.WorkExperienceID != 0)
                {
                    var existingWorkExperience = ConnectionDB.entobj.WorkExperience.FirstOrDefault(work => work.WorkExperienceID == workExperience.WorkExperienceID);
                    if (existingWorkExperience != null)
                    {
                        ConnectionDB.entobj.WorkExperience.Remove(existingWorkExperience);
                    }
                }
                WorkExperienceList.Remove(workExperience);
            }
        }

        private void DeleteMedicalRecord_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (sender is Button button && button.DataContext is MedicalRecordDisplay medicalRecord)
            {
                if (medicalRecord.MedicalRecordID != 0)
                {
                    var existingMedicalRecord = ConnectionDB.entobj.MedicalRecords.FirstOrDefault(medical => medical.MedicalRecordID == medicalRecord.MedicalRecordID);
                    if (existingMedicalRecord != null)
                    {
                        ConnectionDB.entobj.MedicalRecords.Remove(existingMedicalRecord);
                    }
                }
                MedicalRecordList.Remove(medicalRecord);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            NavigationService.GoBack();
        }

       
    }
}
