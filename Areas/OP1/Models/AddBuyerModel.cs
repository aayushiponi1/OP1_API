namespace OP1_API.Areas.OP1.Models
{
    public class AddBuyerModel
    {
        public string? Code { get; set; }
        //public string? Tp { get; set; }
        public string? Client { get; set; }
        public string? Pricingtype { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? EmailId { get; set; }
        public string? Gstin { get; set; }
        public string? TargetAmt { get; set; }
        public string? BillingAddress { get; set; }
        public string? Billingstste { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingPin { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public List<AddressDetails>? AddressDetails { get; set; }
    }

    public class AddressDetails
    {
        public string? sno { get; set; }
        public string? buyerCode { get; set; }
        public string? deliverytype { get; set; }
        public string? deliverytypeName { get; set; }
        public string? deliverygstin { get; set; }
        public string? deliverypersonname { get; set; }
        public string? deliverymobile { get; set; }
        public string? deliveryaddress { get; set; }
        public string? deliverystatename { get; set; }
        public string? deliverystatecode { get; set; }
        public string? deliverycity { get; set; }
        public string? deliverypincode { get; set; }
        public string? codeid { get; set; }
    }

    public class GetBuyerParam 
    { 
        public string? code { get; set; }
    }

}
