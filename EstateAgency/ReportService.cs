using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency
{
    public class ReportService
    {
        private List<int> managersId;

        public async Task<List<string>> Managers()
        {
            List<string> managers = new List<string>();
            managersId = new List<int>();

            using (Agency db = new Agency())
            {
                var result = from Manager in db.Managers
                             select Manager.Surname + " " + Manager.Name[0] + ". " + Manager.Patronymic[0]+". ";
                var id = from Manager in db.Managers select Manager.Id;
                managers = await result.ToListAsync();
                managersId = await id.ToListAsync();
                return managers;
            }
        }

        public async Task<int> CountManagerType(int managerId, int realtyTypeId)
        {
            using (Agency db = new Agency())
            {
                var result = from Trade in db.Trades
                             join EstateObject in db.EstateObjects on Trade.EstateObjectId equals EstateObject.Id
                             where Trade.ManagerId == managerId && EstateObject.RealtyTypeId == realtyTypeId
                             select Trade;
                return await result.CountAsync();
            }
        }

        public async Task<int> TotalCount()
        {
            using (Agency db = new Agency())
            {
                var result = from Trade in db.Trades
                             select Trade;
                return await result.CountAsync();
            }
        }

        public List<SelectListItem> ManagersselectList()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> r = new List<SelectListItem>();
                var res = db.Managers.Select(m => m);
                foreach (var m in res)
                    r.Add(new SelectListItem { Text = m.Surname+" "+m.Name[0]+". "+m.Patronymic[0]+".", Value = m.Id.ToString() });
                return r;
            }
        }

        public async Task CreateChart()
        {
            managersId = new List<int>();
            int i = 0;
            foreach (string manager in await Managers())
            {
                int flats = await CountManagerType(managersId[i], 1);
                int rooms = await CountManagerType(managersId[i], 2);
                int houses = await CountManagerType(managersId[i], 3);
                i++;
                AddSeries(manager, flats, rooms, houses);
            }
        }

    }
}