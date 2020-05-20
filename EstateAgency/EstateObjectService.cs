using EstateAgency.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency
{
    public class EstateObjectService
    {
        private readonly EstateObjectsRepository _estateObjectsRepository;

        public EstateObjectService()
        {
            _estateObjectsRepository = new EstateObjectsRepository();
        }

        public async Task<List<EstateObject>> GetEstateObjects()
        {
            return await _estateObjectsRepository.GetEstateObjects();
        }

        public async Task<EstateObject> GetEstateObject(int id)
        {
            return await _estateObjectsRepository.GetEstateObject(id);
        }

        public async Task<EstateObject> AddEstateObject(EstateObject estateObject)
        {
            return await _estateObjectsRepository.AddEstateObject(estateObject);
        }

        public async Task DeleteEstateObject(int id)
        {
            await _estateObjectsRepository.DeleteEstateObject(id);
        }

        public async Task<EstateObject> UpdateEstateObject(EstateObject estateObject)
        {
            return await _estateObjectsRepository.UpdateEstateObject(estateObject);
        }

        #region PrepareLists

        public List<SelectListItem> PrepareRealtyTypes()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> r = new List<SelectListItem>();
                var res = db.RealtyTypes.Select(m => m);
                foreach (var m in res)
                    r.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return r;
            }
        }

        public List<SelectListItem> PrepareTradeTypes()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> t = new List<SelectListItem>();
                var res2 = db.TradeTypes.Select(m => m);
                foreach (var m in res2)
                    t.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return t;
            }
        }
        public List<SelectListItem> PrepareDistricts()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> d = new List<SelectListItem>();
                var res3 = db.Districts.Select(m => m);
                foreach (var m in res3)
                    d.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return d;
            }
        }
        public List<SelectListItem> PrepareOwners()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> o = new List<SelectListItem>();
                var res4 = db.Owners.Select(m => m);
                foreach (var m in res4)
                    o.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return o;
            }
        }
        #endregion

        public async Task< EstateObjectViewModel> Model(EstateObject eo)
        {
            List<Picture> pictures = await EstateObjectsRepository.Pictures(eo.Id);
            EstateObjectViewModel model = new EstateObjectViewModel
            {
                Id = eo.Id,
                Price = eo.Price,
                Address = eo.Address,
                Description = eo.Description,
                Area = eo.Area,
                Rooms=eo.Rooms,
                LandDescription = eo.LandDescription,
                LandArea = eo.LandArea,
                District = eo.District.Name,
                RealtyType = eo.RealtyType.Name,
                TradeType = eo.TradeType.Name,
                Owner = eo.Owner.Phone,
                Pictures = pictures,
                RealtyTypeId=eo.RealtyTypeId,
                TradeTypeId=eo.TradeTypeId,
                DistrictId=eo.DistrictId,
                OwnerId=eo.OwnerId,
                RealtyTypes=PrepareRealtyTypes(),
                TradeTypes=PrepareTradeTypes(),
                Districts=PrepareDistricts(),
                Owners=PrepareOwners(),
            };
            return model;
        }

        public EstateObjectViewModel Model()
        {
            EstateObjectViewModel model = new EstateObjectViewModel
            {
                RealtyTypes = PrepareRealtyTypes(),
                TradeTypes = PrepareTradeTypes(),
                Districts = PrepareDistricts(),
                Owners = PrepareOwners(),
            };
            return model;
        }

        public async Task<EstateObject> EstateObject(EstateObjectViewModel model)
        {
            List<Picture> pictures = await EstateObjectsRepository.Pictures(model.Id);
            EstateObject eo = new EstateObject
            {
                Id = model.Id,
                Price = model.Price,
                Address = model.Address,
                Description = model.Description,
                Area = model.Area,
                Rooms=model.Rooms,
                LandDescription = model.LandDescription,
                LandArea = model.LandArea,
                RealtyTypeId = model.RealtyTypeId,
                TradeTypeId = model.TradeTypeId,
                DistrictId = model.DistrictId,
                OwnerId = model.OwnerId,
                StatusId=1,
            };
            return eo;
        }
    }
}