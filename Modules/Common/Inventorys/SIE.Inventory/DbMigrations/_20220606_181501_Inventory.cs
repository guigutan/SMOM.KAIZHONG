using SIE.Common.Algorithm;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.Models;
using SIE.Data.DbMigration;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.DbMigrations
{
    internal class _20220606_181501_Inventory: ManualDbMigration
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
            get { return "任务编码规则".L10N(); }
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
                { "任务管理单号生成规则", "RW" },
                { "任务组管理单号生成规则", "RWG" },
                //无年月日
                { "周转规则单号生成规则", "ZZ" },
                { "分配规则单号生成规则", "FP" },
                { "上架规则单号生成规则", "PUR" }
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
                        case "RW":
                        case "RWG":
                            var dtl2 = new InitNumberDetailData() { DetailType = DetailType.Date, DateFormat = DateFormat.yyMMdd };
                            rule.Details.Add(dtl2);
                            var dtl3 = new InitNumberDetailData() { DetailType = DetailType.TodaySequence, Length = 3 };
                            rule.Details.Add(dtl3);
                            break;
                        default:
                            var dtl4 = new InitNumberDetailData() { DetailType = DetailType.Sequence, Length = 3 };
                            rule.Details.Add(dtl4);
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

                if (rule.Code == "任务管理单号生成规则".L10N())
                {
                    NoConfigValue taskManagementNoConfigValue = new NoConfigValue
                    {
                        NumberRuleId = rule.Id
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<TaskManagement, NoConfig, NoConfigValue>(taskManagementNoConfigValue);
                }else if (rule.Code == "任务组管理单号生成规则".L10N())
                {
                    NoConfigValue taskGroupConfigValue = new NoConfigValue
                    {
                        NumberRuleId = rule.Id
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<TaskGroup, NoConfig, NoConfigValue>(taskGroupConfigValue);
                }
                else if (rule.Code == "周转规则单号生成规则".L10N())
                {
                    NoConfigValue turnOverRuleNoConfigValue = new NoConfigValue
                    {
                        NumberRuleId = rule.Id
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<TurnOverRule, NoConfig, NoConfigValue>(turnOverRuleNoConfigValue);
                }
                else if (rule.Code == "分配规则单号生成规则".L10N())
                {
                    NoConfigValue assignRuleConfigValue = new NoConfigValue
                    {
                        NumberRuleId = rule.Id
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssignRule, NoConfig, NoConfigValue>(assignRuleConfigValue);
                }
                else if (rule.Code == "上架规则单号生成规则".L10N())
                {
                    NoConfigValue onShelvesRuleNoConfigValue = new NoConfigValue
                    {
                        NumberRuleId = rule.Id
                    };
                    RT.Service.Resolve<ConfigExtController>().InitModuleConfig<OnShelvesRule, NoConfig, NoConfigValue>(onShelvesRuleNoConfigValue);
                }

            }
        }
    }
}
