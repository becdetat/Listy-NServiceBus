using System;
using System.Linq;
using Listy.Messages;

namespace Listy.Web.Models.Api.List
{
    public class ListUpdateModel
    {
        public string Name { get; set; }
        public ListItemUpdateModel[] Items { get; set; }

        public UpdateList ToUpdateList(Guid id)
        {
            return new UpdateList(id, Name, Items.Select(x => x.ToUpdateListItem()).ToArray());
        }
    }
}