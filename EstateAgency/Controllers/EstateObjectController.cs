using EstateAgency.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency.Controllers
{
    [Authorize]
    public class EstateObjectController : Controller
    {
        private readonly EstateObjectService _estateObjectService;

        public EstateObjectController()
        {
            _estateObjectService = new EstateObjectService();
        }

        // GET: EstateObject
        public async Task<ActionResult> Index()
        {
            var estateObjects = await _estateObjectService.GetEstateObjects();
            return View(estateObjects);
        }

        #region Create

        public ActionResult StartAddEstateObject()
        {
            EstateObjectViewModel model = _estateObjectService.Model();
            return View(model);
        }

        [HttpPost]
        public ActionResult StartAddEstateObject(EstateObjectViewModel model)
        {
            return RedirectToAction("AddEstateObject", new { realtyType = model.RealtyTypeId });
        }
        public ActionResult AddEstateObject(int realtyType)
        {
            EstateObjectViewModel model = _estateObjectService.Model();
            Agency db = new Agency();
            RealtyType result = db.RealtyTypes.FirstOrDefault(f => f.Id == realtyType);
            model.RealtyTypeId = realtyType;
            model.RealtyType = result.Name;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddEstateObject(EstateObjectViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("AddEstateObject", new { realtyType = model.RealtyTypeId });
            EstateObject eo = await _estateObjectService.EstateObject(model);
            eo.StatusId = 1;
            await _estateObjectService.AddEstateObject(eo);
            return RedirectToAction("Index");
        }

        #endregion

        #region Filter
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
                TradeTypes=t,
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
            var res=EstateObjectsRepository.SelectEstateObjects(1, filterViewModel.RealtyTypeId, filterViewModel.TradeTypeId,
                filterViewModel.MinPrice, filterViewModel.MaxPrice, filterViewModel.MinArea, filterViewModel.MaxArea, filterViewModel.SelectedDistricts);
            return View("Index", res);
        }

        #endregion

        public async Task<ActionResult> DetailsEstateObject(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(eo);
            return View(model);
        }

        #region Update
        public async Task<ActionResult> UpdateEstateObject(int id)
        {
            Agency db = new Agency();
            EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(eo);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateEstateObject(EstateObjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                EstateObject eo = await _estateObjectService.EstateObject(model);
                await _estateObjectService.UpdateEstateObject(eo);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("UpdateEstateObject", new { id = model.Id });
        }
        #endregion

        #region Delete
        public async Task<ActionResult> DeleteEstateObject(int id)
        {
            Agency db = new Agency();
            EstateObject result = db.EstateObjects.FirstOrDefault(f => f.Id == id);
            EstateObjectViewModel model = await _estateObjectService.Model(result);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteEstateObject(EstateObjectViewModel model)
        {
            await _estateObjectService.DeleteEstateObject(model.Id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Pictures
        public ActionResult CreateImage(EstateObject eo)
        {
            return View();
        }

        [HttpPost]
        public async Task <ActionResult> CreateImage(EstateObjectViewModel eo, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
            {
                await EstateObjectsRepository.CreatePicture(eo, uploadImage);
            }
            return RedirectToAction("UpdateEstateObject", new { id=eo.Id});
        }


        [HttpGet]
        public async Task<ActionResult> DeleteImage(int id)
        {
            await EstateObjectsRepository.DeletePicture(id);
            return RedirectToAction("Index");
        }
        #endregion

    }
}