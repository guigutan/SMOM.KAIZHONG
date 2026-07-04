using SIE.EventMessages.MES.Dispatchs;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Andons
{
    [Service(FallbackType = typeof(DefaultIAndon))]
    public interface IAndon
    {
        /// <summary>
        /// 删除安灯责任组维护基础表明细用户
        /// </summary>
        /// <param name="userId"></param>
        void DeleteAndonGroupDetailUser(double userId);
    }

    public class DefaultIAndon : IAndon
    {
        public void DeleteAndonGroupDetailUser(double userId)
        {
        }
    }
}
