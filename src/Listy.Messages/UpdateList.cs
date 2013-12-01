using System;
using NServiceBus;

namespace Listy.Messages
{
    public class UpdateList : ICommand
    {
        public UpdateList(Guid id, string name, UpdateListItem[] items)
        {
            Id = id;
            Name = name;
            Items = items;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public UpdateListItem[] Items { get; private set; }
    }
}