namespace DataBundle.Models
{

        public class BundlePackagesModel
        {
            public int? bundleId { get; set; }
            public decimal? price { get; set; }
            public string? volume { get; set; }
        }

        public class PurchaseTranModel
        {
           public int? transactionRef { get; set; }
           public int? telegramUserId { get; set; }
           public int? bundleId { get; set; }
            public string? receipientPhoneNumber  { get; set;} 
           
        }

    public class CustomerInfo
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? phoneNumber { get; set; }
        public string? telegramUserId { get; set; }
        public string? pinNumber { get; set; }
        public string? userName { get; set; }
        public string? dateOfBirth { get; set; }
        public string? gender { get; set; }
    }

    public class User
    {
        public string? userName { get; set; } 
        public string? userPassword { get; set; }
    }

    public class OTPModel
    {
        public string? OTPCode { get; set;}
    }


}
