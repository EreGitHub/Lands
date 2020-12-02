using GalaSoft.MvvmLight.Command;
using Lands.Models;
using Lands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion
        #region Atributes
        private ObservableCollection<Land> lands;
        private bool isRefreshing;
        private string filter;
        private List<Land> landsLst;
        #endregion
        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return lands; }
            set { SetValue(ref lands, value); }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetValue(ref isRefreshing, value); }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                SetValue(ref filter, value);
                Search();
            }
        }
        #endregion
        #region Constructors
        public LandsViewModel()
        {
            apiService = new ApiService();
            LoadLands();
        }
        #endregion
        #region Methods
        private async void LoadLands()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",//Languages.Error,
                    connection.Message,
                    "Acept"//Languages.Accept
                    );
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await apiService.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",//Languages.Error,
                    response.Message,
                    "Acep"//Languages.Accept
                    );
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            landsLst = (List<Land>)response.Result;
            Lands = new ObservableCollection<Land>(landsLst);

            //MainViewModel.GetInstance().LandsList = (List<Land>)response.Result;
            //Lands = new ObservableCollection<LandItemViewModel>(
            //    ToLandItemViewModel());
            IsRefreshing = false;
        }
        #endregion
        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                Lands = new ObservableCollection<Land>(
                    landsLst);
            }
            else
            {
                Lands = new ObservableCollection<Land>(
                    landsLst.Where(q => q.Name.ToLower().Contains(Filter) || q.Capital.ToLower().Contains(Filter)));
            }
        }
        #endregion
    }
}
