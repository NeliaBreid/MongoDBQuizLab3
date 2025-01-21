using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
using QuizLab3.Command;
using QuizLab3.Data;
using QuizLab3.Model;
using QuizLab3.Repositories;

namespace QuizLab3.ViewModel
{
    class CategoryViewModel : ViewModelBase
    {
        //private Category? _selectedCategoryToEdit; 
        //private Category? _newCategoryName;
        //private readonly MainWindowViewModel? mainWindowViewModel;
        //public ObservableCollection<Category> AllCategories { get; set; } //TODO: fixa en metod som lägge till kategorierna här
        //public Category CurrentCategory
        //{
        //    get => _newCategoryName;
        //    set
        //    {
        //        _newCategoryName = value;
        //        RaisePropertyChanged(nameof(CurrentCategory));
        //    }
        //}

        //public Category? SelectedCategoryToEdit
        //{
        //    get => _selectedCategoryToEdit;
        //    set
        //    {
        //        _selectedCategoryToEdit = value;
        //        CurrentCategory = value != null ? new Category { Id = value.Id, Name = value.Name } : new Category();
        //        RaisePropertyChanged(nameof(SelectedCategoryToEdit));
        //    }
        //}

        //private readonly CategoryRepository _categoryRepository;

        //public DelegateCommand UpdateCategoryCommand { get; }
        //public DelegateCommand DeleteCategoryCommand { get; }
        //public DelegateCommand ClearCategoryNameCommand { get; }
        //public CategoryViewModel(MainWindowViewModel? mainWindowViewModel) 
        //{
        //    this.mainWindowViewModel = mainWindowViewModel;

        //    _categoryRepository = new CategoryRepository();

        //    UpdateCategoryCommand = new DelegateCommand(UpdateOrAddCategory, CanUpdateOrAddCategory);
        //    DeleteCategoryCommand = new DelegateCommand(RemoveCategory);

        //    ClearCategoryNameCommand = new DelegateCommand(ClearTextBox);
        //    LoadCategories(); //TODO: sätta den här i början av allt.
        //}
        //public async Task LoadCategories() //TODO: flytta. Laddar in Categorier, om det inte finns några Categorier så laddar den in default kategorier
        //{
        //    if (AllCategories == null)
        //    {
        //        DataBaseInitializer.SetDefaultCategory();
        //    }

        //    AllCategories = new ObservableCollection<Category>(await _categoryRepository.GetAllCategoriesAsync());
        //    RaisePropertyChanged(nameof(AllCategories));
        //}
        //private void ClearTextBox(object parameter)
        //{
        //    SelectedCategoryToEdit = null;
        //    CurrentCategory.Name = string.Empty;
        //}
        //private void ClearNewCategory()
        //{
        //    SelectedCategoryToEdit = null;
        //    CurrentCategory.Name = string.Empty;
        //}

        //private async void UpdateOrAddCategory(object parameter)
        //{
        //    if (string.IsNullOrEmpty(CurrentCategory.Name)) //Kan inte lämna den tom och trycka update
        //    {
        //        MessageBox.Show("Kategorins namn kan inte vara tomt.");
        //        return;
        //    }

        //    if (CurrentCategory.Id == ObjectId.Empty) // Kontrollera om det är en ny kategori jämför på Id
        //    {
        //        var existingCategory = AllCategories.FirstOrDefault(c =>
        //            c.Name.Equals(CurrentCategory.Name, StringComparison.OrdinalIgnoreCase));

        //        if (existingCategory != null) //Kan inte lägga till två av samma
        //        {
        //            MessageBox.Show("En kategori med samma namn finns redan.");
        //            return;
        //        }

        //        _categoryRepository.AddCategory(CurrentCategory); // Lägg till ny kategori

        //    }
        //    else
        //    {
        //        _categoryRepository.UpdateCategory(CurrentCategory); // Uppdatera befintlig kategori
        //    }

        //    LoadCategories();
        //    ClearNewCategory();
        //}
        //private bool CanUpdateOrAddCategory(object parameter)
        //{
        //    // Kan bara uppdatera eller lägga till om en kategori är vald eller en ny kategori finns
        //    return SelectedCategoryToEdit != null || CurrentCategory != null;
        //}
        //private void RemoveCategory(object parameter)
        //{
        //    _categoryRepository.RemoveCategory(SelectedCategoryToEdit);
        //    LoadCategories();
        //    ClearNewCategory();
        //    RaisePropertyChanged(nameof(AllCategories));
        //}
    }
}
