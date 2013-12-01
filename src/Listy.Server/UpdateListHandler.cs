using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listy.Core.Extensions;
using Listy.Data.Entities;
using Listy.Messages;
using NHibernate;
using NServiceBus;

namespace Listy.Server
{
    public class UpdateListHandler : IHandleMessages<UpdateList>
    {
        private readonly ISessionFactory _sessionFactory;

        public UpdateListHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Handle(UpdateList message)
        {
            Console.WriteLine("Received UpdateList: {Id} - {Name}".FormatWith(message));

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var list = session.Get<ListyList>(message.Id);

                list.Name = message.Name;

                var ordinal = 0;
                foreach (var item in message.Items ?? new UpdateListItem[0])
                {
                    Console.WriteLine("\t{Id} - {Name}".FormatWith(item));

                    var listItem = GetOrAddItem(list, item);
                    listItem.Ordinal = ordinal++;
                    listItem.Name = item.Name ?? "";
                }

                transaction.Commit();
            }
        }

        static ListyListItem GetOrAddItem(ListyList list, UpdateListItem message)
        {
            if (message.Id.HasValue)
            {
                return list.Items.First(i => i.Id == message.Id);
            }

            var item = new ListyListItem();

            list.Items.Add(item);
            // I'm sure I shouldn't have to do this but it complains about
            // ListyListId being null...
            item.ListyList = list;

            return item;
        }
    }
}
