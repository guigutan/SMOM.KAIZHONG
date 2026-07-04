using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.MtartProcessLookups
{
    /// <summary>
    /// 物料分类与工序关系对照表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料分类与工序关系对照表查询实体")]
    public class MtartProcessLookupCriteria : Criteria
    {
        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<MtartProcessLookupCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<MtartProcessLookupCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<string> MtartProperty = P<MtartProcessLookupCriteria>.Register(e => e.Mtart);

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
        public static readonly Property<string> DispoProperty = P<MtartProcessLookupCriteria>.Register(e => e.Dispo);

        /// <summary>
        /// Mrb控制者
        /// </summary>
        public string Dispo
        {
            get { return this.GetProperty(DispoProperty); }
            set { this.SetProperty(DispoProperty, value); }
        }
        #endregion

        #region 分类 ItemCategory
        /// <summary>
        /// 分类Id
        /// </summary>
        [Label("分类")]
        public static readonly IRefIdProperty ItemCategoryIdProperty =
            P<MtartProcessLookupCriteria>.RegisterRefId(e => e.ItemCategoryId, ReferenceType.Normal);

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
            P<MtartProcessLookupCriteria>.RegisterRef(e => e.ItemCategory, ItemCategoryIdProperty);

        /// <summary>
        /// 分类
        /// </summary>
        public ItemCategory ItemCategory
        {
            get { return this.GetRefEntity(ItemCategoryProperty); }
            set { this.SetRefEntity(ItemCategoryProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MtartProcessLookupsController>().CriteriaMtartProcessLookups(this);
        }
    }
}
