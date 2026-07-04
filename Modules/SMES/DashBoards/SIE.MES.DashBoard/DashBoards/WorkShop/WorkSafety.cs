using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.DashBoards.WorkShop
{

    /// <summary>
    /// 安全生产天数
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("安全生产天数")]
    public class WorkSafety : DataEntity
    {

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WorkSafety>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)this.GetRefId(FactoryIdProperty); }
            set { this.SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<InvOrg> FactoryProperty =
            P<WorkSafety>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public InvOrg Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 生产初始天数 SafetyDate
        /// <summary>
        /// 生产初始天数
        /// </summary>
        [Label("生产初始天数")]
        public static readonly Property<DateTime?> SafetyDateProperty
            = P<WorkSafety>.Register(e => e.SafetyDate);

        /// <summary>
        /// 生产初始天数
        /// </summary>
        public DateTime? SafetyDate
        {
            get { return this.GetProperty(SafetyDateProperty); }
            set { this.SetProperty(SafetyDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 安全生产天数 实体配置
    /// </summary>
    public class WorkSafetyConfig : EntityConfig<WorkSafety>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(WorkSafety.FactoryIdProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WORK_SAFETY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
