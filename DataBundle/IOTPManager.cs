namespace DataBundle
{
    public interface IOTPManager
    {

        //bool VerifyOTP(string OTP);
        Task<object> SendOTP(string phoneNumber);
    }
}