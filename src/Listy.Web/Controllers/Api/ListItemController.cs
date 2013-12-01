using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Listy.Data.Entities;
using Listy.Messages;
using Listy.Web.Models.Api.List;
using NHibernate;
using NServiceBus;

namespace Listy.Web.Controllers.Api
{
    public class ListItemController : ApiController
    {
        private readonly IBus _bus;

        public ListItemController(IBus bus)
        {
            _bus = bus;
        }

        public void Post(Guid? id, ListItemUpdateModel model)
        {
            _bus.Send(model.ToUpdateListItem());
        }
    }
}
