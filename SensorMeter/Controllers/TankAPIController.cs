using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SensorMeter.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SensorMeter.Controllers
{
    public class TankAPIController : ApiController
    {
        private SMChartEntities db = new SMChartEntities();
        // GET: api/Tank
        public IEnumerable<TankDTO> Get()
        {
            var Collection = from tr in db.Tanks
                             where tr.IsDeleted == false
                             select new TankDTO()
                             {
                                 ID = tr.TankID,
                                 Code = tr.TankCode,
                                 Name = tr.TankName,
                                 Description = tr.TankDescription,
                                 NumCompartment = tr.NumCompartment
                             };
            return Collection;
        }

        // GET: api/Tank/5
        public TankDTO Get(int id)
        {
            var Collection = (from tr in db.Tanks
                              where tr.IsDeleted == false
                              select new TankDTO()
                              {
                                  ID = tr.TankID,
                                  Code = tr.TankCode,
                                  Name = tr.TankName,
                                  Description = tr.TankDescription,
                                  NumCompartment = tr.NumCompartment,
                              }).FirstOrDefault();
            if (Collection != null)
            {
                Collection.Keys = (from tr in db.Compartments
                                   join cm in db.Tanks on tr.CompartmentID equals cm.CompartmentID
                                   select new CompartmentsDTO()
                                   {
                                       Key = tr.CompartmentKey
                                   }).ToList();

            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Collection;
        }

        // POST: api/Tank
        public HttpResponseMessage Post(TankDTO Model)
        {
            if (ModelState.IsValid)
            {
                Tank NewRecord = new Tank()
                {
                    TankCode = Model.Code,
                    TankName = Model.Name,
                    TankDescription = Model.Description,
                    NumCompartment = Model.NumCompartment,
                    CreateDate = DateTime.Now
                };
                db.Tanks.Add(NewRecord);
                for (int i = 1; i <= NewRecord.NumCompartment; i++ )
                {
                    Compartment Comp = new Compartment()
                    {
                        CompartmentKey = GetKey()+"-"+i,
                        CreateDate = DateTime.Now
                    };
                    db.Compartments.Add(Comp);
                }
                try
                {
                    db.SaveChanges();
                    Model.ID = NewRecord.TankID;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, Model);
                    response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = Model.ID }));
                    return response;
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT: api/Tank/5
        public HttpResponseMessage Put(int id, TankDTO Model)
        {
            if (ModelState.IsValid && id == Model.ID)
            {
                Model.ModifyDate = DateTime.Now;
                Tank UpdateRecord = new Tank()
                {
                    TankID = Model.ID,
                    TankCode = Model.Code,
                    TankName = Model.Name,
                    TankDescription = Model.Description,
                    NumCompartment = Model.NumCompartment,
                    ModifyDate = DateTime.Now
                };
                db.Tanks.Attach(UpdateRecord);
                db.Entry(UpdateRecord).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/Tank/5
        public HttpResponseMessage Delete(int id)
        {
            Tank DeleteRecord = db.Tanks.Single(tr => tr.CompartmentID == id);
            DeleteRecord.ModifyDate = DateTime.Now;
            DeleteRecord.IsDeleted = true;
            db.Tanks.Attach(DeleteRecord);
            db.Entry(DeleteRecord).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public string GetKey()
        {
            DateTime FullDate = DateTime.Now;
            string Day = FullDate.Day.ToString().PadLeft(2, '0');
            string Month = FullDate.Month.ToString().PadLeft(2, '0');
            string Year = FullDate.Year.ToString().Remove(0, 2);
            string Code, Key;
            DateTime LastDate = (from tr in db.Compartments
                                      where tr.CompartmentID == db.Compartments.Max(o => o.CompartmentID)
                                      select tr.CreateDate).FirstOrDefault();
            LastDate = LastDate.Date;
            int Counter = Convert.ToInt32(((from tr in db.Compartments
                                            where tr.CompartmentID == db.Compartments.Max(o => o.CompartmentID)
                                            select tr.CompartmentKey).FirstOrDefault()).Substring(6, 2));
            if (LastDate.Month != DateTime.Now.Month)
            {
                Counter = 0;
            }
            Counter++;
            Code = Counter.ToString().PadLeft(2, '0');
            Key = Day + Month + Year + Code;
            return Key;
        }
    }
}
