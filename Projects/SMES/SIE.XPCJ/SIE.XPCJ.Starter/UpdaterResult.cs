namespace SIE.XPCJ.Starter
{
    public class UpdaterResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }


        public T Result { get; set; }
    }

    public class UpdaterResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }


        public object Result { get; set; }
    }
}
