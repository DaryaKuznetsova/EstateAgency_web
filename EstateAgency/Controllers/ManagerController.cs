using EstateAgency.ViewModel;
using EstateAgency.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ManagerController()
        {
            tradeService = new TradeService();
            eoService = new EstateObjectService();
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
            return View(trades);
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

    }
}