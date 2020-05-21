using EstateAgency.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EstateAgency
{
    public class TradeService
    {
        public async Task<MessageViewModel> CreateRequest(int clientId, int estateObjectId)
        {
            using (Agency db = new Agency())
            {
                MessageViewModel message = new MessageViewModel();

                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == estateObjectId);
                if (eo.StatusId == 1)
                {
                    eo.StatusId = 2;
                    Request request = new Request { EstateObjectId = estateObjectId, ClientId = clientId };
                    db.Requests.Add(request);
                    await db.SaveChangesAsync();
                    message.Type = "Заявка подана на рассмотрение!";
                    message.Message = "Обновите страницу Активные заявки";
                }
                else
                {
                    message.Type = "Ошибка";
                    message.Message = "К сожалению, кто-то только что купил этот объект недвижимости. Рекомендуем выбрать другой вариант.";
                }
                return message;
            }
        }

        public async Task<List<EstateObject>> GetClientRequests(int clientId, int status)
        {
            using (var db = new Agency())
            {
                var result = from EstateObject in db.EstateObjects
                             join Request in db.Requests on EstateObject.Id equals Request.EstateObjectId
                             where Request.ClientId == clientId && EstateObject.StatusId == status
                             select EstateObject;
                return await result.ToListAsync();
            }
        }

        public async Task<List<EstateObject>> GetClientTrades(int clientId)
        {
            using (var db = new Agency())
            {
                var result = from EstateObject in db.EstateObjects
                             join Trade in db.Trades on EstateObject.Id equals Trade.EstateObjectId
                             where Trade.ClientId == clientId && EstateObject.StatusId==4
                             select EstateObject;
                return await result.ToListAsync();
            }
        }

        public async Task DeleteRequest(int estateObjectId)
        {
            using (var db = new Agency())
            {
                Request result = await db.Requests.FirstOrDefaultAsync(f => f.EstateObjectId == estateObjectId);
                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == estateObjectId);
                eo.StatusId = 1;
                db.Requests.Remove(result);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateTrade(int estateObjectId, int paymentTypeId, int paymentInstrument)
        {
            using (var db = new Agency())
            {
                Request request = await db.Requests.FirstOrDefaultAsync(f => f.EstateObjectId == estateObjectId);

                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == estateObjectId);
                eo.StatusId = 4;

                int tradeId = Convert.ToInt32(request.TradeId);
                Trade trade= await db.Trades.FirstOrDefaultAsync(f => f.Id == tradeId);
                trade.PaymentTypeId = paymentTypeId;
                trade.PaymentInstrumentId = paymentInstrument;

                db.Requests.Remove(request);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteTrade(int estateObjectId)
        {
            using (Agency db = new Agency())
            {
                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == estateObjectId);
                eo.StatusId = 1;

                Trade trade = await db.Trades.FirstOrDefaultAsync(f => f.EstateObjectId == estateObjectId);
                db.Trades.Remove(trade);

                await db.SaveChangesAsync();
            }
        }

        public async Task<MessageViewModel> CreateTrade(int estateObjectId, int managerId)
        {
            using (Agency db = new Agency())
            {
                MessageViewModel model = new MessageViewModel();
                Request request = await db.Requests.FirstOrDefaultAsync(f => f.EstateObjectId == estateObjectId);
                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == estateObjectId);
                if(eo.StatusId==2)
                {
                    eo.StatusId = 3;
                    Trade trade = new Trade
                    {
                        EstateObjectId = request.EstateObjectId,
                        ClientId = request.ClientId,
                        ManagerId = managerId,
                        Date = DateTime.Now
                    };
                    db.Trades.Add(trade);
                    request.TradeId = trade.Id;
                    await db.SaveChangesAsync();
                }
                else
                {
                    model.Type = "Ошибка";
                    model.Message = "Очевидно, другой менеджер только что рассмотрел эту заявку, или её отменили.";
                }
                return model;
            }
        }

        #region Lists

        public List<SelectListItem> PreparePaymentTypes()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> r = new List<SelectListItem>();
                var res = db.PaymentTypes.Select(m => m);
                foreach (var m in res)
                    r.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return r;
            }
        }

        public List<SelectListItem> PreparePaymentInstruments()
        {
            using (Agency db = new Agency())
            {
                List<SelectListItem> r = new List<SelectListItem>();
                var res = db.PaymentInstruments.Select(m => m);
                foreach (var m in res)
                    r.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return r;
            }
        }

        #endregion

        public async Task<TradeRequestViewModel> Model(EstateObject eo)
        {
            using (Agency db = new Agency())
            {
                Trade trade = await db.Trades.FirstOrDefaultAsync(f => f.EstateObjectId == eo.Id);
                TradeRequestViewModel model = new TradeRequestViewModel();
                model = new TradeRequestViewModel
                {
                    EstateObjectId = eo.Id,
                    ClientId = trade.ClientId,
                    ManagerId = trade.ManagerId,
                    TradeId = Convert.ToInt32(trade.Id),

                    Price = eo.Price,
                    Address = eo.Address,
                    Description = eo.Description,
                    Area = eo.Area,
                    Rooms = eo.Rooms,
                    LandDescription = eo.LandDescription,
                    LandArea = eo.LandArea,
                    District = eo.District.Name,
                    RealtyType = eo.RealtyType.Name,
                    TradeType = eo.TradeType.Name,
                    Owner = eo.Owner.Surname + " " + eo.Owner.Name + " " + eo.Owner.Patronymic + ": " + eo.Owner.Phone,

                    ClientEmail = trade.Client.Email,
                    ClientName = trade.Client.Surname + " " + trade.Client.Name + " " + trade.Client.Patronymic,
                    ClientPhone = trade.Client.Phone,

                    ManagerEmail = trade.Manager.Email,
                    ManagerName = trade.Manager.Surname + " " + trade.Manager.Name + " " + trade.Manager.Patronymic,
                    ManagerPhone = trade.Manager.Phone,

                    Date = trade.Date,
                    PaymentInstruments = PreparePaymentInstruments(),
                    PaymentTypes = PreparePaymentTypes(),

                    RealtyTypeId =eo.RealtyTypeId
                };
                if(trade.PaymentInstrument!=null)
                {
                    model.PaymentInstrument = trade.PaymentInstrument.Name;
                    model.PaymentType = trade.PaymentType.Name;
                }
                return model;
            }
        }


        public async Task<MessageViewModel> RejectRequest(int id)
        {
            using (Agency db = new Agency())
            {
                MessageViewModel message = new MessageViewModel();
                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == id);
                if (eo.StatusId == 2)
                {
                    eo.StatusId = 1;
                    Request req = await db.Requests.FirstOrDefaultAsync(f => f.EstateObjectId == id);
                    db.Requests.Remove(req);
                    await db.SaveChangesAsync();
                }
                else
                {
                    message.Type = "Ошибка";
                    message.Message = "Очевидно, другой менеджер только что рассмотрел эту заявку, или её отменили.";
                }
                return message;
            }
        }

        public async Task<List<TradeRequestViewModel>> AllTrades()
        {
            using (Agency db = new Agency())
            {
                var trades = from Trade in db.Trades
                             select Trade;
                var myTrades = new List<TradeRequestViewModel>();
                foreach (Trade trade in trades)
                {
                    EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == trade.EstateObjectId);
                    TradeRequestViewModel model = await Model(eo);
                    myTrades.Add(model);
                }


                return myTrades;
            }
        }
    }
}