using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EstateAgency.ViewModels
{
    public class TradesViewModel
    {
        [Display(Name = "От")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime FirstDate { get; set; }
        [Display(Name = "До")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime SecondDate { get; set; }
        public List<TradeRequestViewModel> List { get; set; }

        public TradesViewModel(List<TradeRequestViewModel> trades)
        {
            FirstDate = DateTime.Today;
            SecondDate = DateTime.Today;
            List = trades;
        }

        public TradesViewModel()
        {

        }
    }
}