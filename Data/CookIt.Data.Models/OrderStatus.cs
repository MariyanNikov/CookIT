namespace CookIt.Data.Models
{
    using System.Collections.Generic;

    using CookIt.Data.Common.Models;

    public class OrderStatus : BaseModel<int>
    {
        public OrderStatus()
        {
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
