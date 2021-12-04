using Cinema.Desktop.Model;
using Cinema.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Controls;

namespace Cinema.Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly CinemaApiService _service;

        public Boolean IsLoading { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public String UserName { get; set; }

        public event EventHandler LoginSucceeded;

        public event EventHandler LoginFailed;

        
        public LoginViewModel(CinemaApiService service)
        {
            _service = service;

            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox)); //Akkkor lehessen végrehajtani, ha Isloading hamis
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            try
            {
                IsLoading = true;
                var result = await _service.LoginAsync(UserName, passwordBox.Password);

                if (result)
                {
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    LoginFailed?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LogoutAsync()
        {
            
        }
    }
}
