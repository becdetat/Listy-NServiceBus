using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Listy.Data.Entities;
using Listy.Web.Models.Api.List;
using NHibernate;
using NServiceBus;

namespace Listy.Web.Controllers.Api
{
    public class ListController : ApiController
    {
        private IBus _bus;

        public ListController(IBus bus)
        {
            _bus = bus;
        }

        public void Post(Guid id, ListUpdateModel model)
        {
            _bus.Send(model.ToUpdateList(id));
        }
    }
}
