using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Process = SIE.Tech.Processs.Process;

namespace SIE.MES.MtartProcessLookups
{
    /// <summary>
    /// 物料分类与工序关系对照表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MtartProcessLookupCriteria))]
    [EntityWithConfig(typeof(Configs.MtartProcessLookupConfig))]
    [Label("物料分类与工序关系对照表")]
    public class MtartProcessLookup : DataEntity
    {
        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<MtartProcessLookup>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<MtartProcessLookup>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 物料类型 Mtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> MtartProperty = P<MtartProcessLookup>.Register(e => e.Mtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string Mtart
        {
            get { return this.GetProperty(MtartProperty); }
            set { this.SetProperty(MtartProperty, value); }
        }
        #endregion

        #region Mrb控制者 Dispo
        /// <summary>
        /// Mrb控制者
        /// </summary>
        [Label("Mrb控制者")]
        public static readonly Property<string> DispoProperty = P<MtartProcessLookup>.Register(e => e.Dispo);

        /// <summary>
        /// Mrb控制者
        /// </summary>
        public string Dispo
        {
            get { return this.GetProperty(DispoProperty); }
            set { this.SetProperty(DispoProperty, value); }
        }
        #endregion

        #region 工艺属性分类 KzCategory
        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        [Label("工艺属性分类")]
        public static readonly IRefIdProperty KzCategoryIdProperty =
            P<MtartProcessLookup>.RegisterRefId(e => e.KzCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 工艺属性分类Id
        /// </summary>
        public double? KzCategoryId
        {
            get { return (double?)this.GetRefNullableId(KzCategoryIdProperty); }
            set { this.SetRefNullableId(KzCategoryIdProperty, value); }
        }

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public static readonly RefEntityProperty<KzCategory> KzCategoryProperty =
            P<MtartProcessLookup>.RegisterRef(e => e.KzCategory, KzCategoryIdProperty);

        /// <summary>
        /// 工艺属性分类
        /// </summary>
        public KzCategory KzCategory
        {
            get { return this.GetRefEntity(KzCategoryProperty); }
            set { this.SetRefEntity(KzCategoryProperty, value); }
        }
        #endregion


        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<MtartProcessLookup>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 分类Id
        /// </summary>
        public double? ItemCategoryId
        {
            get { return (double?)this.GetRefNullableId(ItemCategoryIdProperty); }
            set { this.SetRefNullableId(ItemCategoryIdProperty, value); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        public static readonly RefEntityProperty<ItemCategory> ItemCategoryProperty =
            P<MtartProcessLookup>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<MtartProcessLookup>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

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
        public static readonly Property<string> ProcessNameProperty = P<MtartProcessLookup>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 分类描述 ItemCategoryName
        /// <summary>
        /// 分类描述
        /// </summary>
        [Label("分类描述")]
        public static readonly Property<string> ItemCategoryNameProperty = P<MtartProcessLookup>.RegisterView(e => e.ItemCategoryName, p => p.ItemCategory.Name);

        /// <summary>
        /// 分类描述
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
        #endregion

        #region 工艺属性分类编码 KzCategoryCode
        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        [Label("工艺属性分类编码")]
        public static readonly Property<string> KzCategoryCodeProperty = P<MtartProcessLookup>.RegisterView(e => e.KzCategoryCode,p=>p.KzCategory.Code);

        /// <summary>
        /// 工艺属性分类编码
        /// </summary>
        public string KzCategoryCode
        {
            get { return this.GetProperty(KzCategoryCodeProperty); }
        }
        #endregion

        #region 工艺属性分类名称 KzCategoryName
        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        [Label("工艺属性分类名称")]
        public static readonly Property<string> KzCategoryNameProperty = P<MtartProcessLookup>.RegisterView(e => e.KzCategoryName, p => p.KzCategory.Name);

        /// <summary>
        /// 工艺属性分类名称
        /// </summary>
        public string KzCategoryName
        {
            get { return this.GetProperty(KzCategoryNameProperty); }
        }
        #endregion

        #region 分类编码 ItemCategoryCode
        /// <summary>
        /// 分类编码
        /// </summary>
        [Label("分类编码")]
        public static readonly Property<string> ItemCategoryCodeProperty = P<MtartProcessLookup>.RegisterView(e => e.ItemCategoryCode, p => p.ItemCategory.Code);

        /// <summary>
        /// 分类编码
        /// </summary>
        public string ItemCategoryCode
        {
            get { return this.GetProperty(ItemCategoryCodeProperty); }
        }
        #endregion

        #endregion
    }

    internal class MtartProcessLookupConfig : EntityConfig<MtartProcessLookup>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            //非空验证
            //rules.AddRule(MtartProcessLookup.MtartProperty, new RequiredRule());
            //rules.AddRule(MtartProcessLookup.DispoProperty, new RequiredRule());

            //根据工序+物料类型+MRP控制者校验数据唯一性，
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                MtartProcessLookup.ProcessIdProperty,
                //MtartProcessLookup.MtartProperty,
                //MtartProcessLookup.DispoProperty
                MtartProcessLookup.ItemCategoryIdProperty,
                MtartProcessLookup.KzCategoryIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同工序、分类、工艺属性分类的数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("MTART_PROCESS_LOOKUP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
