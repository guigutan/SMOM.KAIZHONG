using SIE.Domain;
using SIE.Items;
using SIE.MES.Checker;
using SIE.MES.ItemFixture;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemChecker
{
    /// <summary>
    /// 检具与产品记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("检具与产品记录查询实体")]
    public class CheckerItemCriterial:Criteria
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<CheckerItemCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<CheckerItemCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<CheckerItemCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 产品名称
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
        public static readonly IRefIdProperty ProcessIdProperty = P<CheckerItemCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<CheckerItemCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<string> ProcessCodeProperty = P<CheckerItemCriterial>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 检具编码 CheckerUphold
        /// <summary>
        /// 检具编码Id
        /// </summary>
        [Label("检具编码")]
        public static readonly IRefIdProperty CheckerUpholdIdProperty =
            P<CheckerItemCriterial>.RegisterRefId(e => e.CheckerUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 检具编码Id
        /// </summary>
        public double? CheckerUpholdId
        {
            get { return (double?)this.GetRefNullableId(CheckerUpholdIdProperty); }
            set { this.SetRefNullableId(CheckerUpholdIdProperty, value); }
        }

        /// <summary>
        /// 检具编码
        /// </summary>
        public static readonly RefEntityProperty<CheckerUphold> CheckerUpholdProperty =
            P<CheckerItemCriterial>.RegisterRef(e => e.CheckerUphold, CheckerUpholdIdProperty);

        /// <summary>
        /// 检具编码
        /// </summary>
        public CheckerUphold CheckerUphold
        {
            get { return this.GetRefEntity(CheckerUpholdProperty); }
            set { this.SetRefEntity(CheckerUpholdProperty, value); }
        }
        #endregion

        #region 检具名称 CheckerName
        /// <summary>
        /// 检具名称
        /// </summary>
        [Label("检具名称")]
        public static readonly Property<string> CheckerNameProperty = P<CheckerItemCriterial>.Register(e => e.CheckerName);

        /// <summary>
        /// 检具名称
        /// </summary>
        public string CheckerName
        {
            get { return this.GetProperty(CheckerNameProperty); }
            set { this.SetProperty(CheckerNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CheckerItemController>().CriterialCheckerItem(this);
        }
    }
}
