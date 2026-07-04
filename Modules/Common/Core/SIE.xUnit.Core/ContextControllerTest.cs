using SIE.Common.Configs;
using SIE.Common.Employees;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Core;
using SIE.Core.Common.Controllers;
using SIE.DataPortal;
using SIE.Domain;
using System;
using System.IO;
using System.Linq;

namespace SIE.xUnit.Core
{
    /// <summary>
    /// 上下文控制器
    /// </summary> 
    public class ContextControllerTest : DomainController
    {
        public virtual void InitContext()
        {
            var config = RT.Config.Get<TestContext>("Context");
            RT.InvOrg = config.InvOrg;
            var emp = RT.Service.Resolve<CommonController>().GetData<Employee>(p => p.Name == config.EmployeeName);
            RT.Principal = new DataPortalPrincipal(emp.Id, emp.UserId.Value, config.EmployeeName);
        }

        /// <summary>
        /// 创建编码规则
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual NumberRule CreateNumberRule(string name)
        {
            var ctl = RT.Service.Resolve<NumberRuleController>();
            var rule = ctl.GetNumberRule(name);
            if (rule == null)
            {
                var segments = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法");
                if (segments == null || segments.Count <= 0)
                {
                    ctl.InitNumberSegment();
                    var tempSegments = RF.GetAll<NumberSegment>();
                    ctl.SetNumberSegmentStatus(tempSegments.Select(p => p.Id).ToArray(), StatusType.Enable);
                }

                rule = new NumberRule();
                rule.Code = name;
                rule.Name = name;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法").FirstOrDefault();
                ruleDtl.Regular = "1";
                ruleDtl.Length = 4;
                ruleDtl.Config = @"{""ContString"":""TEST""}";
                rule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = ctl.GetNumberSegment(StatusType.Enable, "时间编码算法").FirstOrDefault();
                ruleDtl1.Regular = "1";
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                rule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = ctl.GetNumberSegment(StatusType.Enable, "序列生成算法(区分当天日期)").FirstOrDefault();
                ruleDtl2.Regular = "1";
                ruleDtl2.Length = 4;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                rule.DetailList.Add(ruleDtl2);
                RF.Save(rule);

            }

            return rule;
        }

        /// <summary>
        /// 创建编码规则
        /// </summary>
        /// <param name="name"></param>
        /// <param name="FixedCode"></param>
        /// <returns></returns>
        public virtual NumberRule CreateNumberRule(string name, string FixedCode)
        {
            var ctl = RT.Service.Resolve<NumberRuleController>();
            var rule = ctl.GetNumberRule(name);
            if (rule == null)
            {
                var segments = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法");
                if (segments == null || segments.Count <= 0)
                {
                    ctl.InitNumberSegment();
                    var tempSegments = RF.GetAll<NumberSegment>();
                    ctl.SetNumberSegmentStatus(tempSegments.Select(p => p.Id).ToArray(), StatusType.Enable);
                }

                rule = new NumberRule();
                rule.Code = name;
                rule.Name = name;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法").FirstOrDefault();
                ruleDtl.Regular = "1";
                ruleDtl.Length = FixedCode.Length;
                ruleDtl.Config = @"{""ContString"":""{0}""}".FormatArgs(FixedCode);
                rule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = ctl.GetNumberSegment(StatusType.Enable, "时间编码算法").FirstOrDefault();
                ruleDtl1.Regular = "1";
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                rule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = ctl.GetNumberSegment(StatusType.Enable, "序列生成算法(区分当天日期)").FirstOrDefault();
                ruleDtl2.Regular = "1";
                ruleDtl2.Length = 8;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                rule.DetailList.Add(ruleDtl2);
                RF.Save(rule);

            }

            return rule;
        }


        /// <summary>
        /// 创建编码规则（条码打印用，需要序列很大的编码规则）
        /// </summary>
        /// <param name="name">编码规则名称</param>
        /// <returns>编码规则</returns>
        public virtual NumberRule CreateLongNumberRule(string name)
        {
            var ctl = RT.Service.Resolve<NumberRuleController>();
            var rule = ctl.GetNumberRule(name);
            if (rule == null)
            {
                var segments = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法");
                if (segments == null || segments.Count <= 0)
                {
                    ctl.InitNumberSegment();
                    var tempSegments = RF.GetAll<NumberSegment>();
                    ctl.SetNumberSegmentStatus(tempSegments.Select(p => p.Id).ToArray(), StatusType.Enable);
                }
                rule = new NumberRule();
                rule.Code = name;
                rule.Name = name;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = ctl.GetNumberSegment(StatusType.Enable, "固定编码算法").FirstOrDefault();
                ruleDtl.Regular = "1";
                ruleDtl.Length = 2;
                ruleDtl.Config = @"{""ContString"":""SN""}";
                rule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = ctl.GetNumberSegment(StatusType.Enable, "时间编码算法").FirstOrDefault();
                ruleDtl1.Regular = "1";
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                rule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = ctl.GetNumberSegment(StatusType.Enable, "序列生成算法(区分当天日期)").FirstOrDefault();
                ruleDtl2.Regular = "1";
                ruleDtl2.Length = 13;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                rule.DetailList.Add(ruleDtl2);
                RF.Save(rule);
            }
            return rule;
        }

        public virtual void CreatePrintTemplate(NumberRule numberRule, string fileName, string entityType)
        {
            using (var tran = DB.TransactionScope(CoreEntityDataProvider.ConnectionStringName))
            {
                var templates = RT.Service.Resolve<PrintsController>().GetPrintTemplates(entityType, true, numberRule.Id);
                if (!templates.Any(p => p.FileName == fileName))
                {
                    //本地模板文件
                    var fileFullName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Templates\\{fileName}");
                    if (!File.Exists(fileFullName))
                        return;
                    var labelTemplate = new PrintTemplate()
                    {
                        State = State.Enable,
                        FileName = fileName,
                        Type = Path.GetExtension(fileName),
                        FilePath = $"PrintTemplates/{Guid.NewGuid()}/{fileName}",
                        EntityType = entityType,
                        Content = File.ReadAllBytes(fileFullName),
                        PrintType = PrintType.Label
                    };
                    labelTemplate.GenerateId();
                    var ruleTemplate = new NumberRuleInTemplate()
                    {
                        Rule = numberRule,
                        Template = labelTemplate
                    };
                    RF.Save(labelTemplate);
                    RF.Save(ruleTemplate);
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 创建配置项
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual SIE.Common.Configs.Config CreateConfig(string entityType, string typeName, string value)
        {
            var ctl = RT.Service.Resolve<ConfigController>();

            var config = ctl.Get(typeName, entityType);
            if (config == null)
            {
                config = new SIE.Common.Configs.Config();
                config.EntityType = entityType;
                config.TypeName = typeName;

                var dtl = new ConfigDetail();
                dtl.Category = "C";
                dtl.Value = value;
                config.ConfigDetailList.Add(dtl);

                RF.Save(config);
            }
            else
            {
                config.ConfigDetailList.FirstOrDefault().Value = value;
                RF.Save(config);
            }
            return config;
        }

        public virtual Config CreateGlobalConfig(string typeName, string value)
        {
            var ctl = RT.Service.Resolve<ConfigController>();

            var config = ctl.Get(typeName);
            if (config == null)
            {
                config = new Config();
                config.EntityType = "G";
                config.TypeName = typeName;

                var dtl = new ConfigDetail();
                dtl.Value = value;
                config.ConfigDetailList.Add(dtl);

                RF.Save(config);
            }
            else
            {
                config.ConfigDetailList.FirstOrDefault().Value = value;
                RF.Save(config);
            }
            return config;
        }
    }

    public class TestContext
    {
        public int InvOrg { get; set; }
        public string EmployeeName { get; set; }
    }
}