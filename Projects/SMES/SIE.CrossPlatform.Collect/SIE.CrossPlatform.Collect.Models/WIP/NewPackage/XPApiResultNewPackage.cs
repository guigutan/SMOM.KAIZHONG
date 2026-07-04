using SIE.CrossPlatform.Collect.Models.Enums;
using SIE.CrossPlatform.Collect.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// XP端新包装采集API方法返回值
    /// </summary>
    [Serializable]
    public class XPApiResultNewPackage
    {
        /// <summary>
        /// 是否切换了工单
        /// </summary>
        public bool IsChangeOrder { get; set; }

        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode { get; set; }

        /// <summary>
        /// 工单信息
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

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
        /// 当前条码
        /// </summary>

        public string CurrentBarcode { get; set; }
    }

    #region 树型Grid数据源 TreeGridModel
    /// <summary>
    /// 树型Grid数据源
    /// </summary>
    public class TreeGridModel
    {
        public static void GenTreeGridDataSource(List<TreeGridModel> dataSource, List<XPPackingRelation> packingRelations, List<XPPackageSnRecord> packageSnRecords, BindingList<XPWorkOrderPackageRuleDetail> packageRules)
        {
            dataSource.Clear();

            string baseUnitName = "";
            if (packageRules != null && packageRules.Count > 0)
                baseUnitName = packageRules[0].PackageUnitName;

            List<string> listSN = new List<string>();

            foreach (XPPackingRelation relation in packingRelations)
            {
                if (string.IsNullOrEmpty(relation.ParentNo))
                {
                    XPPackageSnRecord packageSnRecord = packageSnRecords.Find(p => p.Sn == relation.PackageNo);
                    if (packageSnRecord != null)
                        listSN.Add(relation.PackageNo);
                    TreeGridModel m = new TreeGridModel(relation, packageSnRecord);
                    dataSource.Add(m);
                    CreateChildByRealtion(m, relation, packingRelations, baseUnitName);
                }
            }

            foreach (XPPackageSnRecord r in packageSnRecords.Where(p=> !listSN.Contains(p.Sn)))
            {
                TreeGridModel m = new TreeGridModel(r, 0);
                dataSource.Insert(0, m);
            }
        }

        public TreeGridModel(XPPackingRelation relation, XPPackageSnRecord packageSnRecord)
        {
            this.PackageSnRecord = packageSnRecord;
            this.Sn = relation.PackageNo;
            this.PackageUnitName = relation.PackageUnitName;
            this.WoNo = packageSnRecord == null ? "" : packageSnRecord.WoNo;
            this.WoSn = packageSnRecord == null ? "" : packageSnRecord.WoSn;
            this.ProductName = packageSnRecord == null ? "" : packageSnRecord.ProductName;
            this.PackageUnitName = packageSnRecord == null ? this.PackageUnitName : packageSnRecord.PackageUnitName;
        }

        public TreeGridModel(XPItemLabel itemLabel, string baseUnitName)
        {
            this.PackageSnRecord = null;
            this.Sn = itemLabel.Label;
            this.PackageUnitName = baseUnitName;
            this.WoNo = itemLabel.WorkOrderNo;
            this.WoSn = itemLabel.Label;
            this.ProductName = itemLabel.ItemName;
        }

        private static void CreateChildByRealtion(TreeGridModel parent, XPPackingRelation prentRelation, List<XPPackingRelation> packingRelations, string baseUnitName)
        {
            if (prentRelation.ListItemLabel.Count > 0)
            {
                foreach (XPItemLabel itemLabel in prentRelation.ListItemLabel)
                {
                    parent.AddChild(new TreeGridModel(itemLabel, baseUnitName));
                }
                parent.WoSn = string.Join(",", prentRelation.ListItemLabel.Select(p=>p.Label).ToArray());
                if (string.IsNullOrEmpty(parent.WoNo))
                    parent.WoNo = prentRelation.ListItemLabel[0].WorkOrderNo;

                return;
            }

            foreach (XPPackingRelation relation in packingRelations.Where(p => p.ParentNo == parent.Sn))
            {
                TreeGridModel m = new TreeGridModel(relation, null);
                parent.AddChild(m);
                CreateChildByRealtion(m, relation, packingRelations, baseUnitName);
            }
        }


        public TreeGridModel()
        { 
        }

        public TreeGridModel(string sn, string packageUnitName, int packageUnitLevel, WorkOrder workOrder)
        {
            this.PackageUnitLevel = packageUnitLevel;
            this.Sn = sn;
            this.WoNo = workOrder.No;
            this.PackageUnitName = packageUnitName;
            this.ProductName = workOrder.ProductName;
        }

        public TreeGridModel(XPPackageSnRecord record, int packageUnitLevel)
        {
            this.PackageUnitLevel = packageUnitLevel;
            this.PackageSnRecord = record;
            this.Sn = record.Sn;
            this.PackageUnitName = record.PackageUnitName;
            this.WoNo = record.WoNo;
            this.WoSn = record.WoSn;
            this.ProductName = record.ProductName;
        }

        public TreeGridModel(XPPackingRelation relation, int packageUnitLevel)
        {
            this.Sn = relation.PackageNo;
            this.PackageUnitName = relation.PackageUnitName;
            this.WoNo = "";
            this.WoSn = "";
            this.PackageUnitLevel = packageUnitLevel;
        }

        public string ParentSn { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName { get; set; }


        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工单条码号
        /// </summary>
        public string WoSn { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductName { get; set; }

        public XPPackageSnRecord PackageSnRecord { get; set; }

        public int PackageUnitLevel { get; set; }

        public List<TreeGridModel> Childrens { get; set; }

        public void AddChild(TreeGridModel child)
        {
            if (Childrens == null)
                Childrens = new List<TreeGridModel>();

            this.Childrens.Add(child);
        }

        public void AddChild(XPPackingRelation relation)
        {

        }
    }
    #endregion
}
