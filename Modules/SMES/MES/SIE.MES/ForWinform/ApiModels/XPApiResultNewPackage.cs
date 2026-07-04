using SIE.Inventory.Interfaces;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// XP端新包装采集API方法返回值
    /// </summary>
    [Serializable]
    public class XPApiResultPackageInfo
    {

        public XPApiResultPackageInfo()
        {
            PackageRules = new List<XPWorkOrderPackageRuleDetail>();
            AdvancePackingUnits = new List<XPPackingUnit>();
            PackageSnRecords = new List<XPPackageSnRecord>();
            PrintRelations = new List<XPPackingRelation>();
            AllRelations = new List<XPPackingRelation>();
            AdvanceBarcodeQueue = new Queue<string>();
        }
        /// <summary>
        /// 是否切换了工单
        /// </summary>
        public bool IsChangeOrder { get; set; }

        /// <summary>
        /// 打印方式
        /// </summary>
        public Tech.Stations.Configs.PrintMode PrintMode { get; set; }

        /// <summary>
        /// 工单信息
        /// </summary>
        public ApiModels.WorkOrderInfo WorkOrder { get; set; }

        /// <summary>
        /// 采集的条码
        /// </summary>
        public CollectBarcode CollectBarcode { get; set; }

        /// <summary>
        /// 采集步骤
        /// </summary>
        public XPReworkStep Step { get; set; }

        /// <summary>
        /// 包装规则
        /// </summary>
        public List<XPWorkOrderPackageRuleDetail> PackageRules { get; set; }

        /// <summary>
        /// 待扫描包装号对应单位
        /// </summary>
        public List<XPPackingUnit> AdvancePackingUnits { get; set; }

        /// <summary>
        /// 条码明细
        /// </summary>
        public List<XPPackageSnRecord> PackageSnRecords { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Sns { get; set; }

        /// <summary>
        /// 手动打包号
        /// </summary>
        public string MuanualPackageNo { get; set; }

        /// <summary>
        /// 需要打印的包装关系
        /// </summary>
        public List<XPPackingRelation> PrintRelations { get; set; }

        /// <summary>
        /// 全部的包装关系
        /// </summary>
        public List<XPPackingRelation> AllRelations { get; set; }

        /// <summary>
        /// 待扫描包装号
        /// </summary>
        public Queue<string> AdvanceBarcodeQueue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 当前条码
        /// </summary>
        public string CurrentBarcode { get; set; }

        /// <summary>
        /// 当前包装关系
        /// </summary>
        public PackingRelation CurrentPackingRelation { get; set; }
}

    
}
