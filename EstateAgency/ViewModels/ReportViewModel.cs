using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.ViewModels
{
    public class ReportViewModel
    {
        public  DateTime FirstDate { get; set; }

        public DateTime SecondDate { get; set; }

        public List<SelectListItem> Managers { get; set; }

        public int ManagerId { get; set; }

        public ReportViewModel()
        {

        }

    }
}