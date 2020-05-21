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
    public class ClientController : Controller
    {
        private readonly EstateObjectService _estateObjectService;
        private readonly TradeService _tradeService;

        public ClientController()
        {
            _estateObjectService = new EstateObjectService();
            _tradeService = new TradeService();
        }
        // GET: Client
        public async Task<ActionResult> Index()
        {
            var estateObjects = await _estateObjectService.GetEstateObjects();
            return View(estateObjects);
        }

        public ActionResult Filter()
        {
            Agency db = new Agency();

            List<SelectListItem> d = _estateObjectService.PrepareDistricts();

            List<SelectListItem> t = _estateObjectService.PrepareTradeTypes();

            List<SelectListItem> r = _estateObjectService.PrepareRealtyTypes();

            var model = new FilterViewModel
            {
                MaxArea = 999999,
                MinArea = 0,
                MaxPrice = 9999999,
                MinPrice = 0,
                RealtyTypes = r,
                TradeTypes = t,
                SelectedDistricts = new List<int> { 1, 2, 3, 4, 5, 6, 7 },
                Districts = d
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Filter(FilterViewModel filterViewModel)
        {
            Agency db = new Agency();
            if (!ModelState.IsValid)
                return RedirectToAction("Filter");
            var res = EstateObjectsRepository.SelectEstateObjects(1, filterViewModel.RealtyTypeId, filterViewModel.TradeTypeId,
                filterViewModel.MinPrice, filterViewModel.MaxPrice, filterViewModel.MinArea, filterViewModel.MaxArea, filterViewModel.SelectedDistricts);
            return View("Index", res);
        }

        public async Task<ActionResult> Buy(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(eo);
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Buy(EstateObjectViewModel model)
        {
            Agency db = new Agency();
            int clientId = Convert.ToInt32(User.Identity.Name);
            int estateObjectId = model.Id;
            MessageViewModel error = await _tradeService.CreateRequest(clientId, estateObjectId);
                return RedirectToAction("ClientMessage", new { type=error.Type, message=error.Message });
        }

        public async Task<ActionResult> Requests()
        {
            int clientId = Convert.ToInt32(User.Identity.Name);
            var estateObjects = await _tradeService.GetClientRequests(clientId, 2);
            return View(estateObjects);
        }

        public async Task<ActionResult> DeleteRequest(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(eo);
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> DeleteRequest(EstateObjectViewModel model)
        {
            int estateObjectId = model.Id;
            await _tradeService.DeleteRequest(estateObjectId);
            return RedirectToAction("Requests");
        }

        public async Task<ActionResult> DeleteTrade(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(eo);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTrade(EstateObjectViewModel model)
        {
            int estateObjectId = model.Id;
            await _tradeService.DeleteTrade(estateObjectId);
            return RedirectToAction("AcceptedRequests");
        }

        public async Task<ActionResult> UpdateTrade(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            TradeRequestViewModel model = await _tradeService.Model(eo);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTrade(TradeRequestViewModel model)
        {
            int estateObjectId = model.EstateObjectId;
            int paymentInstrument = model.PaymentInstrumentId;
            int paymentType = model.PaymentTypeId;
            await _tradeService.UpdateTrade(estateObjectId, paymentType, paymentInstrument);
            return RedirectToAction("Trades");
        }

        public async Task<ActionResult> AcceptedRequests()
        {
            int clientId = Convert.ToInt32(User.Identity.Name);
            var estateObjects = await _tradeService.GetClientRequests(clientId, 3);
            return View(estateObjects);
        }

        public async Task<ActionResult> Trades()
        {
            int clientId = Convert.ToInt32(User.Identity.Name);
            var estateObjects = await _tradeService.GetClientTrades(clientId);
            return View(estateObjects);
        }

        public async Task<ActionResult> ViewTrade(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            TradeRequestViewModel model = await _tradeService.Model(eo);
            return View(model);
        }

        public ActionResult ClientMessage(string type, string message)
        {
            ViewBag.Type = type;
            ViewBag.Message = message;
            return View();
        }
    }
}