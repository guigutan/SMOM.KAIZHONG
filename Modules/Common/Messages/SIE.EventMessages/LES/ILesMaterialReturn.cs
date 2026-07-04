using SIE.EventMessages.LES.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 退料申请接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesMaterialReturn))]
    public interface ILesMaterialReturn
    {
        /// <summary>
        /// 收货更新退料明细
        /// </summary>
        /// <param name="datas">收货数据</param>
        void UpDateReturnApply(List<ReturnUpdateData> datas);
    }

    /// <summary>
    /// 默认实现类
    /// </summary>
    public class DefaultILesMaterialReturn : ILesMaterialReturn
    {
        /// <summary>
        /// 收货更新退料明细
        /// </summary>
        /// <param name="datas">收货数据</param>
        public void UpDateReturnApply(List<ReturnUpdateData> datas)
        {
        }
    }
}
