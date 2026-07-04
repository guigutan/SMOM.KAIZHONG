using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using System;

namespace SIE.MES.TaskManagement.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20230201_000000_InitTaskData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return TaskManagementEntityDataProvider.ConnectionStringName; }
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

                //初始化 报工批次号生成规则
                InitReportBatchNoConfig();
            });
        }

        /// <summary>
        /// 初始化 报工批次号生成规则
        /// </summary>
        private void InitReportBatchNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("报工批次号编码规则", "LOT",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                ReportRecordConfigValue configValue = new ReportRecordConfigValue
                {
                    ReportBatchNoRuleId = numberRule.Id,
                };
                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<ReportRecord, ReportRecordDetailConfig, ReportRecordConfigValue>(configValue);
            }
        }
    }
}
