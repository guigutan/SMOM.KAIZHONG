namespace SIE.CrossPlatform.Collect.Common.ApiCall
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ApiContext Context { get; set; }

        public T Result { get; set; }
    }

    public class ApiResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ApiContext Context { get; set; }

        public object Result { get; set; }
    }
}
