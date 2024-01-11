using KGERP.Service.Implementation.Dashboard_service;
using System.Web.Mvc;

namespace KGERP.Controllers
{
    public class SeedController : Controller
    {
        private readonly DashboardService _DashboardService;
        public SeedController(DashboardService dashboardService)
        {
            _DashboardService = dashboardService;
        }

        [HttpGet]
        public ActionResult Index(int companyId)
        {
            var vendor = _DashboardService.AllCount(companyId);
            return View(vendor);
            
        }
    }
}