using Dapper;
using DataBundle.DataAccess;
using DataBundle.Models;
using System.Data;


namespace DataBundle.BundleService
{
    public class TelegramBotService : ITelegramBotService
    {
        public readonly IDataAccess _dataAccess;
        public readonly IJWTBearerConfiguration _bearerConfig;

        public TelegramBotService(IDataAccess dataAccess, IJWTBearerConfiguration bearerConfig)
        {
            _dataAccess = dataAccess;
            _bearerConfig = bearerConfig;
        }

        public async Task<List<BundlePackagesModel>> GetBundles()
        {
            try
            {
                var response = await _dataAccess.LoadData<BundlePackagesModel, dynamic>(
               storedProcedure: "pces_BundlePackages_Sel",
               new { }
               );
                return response.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



         public async Task<string?> BundlePurchaseTran(PurchaseTranModel purchaseTranModel)
         {
            try
            {
                var parameters = new DynamicParameters();
        
                parameters.Add("transactionRef", dbType: DbType.String, direction: ParameterDirection.Output, size: 30);
                parameters.Add("telegramUserId", purchaseTranModel.telegramUserId);
                parameters.Add("receipientPhoneNumber", purchaseTranModel.receipientPhoneNumber);
                parameters.Add("bundleId", purchaseTranModel.bundleId);

                await _dataAccess.SaveData<dynamic>(storedProcedure: "pces_PurchaseTran_Ins", parameters: parameters);

                var transactionRef = parameters.Get<string?>("transactionRef");
                return transactionRef;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
         
         }

        public async Task<bool> CheckPin(CustomerInfo customerInfo)
        {
            try
            {
                var response = await _dataAccess.LoadData<bool, dynamic>(
                    storedProcedure: "pces_CheckPIN",
                    parameters: new { 
                        customerInfo.telegramUserId,
                        customerInfo.pinNumber
                    });
                return response.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> CustomerExists(CustomerInfo customerInfo)
        {
            try
            {
                var response = await _dataAccess.LoadData<bool, dynamic>(
                    storedProcedure: "pces_CheckCustomer", parameters: new
                    {
                        customerInfo.telegramUserId
                    });
                return response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> UsernameExists(CustomerInfo customerInfo)
        {
            try
            {
                var response = await _dataAccess.LoadData<bool, dynamic>(
                    storedProcedure: "pces_CheckUsername", parameters: new
                    {
                        customerInfo.userName
                    });
                return response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<string> getToken(User user)
        {
            try
            {
                var response = await _dataAccess.LoadData<bool, dynamic>(
                    storedProcedure: "pces_CheckUser",
                    parameters: new
                    {
                        user.userName,
                        user.userPassword
                    });
                bool isExists = response.FirstOrDefault();
                if (isExists)
                {
                   var token = _bearerConfig.GenerateToken(user);
                    return token;
                }
                else
                {
                    string noMatch = "No user match found";
                    return noMatch;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<string> AddCustomer(CustomerInfo customer)
        {
            try
            {
                var response = await _dataAccess.SaveDataWithRes(storedProcedure: "dbo.pces_Customer_Ins", parameters: new
                {
                    customer.firstName,
                    customer.lastName,
                    customer.userName,
                    customer.phoneNumber,
                    customer.telegramUserId,
                    customer.pinNumber,
                    customer.dateOfBirth,
                    customer.gender
                });
                 if(response == 1)
                {
                    return "Customer was added successfully";
                }
                else
                {
                    return "Customer was not added. Customer already exists";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

}
