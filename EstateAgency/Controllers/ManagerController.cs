using EstateAgency.ViewModel;
using EstateAgency.ViewModels;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.Controllers
{
    public class ManagerController : Controller
    {
        private readonly TradeService tradeService;
        private readonly EstateObjectService eoService;
        private readonly ReportService reportService;

        public ManagerController()
        {
            tradeService = new TradeService();
            eoService = new EstateObjectService();
            reportService = new ReportService();
        }

        // GET: Manager
        public ActionResult Index()
        {
            return RedirectToAction("Index", "EstateObject");
        }

        public async Task<ActionResult> Requests()
        {
            List<EstateObject> estateObjects = await eoService.GetEstateObjects(2);
            return View(estateObjects);
        }

        public async Task<ActionResult> ViewRequest(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await eoService.Model(eo);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ViewRequest(EstateObjectViewModel model)
        {
            int managerId = Convert.ToInt32(User.Identity.Name);
            MessageViewModel message = await tradeService.CreateTrade(model.Id, managerId);
            if (message.Type != null)
                return RedirectToAction("ManagerMessage", new { type = message.Type, message = message.Message });
            else
                return RedirectToAction("Requests");
        }

        public async Task<ActionResult> RejectRequest(int eoId)
        {
            MessageViewModel message = await tradeService.RejectRequest(eoId);
            if (message.Type != null)
                return RedirectToAction("ManagerMessage", new { type = message.Type, message = message.Message });
            else
                return RedirectToAction("Requests");
        }

        public ActionResult ManagerMessage(string type, string message)
        {
            ViewBag.Type = type;
            ViewBag.Message = message;
            return View();
        }

        public async Task<ActionResult> Trades()
        {
            List<TradeRequestViewModel> trades = await tradeService.AllTrades();
            TradesViewModel model = new TradesViewModel(trades);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Trades(TradesViewModel model)
        {
            List<TradeRequestViewModel> trades = await tradeService.GetTrades(model.FirstDate, model.SecondDate);
            model = new TradesViewModel(trades);
            return View(model);
        }

        public async Task<ActionResult> TradeInfo(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            TradeRequestViewModel model = await tradeService.Model(eo);
            return View(model);
        }

        public async Task<ActionResult> DeleteTrade(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            TradeRequestViewModel model = await tradeService.Model(eo);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTrade(TradeRequestViewModel model)
        {
            Agency db = new Agency();
            int deleteId = model.EstateObjectId;
            await tradeService.DeleteTrade(deleteId);
            return RedirectToAction("Trades");
        }

        public ActionResult Reports()
        {
            ReportViewModel model = new ReportViewModel();
            model.Managers = reportService.ManagersList();
            model.FirstDate = DateTime.Today;
            model.SecondDate = DateTime.Today;
            return View(model);
        }

        [HttpPost]
        public ActionResult Reports(ReportViewModel model)
        {
            string templatePath = Server.MapPath("~/Files/MyTemplate2.docx");
            DateTime date = DateTime.Now;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            string d = date.ToString("o", culture);
            d = d.Replace(':', '_').Replace('+', '_');
            string fileName = "Report"+d+".docx";

            bool ok = reportService.SaveReport(ref model, templatePath, fileName);
            if (ok)
                return RedirectToAction("MyReport", new { name=model.ManagerName, flats=model.Flats,
                    rooms=model.Rooms, houses=model.Houses, managerCount=model.ManagerCount, totalCount=model.TotalCount, file_name = fileName });
            else return RedirectToAction("Reports");
        }

        public ActionResult MyReport(string name, int flats, int rooms, int houses,
            int managerCount, int totalCount, string file_name)
        {
            ReportViewModel model = new ReportViewModel();
            string file_path= @"D:\Рабочий стол\EstateAgency\Templates\" + file_name;
            model.FilePath = file_path;
            model.FileName = file_name;
            model.ManagerName = name;
            model.Flats = flats;
            model.Rooms = rooms;
            model.Houses = houses;
            model.ManagerCount = managerCount;
            model.TotalCount = totalCount;
            return View(model);
        }

        [HttpPost]
        public FileResult MyReport(ReportViewModel model)
        {
            string file_type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            return File(model.FilePath, file_type, model.FileName);
        }
    }
}