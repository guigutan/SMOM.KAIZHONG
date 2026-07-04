using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Projects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Checks.Commands
{
    /// <summary>
    /// 修改界面，设备对应的固定资产对应的点检保养项目选择
    /// </summary>
    
    public class EditChkSelEquipAccountCheckProjectCommand : ViewCommand
    {
        /// <summary>
        /// 修改命令名称
        /// </summary>
        public const string CommandName = "SIE.Web.EMS.Checks.Commands.EditChkSelEquipAccountCheckProjectCommand";

        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            List<CheckProject> list = args.Data.ToJsonObject<List<CheckProject>>();
            Check.NotNullOrEmpty(list, nameof(list));
            if (null == list || list.Count == 0)
                throw new ValidationException("点检项目不存在".L10N());
            foreach (var item in list)
            {
                var entity = new CheckProject();
                entity.CheckPlanId = item.CheckPlanId;
                entity.EquipAccountId = item.EquipAccountId;
                entity.EquipCheckProjectId = item.EquipCheckProjectId;
                savedData.Add(entity);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
