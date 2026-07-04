using SIE.EventMessages.LES.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 工单挪料接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesMaterialMove))]
    public interface ILesMaterialMove
    {
        /// <summary>
        /// 上料挪料
        /// </summary>
        /// <param name="data">挪料数据</param>
        void LoadItemMove(LoadItemMoveData data);
    }

    /// <summary>
    /// 挪料接口默认实现
    /// </summary>
    public class DefaultILesMaterialMove : ILesMaterialMove
    {
        /// <summary>
        /// 上料挪料
        /// </summary>
        /// <param name="data">挪料数据</param>
        public void LoadItemMove(LoadItemMoveData data)
        {

        }
    }
}
