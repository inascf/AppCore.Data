﻿using System.Collections.Generic;
using System.Configuration;
using FutureState.AppCore.Data.SqlServer;

namespace FutureState.AppCore.Data.Tests.Integration
{
    public abstract class DbProviderTestBase
    {
        public IEnumerable<IDbProvider> DbProviders()
        {
            var testDbName = ConfigurationManager.AppSettings["databaseName"];
            var sqlServerConnection = ConfigurationManager.ConnectionStrings["databaseConnection"];

            var sqlDbConnectionProvider = new DbConnectionProvider(
                sqlServerConnection.ConnectionString,
                sqlServerConnection.ProviderName);

            IDbProvider sqlDbProvider = new DbProvider(sqlDbConnectionProvider, testDbName);
            IDbProvider sqliteDbProvider = new Sqlite.DbProvider(testDbName).WithForeignKeyConstraints();

            yield return sqlDbProvider;
            yield return sqliteDbProvider;
        }

        protected string TraceObjectGraphInfo(IDbProvider dbProvider)
        {
            var dbProviderFriendlyName = dbProvider.GetType().ToString()
                                                   .Replace("FutureState.AppCore.Data.", "'")
                                                   .Replace(".", "' ");

            return "Tested against the " + dbProviderFriendlyName;
        }
    }
}