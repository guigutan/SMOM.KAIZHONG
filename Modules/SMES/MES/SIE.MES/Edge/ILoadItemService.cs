using SIE.MES.Edge.Models;
using SIE.MES.LoadItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘上料服务接口
    /// </summary>
    public interface ILoadItemService
    {
        /// <summary>
        /// 获取条码物料信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<MaterialInfo> GetBarcodeInfo(EdgeCollectData data);


        /// <summary>
        /// 上料
        /// </summary>
        List<MaterialInfo> LoadItem(EdgeCollectData data);

    }
}
