using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using SIE.xUnit.Core;

namespace SIE.xUnit.EMS.Equipments.Fixtures
{
    /// <summary>
    /// 设备台账固件
    /// </summary>
    public class EquipAccountFixture : FixtureBase
    {
        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount FixPropEquipAccount { get; set; }
                
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipAccountFixture()
        {
            FixPropEquipAccount = CreateEquipAccount();
        }

        /// <summary>
        /// 创建设备台账
        /// </summary>
        /// <returns></returns>
        protected virtual EquipAccount CreateEquipAccount()
        {
            var account = RT.Service.Resolve<EquipTestController>().CreateEquipAccount();
            
            
            return account;
        }

        /// <summary>
        /// 创建设备台账的校验项目
        /// </summary>
        /// <returns></returns>
        protected virtual EntityList<ProjectDetail> CreateProjectDetails(int count)
        {
            EntityList<ProjectDetail> list = new EntityList<ProjectDetail>();
            for (int i = 0; i < count; i++)
            {
                ProjectDetail dtl = new ProjectDetail();
                dtl.GenerateId();
                dtl.Name = $"Name{dtl.Id}";
                dtl.ProjectType = ProjectType.Verify;                
                dtl.CycleType = CycleType.Month;
                dtl.IsPhoto = YesNo.No;
                dtl.Method = "Method";
                dtl.Standard = "标准";
                dtl.MinValue = 0;
                dtl.MaxValue = 10;
                dtl.Unit = "mm";                
                list.Add(dtl);
            }
            RF.Save(list);
            return list;
        }
    }
}
