using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Documents;
using SIE.Items;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ESop.Documents.Commands
{
    /// <summary>
    /// 选择工单命令
    /// </summary>
    [JsCommand("SIE.Web.ESop.Common.Commands.SelWoCommand")]
    public class SelWoCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            List<ProjectInfo> checkProjectInfos = args.Data.ToJsonObject<List<ProjectInfo>>();
            Check.NotNullOrEmpty(checkProjectInfos, nameof(checkProjectInfos));
            if (null == checkProjectInfos || checkProjectInfos.Count == 0)
                throw new ValidationException("工单列表不能为空".L10N());

            var projectDetailIds = checkProjectInfos.Select(p => p.ProjectDetailId).Distinct().ToList();
            var details = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(projectDetailIds);

            var parentIsExsited = RF.GetById<DocumentCollection>(checkProjectInfos.First().SourceId)!=null;
            if (parentIsExsited)
            {
                foreach (var item in checkProjectInfos)
                {
                    var detail = details.FirstOrDefault(m => m.Id == item.ProjectDetailId);

                    var checkProject = new DocumentCollectionWorkOrder();
                    checkProject.CollectionId = item.SourceId;
                    checkProject.WorkOrderId = detail.Id;
                    savedData.Add(checkProject);
                }
                RF.Save(savedData);
            }
            else {
                throw new ValidationException("请先保存主表后再操作".L10N());
            }
            return true;
        }
    } 
}

