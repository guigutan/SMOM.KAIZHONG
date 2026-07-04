using SIE.Dock.YardMaintains.Service;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.YardMaintains
{
    /// <summary>
    /// 园区维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("园区维护查询实体")]
    public partial class YardMaintainCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<YardMaintainCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<YardMaintainCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
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
            return RT.Service.Resolve<YardMaintainService>().GetYardMaintains(this);
        }
    }
}
