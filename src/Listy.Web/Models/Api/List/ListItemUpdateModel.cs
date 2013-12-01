using System;
using Listy.Messages;

namespace Listy.Web.Models.Api.List
{
    public class ListItemUpdateModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public UpdateListItem ToUpdateListItem()
        {
            return new UpdateListItem(Id, Name);
        }
    }
}