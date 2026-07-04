using SIE.Domain;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 安灯责任组维护基础表明细用户替换ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class UserReplaceViewModel : ViewModel
    {
        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<UserReplaceViewModel>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

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
            P<UserReplaceViewModel>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 替换用户 ReplaceUser
        /// <summary>
        /// 替换用户Id
        /// </summary>
        [Label("替换用户")]
        public static readonly IRefIdProperty ReplaceUserIdProperty =
            P<UserReplaceViewModel>.RegisterRefId(e => e.ReplaceUserId, ReferenceType.Normal);

        /// <summary>
        /// 替换用户Id
        /// </summary>
        public double ReplaceUserId
        {
            get { return (double)this.GetRefId(ReplaceUserIdProperty); }
            set { this.SetRefId(ReplaceUserIdProperty, value); }
        }

        /// <summary>
        /// 替换用户
        /// </summary>
        public static readonly RefEntityProperty<User> ReplaceUserProperty =
            P<UserReplaceViewModel>.RegisterRef(e => e.ReplaceUser, ReplaceUserIdProperty);

        /// <summary>
        /// 替换用户
        /// </summary>
        public User ReplaceUser
        {
            get { return this.GetRefEntity(ReplaceUserProperty); }
            set { this.SetRefEntity(ReplaceUserProperty, value); }
        }
        #endregion

    }
}
