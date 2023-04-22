using System;
using System.Data;
using System.Data.SQLite;
using Database.Schemas;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;

namespace Database {
    public class Database {
        public static Database instance = new Database();
        public IDbConnection connection;
        
        private const string ConnectionString = "Server=127.0.0.1;Database=db_lostkingdom;Uid=root;Pwd=;";

        public void Connect() {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(
                ConnectionString,
                MySqlDialectProvider.Instance);
            connection = dbFactory.Open();
            InitTables();
            Console.WriteLine("Database connected.");
        }

        private void InitTables() {
            if (!connection.TableExists<User_Account>()) {
                connection.CreateTable<User_Account>();
            }
        }
        
    }
}