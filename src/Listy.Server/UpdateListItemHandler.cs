using System;
using Listy.Core.Extensions;
using Listy.Data.Entities;
using Listy.Messages;
using NHibernate;
using NServiceBus;

namespace Listy.Server
{
    public class UpdateListItemHandler : IHandleMessages<UpdateListItem>
    {
        private readonly ISessionFactory _sessionFactory;

        public UpdateListItemHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Handle(UpdateListItem message)
        {
            Console.WriteLine("Received UpdateListItem: {Id} - {Name}".FormatWith(message));
            
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var item = session.Get<ListyListItem>(message.Id);
                item.Name = message.Name ?? "";
                transaction.Commit();
            }
        }
    }
}