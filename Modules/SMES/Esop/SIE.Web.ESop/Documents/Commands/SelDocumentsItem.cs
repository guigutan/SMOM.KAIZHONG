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
    /// 添加点检项目
    /// </summary>
    [JsCommand("SIE.Web.ESop.Documents.Commands.SelDocumentsItem")]
    public class SelDocumentsItem : ViewCommand
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
                throw new ValidationException("物料列表不能为空".L10N());

            var projectDetailIds = checkProjectInfos.Select(p => p.ProjectDetailId).Distinct().ToList();
            var details = RT.Service.Resolve<ItemController>().GetItemDataList(projectDetailIds,null);

            var parentIsExsited = RF.GetById<DocumentCollection>(checkProjectInfos.First().SourceId)!=null;
            if (parentIsExsited)
            {
                foreach (var item in checkProjectInfos)
                {
                    var detail = details.FirstOrDefault(m => m.Id == item.ProjectDetailId);

                    var checkProject = new DocumentCollectionItem();
                    checkProject.DocumentCollectionId = item.SourceId;
                    checkProject.ItemId = detail.Id;
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
    
    /// <summary>
      /// 信息
      /// </summary>
    [Serializable]
    public class ProjectInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId { get; set; }
    }
}

