using bookstore.controller;
using bookstore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace bookstore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, RepositoryListener
    {
        RemoteRepository remoteRepository;

        public MainWindow()
        {
            InitializeComponent();
            remoteRepository = new RemoteRepository(this);
            listBox.ItemsSource = remoteRepository.books;
        }



        private async void getBooksAsync()
        {
            await remoteRepository.GetProductAsync();
        }
        private async void RunAddBookAsync(string title,string author,string year,string cover)
        {
            Book book = new Book();
            book.title = title;
            book.author = author;
            book.cover = cover;
            book.year = year;
            await remoteRepository.CreateProductAsync(book);
        }
        private async void DeleteBook(string id)
        {
            await remoteRepository.DeleteProductAsync(id);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getBooksAsync();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            string title = textBoxTitle.Text;
            string author = textBoxAuthor.Text;
            string year = textBoxYear.Text;
            string cover = textBoxCover.Text;
            RunAddBookAsync(title, author, year, cover);

        }

        public void onGetBooksSucceed()
        {
            listBox.Items.Refresh();
        }

        public void onGetRepositoryFailed(string message)
        {
            MessageBox.Show(message);
        }

        private void selectBookListener(object sender, SelectionChangedEventArgs e)
        {
            var r =(Book) ((ListBox)e.Source).SelectedItem;
            MessageBoxResult result = MessageBox.Show("Do you want to delete?", "Confirm",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DeleteBook(r._id);
            }
                             
        }

        public void onDeleteBooksSucceed()
        {
            throw new NotImplementedException();
        }

        public void onUpdateBooksSucceed()
        {
            throw new NotImplementedException();
        }

        public void onAddBooksSucceed()
        {
            listBox.Items.Refresh();
        }
    }
}
