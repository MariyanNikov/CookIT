namespace CookIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Models;

    public class OrderStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.OrderStatuses.Any())
            {
                return;
            }

            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = GlobalConstants.PendingOrderStatus});
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = GlobalConstants.ProcessedOrderStatus });
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = GlobalConstants.GettingIngredientsOrderStatus });
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = GlobalConstants.DeliveringOrderStatus});
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = GlobalConstants.AcquiredOrderStatus });
        }
    }
}
