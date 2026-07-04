using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class TargetWarnSettingCriteria : Criteria
    {
        #region 编码
        /// <summary>
        /// 编码
        /// </summary>
        [Label("指标分类")]
        public static readonly Property<string> CodeProperty = P<TargetWarnSettingCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }

        #endregion

        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        [Label("指标名称")]
        public static readonly Property<string> NameProperty = P<TargetWarnSettingCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取预警设定列表
        /// </summary>
        /// <returns>预警设定列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<TargetWarnSettingController>().GetTargetWarnSetting(this);
        }
    }
}
