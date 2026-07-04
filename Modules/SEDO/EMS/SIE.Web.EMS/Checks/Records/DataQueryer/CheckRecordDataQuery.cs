using SIE.Domain;
using SIE.EMS.Checks.Projects;
using SIE.Web.Data;

namespace SIE.Web.EMS.Checks.Records.DataQueryers
{
    /// <summary>
    /// 点检记录数据查询体
    /// </summary>
    public class CheckRecordDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取点检项目图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public string GetCheckProjectPhoto(double projectId)
        {
            var photo = RF.GetById<CheckProject>(projectId)?.ProjectPhoto?.Photo;
            if (photo == null)
                return string.Empty;
            return System.Text.Encoding.Default.GetString(photo);
        }
    }
}