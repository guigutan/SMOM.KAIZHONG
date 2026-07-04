using SIE.Data.DbMigration;
using SIE.Defects.Defects;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Defects.DbMigrations
{
    /// <summary>
    /// Defect 库中的初始数据
    /// </summary>
    public class _20220608_000001_InitData : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接字符串名
        /// </summary>
        public override string DbSetting
        {
            get { return DefectEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 添加 Defect 库中的初始数据
        /// </summary>
        public override string Description
        {
            get { return "添加 Defect 库中的初始数据。".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 不支持 Down
        /// </summary>
        protected override void Down() { }

        /// <summary>
        /// 升级数据
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                //由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                InitInspectionMode();
                InitDefectGrade();
                //获取快码组列表
            });
        }

        /// <summary>
        /// 检验方式
        /// </summary>
        private void InitInspectionMode()
        {
            var result = RT.Service.Resolve<InspectionItemController>().GetInspectionMode(new List<string> {"正常".L10N(), "返工".L10N(), "试产".L10N() });
           var model= result.FirstOrDefault(c => c.Name == "正常".L10N());
            var List = new EntityList<InspectionMode>();
            if (model == null)
            {
                List.Add(new InspectionMode()
                {
                    Code = "001",
                    Name = "正常".L10N(),
                });
            }
             model = result.FirstOrDefault(c => c.Name == "试产".L10N());
            if (model == null)
            {
                List.Add(new InspectionMode()
                {
                    Code = "003",
                    Name = "试产".L10N(),
                });
            }
            model = result.FirstOrDefault(c => c.Name == "返工".L10N());
            if (model == null)
            {
                List.Add(new InspectionMode()
                {
                    Code = "002",
                    Name = "返工".L10N(),
                });
            }
			RF.Save(List);
		}

        /// <summary>
        /// 缺陷等级
        /// </summary>
        private void InitDefectGrade()
        {
            var result = RT.Service.Resolve<DefectController>().GetAllDefectGrade();
            var model = result.FirstOrDefault(c => c.Name == "A");
            var List = new EntityList<DefectGrade>();
            if (model == null)
            {
                List.Add(new DefectGrade()
                {
                    DefectSeverity = DefectSeverity.deadly,
                    Name = "A",
                });
            }
            model = result.FirstOrDefault(c => c.Name == "B");
            if (model == null)
            {
                List.Add(new DefectGrade()
                {
                    DefectSeverity = DefectSeverity.high,
                    Name = "B",
                });
            }
            model = result.FirstOrDefault(c => c.Name == "C");
            if (model == null)
            {
                List.Add(new DefectGrade()
                {
                    DefectSeverity = DefectSeverity.middle,
                    Name = "C",
                });
            }
            model = result.FirstOrDefault(c => c.Name == "D");
            if (model == null)
            {
                List.Add(new DefectGrade()
                {
                    DefectSeverity = DefectSeverity.light,
                    Name = "D",
                });
            }
            RF.Save(List);
        }
    }
}