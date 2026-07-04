using Newtonsoft.Json;

namespace SIE.XPCJ.Common.ApiCall
{
    internal class ApiParameter
    {
        public object Value { get; set; }

        public ApiParameter(object value)
        {
            this.Value = value;
        }

        public static string FromParam(object param)
        {
            ApiParameter p = new ApiParameter(param);
            return JsonConvert.SerializeObject(p);
        }
    }
}
