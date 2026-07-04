using SIE.Common.Algorithm;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.Models;
using SIE.Data.DbMigration;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock
{
    /// <summary>
    /// 库内编码规则初始化
    /// </summary>
    public class _20250609_095201_Dock : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return DockEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "月台编码规则".L10N(); }
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
            Dictionary<string, string> ruleCodeDic = new Dictionary<string, string>
            {
                //有年月日
                { "月台预约号生成规则", "DAP" },
            };
            this.RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                //AppRuntime.InvOrg = 1;
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                var ctl = RT.Service.Resolve<NumberRuleExtController>();
                List<InitNumberData> ruleDatas = new List<InitNumberData>();
                ruleCodeDic.ForEach(p =>
                {
                    var rule = new InitNumberData() { Code = p.Key };

                    var dtl1 = new InitNumberDetailData() { DetailType = DetailType.FixedValue, FixedValue = p.Value };
                    rule.Details.Add(dtl1);

                    var dtl11 = new InitNumberDetailData() { DetailType = DetailType.InvOrg };
                    rule.Details.Add(dtl11);
                    switch (p.Value)
                    {
                        case "DAP":
                            var dtl2 = new InitNumberDetailData() { DetailType = DetailType.Date, DateFormat = DateFormat.yyMMdd };
                            rule.Details.Add(dtl2);
                            var dtl3 = new InitNumberDetailData() { DetailType = DetailType.TodaySequence, Length = 3 };
                            rule.Details.Add(dtl3);
                            break;
                    }
                    ruleDatas.Add(rule);
                });


                var rules = ctl.InitNumberRule(ruleDatas);
                rules.ForEach(p =>
                {
                    InitConifgValue(p);
                });
            });
        }

        private void InitConifgValue(NumberRule rule)
        {
            if (rule != null && rule.Id != default)
            {
                if (rule.Code == "月台预约号生成规则".L10N())
                {
                    DockAppointNoConfigValue countPlanNoConfigValue = new DockAppointNoConfigValue
                    {
                        NumberRuleId = rule.Id,
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<DockAppoint, DockAppointNoConfig, DockAppointNoConfigValue>(countPlanNoConfigValue);
                }
            }
        }
    }
}
