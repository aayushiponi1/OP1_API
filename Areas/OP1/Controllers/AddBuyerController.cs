using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OP1_API.Areas.Login.Models;
using OP1_API.Areas.OP1.Models;
using OP1_API.ClassCollection;
using OP1_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace OP1_API.Areas.OP1.Controllers
{
    [Route("api/op1/")]
    [ApiController]
    [Authorize]
    public class AddBuyerController : ControllerBase
    {
        private readonly GlobalClass cl;
        DocumentUpload du;
        public AddBuyerController(DocumentUpload _du)
        {
            cl = new GlobalClass();
            du = _du;
        }


        [Route("Add-Buyer")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult BuyerSave(AddBuyerModel? o)
        {
            ReturnResult r = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
            //End

            try
            {
                string userid = _userDetails.userCode; // cl.checkUserID();
                if (o.Code == "New")
                {
                    o.Code = "0";
                }

                DataTable dt = new DataTable();
                dt = cl.Rj_LoadTableWithProc("sp_OP1_Buyersave_New", new SqlParameter[]
                {
                   new SqlParameter("@Code", o.Code),
                    //new SqlParameter("@tp", o.Tp),
                    new SqlParameter("@Client", o.Client),
                    new SqlParameter("@bilingtype", o.Pricingtype),
                    new SqlParameter("@Name", o.Name),
                    new SqlParameter("@ContactPerson", o.ContactPerson),
                    new SqlParameter("@phone", o.Phone),
                    new SqlParameter("@EmailId", o.EmailId),
                    new SqlParameter("@Gstin", o.Gstin),
                    new SqlParameter("@targetamt", o.TargetAmt),
                    
                    new SqlParameter("@Userid",userid)
                    //new SqlParameter("@ud_AddressDetails", dtUd)
                }, _userDetails.finYear);
                if (dt.Rows.Count > 0)
                {
                    r.result = dt.Rows[0]["result"].ToString();
                    r.retval = dt.Rows[0]["retval"].ToString();
                    r.msg = dt.Rows[0]["msg"].ToString();
                    r.userCode = _userDetails.userCode;
                    r.userRole = _userDetails.userRole;
                    r.userCompany = _userDetails.userCompany;
                }
                else
                {
                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "Something went wrong !";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = ex.ToString();
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }

        [Route("Add-Buyer-Billing")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult BuyerBillingSave(AddBuyerModel? o)
        {
            ReturnResult r = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
            //End

            try
            {
                string userid = _userDetails.userCode; // cl.checkUserID();
               

                DataTable dt = new DataTable();
                dt = cl.Rj_LoadTableWithProc("sp_OP1_BuyerBillingSave", new SqlParameter[]
                {
                   new SqlParameter("@Code", o.Code),
                    
                    new SqlParameter("@Address", o.BillingAddress),
                    new SqlParameter("@state", o.Billingstste),
                    new SqlParameter("@city", o.BillingCity),
                    new SqlParameter("@pincode", o.BillingPin),
                    
                    new SqlParameter("@Userid",userid)
                    //new SqlParameter("@ud_AddressDetails", dtUd)
                }, _userDetails.finYear);
                if (dt.Rows.Count > 0)
                {
                    r.result = dt.Rows[0]["result"].ToString();
                    r.retval = dt.Rows[0]["retval"].ToString();
                    r.msg = dt.Rows[0]["msg"].ToString();
                    r.userCode = _userDetails.userCode;
                    r.userRole = _userDetails.userRole;
                    r.userCompany = _userDetails.userCompany;
                }
                else
                {
                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "Something went wrong !";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = ex.ToString();
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }

        [Route("Add-Buyer-Address")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult BuyerDelieverySave(AddBuyerModel? o)
        {
            ReturnResult r = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
            //End

            try
            {
                string userid = _userDetails.userCode; // cl.checkUserID();
                if (o.Code == "New")
                {
                    o.Code = "0";
                }
                DataTable dtUd = new DataTable();
                dtUd.Columns.Add("sno", typeof(Int32));
                dtUd.Columns.Add("buyerCode", typeof(Int32));
                dtUd.Columns.Add("addressType", typeof(string));
                dtUd.Columns.Add("addressTypeName", typeof(string));
                dtUd.Columns.Add("gstin", typeof(string));
                dtUd.Columns.Add("personname", typeof(string));
                dtUd.Columns.Add("personmobile", typeof(string));
                dtUd.Columns.Add("Address", typeof(string));
                dtUd.Columns.Add("stateId", typeof(Int32));
                dtUd.Columns.Add("cityname", typeof(string));
                dtUd.Columns.Add("pinCode", typeof(string));
                dtUd.Columns.Add("codeid", typeof(Int32));
                DataRow dr;
                for (int i = 0; i < o.AddressDetails.Count; i++)
                {
                    dr = dtUd.NewRow();
                    dr["sno"] = Convert.ToInt32(o.AddressDetails[i].sno);
                    dr["buyerCode"] = Convert.ToInt32(o.Code);
                    dr["addressType"] = o.AddressDetails[i].deliverytype.ToString();
                    dr["addressTypeName"] = o.AddressDetails[i].deliverytypeName.ToString();
                    dr["gstin"] = o.AddressDetails[i].deliverygstin.ToString();
                    dr["personname"] = o.AddressDetails[i].deliverypersonname.ToString();
                    dr["personmobile"] = o.AddressDetails[i].deliverymobile.ToString();
                    dr["Address"] = o.AddressDetails[i].deliveryaddress.ToString();
                    dr["stateId"] = Convert.ToInt32(o.AddressDetails[i].deliverystatecode);
                    dr["cityname"] = o.AddressDetails[i].deliverycity.ToString();
                    dr["pinCode"] = o.AddressDetails[i].deliverypincode.ToString();
                    dr["codeid"] = Convert.ToInt32(o.AddressDetails[i].codeid);
                    dtUd.Rows.Add(dr);
                }

                DataTable dt = new DataTable();
                dt = cl.Rj_LoadTableWithProc("sp_SaveBuyerDeliveryAddresses", new SqlParameter[]
                {
                   new SqlParameter("@Code", o.Code),
                    new SqlParameter("@ud_AddressDetails", dtUd)
                }, _userDetails.finYear);
                if (dt.Rows.Count > 0)
                {
                    r.result = dt.Rows[0]["result"].ToString();
                    r.retval = dt.Rows[0]["retval"].ToString();
                    r.msg = dt.Rows[0]["msg"].ToString();
                    r.userCode = _userDetails.userCode;
                    r.userRole = _userDetails.userRole;
                    r.userCompany = _userDetails.userCompany;
                }
                else
                {
                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "Something went wrong !";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = ex.ToString();
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }





        //Edit
        [Route("Buyer-ByCode")]
        [HttpPost]
        [AllowAnonymous]
        public AddBuyerModel GetBuyerByCode(GetBuyerParam param)
        {
            AddBuyerModel b = new AddBuyerModel();
            try
            {
                //User Details
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = new DataTable();
                dt = cl.Rj_LoadTableWithProc("sp_OP1_GetBuyerDetails", new SqlParameter[]
                {
                    new SqlParameter("@code", param.code)
                }, _userDetails.finYear);
                if (dt.Rows.Count > 0)
                {
                    b.Code = dt.Rows[0]["code"].ToString();
                    b.Client = dt.Rows[0]["Client"].ToString();
                    b.Pricingtype = dt.Rows[0]["bilingtype"].ToString();
                    b.Name = dt.Rows[0]["Name"].ToString();
                    b.ContactPerson = dt.Rows[0]["ContactPerson"].ToString();
                    b.Phone = dt.Rows[0]["phone"].ToString();
                    b.EmailId = dt.Rows[0]["EmailId"].ToString();
                    b.Gstin = dt.Rows[0]["Gstin"].ToString();
                    b.TargetAmt = dt.Rows[0]["targetamt"].ToString();
                    b.BillingAddress = dt.Rows[0]["Address"].ToString();
                    b.Billingstste = dt.Rows[0]["state"].ToString();
                    b.BillingCity = dt.Rows[0]["city"].ToString();
                    b.BillingPin = dt.Rows[0]["pincode"].ToString();
                    b.AddressDetails = new List<AddressDetails>();

                    foreach (DataRow row in dt.Rows)
                    {
                        b.AddressDetails.Add(new AddressDetails
                        {
                            sno = row["sno"].ToString(),
                            deliverytype = row["addressType"].ToString(),
                            deliverytypeName = row["addressTypeName"].ToString(),
                            deliverygstin = row["gstin"].ToString(),
                            deliverypersonname = row["personname"].ToString(),
                            deliverymobile = row["personmobile"].ToString(),
                            deliveryaddress = row["Address"].ToString(),
                            deliverystatename = row["statename"].ToString(),
                            deliverystatecode = row["stateId"].ToString(),
                            deliverycity = row["cityname"].ToString(),
                            deliverypincode = row["pinCode"].ToString()
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return b;
        }

    }
}
