using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Listy.Data.Entities;
using Listy.Data.Persistence;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Listy.Server
{
    public class NhibernateConfig
    {

        public static ISessionFactory Register(string connectionString)
        {
            var persistenceConfigurer =
                MsSqlConfiguration.MsSql2008
                                  .ConnectionString(connectionString);

            return Fluently.Configure()
                           .Database(persistenceConfigurer)
                           .Mappings(m => m.AutoMappings.Add(CreateAutomappings))
                           .BuildSessionFactory();
        }

        static void BuildSchema(Configuration config)
        {
            new SchemaExport(config)
                .Create(false, true);
        }

        static AutoPersistenceModel CreateAutomappings()
        {
            return AutoMap.AssemblyOf<ListyList>(new ListyAutomappingConfiguration())
                .Conventions.Add<CascadeConvention>()
                .Conventions.Add<ListyForeignKeyConvention>()
                ;
        }
    }
}