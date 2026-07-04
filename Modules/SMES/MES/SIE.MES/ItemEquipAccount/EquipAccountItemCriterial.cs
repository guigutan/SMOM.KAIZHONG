using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemEquipAccount
{
    /// <summary>
    /// 模具与产品记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("模具与产品记录查询实体")]
    public class EquipAccountItemCriterial : Criteria
    {
        #region 产品编码 Item
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<EquipAccountItemCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
            P<EquipAccountItemCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ItemNameProperty = P<EquipAccountItemCriterial>.Register(e => e.ItemName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 旧料号 OldItem
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> OldItemProperty = P<EquipAccountItemCriterial>.Register(e => e.OldItem);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string OldItem
        {
            get { return this.GetProperty(OldItemProperty); }
            set { this.SetProperty(OldItemProperty, value); }
        }
        #endregion

        #region 物料类型 ItemMtart
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<string> ItemMtartProperty = P<EquipAccountItemCriterial>.Register(e => e.ItemMtart);

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemMtart
        {
            get { return this.GetProperty(ItemMtartProperty); }
            set { this.SetProperty(ItemMtartProperty, value); }
        }
        #endregion


        #region 工序名称 Process
        /// <summary>
        /// 工序名称Id
        /// </summary>
        [Label("工序名称")]
        public static readonly IRefIdProperty ProcessIdProperty = P<EquipAccountItemCriterial>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EquipAccountItemCriterial>.RegisterRef(e => e.Process, ProcessIdProperty);

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
        public static readonly Property<string> ProcessCodeProperty = P<EquipAccountItemCriterial>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 模具编码 EquipAccount
        /// <summary>
        /// 模具编码Id
        /// </summary>
        [Label("模具编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipAccountItemCriterial>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 模具编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 模具编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<EquipAccountItemCriterial>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 模具编码
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 模具编码 EquipAccountCode
        /// <summary>
        /// 模具编码
        /// </summary>
        [Label("模具编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAccountItemCriterial>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 模具编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 模具名称 EquipAccountName
        /// <summary>
        /// 模具名称
        /// </summary>
        [Label("模具名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipAccountItemCriterial>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 模具名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 模具图号 Drawn
        /// <summary>
        /// 模具图号
        /// </summary>
        [Label("模具图号")]
        public static readonly Property<string> DrawnProperty = P<EquipAccountItemCriterial>.Register(e => e.Drawn);

        /// <summary>
        /// 模具图号
        /// </summary>
        public string Drawn
        {
            get { return this.GetProperty(DrawnProperty); }
            set { this.SetProperty(DrawnProperty, value); }
        }
        #endregion


        #region 模具组 UniqueCode
        /// <summary>
        /// 模具组
        /// </summary>
        [Label("模具组")]
        public static readonly Property<string> UniqueCodeProperty = P<EquipAccountItemCriterial>.Register(e => e.UniqueCode);

        /// <summary>
        /// 模具组
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipAccountItemController>().CriterialEquipAccountItem(this);
        }
    }
}
