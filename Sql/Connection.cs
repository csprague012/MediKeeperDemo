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
                conn.Execute("INSERT INTO ITEMS (name, cost) VALUES (@name, @cost)", item);
            }
        }
        public static void UpdateItem(Item item)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("UPDATE ITEMS SET name=@name, cost=@cost WHERE id=@id", item);
            }
        }
        public static void DeleteItem(Item item)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("DELETE FROM ITEMS WHERE id=@id", item);
            }
        }
        public static void CreateTable() {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Execute("CREATE TABLE \"ITEMS\"(\"id\" INTEGER NOT NULL UNIQUE, \"name\" TEXT, \"cost\" TEXT, PRIMARY KEY(\"id\" AUTOINCREMENT)); ");
            }
        }
        private static string LoadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        
        public static List<Item> GetItems(Item item)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                var items = conn.Query<Item>("SELECT id, name, cost FROM ITEMS WHERE LOWER(name)=LOWER(@name)", item);
                return items.ToList();
            }
        }
    }
}
