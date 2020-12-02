using GalaSoft.MvvmLight.Command;
using Lands.View;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributes    
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion
        #region Properties
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string Password
        {
            get { return password; }
            set { SetValue(ref password, value); }
        }
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref isRunning, value); }
        }
        public bool IsRememberd { get; set; }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetValue(ref isEnabled, value); }
        }
        #endregion
        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter email", "accept");
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter password", "accept");
                Password = string.Empty;
                return;
            }
            if (Email != "1" && Password != "1")
            {
                IsRunning = false;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", "Email or Password incorrect", "accept");
                return;
            }
            IsRunning = false;
            IsEnabled = true;

            Email = string.Empty;
            password = string.Empty;

            MainViewModel.GetInstance().Lands = new LandsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());
        }
        #endregion
        #region Constructors
        public LoginViewModel()
        {
            email = "1";
            password = "1";
            this.IsRememberd = true;
            isEnabled = true;
        }
        #endregion
    }
}
