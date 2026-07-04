using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Fixtures.Repairs;
using System;

namespace SIE.Fixtures.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_163600_InitFixturesData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return KitFixturesEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加快码组初始数据。".L10N(); }
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

                //初始化工治具入库单号生成规则
                InitInboundOrderNoConfig();

                //初始化工治具异常类型单号生成规则
                InitFixtureAbnormalNoConfig();

                //初始化工治具编码号生成规则
                InitFixtureEncodeNoConfig();

                //初始化工治具需求单号生成规则
                InitFixtureDemandNoConfig();

                //初始化工治具保养任务单号生成规则
                InitMaintainTaskNoConfig();

                //初始化工治具报修单号生成规则
                InitFixtureRepairNoConfig();


            });
        }

        /// <summary>
        ///初始化工治具入库单号生成规则
        /// </summary>
        private void InitInboundOrderNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具入库单号编码规则", "GR", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<InboundOrder, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        ///初始化工治具异常类型单号生成规则
        /// </summary>
        private void InitFixtureAbnormalNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具异常类型单号编码规则", "GE", 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureAbnormal, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        ///初始化工治具编码号生成规则
        /// </summary>
        private void InitFixtureEncodeNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具编码号编码规则", "GP", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureEncode, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        ///初始化工治具需求单号生成规则
        /// </summary>
        private void InitFixtureDemandNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具需求清单号编码规则", "GX", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureDemand, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        ///初始化工治具保养任务单号生成规则
        /// </summary>GX
        private void InitMaintainTaskNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具保养任务单号编码规则", "GB", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<MaintainTask, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        ///初始化工治具报修单号生成规则
        /// </summary>
        private void InitFixtureRepairNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具报修单号编码规则", "GW", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureRepair, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
