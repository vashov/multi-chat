namespace MultiChat.Shared
{
    public class OperationResult<T>
    {
        public T Result { get; set; }

        public string ErrorMsg { get; set; }

        public bool IsOk => string.IsNullOrEmpty(ErrorMsg);

        public static OperationResult<T> Ok(T result)
        {
            return new OperationResult<T>
            {
                Result = result
            };
        }

        public static OperationResult<T> Error(string errorMsg)
        {
            return new OperationResult<T>
            {
                Result = default,
                ErrorMsg = errorMsg,
            };
        }
    }
}
