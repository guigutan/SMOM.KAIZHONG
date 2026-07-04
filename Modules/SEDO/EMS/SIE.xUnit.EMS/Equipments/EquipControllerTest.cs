using SIE.EMS.Equipments;
using SIE.Equipments.EquipModels;
using SIE.xUnit.EMS.Equipments.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace SIE.xUnit.EMS.Equipments
{
    /// <summary>
    /// 设备控制器 单元测试
    /// </summary>
    public class EquipControllerTest : EmsControllerTestBase, IClassFixture<EquipFixture>
    {
        /// <summary>
        /// 设备固件
        /// </summary>
        private readonly EquipFixture _equipFixture;

        /// <summary>
        /// 设备控制器
        /// </summary>
        private readonly EquipController _equipController;

        /// <summary>
        /// 设备型号控制器
        /// </summary>
        private readonly EquipModelController equipModelController;

        /// <summary>
        /// 设备控制器单元测试构造函数
        /// </summary>
        /// <param name="equipFixture"></param>
        public EquipControllerTest(EquipFixture equipFixture)
        {
            _equipFixture = equipFixture;
            _equipController = RT.Service.Resolve<EquipController>();
            equipModelController = RT.Service.Resolve<EquipModelController>();
        }

        /// <summary>
        /// 查询设备型号列表 单元测试
        /// </summary>
        [Fact]
        public void GetEquipModelsByCriteriaTest()
        {
            var equipModel = RT.Service.Resolve<EquipTestController>().CreateEquipModel();
            //查询
            var criteria = new EquipModelCriteria()
            {
                Code = equipModel.Code,
                Name = equipModel.Name,
                TypeCategory = equipModel.TypeCategory,
                EquipTypeId = equipModel.EquipTypeId,
                CreateDate = new ObjectModel.DateRange()
                {
                    BeginValue = DateTime.Today.AddDays(-6),
                    EndValue = DateTime.Today.AddDays(1)
                },
                PagingInfo = new PagingInfo()
                {
                    PageSize = 25,
                    PageNumber = 1,
                    IsNeedCount = true
                }
            };
            var list = equipModelController.GetEquipModelsByCriteria(criteria);
            Assert.Single(list);
        }

        /// <summary>
        /// 通过设备型号ID获取校验项目列表 单元测试
        /// </summary>
        [Fact]
        public void GetEquipModelVerifyProjectsTest()
        {
            var verifyProject = _equipFixture.CreateEquipModelVerifyProjects(1).FirstOrDefault();
            var result = _equipController.GetEquipModelVerifyProjects(verifyProject.EquipModelId);
            Assert.Single(result);
        }
    }
}
