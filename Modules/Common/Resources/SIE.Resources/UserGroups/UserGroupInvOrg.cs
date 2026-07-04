using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS.StereoWarhouses.Datas;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.InvOrgs;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    /// <summary>
    /// 用户组与库存组织关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("用户组与库存组织关系")]
    public class UserGroupInvOrg : DataEntity
    {
        #region 库存组织 Inv
        /// <summary>
        /// 库存组织Id
        /// </summary>
        [Label("库存组织")]
        public static readonly IRefIdProperty InvIdProperty =
            P<UserGroupInvOrg>.RegisterRefId(e => e.InvId, ReferenceType.Normal);

        /// <summary>
        /// 库存组织Id
        /// </summary>
        public double InvId
        {
            get { return (double)this.GetRefId(InvIdProperty); }
            set { this.SetRefId(InvIdProperty, value); }
        }

        /// <summary>
        /// 库存组织
        /// </summary>
        public static readonly RefEntityProperty<InvOrg> InvProperty =
            P<UserGroupInvOrg>.RegisterRef(e => e.Inv, InvIdProperty);

        /// <summary>
        /// 库存组织
        /// </summary>
        public InvOrg Inv
        {
            get { return this.GetRefEntity(InvProperty); }
            set { this.SetRefEntity(InvProperty, value); }
        }
        #endregion

        #region 用户组 UserGroup
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Label("用户组")]
        public static readonly IRefIdProperty UserGroupIdProperty =
            P<UserGroupInvOrg>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);

        /// <summary>
        /// 用户组Id
        /// </summary>
        public double UserGroupId
        {
            get { return (double)this.GetRefId(UserGroupIdProperty); }
            set { this.SetRefId(UserGroupIdProperty, value); }
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public static readonly RefEntityProperty<UserGroup> UserGroupProperty =
            P<UserGroupInvOrg>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);

        /// <summary>
        /// 用户组
        /// </summary>
        public UserGroup UserGroup
        {
            get { return this.GetRefEntity(UserGroupProperty); }
            set { this.SetRefEntity(UserGroupProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 库存组织编码 InvOrgCode
        /// <summary>
        /// 库存组织编码
        /// </summary>
        [Label("库存组织编码")]
        public static readonly Property<string> InvOrgCodeProperty = P<UserGroupInvOrg>.RegisterView(e => e.InvOrgCode, p => p.Inv.Code);

        /// <summary>
        /// 库存组织编码
        /// </summary>
        public string InvOrgCode
        {
            get { return this.GetProperty(InvOrgCodeProperty); }
        }
        #endregion

        #region 库存组织名称 InvOrgName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织名称")]
        public static readonly Property<string> InvOrgNameProperty = P<UserGroupInvOrg>.RegisterView(e => e.InvOrgName, p => p.Inv.Name);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName
        {
            get { return this.GetProperty(InvOrgNameProperty); }
        }
        #endregion

        #region 库存组织外部Id InvOrgExternalId
        /// <summary>
        /// 库存组织外部Id
        /// </summary>
        [Label("库存组织外部Id")]
        public static readonly Property<string> InvOrgExternalIdProperty = P<UserGroupInvOrg>.RegisterView(e => e.InvOrgExternalId, p => p.Inv.ExternalId);

        /// <summary>
        /// 库存组织外部Id
        /// </summary>
        public string InvOrgExternalId
        {
            get { return this.GetProperty(InvOrgExternalIdProperty); }
        }
        #endregion

        #region 库存组织备注 InvOrgRemark
        /// <summary>
        /// 库存组织备注
        /// </summary>
        [Label("库存组织备注")]
        public static readonly Property<string> InvOrgRemarkProperty = P<UserGroupInvOrg>.RegisterView(e => e.InvOrgRemark, p => p.Inv.Remark);

        /// <summary>
        /// 库存组织备注
        /// </summary>
        public string InvOrgRemark
        {
            get { return this.GetProperty(InvOrgRemarkProperty); }
        }
        #endregion

        #endregion
    }

    internal class UserGroupInvOrgConfig : EntityConfig<UserGroupInvOrg>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                UserGroupInvOrg.InvIdProperty,
                UserGroupInvOrg.UserGroupIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同库存组织".L10N();
                }
            });
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("USER_GROUP_INV_ORG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
