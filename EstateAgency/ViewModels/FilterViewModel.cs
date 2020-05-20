using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.ViewModel
{
    public class FilterViewModel
    {
        [Display(Name = "Тип недвижимости")]
        public int RealtyTypeId { get; set; }

        [Display(Name = "Вид сделки")]
        public int TradeTypeId { get; set; }
        [Display(Name = "Тип недвижимости")]
        public IEnumerable<SelectListItem> RealtyTypes { get; set; }
        [Display(Name = "Вид сделки")]
        public IEnumerable<SelectListItem> TradeTypes { get; set; }

        [Display(Name = "Цена От: ")]
        public double MinPrice { get; set; }

        [Display(Name = "Цена До: ")]
        public double MaxPrice { get; set; }

        [Display(Name = "Площадь От: ")]
        public double MinArea { get; set; }

        [Display(Name = "Площадь До: ")]
        public double MaxArea { get; set; }
        [Display(Name = "Районы: ")]
        public List<int> SelectedDistricts { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
    }

}