using SIE.Domain;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemLine
{
    /// <summary>
    /// 产品与产线记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产品与产线记录查询实体")]
    public class ProductLineCriterial:Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProductLineCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ProductLineCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ProductLineCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProductLineCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序名称Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProductLineCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序名称
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ProductLineCriterial>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 产线/机台编码 WipResource
        /// <summary>
        /// 产线/机台编码Id
        /// </summary>
        [Label("产线/机台编码")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<ProductLineCriterial>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线/机台编码Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<ProductLineCriterial>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线/机台编码
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 产线/机台名称 LineName
        /// <summary>
        /// 产线/机台名称
        /// </summary>
        [Label("产线/机台名称")]
        public static readonly Property<string> LineNameProperty = P<ProductLineCriterial>.Register(e => e.LineName);

        /// <summary>
        /// 产线/机台名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion

        //#region 状态 State
        ///// <summary>
        ///// 状态
        ///// </summary>
        //[Label("状态")]
        //public static readonly Property<State?> StateProperty = P<ProductLineCriterial>.Register(e => e.State);

        ///// <summary>
        ///// 状态
        ///// </summary>
        //public State? State
        //{
        //    get { return this.GetProperty(StateProperty); }
        //    set { this.SetProperty(StateProperty, value); }
        //}
        //#endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductLineController>().CriterialProductLine(this);
        }
    }
}
