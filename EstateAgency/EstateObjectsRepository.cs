using EstateAgency.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency
{
    public class EstateObjectsRepository
    {
        private static List<EstateObject> _estateObjects;

        public EstateObjectsRepository()
        {
            _estateObjects = new List<EstateObject>();
        }

        public async Task<List<EstateObject>> GetEstateObjects(int status=1)
        {          
            using (var db = new Agency())
            {
                var result = from EstateObject
                             in db.EstateObjects
                             where EstateObject.StatusId == status
                             select EstateObject;
                return await result.ToListAsync();
            }
        }

        public async Task<EstateObject> GetEstateObject(int id)
        {
            EstateObject result = null;
            using (Agency db = new Agency())
            {
                result = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
            }
            return result;
        }

        public async Task<EstateObject> AddEstateObject(EstateObject estateObject)
        {
            EstateObject result = null;
            using (Agency db = new Agency())
            {
                result = db.EstateObjects.Add(estateObject);
                await db.SaveChangesAsync();
            }
            return result;
        }

        public async Task DeleteEstateObject(int id)
        {
            using (Agency db = new Agency())
            {
                var result = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
                db.EstateObjects.Remove(result);
                await db.SaveChangesAsync();
            }
        }

        public async Task<EstateObject > UpdateEstateObject(EstateObject estateObject)
        {
            using (Agency db = new Agency())
            {
                db.Entry(estateObject).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return estateObject;
        }

        public static List<EstateObject> SelectEstateObjects(int status, int realtyType, int tradeType,
            double minPrice, double maxPrice, double minArea, double maxArea, List<int> districts)
        {           
            using (Agency db = new Agency())
            {
                var result = from EstateObject in db.EstateObjects
                             where
                             EstateObject.StatusId == status &&
                             EstateObject.RealtyTypeId == realtyType &&
                             EstateObject.TradeTypeId == tradeType &&
                             (EstateObject.Price >= minPrice && EstateObject.Price <= maxPrice) &&
                             (EstateObject.Area >= minArea && EstateObject.Area <= maxArea) &&
                             districts.Contains(EstateObject.DistrictId)
                             select EstateObject;
                return result.ToList();
            }            
        }

        public static async Task <List<Picture>> Pictures(int id)
        {
            using (Agency db = new Agency())
            {
                var result = from EstateObject in db.EstateObjects
                             join PictureObjectLink in db.PictureObjectLinks on EstateObject.Id equals PictureObjectLink.EstateObjectId
                             join Picture in db.Pictures on PictureObjectLink.PictureId equals Picture.Id
                             where EstateObject.Id == id
                             select Picture;
                return await result.ToListAsync();
            }
        }

        public static async Task DeletePicture(int id)
        {
            using (Agency db = new Agency())
            {
                Picture pic = await db.Pictures.FirstOrDefaultAsync(f => f.Id == id);
                db.Pictures.Remove(pic);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreatePicture(EstateObjectViewModel eo, HttpPostedFileBase uploadImage)
        {
            using (Agency db = new Agency())
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                Picture pic = new Picture
                {
                    Path = imageData
                };

                db.Pictures.Add(pic);
                PictureObjectLink pol = new PictureObjectLink { EstateObjectId = eo.Id, PictureId = pic.Id };
                db.PictureObjectLinks.Add(pol);
                await db.SaveChangesAsync();
            }
        }
    }
}