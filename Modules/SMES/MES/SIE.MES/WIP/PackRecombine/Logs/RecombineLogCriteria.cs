using SIE.Domain;
using SIE.MES.WIP.PackRecombine.Relations;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.PackRecombine.Logs
{
    /// <summary>
    /// 包装操作日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("包装操作日志查询实体")]
    public class RecombineLogCriteria : Criteria
    {
        #region 包装号 PackageNo
        /// <summary>
        /// 包装号
        /// </summary>
        [Label("包装号")]
        public static readonly Property<string> PackageNoProperty = P<RecombineLogCriteria>.Register(e => e.PackageNo);

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 外层包装号 ParentNo
        /// <summary>
        /// 外层包装号
        /// </summary>
        [Label("外层包装号")]
        public static readonly Property<string> ParentNoProperty = P<RecombineLogCriteria>.Register(e => e.ParentNo);

        /// <summary>
        /// 外层包装号
        /// </summary>
        public string ParentNo
        {
            get { return GetProperty(ParentNoProperty); }
            set { SetProperty(ParentNoProperty, value); }
        }
        #endregion

        #region 操作类型 ScanMode
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<ScanMode?> ScanModeProperty = P<RecombineLogCriteria>.Register(e => e.ScanMode);

        /// <summary>
        /// 操作类型
        /// </summary>
        public ScanMode? ScanMode
        {
            get { return GetProperty(ScanModeProperty); }
            set { SetProperty(ScanModeProperty, value); }
        }
        #endregion

        #region 是否批次 IsBatch
        /// <summary>
        /// 是否批次
        /// </summary>
        [Label("是否批次")]
        public static readonly Property<YesNo?> IsBatchProperty = P<RecombineLogCriteria>.Register(e => e.IsBatch);

        /// <summary>
        /// 是否批次
        /// </summary>
        public YesNo? IsBatch
        {
            get { return GetProperty(IsBatchProperty); }
            set { SetProperty(IsBatchProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取包装关系列表
        /// </summary>
        /// <returns>包装关系列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingRelationQueryController>().GetRecombineLogs(this);
        }
    }
}
