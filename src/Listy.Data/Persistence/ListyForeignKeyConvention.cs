using System;
using FluentNHibernate.Conventions;
using Listy.Core.Extensions;

namespace Listy.Data.Persistence
{
    public class ListyForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(FluentNHibernate.Member property, Type type)
        {
            if (property != null) return "{0}Id".FormatWith(property.Name);
            if (type != null) return "{0}Id".FormatWith(type.Name);
            throw new ForeignKeyRelationshipNotSupportedException();
        }
    }
}