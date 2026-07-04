using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Tpms;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Tpms.Commands
{
    /// <summary>
    /// 保存TPM周工作检查评分项
    /// </summary>
    [JsCommand(CommandName)]
    public class SaveWeekJobScoreItemCommand : SaveCommand
    {
        ///// <summary>
        ///// 保存TPM周工作检查评分项命令名称
        ///// </summary>
        //public new const string CommandName = "SIE.Web.EMS.Tpms.Commands.SaveWeekJobScoreItemCommand";

        ///// <summary>
        ///// 保存周工作检查评分项列表
        ///// </summary>
        ///// <param name="data">周工作检查评分项列表</param>
        //protected override void DoSave(EntityList data)
        //{
        //    EntityList<TpmWeekInspectScore> newData = new EntityList<TpmWeekInspectScore>();
        //    EntityList<TpmWeekInspectScore> modifyData = new EntityList<TpmWeekInspectScore>();
        //    EntityList<TpmWeekInspectScore> delData = new EntityList<TpmWeekInspectScore>();
        //    foreach (Entity item in data)
        //    {
        //        if (item.IsNew)
        //            newData.Add(item);
        //        else
        //            modifyData.Add(item);
        //    }
        //    foreach (Entity item in data.DeletedList)
        //    {
        //        delData.Add(item);
        //    }
        //    var ret = RT.Service.Resolve<TpmController>().Validation100(newData, modifyData, delData);

        //    //if (!ret)
        //    //{
        //    //    throw new ValidationException("类型为【检查项】分值比总和必须等于100%".L10N());
        //    //}
        //    base.DoSave(data);
        //}
    }
}
