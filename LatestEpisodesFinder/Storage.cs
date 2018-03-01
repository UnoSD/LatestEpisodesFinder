using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace LatestEpisodesFinder
{
    static class Storage
    {
        internal static IReadOnlyCollection<T> GetAll<T>(string dbFile) => 
            Execute<T, IReadOnlyCollection<T>>(dbFile, collection => collection.FindAll().ToList());

        internal static int SaveAll<T>(string dbFile, IEnumerable<T> entities) => 
            Execute<T, int>(dbFile, collection => collection.InsertBulk(entities));

        static TReturn Execute<T, TReturn>(string dbFile, Func<LiteCollection<T>, TReturn> action)
        {
            using (var database = new LiteDatabase(dbFile))
                return action(database.GetCollection<T>());
        }
    }
}