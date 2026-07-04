using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Fixture;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Threshold
{
    /// <summary>
    /// 可疑品阈值
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ThresholdCriterial))]
    [Label("可疑品阈值")]
    public class Threshold : DataEntity
    {

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<Threshold>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<Threshold>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<Threshold>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<Threshold>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion        

        #region 目标值 ThresholdValue
        /// <summary>
        /// 目标值
        /// </summary>
        [Label("目标值(%)")]
        public static readonly Property<string> ThresholdValueProperty = P<Threshold>.Register(e => e.ThresholdValue);

        /// <summary>
        /// 目标值
        /// </summary>
        public string ThresholdValue
        {
            get { return this.GetProperty(ThresholdValueProperty); }
            set { this.SetProperty(ThresholdValueProperty, value); }
        }
        #endregion

        #region 警戒值 AlertValue
        /// <summary>
        /// 警戒值
        /// </summary>
        [Label("警戒值(%)")]
        public static readonly Property<decimal> AlertValueProperty = P<Threshold>.Register(e => e.AlertValue);

        /// <summary>
        /// 警戒值
        /// </summary>
        public decimal AlertValue
        {
            get { return this.GetProperty(AlertValueProperty); }
            set { this.SetProperty(AlertValueProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<Threshold>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<Threshold>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<Threshold>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<Threshold>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }

        #endregion

        #endregion
    }
    /// <summary>
    /// 可疑品阈值 实体配置
    /// </summary>
    internal class ThresholdConfig : EntityConfig<Threshold>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    //修改这个唯一性校验的时候，要注意导入的时候也会根据工序+产品做为唯一键去修改数据
                    Threshold.ProcessIdProperty,
                    Threshold.ItemIdProperty,
                },
                MessageBuilder = (e) =>
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
            Meta.MapTable("THRESHOLD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
    /// <summary>
    /// 阈值类型
    /// </summary>
    public enum ThresholdType
    {
        /// <summary>
        /// 目标值
        /// </summary>
        [Label("目标值")]
        TargetValue = 10,

        /// <summary>
        /// 警戒值
        /// </summary>
        [Label("警戒值")]
        WarningValue = 20,
    }
}
