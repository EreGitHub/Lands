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
        private ObservableCollection<LandItemViewModel> lands;
        private bool isRefreshing;
        private string filter;
        //private List<Land> landsLst;
        #endregion
        #region Properties
        public ObservableCollection<LandItemViewModel> Lands
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
            //landsLst = (List<Land>)response.Result;
            //Lands = new ObservableCollection<LandItemViewModel>(
            //    ToLandItemViewModel());

            MainViewModel.GetInstance().landsLst = (List<Land>)response.Result;
            Lands = new ObservableCollection<LandItemViewModel>(
                ToLandItemViewModel());
            IsRefreshing = false;
        }

        #region Methods
        private IEnumerable<LandItemViewModel> ToLandItemViewModel()
        {
            return MainViewModel.GetInstance().landsLst.Select(l => new LandItemViewModel
            {
                Alpha2Code = l.Alpha2Code,
                Alpha3Code = l.Alpha3Code,
                AltSpellings = l.AltSpellings,
                Area = l.Area,
                Borders = l.Borders,
                CallingCodes = l.CallingCodes,
                Capital = l.Capital,
                Cioc = l.Cioc,
                Currencies = l.Currencies,
                Demonym = l.Demonym,
                Flag = l.Flag,
                Gini = l.Gini,
                Languages = l.Languages,
                Latlng = l.Latlng,
                Name = l.Name,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Population = l.Population,
                Region = l.Region,
                RegionalBlocs = l.RegionalBlocs,
                Subregion = l.Subregion,
                Timezones = l.Timezones,
                TopLevelDomain = l.TopLevelDomain,
                Translations = l.Translations,
            });
        }
        #endregion
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
                Lands = new ObservableCollection<LandItemViewModel>(
                ToLandItemViewModel());
            }
            else
            {
                Lands = new ObservableCollection<LandItemViewModel>(
                    ToLandItemViewModel()
                    .Where(q => q.Name.ToLower().Contains(Filter) || q.Capital.ToLower().Contains(Filter)));
            }
        }
        #endregion
    }
}
