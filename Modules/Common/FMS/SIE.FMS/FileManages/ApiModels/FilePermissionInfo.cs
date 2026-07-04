using System;

namespace SIE.FMS.FileManages.ApiModels
{
    /// <summary>
    /// 文件权限信息
    /// </summary>
    [Serializable]
    public class FilePermissionInfo
    {
        /// <summary>
        /// 是否修改过
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// 文件权限ID
        /// </summary>
        public double? PermissionId { get; set; }

        /// <summary>
        /// 文件用户组ID
        /// </summary>
        public double FileUserGroupId { get; set; }

        /// <summary>
        /// 文件用户组显示
        /// </summary>
        public string FileUserGroupName { get; set; }

        /// <summary>
        /// 上传权限
        /// </summary>
        public bool Upload { get; set; }

        /// <summary>
        /// 修订权限
        /// </summary>
        public bool Modify { get; set; }

        /// <summary>
        /// 作废权限
        /// </summary>
        public bool Scrap { get; set; }

        /// <summary>
        /// 下载权限
        /// </summary>
        public bool Download { get; set; }

        /// <summary>
        /// 预览权限
        /// </summary>
        public bool Preview { get; set; }

        /// <summary>
        /// 发布权限
        /// </summary>
        public bool Publish { get; set; }

        /// <summary>
        /// 删除权限
        /// </summary>
        public bool Delete { get; set; }
    }
}
