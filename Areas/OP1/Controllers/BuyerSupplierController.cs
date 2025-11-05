using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OP1_API.Areas.Login.Models;
using OP1_API.Areas.OP1.Models;
using OP1_API.ClassCollection;
using OP1_API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace OP1_API.Areas.OP1.Controllers
{
    [Route("api/op1/")]
    [ApiController]
    [Authorize]
    public class BuyerSupplierController : ControllerBase
    {
        private readonly GlobalClass cl;
        DocumentUpload du;
        public BuyerSupplierController(DocumentUpload _du)
        {
            cl = new GlobalClass();
            du = _du;
        }


        [Route("USER-DETAILS")]
        [HttpPost]
        [AllowAnonymous]
        public loginDetailModel GetUserDetails(userProfileparam p)
        {
            loginDetailModel result = new loginDetailModel();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("sp_ProfileDetails", new SqlParameter[]
                {
                    new SqlParameter("@sno", _userDetails.userCode),
                    new SqlParameter("@opGroup", _userDetails.userRole)
                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    result = new loginDetailModel
                    {
                        pwdChange = row["changePwd"].ToString(),
                        code = row["code"].ToString(),
                        username = row["username"].ToString(),
                        name = row["name"].ToString(),
                        phone = row["phone"].ToString(),
                        email = row["email"].ToString(),
                        image = row["image"].ToString(),
                        companyId = row["companyId"].ToString(),
                        companyname = row["companyname"].ToString(),
                        companyImg = row["companyImg"].ToString(),
                        companyAddress = row["companyAddress"].ToString(),
                        companyPhone = row["companyPhone"].ToString(),
                        companyEmail = row["companyEmail"].ToString(),
                        companyGst = row["companyGst"].ToString(),
                        status = row["status"].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }



        [Route("Get-All-BuyerSupplierName/{search?}")]
        [HttpGet]
        //[Authorize]
        [AllowAnonymous]
        public List<BuyerSupplierModel> GetBuyerSupplierName(string search)
        {
            List<BuyerSupplierModel> r = new List<BuyerSupplierModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetBuyerSupplierName", new SqlParameter[]
                {
                    new SqlParameter("@search", search),
                    new SqlParameter("@client", _userDetails.userCompany)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new BuyerSupplierModel
                    {
                        code = row["code"].ToString(),
                        Name = row["Name"].ToString(),
                        phone = row["phone"].ToString()

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }


        [Route("GET_BUYER_ADDRESS")]
        [HttpPost]
        [AllowAnonymous]
        public List<BuyerAddressModel> GetBuyerAddress(addressParam param)
        {
            List<BuyerAddressModel> r = new List<BuyerAddressModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetBuyerSupplierAddress", new SqlParameter[]
                {
                    new SqlParameter("@QueryType", "Get_BuyerAddress_List"),
                    new SqlParameter("@code",param.Code)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new BuyerAddressModel
                    {
                        code = row["Code"].ToString(),
                        buyerCode = row["buyerCode"].ToString(),
                        //buyerCode = row["BuyerCodeParam"]?.ToString() ?? row["buyerCode"]?.ToString(),
                        addressType = row["addressType"].ToString(),
                        Address = row["Address"].ToString(),
                        nearBy = row["nearBy"].ToString(),
                        stateID = row["stateID"].ToString(),
                        Statename = row["Statename"].ToString(),
                        cityname = row["cityname"].ToString(),
                        pinCode = row["pinCode"].ToString(),
                        setAsDefault = row["setAsDefault"].ToString(),

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }


        [Route("GET_PRODUCTCATEGORY_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<ProductCategoryModel> GetProductCategory(productparam param)
        {
            List<ProductCategoryModel> r = new List<ProductCategoryModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_ProductCategory_List"),
                    new SqlParameter("@client", _userDetails.userCompany)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new ProductCategoryModel
                    {
                        Code = row["Code"].ToString(),
                        CategoryName = row["CategoryName"].ToString(),
                        CategoryImage = row["CategoryImage"].ToString(),

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        [Route("GET_PRODUCTSUBCATEGORY_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<ProductSubCategoryModel> GetProductSubCategory(productSubCategoryParam param)
        {
            List<ProductSubCategoryModel> r = new List<ProductSubCategoryModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_ProductSubCategory_List"),
                    new SqlParameter("@Category", param.Category),
                    new SqlParameter("@client", _userDetails.userCompany)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new ProductSubCategoryModel
                    {
                        Code = row["Code"].ToString(),
                        SubCategoryName = row["SubCategoryName"].ToString(),
                        SubCategoryImage = row["SubCategoryImage"].ToString(),

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }


        [Route("GET_PRODUCTS_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<ProductListModel> GetProductList(ProductFilterParam param)
        {
            List<ProductListModel> r = new List<ProductListModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_Products_ListBy_Category_SubCategory"),
                    new SqlParameter("@buyerCode", param.Code),
                    new SqlParameter("@Category", param.Category),
                    new SqlParameter("@SubCategory", param.SubCategory),
                    new SqlParameter("@client", _userDetails.userCompany)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new ProductListModel
                    {
                        buyerCode = row["buyerCode"].ToString(),
                        ClientId = row["clientId"].ToString(),
                        PNO = row["pno"].ToString(),
                        SKUCode = row["skucode"].ToString(),
                        PName = row["pname"].ToString(),
                        MakeName = row["makename"].ToString(),
                        Category = row["category"].ToString(),
                        SubCategory = row["SubCategory"].ToString(),
                        Description = row["Descriptions"].ToString(),
                        DistributorRate = row["DistributorRate"].ToString(),
                        RetailerRate = row["RetailerRate"].ToString(),
                        ContractorRate = row["ContractorRate"].ToString(),
                        EndUserRate = row["EndUserRate"].ToString(),
                        Rate = row["rate"].ToString(),
                        MRP = row["mrp"].ToString(),
                        RecomendadeMRP = row["Recomendademrp"].ToString(),
                        RecomendadeMRPName = row["recomendedmrpName"].ToString(),
                        TaxAmt = row["TaxAmt"].ToString(),
                        RecomendadeRate = row["RecomendadeRate"].ToString(),
                        Img1 = row["Img1"].ToString(),
                        Img2 = row["Img2"].ToString(),
                        Img3 = row["Img3"].ToString(),
                        Img4 = row["Img4"].ToString(),
                        Img5 = row["Img5"].ToString(),
                        Img6 = row["Img6"].ToString(),
                        CartQty = row["cartQty"].ToString(),
                        CartAmt = row["crtAmt"].ToString(),
                        MasterPackageBox = row["MasterPackageBox"].ToString(),
                        CashbackAmount = row["CashbackAmount"].ToString(),
                        CashbackPlan = row["cashbackPlan"].ToString(),
                        MinSaleQty = row["MinSaleQty"].ToString(),
                        StockQty = row["stockQty"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }

        [Route("GET_PRODUCT_CART_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<ProductListModel> GetProductCartList(ProductFilterParam param)
        {
            List<ProductListModel> r = new List<ProductListModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_Product_Cart_List"),
                    new SqlParameter("@buyerCode", param.Code),
                    new SqlParameter("@Category", param.Category),
                    new SqlParameter("@SubCategory", param.SubCategory),
                    new SqlParameter("@client", _userDetails.userCompany),
                    new SqlParameter("@userCode", _userDetails.userCode),
                    new SqlParameter("@orderNo",param.OrderNo)
                }, _userDetails.finYear);

                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new ProductListModel
                    {
                        buyerCode = row["buyerCode"].ToString(),
                        ClientId = row["clientId"].ToString(),
                        PNO = row["pno"].ToString(),
                        SKUCode = row["skucode"].ToString(),
                        PName = row["pname"].ToString(),
                        MakeName = row["makename"].ToString(),
                        Category = row["category"].ToString(),
                        SubCategory = row["SubCategory"].ToString(),
                        Description = row["Descriptions"].ToString(),
                        DistributorRate = row["DistributorRate"].ToString(),
                        RetailerRate = row["RetailerRate"].ToString(),
                        ContractorRate = row["ContractorRate"].ToString(),
                        EndUserRate = row["EndUserRate"].ToString(),
                        Rate = row["rate"].ToString(),
                        MRP = row["mrp"].ToString(),
                        RecomendadeMRP = row["Recomendademrp"].ToString(),
                        RecomendadeMRPName = row["recomendedmrpName"].ToString(),
                        TaxAmt = row["TaxAmt"].ToString(),
                        RecomendadeRate = row["RecomendadeRate"].ToString(),
                        Img1 = row["Img1"].ToString(),
                        Img2 = row["Img2"].ToString(),
                        Img3 = row["Img3"].ToString(),
                        Img4 = row["Img4"].ToString(),
                        Img5 = row["Img5"].ToString(),
                        Img6 = row["Img6"].ToString(),
                        CartQty = row["cartQty"].ToString(),
                        CartAmt = row["crtAmt"].ToString(),
                        MasterPackageBox = row["MasterPackageBox"].ToString(),
                        CashbackAmount = row["CashbackAmount"].ToString(),
                        CashbackPlan = row["cashbackPlan"].ToString(),
                        MinSaleQty = row["MinSaleQty"].ToString(),
                        StockQty = row["stockQty"].ToString(),
                        //addressCode = row["addressCode"].ToString(),
                        addressCode = dt.Columns.Contains("addressCode") ? row["addressCode"].ToString() : null,
                        deliveryCharge = dt.Columns.Contains("deliveryCharge") ? row["deliveryCharge"].ToString() : "0",
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }


        [Route("GET_PRODUCT_CART_DETAIL")]
        [HttpPost]
        [AllowAnonymous]
        public CartDetailModel GetProductCartDetail(ProductFilterParam param)
        {
            CartDetailModel result = new CartDetailModel();

            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "ProductcartDetail"),
                    new SqlParameter("@buyerCode", param.Code),
                    new SqlParameter("@client", _userDetails.userCompany),
                    new SqlParameter("@userCode", _userDetails.userCode)
                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];

                    result.Qty = row["Qty"].ToString();
                    result.TaxAmt = row["taxAmt"].ToString();
                    result.SubTotal = row["SubTotal"].ToString();
                    result.CouponDiscountAmt = row["CouponDiscountAmt"].ToString();
                    result.ExtraDiscount = row["ExtraDiscount"].ToString();
                    result.GrossAmt = row["GrossAmt"].ToString();
                    //result.BuyerSupplierType = row["BuyerSupplierType"].ToString();
                    //result.DiscountPlan = row["discountPlan"].ToString();
                    //result.DiscountPlanName = row["discountPlanName"].ToString();
                    //result.DiscountPer = row["discountPer"].ToString();
                    //result.FromAmtRenge = row["fromAmtRenge"].ToString();
                    //result.ToAmtRenge = row["toAmtRenge"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        [Route("ADD_TO_CART_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult addtoCartlist([FromBody] addtoCartlistModel p)
        {
            ReturnResult r = new ReturnResult();
            DataTable dt = new DataTable();
            try
            {
                // Get logged-in user details from token
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                dt = cl.Rj_LoadTableWithProc("sp_AddProduct_OnlineCart", new SqlParameter[]
                {
                    new SqlParameter("@Client", _userDetails.userCompany),
                    new SqlParameter("@buyerCode", p.buyerCode),
                    new SqlParameter("@pno", p.pno),
                    new SqlParameter("@rate", p.rate),
                    new SqlParameter("@qty", p.qty),
                    new SqlParameter("@amt", p.amt),
                    new SqlParameter("@userCode", _userDetails.userCode),
                    new SqlParameter("@entryBy", p.entryBy),
                    new SqlParameter("@OrderNo", string.IsNullOrEmpty(p.orderNo) ? (object)DBNull.Value : (object)p.orderNo)

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
                    r.msg = "Could not add to Cart. Please contact Administrator!";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "Could not add to Cart. Please contact Administrator!";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }

            return r;
        }


        [Route("Place_Order_Online")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult placeOrderOnline(placeOrderOnlineParamModel p)
        {

            ReturnResult r = new ReturnResult();
            DataTable dt = new DataTable();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);


                dt = cl.Rj_LoadTableWithProc("sp_PlaceOnlineOrder_oo", new SqlParameter[]
                {
                     new SqlParameter("@Client", _userDetails.userCompany),
                     new SqlParameter("@buyerCode", p.buyerCode),
                     new SqlParameter("@Address", p.addressCode),
                     new SqlParameter("@entryBy", p.entryBy),
                     new SqlParameter("@deliveryCharge", string.IsNullOrEmpty(p.deliveryCharge) ? (object)DBNull.Value : (object)p.deliveryCharge),
                     new SqlParameter("@OrderNo", string.IsNullOrEmpty(p.orderNo) ? (object)DBNull.Value : (object)p.orderNo)
                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {
                    r.result = dt.Rows[0]["result"].ToString();
                    r.retval = dt.Rows[0]["retval"].ToString();
                    r.msg = dt.Rows[0]["msg"].ToString();
                    // ✅ Add your custom success message
                    if (r.result == "OK")
                    {
                        if (!string.IsNullOrEmpty(p.orderNo))
                            r.msg = $"Order updated successfully (Order No: {p.orderNo})";
                        else
                            r.msg = $"Order placed successfully (Order No: {r.msg})";
                    }
                    r.userCode = _userDetails.userCode;
                    r.userRole = _userDetails.userRole;
                    r.userCompany = _userDetails.userCompany;
                }
                else
                {

                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "FAILED";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";

                }


            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "FAILED";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }


        [Route("GET_ORDER_HISTORY_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<orderhestoryListModel> GetOrderHistory(orderhestoryparamModel param)
        {
            List<orderhestoryListModel> r = new List<orderhestoryListModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("SP_GetOrderList", new SqlParameter[]
                {
                    new SqlParameter("@buyerCode", param.buyerCode),
                    new SqlParameter("@client", _userDetails.userCompany),
                    new SqlParameter("@OrderNo", param.orderNo),
                    new SqlParameter("@FromDate", param.fromDate),
                    new SqlParameter("@ToDate", param.toDate),
                    new SqlParameter("@OrderApprovedStatus", param.orderApprovedStatus),
                    new SqlParameter("@PaymentStatus", param.paymentStatus)
                }, _userDetails.finYear);

                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new orderhestoryListModel
                    {
                        sno = row["sno"].ToString(),
                        orderNo = row["OrderNo"].ToString(),
                        orderSource = row["OrderSource"].ToString(),
                        orderDate = row["OrderDate"].ToString(),
                        orderTime = row["OrderTime"].ToString(),
                        totAmt = row["totAmt"].ToString(),
                        discountAmt = row["DiscountAmt"].ToString(),
                        taxAmt = row["TaxAmt"].ToString(),
                        orderAmt = row["OrderAmt"].ToString(),
                        deliveryAddressCode = row["DeliveryAddressCode"].ToString(),
                        addressType = row["addressType"].ToString(),
                        deliveryAddress = row["DeliveryAddress"].ToString(),
                        nearBy = row["nearBy"].ToString(),
                        stateId = row["stateId"].ToString(),
                        Statename = row["Statename"].ToString(),
                        cityname = row["cityname"].ToString(),
                        pinCode = row["pinCode"].ToString(),
                        orderApprovedStatus = row["OrderApprovedStatus"].ToString(),
                        paymentDate = row["PaymentDate"].ToString(),
                        paymentTime = row["PaymentTime"].ToString(),
                        paymentAmt = row["PaymentAmt"].ToString(),
                        paymentStatus = row["PaymentStatus"].ToString(),
                        paymentImg = row["PaymentImg"].ToString(),
                        remarks = row["Remarks"].ToString(),
                        buyerName = row["Name"].ToString(),
                        userId = row["UserId"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }



        [Route("GET_BUYERS_LIST")]
        [HttpPost]
        [AllowAnonymous]
        public List<BuyerNameModel> GetAllBuyers(productparam param)
        {
            List<BuyerNameModel> r = new List<BuyerNameModel>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetProductCategory", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_All_Buyers"),
                    new SqlParameter("@client", _userDetails.userCompany)
                }, _userDetails.finYear);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new BuyerNameModel
                    {
                        Code = row["Code"].ToString(),
                        Name = row["Name"].ToString(),

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return r;
        }


        [Route("GET_ORDER_INVOICE")]
        [HttpPost]
        [AllowAnonymous]
        public orderinvoiceListModel GetInvoice(orderinvoiceparamModel param)
        {
            orderinvoiceListModel r = new orderinvoiceListModel();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("sp_OP1InvoicePrintDetails", new SqlParameter[]
                {
                    new SqlParameter("@OrderNo", param.orderNo),

                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {

                    DataRow row = dt.Rows[0];

                    r.sno = row["sno"].ToString();
                    r.orderNo = row["OrderNo"].ToString();
                    r.orderSource = row["OrderSource"].ToString();
                    r.orderDate = row["OrderDate"].ToString();
                    r.orderTime = row["OrderTime"].ToString();
                    r.totAmt = row["totAmt"].ToString();
                    r.discountAmt = row["DiscountAmt"].ToString();
                    r.taxAmt = row["TaxAmt"].ToString();
                    r.orderAmt = row["OrderAmt"].ToString();
                    r.deliveryAddressCode = row["DeliveryAddressCode"].ToString();
                    r.addressType = row["addressType"].ToString();
                    r.deliveryAddress = row["DeliveryAddress"].ToString();
                    r.nearBy = row["nearBy"].ToString();
                    r.stateId = row["stateId"].ToString();
                    r.Statename = row["Statename"].ToString();
                    r.cityname = row["cityname"].ToString();
                    r.pinCode = row["pinCode"].ToString();
                    r.orderApprovedStatus = row["OrderApprovedStatus"].ToString();
                    r.paymentDate = row["PaymentDate"].ToString();
                    r.paymentTime = row["PaymentTime"].ToString();
                    r.paymentAmt = row["PaymentAmt"].ToString();
                    r.paymentStatus = row["PaymentStatus"].ToString();
                    r.paymentImg = row["PaymentImg"].ToString();
                    r.remarks = row["Remarks"].ToString();
                    r.buyerName = row["BuyerName"].ToString();
                    r.companyname = row["companyname"].ToString();
                    r.companyImg = row["companyImg"].ToString();
                    r.companyAddress = row["companyAddress"].ToString();
                    r.companyPhone = row["companyPhone"].ToString();
                    r.companyEmail = row["companyEmail"].ToString();
                    r.companyGst = row["companyGst"].ToString();
                    r.ShippingAddress = row["ShippingAddress"].ToString();
                    r.ShippingStateName = row["ShippingStateName"].ToString();
                    r.ShippingCity = row["ShippingCity"].ToString();
                    r.ShippingPincode = row["ShippingPincode"].ToString();
                    r.BuyerGst = row["BuyerGst"].ToString();
                    r.BuyerEmail = row["BuyerEmail"].ToString();
                    r.BuyerPhone = row["BuyerPhone"].ToString();
                    r.bankName = row["bankName"].ToString();
                    r.AccountNo = row["AccountNo"].ToString();
                    r.IFSCCode = row["IFSCCode"].ToString();
                    r.deliveryCharge = row["deliveryCharge"].ToString();

                    r.productList = new List<orderinvoiceProductsModel>();
                    foreach (DataRow detailRow in dt.Rows)
                    {
                        r.productList.Add(new orderinvoiceProductsModel
                        {
                            pno = detailRow["pno"].ToString(),
                            pno1 = detailRow["pno1"].ToString(),
                            skumakename = detailRow["skumakename"].ToString(),
                            category = detailRow["category"].ToString(),
                            subCategory = detailRow["subCategory"].ToString(),
                            pname = detailRow["pname"].ToString(),
                            qty = detailRow["qty"].ToString(),
                            NetAmt = detailRow["NetAmt"].ToString(),
                            hsn = detailRow["hsn"].ToString(),
                            saleTax = detailRow["saleTax"].ToString(),
                            unitName = detailRow["unitName"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }

        [Route("Cancel_Order_Online")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult cancelOrderOnline(CancelOrderOnlineParamModel p)
        {
            ReturnResult r = new ReturnResult();
            DataTable dt = new DataTable();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                // Call stored procedure to cancel the order
                dt = cl.Rj_LoadTableWithProc("sp_CancelOnlineOrder_oo", new SqlParameter[]
                {
                    new SqlParameter("@orderNo", p.orderNo),
                    new SqlParameter("@entryBy", p.entryBy)
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
                    r.msg = "FAILED";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "FAILED";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }


        [Route("GET_ORDER_PAYMENT")]
        [HttpPost]
        [AllowAnonymous]
        public OrderPaymentModel GetOrderPayment(orderinvoiceparamModel param)
        {
            OrderPaymentModel r = new OrderPaymentModel();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                DataTable dt = cl.Rj_LoadTableWithProc("sp_MakePaymentOP1", new SqlParameter[]
                {
                    new SqlParameter("@OrderNo", param.orderNo),

                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {

                    DataRow row = dt.Rows[0];

                    r.sno = row["sno"].ToString();
                    r.orderNo = row["OrderNo"].ToString();
                    r.orderAmt = row["orderAmt"].ToString();
                    r.totQty = row["totQty"].ToString();
                    r.deliveryCharge = row["deliveryCharge"].ToString();
                    r.TotalAmount = row["TotalAmount"].ToString();
                    r.bankName = row["bankName"].ToString();
                    r.AccountNo = row["AccountNo"].ToString();
                    r.IFSCCode = row["IFSCCode"].ToString();

                    r.paymentproductList = new List<orderpaymentProductsModel>();
                    foreach (DataRow detailRow in dt.Rows)
                    {
                        r.paymentproductList.Add(new orderpaymentProductsModel
                        {
                            pname = detailRow["pname"].ToString(),
                            qty = detailRow["Qty"].ToString(),
                            NetAmt = detailRow["NetAmt"].ToString(),
                            gst = detailRow["gst"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }


        [Route("Save-Payment-Documents")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult SubmitPaymentDocuments([FromForm] PaymentDocuments p)
        {
            ReturnResult r = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

            try
            {
                //string userid = _userDetails.userCode;

                string paymentDoc = p.paymentDoc ?? "";


                // --- CLAIM BILL FILE ---
                if (p.paymentFile != null && p.paymentFile.Length > 0)
                {
                    var rootPath = @"C:\Website\Bharat\Admin\OP1\PaymentDoc";

                    uploadDocumentsParam imgParam = new uploadDocumentsParam
                    {
                        file = p.paymentFile,
                        rootPath = rootPath
                    };
                    r = du.UploadpaymentDoc(imgParam);
                    if (r.result == "OK")
                        paymentDoc = r.retval;
                    else
                        return r;
                }



                DataTable dt = cl.Rj_LoadTableWithProc("sp_op1paymentImageUpload", new SqlParameter[]
                {
                     new SqlParameter("@orderNo", p.orderNo),
                     new SqlParameter("@filename", paymentDoc),
                }, _userDetails.finYear);

                if (dt.Rows.Count > 0)
                {
                    r.result = dt.Rows[0]["type"].ToString();
                    r.retval = dt.Rows[0]["des"].ToString();
                    r.msg = dt.Rows[0]["msg"].ToString();
                    r.userCode = _userDetails.userCode;
                    r.userRole = _userDetails.userRole;
                    r.userCompany = _userDetails.userCompany;
                }
                else
                {
                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "Something went wrong!";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = ex.Message;
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }

            return r;
        }

        [Route("Save_Order_Payment")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult SubmitPaymentOnline(PaymentAmout p)
        {
            ReturnResult r = new ReturnResult();
            DataTable dt = new DataTable();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                // Call stored procedure to cancel the order
                dt = cl.Rj_LoadTableWithProc("sp_submitop1payment", new SqlParameter[]
                {
                    new SqlParameter("@orderNo", p.orderNo),
                    new SqlParameter("@PaymentAmt", p.PaymentAmt)
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
                    r.msg = "FAILED";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "FAILED";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }

        [Route("Change_Password")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnResult ChangePassword(ChangePassword p)
        {
            ReturnResult r = new ReturnResult();
            DataTable dt = new DataTable();

            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);

                string encNewPassword = cl.rj_encrypt(p.password);
                string encOldPassword = cl.rj_encrypt(p.oldPassword);  // <-- encrypt old too!

                dt = cl.Rj_LoadTableWithProc("sp_changeOP1Password", new SqlParameter[]
                 {
                    new SqlParameter("@username", p.username),
                    new SqlParameter("@password", encNewPassword),
                    new SqlParameter("@oldPassword", encOldPassword),
                    new SqlParameter("@plainPassword", p.password)
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
                    r.msg = "FAILED";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.msg = ex.Message;
            }
            return r;
        }


        [Route("GET_ABOUT_COMPANY")]
        [HttpPost]
        [AllowAnonymous]
        public List<AboutCompany> GetAboutCompany(productparam param)
        {
            List<AboutCompany> r = new List<AboutCompany>();
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                LoginResultModel? _userDetails = cl.GetLiveUserDetails(authorizationHeader);
                DataTable dt = cl.Rj_LoadTableWithProc("sp_GetAboutCompany", new SqlParameter[]
                {
                    new SqlParameter("@QUERYTYPE", "Get_About_Company"),
                    new SqlParameter("@company", _userDetails.userCompany)
                }, _userDetails.finYear);
                Regex htmlTagRegex = new Regex("<.*?>", RegexOptions.Compiled);
                foreach (DataRow row in dt.Rows)
                {
                    r.Add(new AboutCompany
                    {
                        aBoutbusiness = htmlTagRegex.Replace(row["aBoutbusiness"].ToString() ?? "", "").Trim(),
                        oUrmission = htmlTagRegex.Replace(row["oUrmission"].ToString() ?? "", "").Trim(),
                        oUrvision = htmlTagRegex.Replace(row["oUrvision"].ToString() ?? "", "").Trim(),
                        wHychooseus = htmlTagRegex.Replace(row["wHychooseus"].ToString() ?? "", "").Trim(),
                        aBoutcontact = htmlTagRegex.Replace(row["aBoutcontact"].ToString() ?? "", "").Trim(),
                        aBouttermscond = htmlTagRegex.Replace(row["aBouttermscond"].ToString() ?? "", "").Trim(),
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
