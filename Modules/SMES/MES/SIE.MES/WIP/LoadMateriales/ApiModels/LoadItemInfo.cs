using SIE.Items;
using SIE.MES.SingleLabels;
using SIE.MES.WorkOrders;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.LoadMateriales.ApiModels
{

    /// <summary>
    /// 上料信息
    /// </summary>
    [Serializable]
    public class LoadItemInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id
        {
            get;
            set;
        }

        /// <summary>
        /// 来源条码 
        /// </summary>
        public string SourceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get;
            set;
        }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadQty
        {
            get;
            set;
        }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get;
            set;
        }

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal UnloadQty
        {
            get;
            set;
        }

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get;
            set;
        }

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get;
            set;
        }


        /// <summary>
        /// 班次
        /// </summary>
        public string ShiftName
        {
            get;
            set;
        }

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get;
            set;
        }


        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get;
            set;
        }


        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get;
            set;
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get;
            set;
        }


        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get;
            set;
        }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get;
            set;
        }

        /// <summary>
        /// 上料来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get;
            set;
        }


        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 挪料上料
        /// </summary>
        public YesNo IsMoveItem
        {
            get;
            set;
        }

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter
        {
            get;
            set;
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 物料消耗属性
        /// </summary>
        public ConsumeMode ItemConsumeMode
        {
            get;
            set;
        }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get;
            set;
        }
    }
}
