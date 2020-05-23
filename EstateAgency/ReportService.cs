using EstateAgency.ViewModels;
using Microsoft.Office.Interop.Word;
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
                             select Manager.Surname + "_" + Manager.Name[0] + "._" + Manager.Patronymic[0]+".";
                var id = from Manager in db.Managers select Manager.Id;
                managers = await result.ToListAsync();
                managersId = await id.ToListAsync();
                return managers;
            }
        }

        public string Manager(int id)
        {
            List<string> managers = new List<string>();
            managersId = new List<int>();

            using (Agency db = new Agency())
            {
                Manager Manager = db.Managers.FirstOrDefault(f => f.Id == id);
                string result = Manager.Surname + " " + Manager.Name[0] + ". " + Manager.Patronymic[0] + ".";
                return result;
            }
        }

        public int CountManagerType(int managerId, int realtyTypeId)
        {
            using (Agency db = new Agency())
            {
                var result = from Trade in db.Trades
                             join EstateObject in db.EstateObjects on Trade.EstateObjectId equals EstateObject.Id
                             where Trade.ManagerId == managerId && EstateObject.RealtyTypeId == realtyTypeId
                             select Trade;
                return result.Count();
            }
        }

        public int TotalCount(DateTime first, DateTime second)
        {
            using (Agency db = new Agency())
            {
                var result = from Trade in db.Trades
                             where Trade.Date>=first&&Trade.Date<=second
                             select Trade;
                return  result.Count();
            }
        }

        public List<SelectListItem> ManagersList()
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

        //public async Task CreateChart()
        //{
        //    managersId = new List<int>();
        //    int i = 0;
        //    foreach (string manager in await Managers())
        //    {
        //        int flats = await CountManagerType(managersId[i], 1);
        //        int rooms = await CountManagerType(managersId[i], 2);
        //        int houses = await CountManagerType(managersId[i], 3);
        //        i++;
        //        AddSeries(manager, flats, rooms, houses);
        //    }
        //}

        public bool SaveReport(ref ReportViewModel model, string templatePath, string saveName)
        {
            model.Flats =  CountManagerType(model.ManagerId, 1);
            model.Rooms =  CountManagerType(model.ManagerId, 2);
            model.Houses =  CountManagerType(model.ManagerId, 3);
            model.ManagerCount = model.Flats + model.Rooms + model.Houses;
            model.TotalCount =  TotalCount(model.FirstDate, model.SecondDate);
            model.ManagerName =  Manager(model.ManagerId);


            Application app = new Application();
            app.Visible = false;
            Document doc = new Document();
            doc = app.Documents.Open(templatePath);
            doc.Activate();

            if (doc.Bookmarks.Exists("firstDate"))
            {
                doc.Bookmarks["firstDate"].Range.Text = model.FirstDate.ToShortDateString();
            }
            if (doc.Bookmarks.Exists("secondDate"))
            {
                doc.Bookmarks["secondDate"].Range.Text = model.SecondDate.ToShortDateString();
            }
            if (doc.Bookmarks.Exists("flatCount"))
            {
                doc.Bookmarks["flatCount"].Range.Text = model.Flats.ToString();
            }
            if (doc.Bookmarks.Exists("roomCount"))
            {
                doc.Bookmarks["roomCount"].Range.Text =model.Rooms.ToString();
            }
            if (doc.Bookmarks.Exists("houseCount"))
            {
                doc.Bookmarks["houseCount"].Range.Text = model.Houses.ToString();
            }
            if (doc.Bookmarks.Exists("managerName"))
            {
                doc.Bookmarks["managerName"].Range.Text = model.ManagerName;
            }
            if (doc.Bookmarks.Exists("totalCount"))
            {
                doc.Bookmarks["totalCount"].Range.Text = model.TotalCount.ToString();
            }
            if (doc.Bookmarks.Exists("totalManager"))
            {
                doc.Bookmarks["totalManager"].Range.Text = model.ManagerCount.ToString();
            }
            string savePath = @"D:\Рабочий стол\EstateAgency\Templates\" + saveName;
            bool ok;
            try
            {
                doc.SaveAs2(savePath);
                ok = true;
            }
            catch(Exception)
            {
                ok = false;
            }
            finally
            {
                app.Application.Quit();
            }
            return ok;
        }
    }
}