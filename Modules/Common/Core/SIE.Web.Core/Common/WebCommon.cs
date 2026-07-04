namespace SIE.Web.Core.Common
{
    /// <summary>
    /// Web通用方法
    /// </summary>
    public static class WebCommon
    {
        /// <summary>
        /// 获取附件下载路径
        /// </summary>
        /// <returns></returns>
        public static string GetAttachmentDownloadUrl()
        {
           return  RT.Config.Get("path.attachmentType") == "ftp" ? RT.Config.Get("ftp.path") : RT.Config.Get("client.attachmentDownloadUrl");
        }

        /// <summary>
        /// 获取引用实体在前端的显示属性
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static string GetDisplayName(string entityName)
        {
            return entityName + "_Display";
        }
    }
}
