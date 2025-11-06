using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OP1_API.Areas.Login.Models;
using OP1_API.Areas.OP1.Models;
using System.Data.SqlClient;
using System.Data;
using OP1_API.ClassCollection;

namespace OP1_API.Areas.OP1.Controllers
{
    [Route("api/op1/")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly GlobalClass cl;
        DocumentUpload du;
        public DashboardController(DocumentUpload _du)
        {
            cl = new GlobalClass();
            du = _du;
        }

        [Route("GET_DASHBOARD_COUNT")]
        [HttpPost]
        [AllowAnonymous]
        public List<DashboardModel> GetDashboardCount(DashboardModelParam db)
        {
            List<DashboardModel> r = new List<DashboardModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetDashboardCount", new SqlParameter[]
                {
                    new SqlParameter("@EntryBy", _userDetails.userCode),
                   
                }, _userDetails.finYear);


                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new DashboardModel
                    {
                        OrderCount = Convert.ToInt32(row["OrderCount"]),
                        InvoiceCount = Convert.ToInt32(row["SalesInvoiceCount"]),
                        TotalAmount = Convert.ToInt32(row["SalesAmt"]),
                        DuesAmt = Convert.ToInt32(row["TotalSalesDues"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        [Route("GET_GetSalesSummaryCount_COUNT")]
        [HttpGet]
        [AllowAnonymous]
        public List<GetSalesSummaryModel> GetSalesSummaryCount(GetSalesSummaryModelParam db)
        {
            List<GetSalesSummaryModel> r = new List<GetSalesSummaryModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetSalesSummaryCount", new SqlParameter[]
                {
                    new SqlParameter("@UserId", _userDetails.userCode),

                }, _userDetails.finYear);


                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new GetSalesSummaryModel
                    {
                        SalesTarget = Convert.ToInt32(row["SalesTarget"]),
                        SalesTargetForToday = Convert.ToInt32(row["SalesTargetForToday"]),
                        SalesTargetForWeek = Convert.ToInt32(row["SalesTargetForWeek"]),
                        SalesTargetForMonth = Convert.ToInt32(row["SalesTargetForMonth"]),
                        SalesTargetForLast3Months = Convert.ToInt32(row["SalesTargetForLast3Months"]),
                        TotalSales = Convert.ToInt32(row["TotalSales"])

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

    }
}
