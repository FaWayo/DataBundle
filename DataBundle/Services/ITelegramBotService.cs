using DataBundle.Models;

namespace DataBundle.BundleService
{
    public interface ITelegramBotService
    {
        Task<bool> CheckPin(CustomerInfo customerInfo);
        Task<List<BundlePackagesModel>> GetBundles();
        Task<string?> BundlePurchaseTran(PurchaseTranModel purchaseTranModel);
        Task<string> getToken(User user);
        Task<bool> CustomerExists(CustomerInfo customerInfo);
        Task<bool> UsernameExists(CustomerInfo customerInfo);
        Task<string> AddCustomer(CustomerInfo customer);
    }
}