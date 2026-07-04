using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.MainenanceProjects;
using SIE.xUnit.Core;
using System;

namespace SIE.xUnit.EMS.Equipments.Fixtures
{
    /// <summary>
    /// 设备标准固件
    /// </summary>
    public class EquipFixture : FixtureBase
    {
        /// <summary>
        /// 创建校验项目
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns>校验项目集合</returns>
        public virtual EntityList<EquipModelVerifyProject> CreateEquipModelVerifyProjects(int count)
        {
            var verifyProjects = new EntityList<EquipModelVerifyProject>();
            for (int i = 0; i < count; i++)
            {
                var verifyProject = new EquipModelVerifyProject();
                verifyProject.GenerateId();
                verifyProject.EquipModelId = RT.Service.Resolve<EquipTestController>().CreateEquipModel().Id;
                verifyProject.ProjectDetailId = CreateProjectDetail(ProjectType.Verify).Id;

                verifyProjects.Add(verifyProject);
            }
            RF.Save(verifyProjects);

            return verifyProjects;
        }

        /// <summary>
        /// 创建点检保养项目
        /// </summary>
        /// <param name="projectType">项目类型</param>
        /// <returns>点检保养项目</returns>
        public virtual ProjectDetail CreateProjectDetail(ProjectType projectType)
        {
            var projectDetail = new ProjectDetail();
            projectDetail.GenerateId();
            var id = projectDetail.Id;
            projectDetail.Name = $"Name{id}";
            projectDetail.Part = $"Part{id}";
            projectDetail.Consumable = $"Consumable{id}";
            projectDetail.Method = $"Method{id}";
            projectDetail.Standard = $"Standard{id}";
            projectDetail.MinValue = 1;
            projectDetail.MaxValue = projectDetail.MaxValue + 5;
            projectDetail.Unit = $"Unit{id}";
            projectDetail.UseTime = "30";
            projectDetail.ProjectType = projectType;
            projectDetail.IsPhoto = YesNo.No;
            projectDetail.CycleType = CycleType.Month;
            RF.Save(projectDetail);

            return projectDetail;
        }
    }
}
