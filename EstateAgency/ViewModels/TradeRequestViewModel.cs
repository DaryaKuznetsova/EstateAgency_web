using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.ViewModels
{
    public class TradeRequestViewModel
    {
        public int EstateObjectId { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int TradeId { get; set; }

        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Площадь")]
        public double Area { get; set; }
        [Display(Name = "Количество комнат")]
        public int? Rooms { get; set; }
        [Display(Name = "Описание участка")]
        public string LandDescription { get; set; }
        [Display(Name = "Площадь участка")]
        public double? LandArea { get; set; }
        [Display(Name = "Тип недвижимости")]
        public string RealtyType { get; set; }
        [Display(Name = "Вид сделки")]
        public string TradeType { get; set; }
        [Display(Name = "Район")]
        public string District { get; set; }
        [Display(Name = "Контактное лицо:")]
        public string Owner { get; set; }

        [Display(Name = "Клиент:")]
        public string ClientName { get; set; }
        [Display(Name = "Телефон:")]
        public string ClientPhone { get; set; }
        [Display(Name = "Email:")]
        public string ClientEmail { get; set; }

        [Display(Name = "Менеджер:")]
        public string ManagerName { get; set; }
        [Display(Name = "Телефон:")]
        public string ManagerPhone { get; set; }
        [Display(Name = "Email:")]
        public string ManagerEmail { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        public List<SelectListItem> PaymentTypes { get; set; }
        public List<SelectListItem> PaymentInstruments { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentInstrumentId { get; set; }

        public int RealtyTypeId { get; set; }
        [Display(Name = "Способ оплаты")]
        public string PaymentType { get; set; }
        [Display(Name = "Платёжное средство")]
        public string PaymentInstrument { get; set; }
    }
}