using Lands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Lands.ViewModels
{
    public class LandViewModel : BaseViewModel
    {
        #region Atributes
        private ObservableCollection<Border> borders;
        private ObservableCollection<Currency> currencies;
        private ObservableCollection<Language> languages;
        #endregion
        #region Properties
        public Land Land { get; set; }
        public ObservableCollection<Border> Borders
        {
            get { return borders; }
            set { SetValue(ref borders, value); }
        }
        public ObservableCollection<Currency> Currencies
        {
            get { return currencies; }
            set { SetValue(ref currencies, value); }
        }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set { SetValue(ref languages, value); }
        }
        #endregion

        #region Contructors
        public LandViewModel(Land land)
        {
            this.Land = land;
            LoadBorders();
            Currencies = new ObservableCollection<Currency>(land.Currencies);
            Languages = new ObservableCollection<Language>(Land.Languages);
        }
        #endregion
        #region Methods
        private void LoadBorders()
        {
            Borders = new ObservableCollection<Border>();
            foreach (var border in Land.Borders)
            {
                var land = MainViewModel.GetInstance().landsLst
                    .Where(q => q.Alpha3Code == border).FirstOrDefault();
                if (!(land is null))
                {
                    Borders.Add(new Border
                    {
                        Code = land.Alpha3Code,
                        Name = land.Name
                    });
                }
            }
        }
        #endregion

    }
}
