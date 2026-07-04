using SIE.Domain;
using SIE.Items;
using SIE.MES.Fixture;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemFixture
{
    /// <summary>
    /// 工装与产品记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工装与产品记录查询实体")]
    public class FixtureItemCriterial : Criteria
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<FixtureItemCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<FixtureItemCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemNameProperty = P<FixtureItemCriterial>.Register(e => e.ItemName);

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
        public static readonly IRefIdProperty ProcessIdProperty = P<FixtureItemCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<FixtureItemCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<string> ProcessCodeProperty = P<FixtureItemCriterial>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工装编码 FixtureUphold
        /// <summary>
        /// 工装编码Id
        /// </summary>
        [Label("工装编码")]
        public static readonly IRefIdProperty FixtureUpholdIdProperty =
            P<FixtureItemCriterial>.RegisterRefId(e => e.FixtureUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 工装编码Id
        /// </summary>
        public double? FixtureUpholdId
        {
            get { return (double?)this.GetRefNullableId(FixtureUpholdIdProperty); }
            set { this.SetRefNullableId(FixtureUpholdIdProperty, value); }
        }

        /// <summary>
        /// 工装编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureUphold> FixtureUpholdProperty =
            P<FixtureItemCriterial>.RegisterRef(e => e.FixtureUphold, FixtureUpholdIdProperty);

        /// <summary>
        /// 工装编码
        /// </summary>
        public FixtureUphold FixtureUphold
        {
            get { return this.GetRefEntity(FixtureUpholdProperty); }
            set { this.SetRefEntity(FixtureUpholdProperty, value); }
        }
        #endregion

        #region 工装名称 FixtureName
        /// <summary>
        /// 工装名称
        /// </summary>
        [Label("工装名称")]
        public static readonly Property<string> FixtureNameProperty = P<FixtureItemCriterial>.Register(e => e.FixtureName);

        /// <summary>
        /// 工装名称
        /// </summary>
        public string FixtureName
        {
            get { return this.GetProperty(FixtureNameProperty); }
            set { this.SetProperty(FixtureNameProperty, value); }
        }
        #endregion

        #region 图号 Drawn
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawnProperty = P<FixtureItemCriterial>.Register(e => e.Drawn);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
            set { this.SetProperty(DrawnProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<FixtureItemController>().CriterialFixtureItem(this);
        }
    }
}
