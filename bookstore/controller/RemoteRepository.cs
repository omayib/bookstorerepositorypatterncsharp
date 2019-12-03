using bookstore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bookstore.controller
{
    class RemoteRepository
    {
        private HttpClient client;
        RepositoryListener listener;
        public List<Book> books = new List<Book>();
        private static string DOMAIN = "http://35.165.134.101:8080";
        private static string ENDPOINT_BOOKS = "/api/v1/books";

        public RemoteRepository(RepositoryListener listener)
        {
            this.listener = listener;
            client = new HttpClient();
            client.BaseAddress = new Uri(DOMAIN);
            client.DefaultRequestHeaders.Accept.Clear();
        }
        public async Task GetProductAsync()
        {
            HttpResponseMessage response = await client.GetAsync(ENDPOINT_BOOKS);
            if (response.IsSuccessStatusCode)
            {
                books.Clear();
                var resp = await response.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<Response>(resp);
                var rv = JsonConvert.DeserializeObject<List<Book>>(r.data.ToString());
                books.AddRange(rv);
                listener.onGetBooksSucceed();
            }
            else
            {
                listener.onGetRepositoryFailed("terjadi kesalahan");
            }
        }

        public async Task CreateProductAsync(Book book)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                ENDPOINT_BOOKS, book);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<Response>(resp);
                var rv = JsonConvert.DeserializeObject<Book>(r.data.ToString());
                books.Add(rv);
                listener.onAddBooksSucceed();

            }

        }

        public async Task UpdateProductAsync(Book book)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                ENDPOINT_BOOKS+"/"+book._id, book);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();

            await response.Content.ReadAsAsync<Book>();
        }
        public async Task DeleteProductAsync(string id)
        {
           
            Console.WriteLine("DeleteProductAsync " + id);
            HttpResponseMessage response = await client.DeleteAsync(
                 ENDPOINT_BOOKS + "/" + id);
            Console.WriteLine("DeleteProductAsync" + response.RequestMessage);
            Console.WriteLine("DeleteProductAsync" + response);
            if (response.IsSuccessStatusCode)
            {
                GetProductAsync();
            }
            else
            {
                listener.onGetRepositoryFailed("gagal"); 
            }
        }

    }
}
