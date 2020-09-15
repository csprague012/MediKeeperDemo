using Dapper;
using MediKeeperDemo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Sql
{
    public class Connection
    {
        public static List<Item> LoadItems() {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString())) 
            {
                var items = conn.Query<Item>("SELECT * FROM ITEMS", new DynamicParameters());
                return items.ToList();
            }
        }
        public static void SaveItem(Item item) {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("INSERT INTO ITEMS (name, cost) VALUES (@name, @cost) WHERE id=@id", item);
            }
        }
        public static void DeleteItem(Item item)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("DELETE FROM ITEMS WHERE id=@id", item);
            }
        }
        private static string LoadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
