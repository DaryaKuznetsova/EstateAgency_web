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
                             where Trade.ClientId == clientId
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

        public async Task<MessageViewModel> CreateTrade(int requestId, int managerId)
        {
            using (Agency db = new Agency())
            {
                MessageViewModel model = new MessageViewModel();
                Request request = await db.Requests.FirstOrDefaultAsync(f => f.Id == requestId);
                EstateObject eo = await db.EstateObjects.FirstOrDefaultAsync(f => f.Id == request.EstateObjectId);
                if(eo.StatusId==2)
                {
                    eo.StatusId = 3;
                    Trade trade = new Trade
                    {
                        EstateObjectId = request.EstateObjectId,
                        ClientId = request.ClientId,
                        ManagerId = managerId,
                        Date = DateTime.Today
                    };
                    db.Trades.Add(trade);
                    request.TradeId = trade.Id;
                    await db.SaveChangesAsync();
                    model.Type = "Заявка принята";
                }
                else
                {
                    model.Type = "Ошибка";
                    model.Message = "Заявка не была принята. Возможно, её рассмотрел другой менеджер.";
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
                Request request = await db.Requests.FirstOrDefaultAsync(f => f.EstateObjectId == eo.Id);
                TradeRequestViewModel model = new TradeRequestViewModel
                {
                    EstateObjectId = eo.Id,
                    ClientId = request.ClientId,
                    ManagerId = request.Trade.ManagerId,
                    TradeId = Convert.ToInt32(request.TradeId),

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

                    ClientEmail = request.Client.Email,
                    ClientName = request.Client.Surname + " " + request.Client.Name + " " + request.Client.Patronymic,
                    ClientPhone = request.Client.Phone,

                    ManagerEmail = request.Trade.Manager.Email,
                    ManagerName = request.Trade.Manager.Surname + " " + request.Trade.Manager.Name + " " + request.Trade.Manager.Patronymic,
                    ManagerPhone = request.Trade.Manager.Phone,

                    Date = request.Trade.Date,
                    PaymentInstrumentId = 0,
                    PaymentInstruments = PreparePaymentInstruments(),
                    PaymentTypeId = 0,
                    PaymentTypes = PreparePaymentTypes(),
                    PaymentInstrument=request.Trade.PaymentInstrument.Name,
                    PaymentType = request.Trade.PaymentType.Name,

                    RealtyTypeId =eo.RealtyTypeId
                };
                return model;
            }
        }
    }
}