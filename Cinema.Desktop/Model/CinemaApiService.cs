using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cinema.Desktop.ViewModel;
using Cinema.Persistence;
using Cinema.Persistence.DTO;

namespace Cinema.Desktop.Model
{
    public class CinemaApiService
    {
        private readonly HttpClient _client;

        public CinemaApiService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<ListDto>> LoadListsAsync()
        {
            var response = await _client.GetAsync("api/Lists");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ListDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        

        public async Task<bool> LoginAsync(string userName, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = userName,
                Password = password
            };

            var response = await _client.PostAsJsonAsync("api/Account/Login", user); //Mi a végpont(api-s), mit akarunk posztolni

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            var response = await _client.PostAsync("api/Account/Logout" , null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #region List
        public async Task CreateListAsync(ListDto list)
        {
            System.Diagnostics.Debug.WriteLine("List: " + list.Id + " " + list.Name);
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/lists/", list);
            list.Id = (await response.Content.ReadAsAsync<ListDto>()).Id;

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task UpdateListAsync(ListDto list)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/lists/{list.Id}", list);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task DeleteListAsync(Int32 listId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/lists/{listId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        #endregion

        #region Movie

        public async Task<IEnumerable<MovieDto>> LoadMoviesAsync(int listId)
        {
            var response = await _client.GetAsync($"api/Movies/List/{listId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<MovieDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<ScreeningDto>> LoadScreeningsAsync(int movieId)
        {
            var response = await _client.GetAsync($"api/Screenings/List/{movieId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ScreeningDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }


        public async Task CreateMovieAsync(MovieDto movie)
        {
           
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Movies/", movie);
            movie.Id = (await response.Content.ReadAsAsync<MovieDto>()).Id;

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task CreateScreeningAsync(ScreeningDto screening)
        {
            System.Diagnostics.Debug.WriteLine("Itt screening");

            //var newScreening = (Screening)screening;
            //var response = await _client.GetAsync($"api/Screenings/{screening.MovieId}");
            var response = await _client.PostAsJsonAsync("api/Screenings/", screening);
            //await response.Content.ReadAsAsync<MovieDto>();

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task UpdateMovieAsync(MovieDto movie)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/movies/{movie.Id}", movie);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        public async Task DeleteMovieAsync(Int32 movieId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/movies/{movieId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        
        
        public async Task ApiServicePurchaseAsync(int id, ScreeningDto screening)
        {
            
            System.Diagnostics.Debug.WriteLine("ScreeningId: " + screening.Id);
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/screenings/{id}",screening);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        #endregion
    }
}
