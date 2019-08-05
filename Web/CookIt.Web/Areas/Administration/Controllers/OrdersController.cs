namespace CookIt.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CookIt.Common;
    using CookIt.Services.Data;
    using CookIt.Web.ViewModels.Order;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using X.PagedList;

    public class OrdersController : AdministrationController
    {
        private const string ErrorMessageOrderAlreadyProcessed = "This order has been processed already.";
        private const string ErrorMessageCancelProcessedOrder = "This order has been processed and cannot be canceled.";
        private const string SuccessMessageOrderConfirmed = "You have successfully confirmed order with ID: {0}";
        private const string SuccessMessageCancelOrder = "You have successfully canceled order with ID: {0}";
        private const string CancellingEmailTitle = "You order has been canceled.";
        private const string CancellingEmailBody = "We are sorry to inform you that the order you placed with ID: {0} has been canceled.";

        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        private readonly IOrderService orderService;
        private readonly IEmailSender emailSender;

        public OrdersController(
            IOrderService orderService,
            IEmailSender emailSender)
        {
            this.orderService = orderService;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Active(int? p)
        {
            var page = p ?? DefaultPage;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() != GlobalConstants.PendingOrderStatus.ToLower() &&
                        x.OrderStatusName.ToLower() != GlobalConstants.AcquiredOrderStatus.ToLower())
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        public async Task<IActionResult> Pending(int? p)
        {
            var page = p ?? DefaultPage;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() == GlobalConstants.PendingOrderStatus.ToLower())
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        public async Task<IActionResult> Finished(int? p)
        {
            var page = p ?? DefaultPage;

            var orders = await this.orderService
                .GetAllOrders<OrderAllViewModel>()
                .Where(x => x.OrderStatusName.ToLower() == GlobalConstants.AcquiredOrderStatus.ToLower())
                .OrderByDescending(x => x.IssuedOn)
                .ToPagedListAsync(page, DefaultPageSize);

            return this.View(orders);
        }

        public async Task<IActionResult> Confirm(string id)
        {
            if (!this.orderService.IsPending(id))
            {
                this.TempData["StatusMessage"] = ErrorMessageOrderAlreadyProcessed;
                return this.RedirectToAction(nameof(this.Active));
            }

            await this.orderService.ConfirmOrder(id);

            this.TempData["StatusMessage"] = string.Format(SuccessMessageOrderConfirmed, id);
            return this.RedirectToAction(nameof(this.Active));
        }

        public async Task<IActionResult> Cancel(string id)
        {
            if (!this.orderService.IsPending(id))
            {
                this.TempData["StatusMessage"] = ErrorMessageCancelProcessedOrder;
                return this.RedirectToAction(nameof(this.Active));
            }

            var issuerEmail = this.orderService.GetIssuerEmailByOrderId(id);
            await this.orderService.CancelOrder(id);

            await this.emailSender.SendEmailAsync(
               issuerEmail,
               CancellingEmailTitle,
               string.Format(CancellingEmailBody, id));

            this.TempData["StatusMessage"] = string.Format(SuccessMessageCancelOrder, id);
            return this.RedirectToAction(nameof(this.Pending));
        }
    }
}
