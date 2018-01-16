using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameRoulette.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int Role { get; set; }
        public int Money { get; set; }
        public string Image { get; set; }

    }
}