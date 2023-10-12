using System;

namespace Marketplace.Models.db;

public class Review
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}