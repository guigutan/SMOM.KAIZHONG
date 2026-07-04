using SIE.Data.DbMigration;
using SIE.Kit.APS.EngineerPlans;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.DbMigrations
{
   public class _20210902_000011_InitDefectData : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接字符串名
        /// </summary>
        public override string DbSetting
        {
            get { return KitAPSEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 添加 Defect 库中的初始数据
        /// </summary>
        public override string Description
        {
            get { return "添加 客户优先级 的初始数据。"; }
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

                RT.Service.Resolve<EngineerPlanController>().Init();
            });
        }
    }
}
