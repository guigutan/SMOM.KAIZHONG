using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards
{
    /// <summary>
    /// 检验标准基类
    /// </summary>
    public class InspStandardDataBase : ErpInfoData
    {
        /// <summary>
        /// 不对外公开构造函数
        /// </summary>
        protected InspStandardDataBase() { }
        /// <summary>
        /// 检验类型
        /// </summary>
        public string InspectionType { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 明细集合
        /// </summary>
        public List<InspStandardDataDetailBase> DetailList { get; set; }
    }
}
