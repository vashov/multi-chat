namespace ShareCode.Client.Services
{
    public class ServiceResult<T>
    {
        public T Result { get; set; }

        public string ErrorMsg { get; set; }

        public bool IsOk => string.IsNullOrEmpty(ErrorMsg);

        private ServiceResult() { }

        public static ServiceResult<T> Ok(T result)
        {
            return new ServiceResult<T>
            {
                Result = result
            };
        }

        public static ServiceResult<T> Error(string errorMsg)
        {
            return new ServiceResult<T>
            {
                Result = default,
                ErrorMsg = errorMsg,
            };
        }
    }
}
