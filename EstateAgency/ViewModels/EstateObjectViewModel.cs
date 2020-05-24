using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.ViewModel
{
    public class EstateObjectViewModel
    {
        public string Title { get; set; }
        public string ButtonTitle { get; set; }
        public string RedirectUrl { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name="Цена")]
        [Required(ErrorMessage = ("Поле обязательно для заполнения.")), RegularExpression(@"^[0-9]*[.,]?[0-9]", ErrorMessage = "Разрешены только цифры и запятая.")]
        public double Price { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = ("Поле обязательно для заполнения."))]
        public string Address { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Площадь")]
        [Required(ErrorMessage = ("Поле обязательно для заполнения.")), RegularExpression(@"^[0-9]*[.,]?[0-9]", ErrorMessage = "Разрешены только цифры и запятая.")]
        public double Area { get; set; }

        [Display(Name = "Количество комнат")]
        [RegularExpression(@"^[0-9]*[.,]?[0-9]", ErrorMessage = "Only alphabetic characters are allowed.")]
        public int? Rooms { get; set; }

        [Display(Name = "Описание участка")]
        public string LandDescription { get; set; }

        [Display(Name = "Площадь участка")]
        public double? LandArea { get; set; }

        public List<SelectListItem> RealtyTypes { get; set; }

        public List<SelectListItem> TradeTypes { get; set; }

        public List<SelectListItem> Districts { get; set; }

        public List<SelectListItem> Owners { get; set; }

        [Display(Name = "Тип недвижимости")]
        public string RealtyType { get; set; }
        [Display(Name = "Вид сделки")]
        public string TradeType { get; set; }
        [Display(Name = "Район")]
        public string District { get; set; }
        [Display(Name = "Контактное лицо:")]
        public string Owner { get; set; }

        public int RealtyTypeId { get; set; }
        public int TradeTypeId { get; set; }
        public int DistrictId { get; set; }
        public int OwnerId { get; set; }

        public List<Picture> Pictures { get; set; }

    }
}