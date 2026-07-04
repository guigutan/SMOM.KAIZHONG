using SIE.Domain;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯责任组维护基础表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯责任组维护基础表查询实体")]
    public class AndonGroupCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AndonGroupCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<AndonGroupCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
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


        #region 用户 UserCode
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户")]
        public static readonly Property<string> UserCodeProperty = P<AndonGroupCriteria>.Register(e => e.UserCode);

        /// <summary>
        /// 用户
        /// </summary>
        public string UserCode
        {
            get { return this.GetProperty(UserCodeProperty); }
            set { this.SetProperty(UserCodeProperty, value); }
        }
        #endregion

        /*        #region 用户状态 UserState
                /// <summary>
                /// 用户状态
                /// </summary>
                [Label("用户状态")]
                public static readonly Property<UserState> UserStateProperty = P<AndonGroupCriteria>.Register(e => e.UserState);

                /// <summary>
                /// 用户状态
                /// </summary>
                public UserState UserState
                {
                    get { return this.GetProperty(UserStateProperty); }
                    set { this.SetProperty(UserStateProperty, value); }
                }
                #endregion*/

        #region 员工状态 UserState
        /// <summary>
        /// 员工状态
        /// </summary>
        [Label("员工状态")]
        public static readonly Property<UserState> UserStateProperty = P<AndonGroupCriteria>.RegisterView(e => e.UserState, p => p.User.State);

        /// <summary>
        /// 员工状态
        /// </summary>
        public UserState UserState
        {
            get { return this.GetProperty(UserStateProperty); }
        }
        #endregion

        #region 员工名称 UserName
        /// <summary>
        /// 员工名称
        /// </summary>
        [Label("员工名称")]
        public static readonly Property<string> UserNameProperty = P<AndonGroupCriteria>.Register(e => e.UserName);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string UserName
        {
            get { return this.GetProperty(UserNameProperty); }
            set { this.SetProperty(UserNameProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AndonController>().CriteriaAndonGroup(this);
        }
    }
}
