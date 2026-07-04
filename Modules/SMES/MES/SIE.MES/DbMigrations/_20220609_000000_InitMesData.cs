using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using System;

namespace SIE.MES.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220609_000000_InitMesData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
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
            this.RunCode(db =>
            {
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                //初始化 工单号生成规则
                InitWorkOrderNoConfig();

                InitReferenceWoBomConfig();
            });
        }

        /// <summary>
        /// 初始化 工单号生成规则
        /// </summary>
        private void InitWorkOrderNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工单号生成规则", "WO",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3, SIE.Common.NumberRules.RuleType.WorkOrder);

            if (numberRule != null && numberRule.Id != default)
            {
                WorkOrderNoConfigValue configValue = new WorkOrderNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<WorkOrder, WorkOrderNoConfig, WorkOrderNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 工序 BOM 参考工单 BOM 配置项
        /// </summary>
        private void InitReferenceWoBomConfig()
        {
            ReferenceWoBomConfigValue configValue = new ReferenceWoBomConfigValue
            {
                ReferenceWoBom = true
            };

            RT.Service.Resolve<ConfigExtController>()
                .InitModuleConfig<WorkOrder, ReferenceWoBomConfig, ReferenceWoBomConfigValue>(configValue);
        }
    }
}
