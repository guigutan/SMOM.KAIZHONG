using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分绩效等级初始化按钮
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetIniCommand")]
    public class AchieveLevelSetIniCommand : ViewCommand
    {
        /// <summary>
        /// 评分绩效等级初始化执行发放
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">视图参数类型</param>
        /// <returns>绩效等级配置集合</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            EntityList<AchieveLevelSetting> achieveLevels = null;
            try
            {
                achieveLevels = RT.Service.Resolve<ScoreRecordController>().AchieveLevelSetIni();
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("绩效等级配置初始化失败, ".L10N() + exMsg.Message);
            }

            return achieveLevels;
        }
    }
}
