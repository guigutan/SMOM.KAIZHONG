using SIE.Barcodes.Panels;
using SIE.Barcodes.Panels.Configs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using System;

namespace SIE.Barcodes.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_000000_InitBarcodeData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return BarcodeEntityDataProvider.ConnectionStringName; }
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

                //初始化 生产条码编码规则
                InitBarcodeNoConfig();

                //初始化 拼板码编码规则
                InitPanelPrintConfig();
            });
        }
        /// <summary>
        /// 初始化 生产条码编码规则
        /// </summary>
        private void InitBarcodeNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            numberExtCtl.CreateFormNoNumberRule("生产条码编码规则", "SN",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 5, Common.NumberRules.RuleType.Barcode);
        }

        /// <summary>
        /// 初始化 拼板码编码规则
        /// </summary>
        private void InitPanelPrintConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("拼板码编码规则", "P",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 5, Common.NumberRules.RuleType.Barcode);

            if (numberRule != null && numberRule.Id != default)
            {
                PanelPrintConfigValue configValue = new PanelPrintConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<PanelWorkOrder, PanelPrintConfig, PanelPrintConfigValue>(configValue);
            }
        }
    }
}
