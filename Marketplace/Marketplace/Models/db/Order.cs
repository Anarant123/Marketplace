using System;

namespace Marketplace.Models.db;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
    public virtual User User { get; set; } = null!;
}