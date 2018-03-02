using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiteDB;

namespace LatestEpisodesFinder
{
    static class Storage
    {
        internal static IReadOnlyCollection<T> GetAll<T>(string dbFile) => 
            Execute<T, IReadOnlyCollection<T>>(dbFile, collection => collection.FindAll().ToList());

        internal static IReadOnlyCollection<T> GetAll<T>(string dbFile, Expression<Func<T, bool>> predicate) => 
            Execute<T, IReadOnlyCollection<T>>(dbFile, collection => collection.Find(predicate).ToList());

        internal static void Save<T>(string dbFile, T entity) => 
            Execute<T, object>(dbFile, collection => collection.Insert(entity));

        internal static int SaveAll<T>(string dbFile, IEnumerable<T> entities) => 
            Execute<T, int>(dbFile, collection => collection.InsertBulk(entities));

        internal static int UpdateAll<T>(string dbFile, IEnumerable<T> entities) => 
            Execute<T, int>(dbFile, collection => collection.Update(entities));

        static TReturn Execute<T, TReturn>(string dbFile, Func<LiteCollection<T>, TReturn> action)
        {
            using (var database = new LiteDatabase(dbFile))
                return action(database.GetCollection<T>());
        }
    }
}