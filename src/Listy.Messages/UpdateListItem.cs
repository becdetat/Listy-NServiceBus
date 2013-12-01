using System;
using NServiceBus;

namespace Listy.Messages
{
    public class UpdateListItem : ICommand
    {
        public UpdateListItem(Guid? id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid? Id { get; private set; }
        public string Name { get; private set; }
    }
}