using SIE.Domain;
using SIE.Items;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemProcess
{
    /// <summary>
    /// 物料与工序记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料与工序记录查询实体")]
    public class ProcessItemCriterial : Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProcessItemCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<ProcessItemCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemNameProperty = P<ProcessItemCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料组 ItemGroup
        /// <summary>
        /// 物料组
        /// </summary>
        [Label("物料组")]
        public static readonly Property<string> ItemGroupProperty = P<ProcessItemCriterial>.Register(e => e.ItemGroup);

        /// <summary>
        /// 物料组
        /// </summary>
        public string ItemGroup
        {
            get { return this.GetProperty(ItemGroupProperty); }
            set { this.SetProperty(ItemGroupProperty, value); }
        }
        #endregion

        #region 物料组描述 ItemGroupDesc
        /// <summary>
        /// 物料组
        /// </summary>
        [Label("物料组")]
        public static readonly Property<string> ItemGroupDescProperty = P<ProcessItemCriterial>.Register(e => e.ItemGroupDesc);

        /// <summary>
        /// 物料组
        /// </summary>
        public string ItemGroupDesc
        {
            get { return this.GetProperty(ItemGroupDescProperty); }
            set { this.SetProperty(ItemGroupDescProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessItemCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessItemCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<ProcessItemCriterial>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessItemController>().CriterialProcessItem(this);
        }
    }
}
