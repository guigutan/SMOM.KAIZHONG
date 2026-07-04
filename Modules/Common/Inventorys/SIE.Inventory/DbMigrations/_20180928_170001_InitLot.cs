using SIE.Data.DbMigration;
using SIE.Inventory.Commom;
using System;

namespace SIE.Inventory.DbMigrations
{
    /// <summary>
    /// 初始化默认批次信息
    /// </summary>
    public class _20180928_170001_InitLot : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return InveEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加默认批次信息".L10N(); }
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
        /// 注入
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                ////插入默认物料批次方案
                var ctl = RT.Service.Resolve<LotController>();
                if (ctl.GetLotDefault() == null)
                {
                    ctl.InsertDefaultLot();
                }
            });
        }
    }
}
