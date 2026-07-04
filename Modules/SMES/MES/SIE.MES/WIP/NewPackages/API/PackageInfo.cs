using SIE.MES.LoadItems;
using SIE.Packages.Packages;
using System.Collections.Generic;

namespace SIE.MES.WIP.NewPackages.API
{
    /// <summary>
    /// 包装采集app信息
    /// </summary>
    public class PackageInfo
    {
        /// <summary>
        /// 在制工单信息
        /// </summary>
        public WorkOrderInfo WorkOrderInfo { get; set; }

        /// <summary>
        /// 包装记录
        /// </summary>
        public List<PackageSn> DirectPackageSnRecordList { get; set; }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public List<PackageRuleInfo> WorkOrderPackageRuleDetailList { get; set; }
    }

    /// <summary>
    /// 提前打印预包装
    /// </summary>
    public class AdvanceInfo
    {
        /// <summary>
        /// 提前打印是否需要输入包装号
        /// </summary>
        public bool IsNeedPackageNo { get; set; }

        /// <summary>
        /// 预输入包装号队列
        /// </summary>
        public Queue<PackingUnit> PackageUnitList { get; set; } = new Queue<PackingUnit>();
    }

    /// <summary>
    /// 条码扫描信息
    /// </summary>
    public class PackageScanInfo
    {
        /// <summary>
        /// 新在制工单信息
        /// </summary>
        public WorkOrderInfo WorkOrderInfo { get; set; }

        /// <summary>
        /// 扫描后的包装记录
        /// </summary>
        public List<PackageSn> DirectPackageSnRecordList { get; set; }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public List<PackageRuleInfo> WorkOrderPackageRuleDetailList { get; set; }

        /// <summary>
        /// 扫描信息
        /// </summary>
        public string ResultMessage { get; set; }
    }

    /// <summary>
    /// 手动打包信息
    /// </summary>
    public class PackageHand
    {
        /// <summary>
        /// 手动包装生成的包装号
        /// </summary>
        public string PkgNo { get; set; }

        /// <summary>
        /// 包装记录
        /// </summary>
        public List<PackageSn> DirectPackageSnRecordList { get; set; }
    }

    /// <summary>
    /// 在制工单信息
    /// </summary>
    public class WorkOrderInfo
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }


        /// <summary>
        /// 当前条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 当班采集数
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 包装记录
    /// </summary>
    public class PackageSn
    {
        /// <summary>
        /// 数据id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 包装单位id
        /// </summary>
        public double UnitId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工单条码号
        /// </summary>
        public string ItemLabelList { get; set; }
    }

    /// <summary>
    /// 包装规则
    /// </summary>
    public class PackageRuleInfo
    {
        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnit { get; set; }

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty { get; set; }

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 编码规则
        /// </summary>
        public string NumRule { get; set; }
    }

    /// <summary>
    /// 条码是否手动打包
    /// </summary>
    public class IsPackInfo
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public IsPackInfo()
        {
            CanPackage = true;
            IsFull = false;
            Message = string.Empty;
        }

        /// <summary>
        /// 是否能打包
        /// </summary>
        public bool CanPackage { get; set; }

        /// <summary>
        /// 是否是满层包装
        /// </summary>
        public bool IsFull { get; set; }

        /// <summary>
        /// 标签信息
        /// </summary>
        public string Message { get; set; }
    }
}
