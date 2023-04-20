using System;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace Database.Schemas {
    public class User_Account {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string login { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string email { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string pasword { get; set; }
        
        [Default(OrmLiteVariables.SystemUtc)]
        public DateTime timeRegistered { get; set; }
        public DateTime timeLoggedIn { get; set; }
    }
}