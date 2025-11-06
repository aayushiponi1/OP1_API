using OP1_API.Models;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace OP1_API.Areas.OP1.Models
{
    public class BuyerSupplierModel
    {
        public string code { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
    }

    public class addressParam
    {
        public string? Code { get; set; }
    }

    public class BuyerAddressModel
    {
        public string? code { get; set; }
        public string? buyerCode { get; set; }
        public string? addressType { get; set; }
        public string? Address { get; set; }
        public string? nearBy { get; set; }
        public string? stateID { get; set; }
        public string? Statename { get; set; }
        public string? cityname { get; set; }
        public string? pinCode { get; set; }
        public string? setAsDefault { get; set; }
    }
    public class productparam
    {
        public string? userCompany { get; set; }

    }

    public class ProductCategoryModel
    {
        public string? Code { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryImage { get; set; }
    }
    public class productSubCategoryParam
    {
        public string? Category { get; set; }
        public string? Company { get; set; }
    }
    public class ProductSubCategoryModel
    {
        public string? Code { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SubCategoryImage { get; set; }
    }

    public class ProductFilterParam
    {
        public string? Code { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; } = "0";
        public string? OrderNo { get; set; }
    }

    public class ProductListModel
    {
        public string? buyerCode { get; set; }
        public string? ClientId { get; set; }
        public string? PNO { get; set; }
        public string? SKUCode { get; set; }
        public string? PName { get; set; }
        public string? MakeName { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? Description { get; set; }
        public string? DistributorRate { get; set; }
        public string? RetailerRate { get; set; }
        public string? ContractorRate { get; set; }
        public string? EndUserRate { get; set; }
        public string? Rate { get; set; }
        public string? MRP { get; set; }
        public string? RecomendadeMRP { get; set; }
        public string? RecomendadeMRPName { get; set; }
        public string? TaxAmt { get; set; }
        public string? RecomendadeRate { get; set; }
        public string? Img1 { get; set; }
        public string? Img2 { get; set; }
        public string? Img3 { get; set; }
        public string? Img4 { get; set; }
        public string? Img5 { get; set; }
        public string? Img6 { get; set; }
        public string? CartQty { get; set; }
        public string? CartAmt { get; set; }
        public string? MasterPackageBox { get; set; }
        public string? CashbackAmount { get; set; }
        public string? CashbackPlan { get; set; }
        public string? MinSaleQty { get; set; }
        public string? StockQty { get; set; }
        public string? addressCode { get; set; }
        public string? deliveryCharge { get; set; }
    }

    public class CartDetailModel
    {
        public string? Qty { get; set; }
        public string? TaxAmt { get; set; }
        public string? SubTotal { get; set; }
        public string? CouponDiscountAmt { get; set; }
        public string? ExtraDiscount { get; set; }
        public string? GrossAmt { get; set; }
        //public string? BuyerSupplierType { get; set; }
        //public string? DiscountPlan { get; set; }
        //public string? DiscountPlanName { get; set; }
        //public string? DiscountPer { get; set; }
        //public string? FromAmtRenge { get; set; }
        //public string? ToAmtRenge { get; set; }
    }


    public class addtoCartlistModel
    {
        public string? buyerCode { get; set; }
        public string? pno { get; set; }
        public string? rate { get; set; }
        public string? qty { get; set; }
        public string? amt { get; set; }
        public string? entryBy { get; set; }
        public string? orderNo { get; set; }
    }

    public class placeOrderOnlineParamModel
    {
        public string? client { get; set; }
        public string? buyerCode { get; set; }
        public string? addressCode { get; set; }
        public string? entryBy { get; set; }
        public string? orderNo { get; set; }
        public string? deliveryCharge { get; set; }

    }

    public class orderhestoryparamModel
    {
        public string? client { get; set; }
        public string? buyerCode { get; set; }
        public long? orderNo { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public int? orderApprovedStatus { get; set; }
        public int? paymentStatus { get; set; }
    }

    public class orderhestoryListModel
    {
        public string? sno { get; set; }
        public string? orderNo { get; set; }
        public string? orderSource { get; set; }
        public string? orderDate { get; set; }
        public string? orderTime { get; set; }
        public string? totAmt { get; set; }
        public string? discountAmt { get; set; }
        public string? taxAmt { get; set; }
        public string? orderAmt { get; set; }
        public string? deliveryAddressCode { get; set; }
        public string? addressType { get; set; }
        public string? deliveryAddress { get; set; }
        public string? nearBy { get; set; }
        public string? stateId { get; set; }
        public string? Statename { get; set; }
        public string? cityname { get; set; }
        public string? pinCode { get; set; }
        public string? orderApprovedStatus { get; set; }
        public string? paymentDate { get; set; }
        public string? paymentTime { get; set; }
        public string? paymentAmt { get; set; }
        public string? paymentStatus { get; set; }
        public string? paymentImg { get; set; }
        public string? remarks { get; set; }
        public string? buyerName { get; set; }
        public string? userId { get; set; }
    }

    public class BuyerNameModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    public class orderinvoiceparamModel
    {
        public string? orderNo { get; set; }

    }

    public class orderinvoiceListModel
    {
        public string? sno { get; set; }
        public string? orderNo { get; set; }
        public string? orderSource { get; set; }
        public string? orderDate { get; set; }
        public string? orderTime { get; set; }
        public string? totAmt { get; set; }
        public string? discountAmt { get; set; }
        public string? taxAmt { get; set; }
        public string? orderAmt { get; set; }
        public string? deliveryAddressCode { get; set; }
        public string? addressType { get; set; }
        public string? deliveryAddress { get; set; }
        public string? nearBy { get; set; }
        public string? stateId { get; set; }
        public string? Statename { get; set; }
        public string? cityname { get; set; }
        public string? pinCode { get; set; }
        public string? orderApprovedStatus { get; set; }
        public string? paymentDate { get; set; }
        public string? paymentTime { get; set; }
        public string? paymentAmt { get; set; }
        public string? paymentStatus { get; set; }
        public string? paymentImg { get; set; }
        public string? remarks { get; set; }
        public string? buyerName { get; set; }
        public string? companyname { get; set; }
        public string? companyImg { get; set; }
        public string? companyAddress { get; set; }
        public string? companyPhone { get; set; }
        public string? companyEmail { get; set; }
        public string? companyGst { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingStateName { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingPincode { get; set; }
        public string? BuyerGst { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }
        public string? bankName { get; set; }
        public string? AccountNo { get; set; }
        public string? IFSCCode { get; set; }
        public string? deliveryCharge { get; set; }

        public List<orderinvoiceProductsModel>? productList { get; set; }
    }

    public class orderinvoiceProductsModel
    {
        public string? pno { get; set; }
        public string? pno1 { get; set; }
        public string? skumakename { get; set; }
        public string? category { get; set; }
        public string? subCategory { get; set; }
        public string? pname { get; set; }
        public string? qty { get; set; }
        public string? NetAmt { get; set; }

        public string? hsn { get; set; }
        public string? saleTax { get; set; }
        public string? unitName { get; set; }
    }

    public class CancelOrderOnlineParamModel
    {
        public string? orderNo { get; set; }
        public string? entryBy { get; set; }
    }

    public class OrderPaymentModel
    {
        public string? sno { get; set; }
        public string? orderNo { get; set; }
        public string? orderAmt { get; set; }
        public string? totQty { get; set; }
        public string? deliveryCharge { get; set; }
        public string? TotalAmount { get; set; }
        public string? bankName { get; set; }
        public string? AccountNo { get; set; }
        public string? IFSCCode { get; set; }
        public List<orderpaymentProductsModel>? paymentproductList { get; set; }
    }
    public class orderpaymentProductsModel
    {
        public string? pname { get; set; }
        public string? qty { get; set; }
        public string? NetAmt { get; set; }
        public string? gst { get; set; }
    }


    public class PaymentDocuments
    {
        public string? orderNo { get; set; }
        public string? paymentDoc { get; set; }
        public IFormFile? paymentFile { get; set; }
    }
    public class PaymentAmout
    {
        public string? PaymentAmt { get; set; }
        public string? orderNo { get; set; }
    }

    public class ChangePassword
    {
        public string? password { get; set; }
        public string? username { get; set; }
        public string? oldPassword { get; set; }
    }

    public class AboutCompany
    {
        public string? aBoutbusiness { get; set; }
        public string? oUrmission { get; set; }
        public string? oUrvision { get; set; }
        public string? wHychooseus { get; set; }
        public string? aBoutcontact { get; set; }
        public string? aBouttermscond { get; set; }
    }

    public class GetBuyerSupplierListParamModel
    {
        public int client {  get; set; }
        public int bilingtype {  get; set; }
        public string? search {  get; set; }
    }

    public class GetBuyerSupplierListModel
    {
        public string? code {  get; set; }
        public string? companyname { get; set; }
        public string? buyercompanyname { get; set; }
        public string? pricingtype { get; set; }
        public string? contactpersonname { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? gstin { get; set; }
        public string? address { get; set; }
        public string? state { get; set; }
        public string? city { get; set; }
        public string? pincode { get; set; }
        public string? targetamt { get; set; }

    }

    public class GetBuyerMultipleAddressParamModel
    {
        public int code { get; set; }
    }

    public class GetBuyerMultipleAdressModel
    {
        public string? sno { get; set; }
        public string? addressType { get; set; }
        public string? gstin { get; set; }
        public string? personname { get; set; }
        public string? personmobile { get; set; }
        public string? Address { get; set; }
        public string? statename { get; set; }
        public string? cityname { get; set; }
        public string? pinCode { get; set; }
        public string? Active { get; set; }
        public string? Code { get; set; }
    }

}
