using SIE.Common.NumberRules;
using SIE.Core.Common.Models;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 编码规则扩展类
    /// </summary>
    public class NumberRuleExtController : DomainController
    {
        /// <summary>
        /// 根据编码规则编码返回编码规则列表
        /// </summary>        
        /// <param name="codes">编码规则编码</param>
        /// <returns>返回编码规则集合</returns>
        private EntityList<NumberRule> GetNumberRules(List<string> codes)
        {
            return codes.SplitContains(tempCodes =>
            {
                var query = Query<NumberRule>().Where(p => tempCodes.Contains(p.Code));
                return query.ToList();
            });
        }

        /// <summary>
        /// 根据编码规则名称返回编码规则列表
        /// </summary>        
        /// <param name="codes">编码规则编码</param>
        /// <returns>返回编码规则集合</returns>
        private EntityList<NumberRule> GetNumberRulesByName(List<string> codes)
        {
            return codes.SplitContains(tempCodes =>
            {
                var query = Query<NumberRule>().Where(p => tempCodes.Contains(p.Name));
                return query.ToList();
            });
        }

        /// <summary>
        /// 查询编码段的记录数
        /// </summary>        
        /// <returns>返回编码规则集合</returns>
        public virtual int GetNumberSegmentCount()
        {
            var query = Query<NumberSegment>();
            return query.Count();
        }

        /// <summary>
        /// 查询所有的编码段
        /// </summary>        
        /// <returns>返回编码规则集合</returns>
        private EntityList<NumberSegment> GetAllNumberSegments()
        {
            var query = Query<NumberSegment>();
            return query.ToList();
        }

        /// <summary>
        /// 初始化编码规则
        /// </summary>
        /// <param name="datas"></param>
        public virtual EntityList<NumberRule> InitNumberRule(List<InitNumberData> datas)
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var rules = new EntityList<NumberRule>();

            var ruleCodes = datas.Select(p => p.Code).Distinct().ToList();
            var ruleDic = numberExtCtl.GetNumberRules(ruleCodes).ToDictionary(p => p.Code);
                        
            var ruleDicByName = GetNumberRulesByName(ruleCodes).ToDictionary(p => p.Name);

            //获取一次编码段
            var numberSegments = GetAllNumberSegments();

            if (!numberSegments.Any(x => x.Status == StatusType.Enable))
            {
                InitNumberSegment();

                //重新获取一次编码段
                numberSegments = GetAllNumberSegments();
            }

            var segmentDic = numberSegments.Where(x => x.Status == StatusType.Enable).ToDictionary(p => p.Name);

            foreach (var data in datas)
            {
                if (ruleDic.ContainsKey(data.Code)|| ruleDicByName.ContainsKey(data.Code))
                {
                    continue;
                }

                bool hasNumberSegmentNotFound = false;

                var rule = new NumberRule()
                {
                    Code = data.Code.L10N(),
                    Name = data.Code.L10N(),
                    Type = data.RuleType
                };

                foreach (var dtlData in data.Details)
                {

                    string config = null;
                    int length = 0;
                    switch (dtlData.DetailType)
                    {
                        case DetailType.FixedValue:
                            {
                                config = @"{""ContString"":""{0}""}".FormatArgs(dtlData.FixedValue);
                                length = dtlData.FixedValue.Length;
                                break;
                            }
                        case DetailType.Date:
                            {
                                config = @"{""DateFormat"":{0}}".FormatArgs((int)dtlData.DateFormat);
                                length = dtlData.DateFormat.ToString().Length;
                                break;
                            }
                        case DetailType.TodaySequence:
                            {
                                config = @"{""StartValue"":1,""Step"":1}";
                                length = dtlData.Length;
                                break;
                            }
                        case DetailType.Sequence:
                            {
                                config = @"{""StartValue"":0,""Step"":1,""SystemType"":0,""DisableChars"":""""}";
                                length = dtlData.Length;
                                break;
                            }
                        //case DetailType.InvOrg:
                        //    {
                        //        config = @"{}";
                        //        length = RT.InvOrg.ToString().Length;
                        //        break;
                        //    }
                        default: continue;
                    }

                    if (!segmentDic.TryGetValue(dtlData.DetailType.ToLabel(), out NumberSegment segment))
                    {
                        hasNumberSegmentNotFound = true;
                        break;
                    }

                    var dtl = new NumberRuleDetail()
                    {
                        Config = config,
                        Length = length,
                        SegmentId = segment.Id,
                    };

                    rule.DetailList.Add(dtl);
                }

                if (!hasNumberSegmentNotFound)
                {
                    rules.Add(rule);
                }
            }

            if (rules.Any())
            {
                RF.Save(rules);
            }

            return rules;
        }

        /// <summary>
        /// 创建常用单号编码规则，例：BR+YYMMDD+3位流水号
        /// </summary>
        /// <param name="code">编码规则编码,例："备件入库单号生成规则"</param>
        /// <param name="fixedValue">固定值,例:"BR"</param>
        /// <param name="dateFormat">日期格式,例：DateFormat.yyMMdd</param>
        /// <param name="todaySequenceLength">序列长度,例：3</param>
        /// <param name="ruleType">规则类型,默认：通用</param>
        /// <returns></returns>
        public virtual NumberRule CreateFormNoNumberRule(string code, string fixedValue, SIE.Common.Algorithm.DateFormat? dateFormat,
            int todaySequenceLength, RuleType ruleType = RuleType.Common)
        {
            if (fixedValue.IsNullOrEmpty())
            {
                return null;
            }

            List<InitNumberData> datas = new List<InitNumberData>();
            var initNumberData = new InitNumberData()
            {
                Code = code,
                RuleType = ruleType
            };

            //固定值：BR+YYMMDD+3位流水号
            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.FixedValue,
                FixedValue = fixedValue,
                Length = fixedValue.Length
            });

            //当不没有日期格式时，不生成日期格式编码明细
            if (dateFormat.HasValue)
            {
                int datePartLength = 6;
                switch (dateFormat)
                {
                    case SIE.Common.Algorithm.DateFormat.yyyyMMdd:
                        datePartLength = 8;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yyMMdd:
                        datePartLength = 6;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yMMdd:
                        datePartLength = 5;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yyyyMM:
                        datePartLength = 6;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yyMM:
                        datePartLength = 4;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yyyyWW:
                        datePartLength = 6;
                        break;
                    case SIE.Common.Algorithm.DateFormat.yyWW:
                        datePartLength = 4;
                        break;
                    case SIE.Common.Algorithm.DateFormat.MMdd:
                        datePartLength = 4;
                        break;
                    case SIE.Common.Algorithm.DateFormat.MM:
                        datePartLength = 2;
                        break;
                    case SIE.Common.Algorithm.DateFormat.dd:
                        datePartLength = 2;
                        break;
                    default:
                        break;
                }

                initNumberData.Details.Add(new InitNumberDetailData()
                {
                    DetailType = DetailType.Date,
                    DateFormat = dateFormat.Value,
                    Length = datePartLength
                });
            }

            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.TodaySequence,
                Length = todaySequenceLength,
            });

            datas.Add(initNumberData);
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRules = numberExtCtl.InitNumberRule(datas);
            return numberRules.FirstOrDefault();
        }


        /// <summary>
        /// 创建常用单号编码规则，例：BR+3位流水号
        /// </summary>
        /// <param name="code">编码规则编码,例："备件入库单号生成规则"</param>
        /// <param name="fixedValue">固定值,例:"BR"</param>
        /// <param name="todaySequenceLength">序列长度,例：3</param>
        /// <returns></returns>
        public virtual NumberRule CreateFormNoNumberRule(string code, string fixedValue, int todaySequenceLength)
        {
            if (fixedValue.IsNullOrEmpty())
            {
                return null;
            }

            List<InitNumberData> datas = new List<InitNumberData>();
            var initNumberData = new InitNumberData()
            {
                Code = code,
            };

            //固定值：BR+3位流水号
            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.FixedValue,
                FixedValue = fixedValue,
                Length = fixedValue.Length
            });

            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.Sequence,
                Length = todaySequenceLength,
            });

            datas.Add(initNumberData);
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRules = numberExtCtl.InitNumberRule(datas);
            return numberRules.FirstOrDefault();
        }

        /// <summary>
        /// 初始编码段算法
        /// </summary>
        private void InitNumberSegment()
        {
            //编码段不存在，则初始化编码段
            var nrcontrol = RT.Service.Resolve<NumberRuleController>();
            nrcontrol.InitNumberSegment();

            //更新所有编码段为启用
            DB.Update<NumberSegment>()
                .Set(x => x.Status, StatusType.Enable)
                .Execute();
        }
    }
}
