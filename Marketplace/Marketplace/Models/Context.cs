using Marketplace.Models.db;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Models
{
    public static class Context
    {
        public static string host = "https://711d-95-26-78-239.ngrok-free.app";
        public static User? CurrentUser { get; set; } = null;
        public static List<Product> ProductsList { get; set; } = new List<Product>();
    }
}
