using Autofac;
using BroadCapture.Infrastructures.Sqlite;
using BroadCapture.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture
{
    public static class ConfigurationContainer
    {
        private static IContainer container;
        public static IContainer Configure()
        {
            if (container != null)
                return container;

            var builder = new ContainerBuilder();
            builder.RegisterType<Application>().AsSelf();
            builder.RegisterType<SqliteDbContext>().As<IDbContext>();

            return container = builder.Build();
        }
    }
}
