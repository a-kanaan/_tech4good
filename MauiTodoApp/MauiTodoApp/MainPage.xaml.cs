using MauiTodoApp.Data;
using MauiTodoApp.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiTodoApp
{
    public partial class MainPage : ContentPage
    {
        readonly Database _database;
        StringBuilder todoListData;
        public ObservableCollection<TodoItem> Todos { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();

            _database = new Database();
            todoListData = new StringBuilder();

            _ = InitializeTodos();
        }

        private async Task InitializeTodos()
        {
            foreach (TodoItem todo in await _database.GetTodos())
            {
                //todoListData.Append($"{todo.Title} - {todo.Due.ToShortDateString()}{Environment.NewLine}");
                //await Task.Delay(1000);
                Todos.Add(todo);
            }

            //lblTodos.Text = todoListData.ToString();
        }

        private async void btnAddTodo_Clicked(object sender, EventArgs e)
        {
            var todo = new TodoItem()
            {
                Title = enTodoTitle.Text,
                Due = dpDueDate.Date
            };

            var inserted = await _database.AddTodo(todo);

            if (inserted > 0)
            {
                await DisplayAlert("Success", "Todo item has been added successfully", "OK");
                enTodoTitle.Text = string.Empty;
                dpDueDate.Date = DateTime.Now;

                //todoListData.Append($"{todo.Title} - {todo.Due.ToShortDateString()}{Environment.NewLine}");
                //lblTodos.Text = todoListData.ToString();

                Todos.Add(todo);
            }
        }
    }

}
