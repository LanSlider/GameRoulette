using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameRoulette.Models
{
    public class AppID
    {
        public int ID { get; set; }
        public int appID { get; set; }
        public string PrivateKey { get; set; }
        public string SteamKey { get; set; }
    }
}