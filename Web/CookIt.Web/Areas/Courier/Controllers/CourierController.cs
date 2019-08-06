namespace CookIt.Web.Areas.Courier.Controllers
{
    using CookIt.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.CourierRoleName)]
    [Area("Courier")]
    public class CourierController : Controller
    {
    }
}
