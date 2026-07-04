using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.DbMigrations
{
    /// <summary>
    /// 初始化工单耗用量耗用单号编码规则
    /// </summary>
    public class _20230531_100000_initWoCostNo : ManualDbMigration
    {
        /// <summary>
        /// 链接数据库
        /// </summary>
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加初始数据。".L10N(); }
        }

        /// <summary>
        /// 数据库回滚
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Down()
        {
            
        }

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
                InitwoCostNoConfig();
            });
        }

        private void InitwoCostNoConfig()
        {
            var numberRule = RT.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("工单耗用单编码规则", "XHD", SIE.Common.Algorithm.DateFormat.yyyyMMdd, 4);
            if (numberRule != null && numberRule.Id != default)
            {
                WoCostItemNoConfigValue configValue = new WoCostItemNoConfigValue
                {
                    CostNoRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<WoCostItem, WoCostItemNoConfig, WoCostItemNoConfigValue>(configValue);
            }
        }
    }
}
