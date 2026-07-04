using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备项目维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产前准备项目维护查询实体")]
    public class PrepareProjectCriteria : Criteria
    {
        #region 项目编码 ProCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProCodeProperty = P<PrepareProjectCriteria>.Register(e => e.ProCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
            set { this.SetProperty(ProCodeProperty, value); }
        }
        #endregion

        #region 项目名称 ProName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProNameProperty = P<PrepareProjectCriteria>.Register(e => e.ProName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
            set { this.SetProperty(ProNameProperty, value); }
        }
        #endregion

        #region 项目类型 ProType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> ProTypeProperty = P<PrepareProjectCriteria>.Register(e => e.ProType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProType
        {
            get { return this.GetProperty(ProTypeProperty); }
            set { this.SetProperty(ProTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PrepareProjectService>().QueryPrepareProjectList(this);
        }
    }
}
