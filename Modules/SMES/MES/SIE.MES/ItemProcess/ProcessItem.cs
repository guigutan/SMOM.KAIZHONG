using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Checker;
using SIE.MES.ItemLine;
using SIE.MetaModel;
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
    /// 工序与物料的关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProcessItemCriterial))]
    [Label("工序与物料的关系")]
    public partial class ProcessItem : DataEntity
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ProcessItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ProcessItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料组 ItemGroup
        /// <summary>
        /// 物料组
        /// </summary>
        [Label("物料组")]
        public static readonly Property<string> ItemGroupProperty = P<ProcessItem>.Register(e => e.ItemGroup);

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
        public static readonly Property<string> ItemGroupDescProperty = P<ProcessItem>.Register(e => e.ItemGroupDesc);

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
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessItem>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<State> StateProperty = P<ProcessItem>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<ProcessItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 工序与物料的关系 实体配置
    /// </summary>
    internal class ProcessItemConfig : EntityConfig<ProcessItem>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    ProcessItem.ItemIdProperty,
                    ProcessItem.ProcessIdProperty
                },
                MessageBuilder=(e)=>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
