using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using TodoListApp.Models;
using TodoListApp.Repositories;
using TodoListApp.Services;

namespace TodoListApp
{
    public partial class MainWindow : Window
    {
        private readonly TodoService _todoService;
        private TodoModel? _selectedTask;

        public MainWindow()
        {
            InitializeComponent();
            _todoService = new TodoService(new JsonTaskRepository());

            CmbPriority.ItemsSource = Enum.GetValues(typeof(TaskPriority));
            CmbStatus.ItemsSource = Enum.GetValues(typeof(TaskItemStatus));

            ClearForm();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            TasksDataGrid.ItemsSource = null;
            TasksDataGrid.ItemsSource = _todoService.GetAllTasks().ToList();
        }

        private void ClearForm()
        {
            _selectedTask = null;
            TxtTitle.Text = string.Empty;
            TxtDescription.Text = string.Empty;
            DpDueDate.SelectedDate = DateTime.Now.AddDays(1);
            CmbPriority.SelectedItem = TaskPriority.Medium;
            CmbStatus.SelectedItem = TaskItemStatus.NotStarted;
            TasksDataGrid.SelectedItem = null;
        }

        private void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is TodoModel selected)
            {
                _selectedTask = selected;
                TxtTitle.Text = selected.Title;
                TxtDescription.Text = selected.Description;
                DpDueDate.SelectedDate = selected.DueDate;
                CmbPriority.SelectedItem = selected.Priority;
                CmbStatus.SelectedItem = selected.Status;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var task = new TodoModel(
                    TxtTitle.Text,
                    TxtDescription.Text,
                    DpDueDate.SelectedDate ?? DateTime.Now.AddDays(1),
                    (TaskPriority)(CmbPriority.SelectedItem ?? TaskPriority.Medium),
                    (TaskItemStatus)(CmbStatus.SelectedItem ?? TaskItemStatus.NotStarted)
                );

                _todoService.AddTask(task);
                RefreshGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null)
            {
                MessageBox.Show("Please select a task from the table to edit.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                _todoService.UpdateTask(
                    _selectedTask.Id,
                    TxtTitle.Text,
                    TxtDescription.Text,
                    DpDueDate.SelectedDate ?? DateTime.Now.AddDays(1),
                    (TaskPriority)(CmbPriority.SelectedItem ?? TaskPriority.Medium)
                );

                _todoService.UpdateStatus(_selectedTask.Id, (TaskItemStatus)(CmbStatus.SelectedItem ?? TaskItemStatus.NotStarted));
                RefreshGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask == null)
            {
                MessageBox.Show("Please select a task to delete.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _todoService.DeleteTask(_selectedTask.Id);
            RefreshGrid();
            ClearForm();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                FileName = "tasks.json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _todoService.SaveToFile(dialog.FileName);
                    MessageBox.Show("Tasks saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _todoService.LoadFromFile(dialog.FileName);
                    RefreshGrid();
                    ClearForm();
                    MessageBox.Show("Tasks loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}