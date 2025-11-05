using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OP1_API.Areas.Login.Models;
using OP1_API.ClassCollection;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace OP1_API.Areas.LoginAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        GlobalClass _cl;
        IConfiguration _configuration;
        public LoginController(GlobalClass cl, IConfiguration confuguration)
        {
            _configuration = confuguration;
            _cl = cl;
        }


        //[Route("login")]
        //[HttpPost]
        //[AllowAnonymous]
        //public LoginResultModel login(LoginParam p)
        //{
        //    LoginAuthentication lg = new LoginAuthentication(_cl, _configuration);
        //    LoginResultModel r = new LoginResultModel();
        //    r = lg.login(p);
        //    //TokenClass tc = new TokenClass();

        //    TokenGeneration tk = new TokenGeneration(_cl, _configuration);
        //    r.token = "";
        //    //HttpContext.Session.SetString("finYear", p.finYear);

        //    _cl.pwdEnc(p.username, p.password, p.finYear);
        //    string passWord = _cl.rj_encrypt(p.password);
        //    DataTable dt = new DataTable();
        //    dt = _cl.Rj_LoadTableWithProc("wms_userLogin_v3", new SqlParameter[]
        //    {
        //        new SqlParameter("@username", p.username),
        //        new SqlParameter("@password", passWord),
        //        new SqlParameter("@ipAddress", p.ipAddress)
        //    }, p.finYear);
        //    if (dt.Rows.Count > 0)
        //    {
        //        r.result = dt.Rows[0]["type"].ToString();
        //        r.retval = dt.Rows[0]["sno"].ToString();
        //        r.msg = dt.Rows[0]["msg"].ToString();
        //        r.userCode = dt.Rows[0]["sno"].ToString();
        //        r.userRole = dt.Rows[0]["opGroup"].ToString();
        //        r.finYear = p.finYear;
        //        r.userCompany = "0";
        //        if (r.result == "OK" || r.result== "LOGGEDIN")
        //        {
        //            r.token = tk.GetToken(r);
        //        }
        //        else
        //        {
        //            r.token = "";
        //        }
        //    }
        //    else
        //    {
        //        r.result = "FAILED";
        //        r.retval = "0";
        //        r.msg = "Incorrect Username and Password !";
        //        r.token = "";
        //        r.userCode = "0";
        //        r.userRole = "0";
        //        r.userCompany = "0";
        //    }


        //    return r;
        //}


        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public LoginResultModel login(LoginParam p)
        {
            LoginResultModel r = new LoginResultModel();
            LoginAuthentication lg = new LoginAuthentication(_cl, _configuration);
            r = lg.login(p);

            TokenGeneration tk = new TokenGeneration(_cl, _configuration);
            r.token = "";

            _cl.pwdEnc(p.username, p.password, p.finYear);
            string passWord = _cl.rj_encrypt(p.password);

            DataTable dt = _cl.Rj_LoadTableWithProc("sp_bookingLoginValidation", new SqlParameter[]
            {
        new SqlParameter("@username", p.username),
        new SqlParameter("@password", passWord),
        new SqlParameter("@ipAddress", p.ipAddress)
        //new SqlParameter("@opGroup", p.opGroup)
            }, p.finYear);

            if (dt.Rows.Count > 0 && dt.Columns.Contains("type") && dt.Rows[0]["type"].ToString() == "OK")
            {
                DataRow row = dt.Rows[0];
                r.result = "OK";
                r.retval = dt.Rows[0]["sno"].ToString();
                r.msg = dt.Rows[0]["msg"].ToString();
                r.userCode = dt.Rows[0]["sno"].ToString();
                r.userRole = dt.Rows[0]["opGroup"].ToString();
                r.finYear = p.finYear;
                r.userCompany = dt.Columns.Contains("companyId") ? dt.Rows[0]["companyId"].ToString() : "0";

                r.token = tk.GetToken(r);

                r.buyerCode = row["buyerCode"].ToString();
                r.pwdChange = row["changePwd"].ToString();
                r.username = row["username"].ToString();
                r.name = row["name"].ToString();
                r.phone = row["phone"].ToString();
                r.image = row["image"].ToString();
                r.companyId = row["companyId"].ToString();
                r.companyname = row["companyname"].ToString();
                r.companyImg = row["companyImg"].ToString();
                r.companyAddress = row["companyAddress"].ToString();
                r.companyPhone = row["companyPhone"].ToString();
                r.companyEmail = row["companyEmail"].ToString();
                r.companyGst = row["companyGst"].ToString();
                r.status = row["status"].ToString();
                r.sdate = row["sdate"].ToString();
            }
            else
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = dt.Rows[0]["msg"].ToString();
                r.token = "";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }

            return r;
        }


        [Route("financial-year-list")]
        [HttpGet]
        [AllowAnonymous]
        public List<GetFinYearWithConnection_model> finYearList() {
            List<GetFinYearWithConnection_model> lst= new List<GetFinYearWithConnection_model>();
            List<GetFinYearWithConnection_model> rlst = new List<GetFinYearWithConnection_model>();
            JsonToDataTableService jsn = new JsonToDataTableService();
            lst = jsn.GetFinYearWithConnection();
            foreach (var item in lst)
            {
                rlst.Add(new GetFinYearWithConnection_model
                {
                    sno = item.sno,
                    FinYear = item.FinYear,
                    Server = null,
                    Database = null,
                    User_Id = null,
                    Password = null,
                    Default = item.Default

                });
            }

            return rlst;
        }


        [Route("financial-year")]
        [HttpGet]
        [AllowAnonymous]
        public List<GetFinYearWithConnection_model> finYearListDecripted()
        {
            List<GetFinYearWithConnection_model> lst = new List<GetFinYearWithConnection_model>();
            List<GetFinYearWithConnection_model> rlst = new List<GetFinYearWithConnection_model>();
            JsonToDataTableService jsn = new JsonToDataTableService();
            lst = jsn.GetFinYearWithConnection();

            foreach (var item in lst)
            {
                rlst.Add(new GetFinYearWithConnection_model
                {
                    sno = _cl.rj_decrypt(item.sno),
                    FinYear = _cl.rj_decrypt(item.FinYear),
                    Server = null,
                    Database = null,
                    User_Id = null,
                    Password = null,
                    Default = _cl.rj_decrypt(item.Default)
                });
            }


            return rlst;
        }



    }


}
