using SIE.EventMessages.LES.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 外部调用物料接收接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesMaterialReceive))]
    public interface ILesMaterialReceive
    {
        /// <summary>
        /// 创建物料接收数据
        /// </summary>
        /// <param name="datas"></param>
        void CreateMaterialReceives(List<ShippingOrderData> datas);
        /// <summary>
        /// LES物料自动接收
        /// </summary>
        /// <param name="datas">物料接收数据</param>
        void MaterialAutoReceive(List<MaterialReceiveData> datas);
    }

    /// <summary>
    /// 外部调用物料接收 默认接口
    /// </summary>
    class DefaultILesMaterialReceive : ILesMaterialReceive
    {

        /// <summary>
        /// 创建物料接收数据
        /// </summary>
        /// <param name="datas"></param>
        public void CreateMaterialReceives(List<ShippingOrderData> datas)
        {

        }
        /// <summary>
        /// LES物料自动接收
        /// </summary>
        /// <param name="datas">物料接收数据</param>
        public void MaterialAutoReceive(List<MaterialReceiveData> datas)
        {

        }
    }
}
