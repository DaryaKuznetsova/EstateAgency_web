using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.ViewModels
{
    public class ReportViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public  DateTime FirstDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime SecondDate { get; set; }

        public List<SelectListItem> Managers { get; set; }

        public int ManagerId { get; set; }

        public ReportViewModel()
        {

        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int Flats { get; set; }
        public int Rooms { get; set; }
        public int Houses { get; set; }

        public int ManagerCount { get; set; }
        public int TotalCount { get; set; }
        public string ManagerName { get; set; }

    }
}