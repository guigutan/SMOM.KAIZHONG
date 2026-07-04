using log4net.Core;
using System.Reflection;

namespace SIE.XPCJ.Common.Log
{
    public class LogFactory
    {
        public static ILoging GetLogger<T>()
        {

            var r = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), typeof(T));

            //todo 放缓存里
            return new Loging(r);
        }

    }
}
