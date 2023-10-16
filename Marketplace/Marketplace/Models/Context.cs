using Marketplace.Models.db;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Models
{
    public static class Context
    {
        public static string host = "https://d843-95-26-76-46.ngrok-free.app";
        public static User? CurrentUser { get; set; } = null;
        public static List<Product> ProductsList { get; set; } = new List<Product>();
        public static List<Order> OrdersList { get; set; } = new List<Order>();
        public static Product? CurrentProduct { get; set; } = null;
    }
}
