using SIE.Domain;
using SIE.MES.LineAndon;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using System;

namespace SIE.MES.BlueLable
{
    /// <summary>
    /// 蓝标替换查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("蓝标替换查询实体")]
    public class BlueLabelReplaceCriteria : Criteria
    {
        #region 旧蓝标 OldBlueLabel
        /// <summary>
        /// 旧蓝标
        /// </summary>
        [Label("旧蓝标")]
        public static readonly Property<string> OldBlueLabelProperty = P<BlueLabelReplaceCriteria>.Register(e => e.OldBlueLabel);

        /// <summary>
        /// 旧蓝标
        /// </summary>
        public string OldBlueLabel
        {
            get { return GetProperty(OldBlueLabelProperty); }
            set { SetProperty(OldBlueLabelProperty, value); }
        }
        #endregion

        #region 新蓝标 NewBlueLabel
        /// <summary>
        /// 新蓝标
        /// </summary>
        [Label("新蓝标")]
        public static readonly Property<string> NewBlueLabelProperty = P<BlueLabelReplaceCriteria>.Register(e => e.NewBlueLabel);

        /// <summary>
        /// 新蓝标
        /// </summary>
        public string NewBlueLabel
        {
            get { return GetProperty(NewBlueLabelProperty); }
            set { SetProperty(NewBlueLabelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BlueLabelReplaceController>().CriteriaBlueLabelReplace(this);
        }

    }
}