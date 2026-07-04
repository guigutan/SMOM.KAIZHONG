using SIE.Domain;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class FeedingAreaResourceSelectCommand : ViewCommand
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var datas = args.Data.ToJsonObject<List<FeedingAreaReource>>();
            Check.NotNullOrEmpty(datas, nameof(datas));

            if (null == datas || datas.Count == 0)
                throw new ArgumentNullException(nameof(datas));

            foreach (var item in datas)
            {
                var data = new FeedingAreaReource();
                data.FeedingAreaId = item.FeedingAreaId;
                data.ResourceId = item.ResourceId;
                savedData.Add(data);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
