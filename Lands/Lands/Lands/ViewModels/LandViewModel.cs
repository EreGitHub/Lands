using Lands.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.ViewModels
{
    public class LandViewModel
    {
        #region Properties
        public Land Land { get; set; }
        #endregion

        #region Contructors
        public LandViewModel(Land land)
        {
            this.Land = land;
        } 
        #endregion

    }
}
