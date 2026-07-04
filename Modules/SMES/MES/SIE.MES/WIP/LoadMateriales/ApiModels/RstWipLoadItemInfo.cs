using SIE.MES.WIP.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.LoadMateriales.ApiModels
{
    /// <summary>
    /// 上料采集结果
    /// </summary>
    [Serializable]
    public class RstWipLoadItemInfo: RstWipInfo
    {
        public RstWipLoadItemInfo()
        {
            this.LoadItemInfos = new List<LoadItemInfo>();
        }
        /// <summary>
        /// 上料列表
        /// </summary>
        public List<LoadItemInfo> LoadItemInfos { get; set; }
    }
}
