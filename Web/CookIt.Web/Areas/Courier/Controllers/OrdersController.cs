namespace CookIt.Web.Areas.Courier.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookIt.Common;
    using CookIt.Data.Extensions;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Order;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using X.PagedList;

    public class OrdersController : CourierController
    {
        private const string ErrorMessageOrderAlreadyTaken = "This order has been taken.";
        private const string ErrorMessageOrderAlreadyDelivered = "This order has been delivered.";
        private const string SuccessMessageTakeOrder = "You have successfully taken order with ID: {0}";
        private const string SuccessMessageDeliverOrder = "You are delivering order with ID: {0}";
        private const string SuccessMessageDeliveredOrder = "You have successfully delivered order with ID: {0}";

        private const string DeliveringEmailTitle = "CookIt - Delivering";
        private const string DeliveringEmailBody = "Your order will be delivered by {0} on {1}.";
        private const string RecipesInstructionsEmailTitle = "Recipe Instructions";

        private const int DefaultPage = 1;
        private const int DefaultPageSize = 5;

        private readonly IOrderService orderService;
        private readonly IEmailSender emailSender;

        public OrdersController(
            IOrderService orderService,
            IEmailSender emailSender)
        {
            this.orderService = orderService;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Processed(int? p)
        {
            var page = p ?? DefaultPage;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() == GlobalConstants.ProcessedOrderStatus.ToLower())
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Take(string id)
        {
            if (!this.orderService.IsAtStatus(id, GlobalConstants.ProcessedOrderStatus))
            {
                this.TempData["StatusMessage"] = ErrorMessageOrderAlreadyTaken;
                return this.RedirectToAction(nameof(this.Processed));
            }

            var courierId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.orderService.TakeOrder(id, courierId);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageTakeOrder, id);
            return this.RedirectToAction(nameof(this.ToDeliver));
        }

        public async Task<IActionResult> ToDeliver(int? p)
        {
            var page = p ?? DefaultPage;

            var courierId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() == GlobalConstants.GettingIngredientsOrderStatus.ToLower())
                .Where(x => x.CourierId == courierId)
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(string id)
        {
            if (!this.orderService.IsAtStatus(id, GlobalConstants.GettingIngredientsOrderStatus))
            {
                this.TempData["StatusMessage"] = ErrorMessageOrderAlreadyDelivered;
                return this.RedirectToAction(nameof(this.Processed));
            }

            await this.orderService.DeliverOrder(id);

            var issuerEmail = this.orderService.GetIssuerEmailByOrderId(id);
            var courierFullName = this.User.Identity.GetFullName();

            await this.emailSender.SendEmailAsync(
               issuerEmail,
               DeliveringEmailTitle,
               string.Format(DeliveringEmailBody, courierFullName, DateTime.UtcNow.ToString("D")));

            this.TempData["StatusMessage"] = string.Format(SuccessMessageDeliverOrder, id);
            return this.RedirectToAction(nameof(this.Delivered));
        }

        public async Task<IActionResult> Delivered(int? p)
        {
            var page = p ?? DefaultPage;

            var courierId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() == GlobalConstants.DeliveringOrderStatus.ToLower())
                .Where(x => x.CourierId == courierId)
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Delivered(string id)
        {
            if (!this.orderService.IsAtStatus(id, GlobalConstants.DeliveringOrderStatus))
            {
                this.TempData["StatusMessage"] = ErrorMessageOrderAlreadyDelivered;
                return this.RedirectToAction(nameof(this.Processed));
            }

            await this.orderService.AcquiredOrder(id);

            var issuerEmail = this.orderService.GetIssuerEmailByOrderId(id);
            var instructions = this.orderService.GetAllRecipesInstructionsForOrder(id);

            await this.emailSender.SendEmailAsync(
               issuerEmail,
               RecipesInstructionsEmailTitle,
               instructions);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageDeliveredOrder, id);
            return this.RedirectToAction(nameof(this.Delivered));
        }
    }
}
