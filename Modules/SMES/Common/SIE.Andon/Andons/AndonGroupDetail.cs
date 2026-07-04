using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("明细")]
    public class AndonGroupDetail : DataEntity
    {
        #region 安灯责任组 AndonGroup
        /// <summary>
        /// 安灯责任组Id
        /// </summary>
        [Label("安灯责任组")]
        public static readonly IRefIdProperty AndonGroupIdProperty =
            P<AndonGroupDetail>.RegisterRefId(e => e.AndonGroupId, ReferenceType.Parent);

        /// <summary>
        /// 安灯责任组Id
        /// </summary>
        public double AndonGroupId
        {
            get { return (double)this.GetRefId(AndonGroupIdProperty); }
            set { this.SetRefId(AndonGroupIdProperty, value); }
        }

        /// <summary>
        /// 安灯责任组
        /// </summary>
        public static readonly RefEntityProperty<AndonGroup> AndonGroupProperty =
            P<AndonGroupDetail>.RegisterRef(e => e.AndonGroup, AndonGroupIdProperty);

        /// <summary>
        /// 安灯责任组
        /// </summary>
        public AndonGroup AndonGroup
        {
            get { return this.GetRefEntity(AndonGroupProperty); }
            set { this.SetRefEntity(AndonGroupProperty, value); }
        }
        #endregion

        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<AndonGroupDetail>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double UserId
        {
            get { return (double)this.GetRefId(UserIdProperty); }
            set { this.SetRefId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty =
            P<AndonGroupDetail>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 是否责任人 IsResponser
        /// <summary>
        /// 是否责任人
        /// </summary>
        [Label("是否责任人")]
        public static readonly Property<bool?> IsResponserProperty = P<AndonGroupDetail>.Register(e => e.IsResponser);

        /// <summary>
        /// 是否责任人
        /// </summary>
        public bool? IsResponser
        {
            get { return this.GetProperty(IsResponserProperty); }
            set { this.SetProperty(IsResponserProperty, value); }
        }
        #endregion

        #region 是否验收人 IsAcceptancer
        /// <summary>
        /// 是否验收人
        /// </summary>
        [Label("是否验收人")]
        public static readonly Property<bool?> IsAcceptancerProperty = P<AndonGroupDetail>.Register(e => e.IsAcceptancer);

        /// <summary>
        /// 是否验收人
        /// </summary>
        public bool? IsAcceptancer
        {
            get { return this.GetProperty(IsAcceptancerProperty); }
            set { this.SetProperty(IsAcceptancerProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 安灯责任组编码 AndonGroupCode
        /// <summary>
        /// 安灯责任组编码
        /// </summary>
        [Label("安灯责任组编码")]
        public static readonly Property<string> AndonGroupCodeProperty = P<AndonGroupDetail>.RegisterView(e => e.AndonGroupCode, p => p.AndonGroup.Code);

        /// <summary>
        /// 安灯责任组编码
        /// </summary>
        public string AndonGroupCode
        {
            get { return this.GetProperty(AndonGroupCodeProperty); }
        }
        #endregion

        #region 员工名称 UserName
        /// <summary>
        /// 员工名称
        /// </summary>
        [Label("员工名称")]
        public static readonly Property<string> UserNameProperty = P<AndonGroupDetail>.RegisterView(e => e.UserName, p => p.User.Employee.Name);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string UserName
        {
            get { return this.GetProperty(UserNameProperty); }
        }
        #endregion

        #region 员工编码 UserCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工编码")]
        public static readonly Property<string> UserCodeProperty = P<AndonGroupDetail>.RegisterView(e => e.UserCode, p => p.User.Employee.Code);

        /// <summary>
        /// 员工编码
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
        }
        #endregion

        #region 员工状态 UserState
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<State> UserStateProperty = P<AndonGroupDetail>.RegisterView(e => e.UserState, p => p.User.State);

        /// <summary>
        /// 员工状态
        /// </summary>
        public State UserState
        {
            get { return this.GetProperty(UserStateProperty); }
        }
        #endregion

        #endregion
    
    }

    internal class AndonGroupDetailConfig : EntityConfig<AndonGroupDetail>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                AndonGroupDetail.AndonGroupIdProperty,
                AndonGroupDetail.UserIdProperty
                },
                MessageBuilder = (e) => {
                    return "存在相同数据".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_GROUP_DTL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
