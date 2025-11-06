using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OP1_API.Areas.Login.Models;
using OP1_API.Areas.OP1.Models;
using OP1_API.ClassCollection;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace OP1_API.Areas.OP1.Controllers
{
    [Route("api/op1/")]
    [ApiController]
    public class GetSalesSummaryListController : ControllerBase
    {
        private readonly GlobalClass cl;
        private readonly DocumentUpload du;

        public GetSalesSummaryListController(DocumentUpload _du)
        {
            cl = new GlobalClass();
            du = _du;
        }

        [Route("GET_SALES_SUMMARY_LIST")]
        [HttpGet]
        [AllowAnonymous]
        public GetSalesSummaryListModel GetSalesSummaryList(int companyCode)
        {
            GetSalesSummaryListModel model = new GetSalesSummaryListModel();

            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                // Call Stored Procedure
                DataSet ds = cl.Rj_LoadDataSetWithProc("sp_Get_Sales_SummaryList", new SqlParameter[]
                {
                    new SqlParameter("@CompanyCode", _userDetails.userCompany)
                }, _userDetails.finYear);

                // 1️⃣ Top 10 Customers
                if (ds.Tables.Count > 0)
                {
                    model.GetTopTenCustomerModels = (from DataRow row in ds.Tables[0].Rows
                                                     select new GetTopTenCustomerModel
                                                     {
                                                         CustomerName = row["CustomerName"]?.ToString(),
                                                         TotalAmount = row["TotalAmount"]?.ToString()
                                                     }).ToList();
                }

                // 2️⃣ Low Stock Levels
                if (ds.Tables.Count > 1)
                {
                    model.LowStockLevelsModels = (from DataRow row in ds.Tables[1].Rows
                                                  select new LowStockLevelsModel
                                                  {
                                                      Items = row["Items"]?.ToString(),
                                                      quantity = row["quantity"]?.ToString()
                                                  }).ToList();
                }

                // 3️⃣ Current Stock Status
                if (ds.Tables.Count > 2)
                {
                    model.CurrentStockStatusModels = (from DataRow row in ds.Tables[2].Rows
                                                      select new CurrentStockStatusModel
                                                      {
                                                          Items = row["Items"]?.ToString(),
                                                          quantity = row["quantity"]?.ToString()
                                                      }).ToList();
                }

                // 4️⃣ Cancelled Orders in Last 7 Days
                if (ds.Tables.Count > 3)
                {
                    model.cancelledOrdersModels = (from DataRow row in ds.Tables[3].Rows
                                                   select new CancelledOrdersModel
                                                   {
                                                       OrderNo = row["OrderNo"]?.ToString(),
                                                       CustomerName = row["CustomerName"]?.ToString(),
                                                       TotalAmount = row["TotalAmount"]?.ToString()
                                                   }).ToList();
                }

                // 5️⃣ Top 10 Selling Products
                if (ds.Tables.Count > 4)
                {
                    model.SellingProductsModels = (from DataRow row in ds.Tables[4].Rows
                                                   select new SellingProductsModel
                                                   {
                                                       Items = row["Items"]?.ToString(),
                                                       Qty = row["Qty"]?.ToString(),
                                                       Amount = row["Amount"]?.ToString()
                                                   }).ToList();
                }

                model.CompanyCode = companyCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }
    }
}
