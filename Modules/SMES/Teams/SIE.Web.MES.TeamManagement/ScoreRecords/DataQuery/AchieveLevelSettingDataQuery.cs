using SIE.Domain;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.Web.Data;

namespace SIE.Web.MES.TeamManagement.ScoreRecords.DataQuery
{
    /// <summary>
    /// 获取评分绩效配置数据查询类
    /// </summary>
    public class AchieveLevelSettingDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取评分绩效配置集合
        /// </summary>
        /// <returns>评分绩效配置集合</returns>
        public EntityList<AchieveLevelSetting> GetAchieveLevelSettings()
        {
            var achiLevelSettings = RT.Service.Resolve<ScoreRecordController>().GetAchieveLevelSettings();
            return achiLevelSettings;
        }
    }
}
