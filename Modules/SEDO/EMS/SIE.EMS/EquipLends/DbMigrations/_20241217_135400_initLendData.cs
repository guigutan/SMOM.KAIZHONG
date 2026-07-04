using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.EMS.EquipLends.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.DbMigrations
{
    /// <summary>
    /// 设备借还数据库初始化
    /// </summary>
    public class _20241217_135400_initLendData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return EmsEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加初始数据。".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 数据库回滚
        /// </summary>
        protected override void Down() { }


        /// <summary>
        /// 数据库升级
        /// </summary>
        protected override void Up()
        {
            RunCode(db =>
            {
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                // 初始化设备借还单号编码规则
                InitLendNoRule();
            });
        }

        /// <summary>
        /// 初始化设备借还单号编码规则
        /// </summary>
        private void InitLendNoRule()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备借还单号生成规则", "LD",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                EquipLendManageConfigValue configValue = new EquipLendManageConfigValue();
                configValue.NoRuleId = numberRule.Id;
                // 初始化配置项
                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<EquipLendManage, EquipLendManageConfig, EquipLendManageConfigValue>(configValue);
            }
        }
    }
}
