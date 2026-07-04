using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Common.HomeMenusConfigs
{
    /// <summary>
    /// 查询条件
    /// </summary>
    [QueryEntity, Serializable]
    public class HomeMenusConfigCriteria : Criteria
    {
        #region 用户名 UseName
        /// <summary>
        /// 用户名
        /// </summary>
        [Label("用户名")]
        public static readonly Property<string> UseNameProperty = P<HomeMenusConfigCriteria>.Register(e => e.UseName);

        /// <summary>
        /// 用户名
        /// </summary>
        public string UseName
        {
            get { return this.GetProperty(UseNameProperty); }
            set { this.SetProperty(UseNameProperty, value); }
        }
        #endregion


        #region 角色名称 RoleName
        /// <summary>
        /// 角色名称
        /// </summary>
        [Label("角色名称")]
        public static readonly Property<string> RoleNameProperty = P<HomeMenusConfigCriteria>.Register(e => e.RoleName);

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get { return this.GetProperty(RoleNameProperty); }
            set { this.SetProperty(RoleNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
           return RT.Service.Resolve<HomeMenusConfigsController>().Fetch(this);
        }

    }
}
