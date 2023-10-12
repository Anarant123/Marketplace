using System.Collections.Generic;

namespace Marketplace.Models.db;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}