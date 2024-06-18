using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using StaffEasy.AppFiles.DataBase;
using StaffEasy.AppFiles;

namespace StaffEasy.Pages.User
{
    public partial class ArchivePage : Page
    {
        public ArchivePage()
        {
            InitializeComponent();
            LoadData(); // Загрузка данных при инициализации страницы
        }

        private void LoadData()
        {
            try
            {
                using (var context = new HRDatabaseEntities()) // Замените HRDatabaseEntities на ваш контекст базы данных
                {
                    // Загрузка данных из таблицы Archive с включением связанных данных
                    var archiveData = context.Archive
                        .Include(a => a.CountryCode1)
                        .Include(a => a.DocType)
                        .Include(a => a.EducationForm)
                        .Include(a => a.EducationLevel)
                        .Include(a => a.FormOfEducationTermination1)
                        .Include(a => a.SourceOfFinancing1)
                        .Include(a => a.SpecializationCode1)
                        .ToList();

                    // Привязка данных к DataGrid
                    DGItems.ItemsSource = archiveData;

                    // Загрузка типов документов в ComboBox
                    var docTypes = context.DocType.ToList();
                    TypeOfDocSel.ItemsSource = docTypes;
                    TypeOfDocSel.DisplayMemberPath = "Name";
                    TypeOfDocSel.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void RefreshDGBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ApplyFilters();
        }

        private void TypeOfDocSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            try
            {
                using (var context = new HRDatabaseEntities())
                {
                    var query = context.Archive
                        .Include(a => a.CountryCode1)
                        .Include(a => a.DocType)
                        .Include(a => a.EducationForm)
                        .Include(a => a.EducationLevel)
                        .Include(a => a.FormOfEducationTermination1)
                        .Include(a => a.SourceOfFinancing1)
                        .Include(a => a.SpecializationCode1)
                        .AsQueryable();

                    // Фильтр по типу документа
                    if (TypeOfDocSel.SelectedValue != null)
                    {
                        int selectedDocTypeId = (int)TypeOfDocSel.SelectedValue;
                        query = query.Where(a => a.IdTypeDoc == selectedDocTypeId);
                    }

                    // Фильтр по поисковому запросу
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                    {
                        string searchText = SearchBox.Text.ToLower();
                        query = query.Where(a =>
                            a.DocName.ToLower().Contains(searchText) ||
                            a.DocSeries.ToLower().Contains(searchText) ||
                            a.DocNumber.ToLower().Contains(searchText) ||
                            a.SpecializationName.ToLower().Contains(searchText) ||
                            a.SurnameOfRecepient.ToLower().Contains(searchText) ||
                            a.NameOfRecepient.ToLower().Contains(searchText) ||
                            a.MiddleNameOfRecepinet.ToLower().Contains(searchText));
                    }

                    // Привязка отфильтрованных данных к DataGrid
                    DGItems.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка применения фильтров: {ex.Message}");
            }
        }

        private void DGItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
