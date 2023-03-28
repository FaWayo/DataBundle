using MojoAuth.NET;
using Newtonsoft.Json;

namespace DataBundle
{
    public class OTPManager : IOTPManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OTPManager> _logger;

        public OTPManager(ILogger<OTPManager> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<object> SendOTP(string phoneNumber)
        {
            try
            {
                var mojoAuthHttpClient = new MojoAuthHttpClient("test-2b628247-8999-49cc-82b5-661270138e6c", "cgct7obau9ls70gmarkg.fZUDneRhvtQ5W4S9CYJqxd");
                var response = await mojoAuthHttpClient.SendPhoneOTP(phoneNumber);

                var result = response.Result;

                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    /*    public bool VerifyOTP(string OTP)
        {
            if (OTP == null)
            {
                return false;
            }
            if (OTP == OtpCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
}
