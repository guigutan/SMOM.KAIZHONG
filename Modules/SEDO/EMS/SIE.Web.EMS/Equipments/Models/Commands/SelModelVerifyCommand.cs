using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Models;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Models.Commands
{
    /// <summary>
    /// 添加校验项目
    /// </summary>
    public class SelModelVerifyCommand : ViewCommand
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

            var verifyProjectInfos = args.Data.ToJsonObject<List<VerifyProjectInfo>>();

            Check.NotNullOrEmpty(verifyProjectInfos, nameof(verifyProjectInfos));

            if (null == verifyProjectInfos || verifyProjectInfos.Count == 0)
                throw new ValidationException("校验项目列表不能为空".L10N());


            var projectDetailIds = verifyProjectInfos.Select(m => m.ProjectDetailId).ToList();
            var projectDetails = RT.Service.Resolve<ProjectDetailController>().GetProjectDetails(projectDetailIds);

            foreach (var item in verifyProjectInfos)
            {
                var detail = projectDetails.FirstOrDefault(m => m.Id == item.ProjectDetailId);

                if (!detail.CycleType.HasValue)
                {
                    throw new ValidationException("校验项目项目【{0}】的周期类型为空".L10nFormat(detail.Name));
                }

                var checkProject = new EquipModelVerifyProject();
                checkProject.EquipModelId = item.SourceId;
                checkProject.ProjectDetailId = item.ProjectDetailId;
                checkProject.ProjectType = detail.ProjectType;
                checkProject.Consumable = detail.Consumable;
                checkProject.CycleType = detail.CycleType.Value;
                checkProject.MaxValue = detail.MaxValue;
                checkProject.Method = detail.Method;
                checkProject.MinValue = detail.MinValue;
                checkProject.Part = detail.Part;
                checkProject.Standard = detail.Standard;
                checkProject.Unit = detail.Unit;
                checkProject.UseTime = detail.UseTime.ToString();                
                savedData.Add(checkProject);
            }
            RF.Save(savedData);
            return true;
        }
    }

    /// <summary>
    /// 校验信息
    /// </summary>
    public class VerifyProjectInfo
    {
        /// <summary>
        /// 源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel { get; set; }

        /// <summary>
        /// 设备型号是否为新增
        /// </summary>
        public bool EquipModelIsNew { get; set; }
    }
}
