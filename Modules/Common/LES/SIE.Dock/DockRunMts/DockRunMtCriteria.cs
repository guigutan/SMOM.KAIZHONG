using SIE.Dock.DockMaintains.Service;
using SIE.Dock.DockRunMts.Service;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockRunMts
{
    /// <summary>
    /// 月台运行维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("月台运行维护查询实体")]
    public partial class DockRunMtCriteria : Criteria
    {
        #region 月台编码 Code
        /// <summary>
        /// 月台编码
        /// </summary>
        [Label("月台编码")]
        public static readonly Property<string> CodeProperty = P<DockRunMtCriteria>.Register(e => e.Code);

        /// <summary>
        /// 月台编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 月台名称 Name
        /// <summary>
        /// 月台名称
        /// </summary>
        [Label("月台名称")]
        public static readonly Property<string> NameProperty = P<DockRunMtCriteria>.Register(e => e.Name);

        /// <summary>
        /// 月台名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DockRunMtService>().GetDockRunMts(this);
        }
    }
}
