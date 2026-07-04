using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Text;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
	/// 设备台账保养项目
	/// </summary>
	[RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("设备台账保养项目")]
    public partial class EquipAccountMaintainProject : EquipAccountMaintainProjectBase
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipAccountMaintainProject>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Parent);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty = P<EquipAccountMaintainProject>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAccountMaintainProject>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipAccountMaintainProject>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备台账保养项目 实体配置
    /// </summary>
    internal class EquipAccountMaintainProjectConfig : EntityConfig<EquipAccountMaintainProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MAINTAIN_PRJ").MapAllProperties();
            Meta.EnablePhantoms();
        }

        /// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<EquipAccountMaintainProject>();
                    StringBuilder sb = new StringBuilder();

                    if (para.MinValue != null && para.MaxValue != null)
                    {
                        para.MinValue = para.MinValue ?? 0;
                        para.MaxValue = para.MaxValue ?? 0;

                        if (para.MinValue > para.MaxValue)
                        {
                            sb.AppendLine("保养项目的【最小值】不能大于【最大值】！".L10N());
                        }
                    }
                    e.BrokenDescription = sb.ToString();
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}
