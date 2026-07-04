using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 权限类型
    /// </summary>
	[Flags]
    public enum PermissionType
    {
        /// <summary>
        /// 上传
        /// </summary>
        [Label("上传")]
        Upload = 1,
        /// <summary>
        /// 修订
        /// </summary>
        [Label("修订")]
        Modify = 2,
        /// <summary>
        /// 作废
        /// </summary>
        [Label("作废")]
        Scrap = 4,
        /// <summary>
        /// 下载
        /// </summary>
        [Label("下载")]
        Download = 8,
        /// <summary>
        /// 预览
        /// </summary>
        [Label("预览")]
        Preview = 16,
        /// <summary>
        /// 发布
        /// </summary>
        [Label("发布")]
        Publish = 32,
        /// <summary>
        /// 删除
        /// </summary>
        [Label("删除")]
        Delete = 64,
    }
}