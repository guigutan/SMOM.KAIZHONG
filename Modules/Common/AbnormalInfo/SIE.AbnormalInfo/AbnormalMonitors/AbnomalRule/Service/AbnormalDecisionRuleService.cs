
using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalMonitors.Dao;
using SIE.AbnormalInfo.AbnormalMonitors.SimpleCalculator;
using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Service
{
    /// <summary>
    /// 异常判定规则控制类
    /// </summary>
    public class AbnormalDecisionRuleService : DomainService
    {
        private readonly AbnormalDecisionRuleDao _abnormalDecisionRuleDao;

        private readonly CollectionService _collectionService;

        private readonly List<string> opra = new List<string> { "+", "-", "*", "/", "(", ")" };
        /// <summary>
        /// 计算字典
        /// </summary>
        private readonly Dictionary<string, string> DataDictionary = new Dictionary<string, string>();
        /// <summary>
        /// AbnormalDecisionRuleDao
        /// </summary>
        public  AbnormalDecisionRuleService(AbnormalDecisionRuleDao abnormalDecisionRuleDao, CollectionService collectionService)
        {
            _abnormalDecisionRuleDao = abnormalDecisionRuleDao;
            _collectionService = collectionService;
            if (DataDictionary.Count <= 0)
            {
                DataDictionary["SysDate"] = "系统当前时间";
                DataDictionary["Total"] = "当前数据集总数";
            }
        }

        /// <summary>
        /// 获取异常判定规则编码配置项的值
        /// </summary>
        /// <returns></returns>
        public virtual string GetAbnormalDecisionRuleCode()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AbnormalDecisionRule));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到异常判定规则编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule, 1)
                .FirstOrDefault();
        }


        /// <summary>
        /// 异常判定规则初始化
        /// </summary>
        /// <returns></returns>
        public virtual bool Initialization() {
            return true;// _abnormalDecisionRuleDao.Initialization();
        }

        #region 判异规则明细
        /// <summary>
        /// 获取指标计算编码
        /// </summary>
        /// <returns></returns>
        public virtual string GetInitialzationCodes()
        {
            return _abnormalDecisionRuleDao.GetInitialzationCodes();
        }

        #region 数据保存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalDecisionRule"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void Save(AbnormalDecisionRule abnormalDecisionRule)
        {
            var newList = JsonConvert.DeserializeObject<List<AbnormalRuleTabRelation>>(abnormalDecisionRule.TabRelations);
            var list = abnormalDecisionRule.TabRelationList;
            newList?.ForEach(tab => {
                var data = list.FirstOrDefault(c => c.TabName == tab.TabName);
                if (data != null)
                {
                    tab.PersistenceStatus = PersistenceStatus.Modified;
                    data.parentTabName = tab.parentTabName;
                    data.RefPColumnName = tab.RefPColumnName;
                    data.SuperRefColumnName = tab.SuperRefColumnName;
                    data.TabType = tab.TabType;
                }
                else
                {
                    tab.PersistenceStatus = PersistenceStatus.New;
                    tab.GenerateId();
                    list.Add(tab);
                }
            });
            _abnormalDecisionRuleDao.Save(abnormalDecisionRule);
        }

        private string ValidateRuleExpress(AbnormalDecisionRule entity)
        {
            entity.GetAllChildData<AbnormalDecisionRule, IndicatorCondition>();
            var calcuExpress = AbnormalRuleSqlHelper.DecomposeExpression(entity.IndicatorOperation);
            return calcuExpress
                .FirstOrDefault(c => !opra.Any(p => p == c) //是否操作符
                && !entity.IndicatorCondtionList.Any(conditio => conditio.Code == c) && !double.TryParse(c, out double val)//是否指标条件
                && !DataDictionary.Keys.Contains(c));//是否计算字典
        }
        #endregion

        #region 判异规则测试场景
        /// <summary>
        /// 多表组合
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual (bool, string) AbnomalRuleMultTest(double id)
        {
            StringBuilder msg = new StringBuilder();
            var entity = _abnormalDecisionRuleDao.GetAbnormalDecisionRule(id);
            var code = ValidateRuleExpress(entity);
            if (!string.IsNullOrEmpty(code))
            {
                throw new ValidationException($"指标运算表达式{entity.IndicatorOperation}中的计算标识{code}无效！，".L10N());
            }
            try
            {
                string sql = string.Empty;
                //启用sql脚本
                if (entity.IsSQL)
                {
                    sql = entity.DisPlaySelect;
                }
                else
                {
                    sql = GeneralSqlByWhereCondition(entity);
                }
                var dt = _collectionService.QueryData(sql, entity.RuleName);
                msg.Append($"查询数据源:[{dt.Rows.Count}]-- \r\n".L10N());
                var calcuExpress = AbnormalRuleSqlHelper.DecomposeExpression(entity.IndicatorOperation);
                //是否分组
                var isGroup = entity.LayerConditionsList.Any(conditio => conditio.IsGroup);
                if (isGroup)
                {
                    var groups = GetGoupDatas(dt, entity);
                    Dictionary<string, bool> calculatorResult = new Dictionary<string, bool>();
                    msg.Append($"数据分组数:[{groups.Count()}]--".L10N()+ "\r\n");
                    foreach (var group in groups)
                    {
                        // 执行指标运算
                        var calcuResult = AnalyzeCalculatoruExpress(entity, calcuExpress, group.Rows, msg);
                        var compare = CheckIndicatorCalcuResult(entity, calcuResult);
                        calculatorResult.Add(group.Key.GroupFields, compare);
                    }
                    var abnormalCount = calculatorResult.Count(c => c.Value);
                    msg.Append($"运算结果:异常组数[{abnormalCount}]".L10N()+ "\r\n");
                    return (abnormalCount > 0, msg.ToString());
                }
                else {
                    // 执行指标运算
                    var result = AnalyzeCalculatoruExpress(entity, calcuExpress, dt.AsEnumerable(), msg);
                    var abnormalCount = CheckIndicatorCalcuResult(entity, result);
                    msg.Append($"运算结果:异常数据[{abnormalCount}]".L10N()+ "\r\n");
                    // 检验异常执行结果
                    return (abnormalCount > 0, msg.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("异常判异规则[{0}]执行信息{1}。".L10nFormat(entity.RuleName,msg)+"\r\n"+"异常信息:{0}".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 生成默认Sql
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GenerenDefaultSql(double id) {
            var entity = _abnormalDecisionRuleDao.GetAbnormalDecisionRule(id);
            return GeneralSqlByWhereCondition(entity);
        }

        /// <summary>
        /// 根据层别 & where条件，生成sql
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GeneralSqlByWhereCondition(AbnormalDecisionRule entity)
        {
            var type = Type.GetType(entity.MonitorType);
            var mainRep = RepositoryFactoryHost.Factory.FindByEntity(type) as EntityRepository;
            //获取查询列
            var abTab = mainRep.TableInfo.TabelName;
            var selects = entity.LayerConditionsList.Select(c => {
                return $"{c.PropTabName}.{c.LayerColumn} AS {GetColumnAsName(c)}";
            }).ToList();

            var select = AbnormalRuleSqlHelper.GetTabSelect(selects);
            //2.0 组装where条件
            var whereList = AddWhere(abTab, entity, mainRep);
            //Join
            var joins = AbnormalRuleSqlHelper.GenerateJoinSql(entity.TabRelationList, abTab);
            var sql = $"{select} \r\n" +
                $"FROM {abTab} \r\n" +
                $"{joins}" +
                $"WHERE {whereList}";
            //剔除换行符
            sql = sql.Trim();
            return sql;
        }

        /// <summary>
        /// 添加where条件
        /// </summary>
        /// <param name="abTab"></param>
        /// <param name="entity"></param>
        /// <param name="mainRep"></param>
        /// <returns></returns>
        private string AddWhere(string abTab, AbnormalDecisionRule entity, EntityRepository mainRep) {
            var whereList = new List<string>();
            whereList.Add($"{abTab}.IS_PHANTOM = 0");
            whereList.Add($"AND {abTab}.INV_ORG_ID = {RT.InvOrg}");
            var count = entity.WhereList.Count();
            var i = 0;
            if (entity.WhereList.Count > 0)
            {
                var dynamicWhere = string.Join(" ", entity.WhereList
                    .Select(item =>
                    {
                        var layer = entity.LayerConditionsList.FirstOrDefault(c => c.Id == item.LayerConditionId);
                        if (layer != null)
                        {
                            IPersistanceColumnInfo column;
                            var tempTabName = "";
                            //当前表数据
                            if (layer.PropTabName == abTab)
                            {
                                column = mainRep.TableInfo.Columns.FirstOrDefault(c => c.Name == layer.LayerColumn);
                                tempTabName = abTab;
                            }
                            //关联表数据
                            else
                            {
                                var tab = entity.TabRelationList.FirstOrDefault(c => c.TabName == layer.PropTabName);
                                var rep = _collectionService.GetAnomalyMonitorRepository(tab?.TabType);
                                column = rep.TableInfo.Columns.FirstOrDefault(c => c.Name == layer.LayerColumn);
                                tempTabName = rep.TableInfo.TabelName;
                            }
                            if (column != null)
                            {
                                var propType = column.Property.PropertyType;
                                layer.PropName = column.Property.Name;
                                if (propType.IsEnum)
                                {
                                    layer.Value1 = AbnormalRuleSqlHelper.GetEnumByType(propType, layer.Value1).ToString();
                                }
                                i++;
                                if (count <= 1 || i == count)
                                    return AbnormalRuleSqlHelper.AddWhere(layer.LayerColumn, layer.FieldProp, layer.Value1, layer.Value2, tempTabName, null);
                                else
                                    return AbnormalRuleSqlHelper.AddWhere(layer.LayerColumn, layer.FieldProp, layer.Value1, layer.Value2, tempTabName, item.LogicOpt);
                            }
                        }
                        return null;
                    })
                    .Where(s => !string.IsNullOrEmpty(s)));
                whereList.Add($"AND {dynamicWhere}");
            }
            return string.Join("\r\n", whereList);
        }

        /// <summary>
        /// 数据分组
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        private IEnumerable<dynamic> GetGoupDatas(DataTable dataTable, AbnormalDecisionRule rule)
        {
            //分组计算
            var groupFields = rule.LayerConditionsList.Where(c => c.IsGroup).Select(c =>
            {
                return GetColumnAsName(c);
            }).ToList();
            return AbnormalRuleSqlHelper.DataGoup(dataTable, groupFields);
        }


        /// <summary>
        /// 查询字段 AsName
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private string GetColumnAsName(LayerConditions layer)
        {
            var field = $"{layer.PropTabName}_{layer.LayerColumn}";
            if (field.Length > 25)
                field = field.Substring(0, 25);
            return field;
        }

        #endregion

        #region 指标计算

        #region 指标-表达式

        /// <summary>
        /// 解析/运算
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="calcuExpress"></param>
        /// <param name="Rows"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private List<double> AnalyzeCalculatoruExpress(AbnormalDecisionRule rule, List<string> calcuExpress, IEnumerable<DataRow> Rows, StringBuilder msg)
        {
            try
            {
                //是否计算每一条数据
                var flag = rule.LayerConditionsList.Any(c =>
             rule.IndicatorCondtionList.Any(p => p.LayerConditionId == c.Id && calcuExpress.Contains(p.Code) && p.IndicatorValue == IndicatorValue.Actual));
                var result = new List<double>();
                var sysDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var count = Rows.Count().ToString();
                if (!flag)
                {
                    for (int j = 0; j < calcuExpress.Count; j++)
                    {
                        var indicatorCondition = rule.IndicatorCondtionList.FirstOrDefault(c => c.Code == calcuExpress[j]);
                        if (indicatorCondition != null)
                        {
                            var layer = rule.LayerConditionsList.FirstOrDefault(c => c.Id == indicatorCondition.LayerConditionId);
                            if (!double.TryParse(calcuExpress[j], out double val))
                                calcuExpress[j] = AssignCalcuExpress(indicatorCondition, layer, Rows, null);
                        }
                        else if (calcuExpress[j] == "SysDate")
                        {
                            calcuExpress[j] = sysDate;
                        }
                        else if (calcuExpress[j] == "Total")
                        {
                            calcuExpress[j] = count;
                        }
                    }
                    var (res, express) = CalculatorExpression(calcuExpress, rule.IndicatorUnit);
                    msg.Append($"指标运算:[{express}]= [{res}] ".L10N());
                    result.Add(res);
                }
                else
                {
                    foreach (DataRow row in Rows)
                    {
                        var tempExpress = new List<string>(calcuExpress);
                        // 处理每一行的数据
                        for (int j = 0; j < tempExpress.Count; j++)
                        {
                            var indicatorCondition = rule.IndicatorCondtionList.FirstOrDefault(c => c.Code == tempExpress[j]);
                            if (indicatorCondition != null)
                            {
                                var layer = rule.LayerConditionsList.FirstOrDefault(c => c.Id == indicatorCondition.LayerConditionId);
                                if (!double.TryParse(tempExpress[j], out double val))
                                    tempExpress[j] = AssignCalcuExpress(indicatorCondition, layer, Rows, row);
                            }
                            else if (tempExpress[j] == "SysDate")
                            {
                                tempExpress[j] = sysDate;
                            }
                            else if (tempExpress[j] == "Total")
                            {
                                tempExpress[j] = count;
                            }
                        }
                        var (res, express) = CalculatorExpression(tempExpress, rule.IndicatorUnit);
                        result.Add(res);
                    }
                    msg.Append($"指标运算:[实际值]计算数据集:[{result.Count}] ".L10N());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ValidationException("[{0}]判异规则，指标运算表达式，计算异常:[{1}]！".L10nFormat(rule.RuleName, ex.Message));
            }
        }

        /// <summary>
        /// 表达式-赋值
        /// </summary>
        /// <param name="indicatorCondition"></param>
        /// <param name="layer"></param>
        /// <param name="Rows"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string AssignCalcuExpress(IndicatorCondition indicatorCondition, LayerConditions layer, IEnumerable<DataRow> Rows, DataRow row)
        {
            if (layer.FieldProp == FieldProp.EnumField)
            {
                layer.Value1 = AbnormalRuleSqlHelper.GetEnumByType(Type.GetType(layer.PropType), layer.Value1).ToString();
            }
            var calcuExpressValue = "";
            if (indicatorCondition.IndicatorValue == IndicatorValue.CountAmount)
            {
                calcuExpressValue = CalculationCount(Rows, layer).ToString();
            }
            else if (indicatorCondition.IndicatorValue == IndicatorValue.LayerVal)
            {
                calcuExpressValue = layer?.Value1;
            }
            else if (indicatorCondition.IndicatorValue == IndicatorValue.Actual)
            {
                var filed = GetColumnAsName(layer);
                calcuExpressValue = row[filed].ToString();
            }
            return calcuExpressValue;
        }


        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="calcuExpress"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        private (double, string) CalculatorExpression(List<string> calcuExpress, IndicatorUnit unit)
        {
            double res = 0;
            string express = string.Empty;
            try
            {
                //时间类的运算，只支持a - b
                if (unit != IndicatorUnit.Number && unit != IndicatorUnit.Scale)
                {
                    if (calcuExpress.Count < 3 || calcuExpress[1] != "-") throw new ValidationException("时间类的运算表达式只能是[a - b]形式！".L10N());
                    var flag1 = DateTime.TryParse(calcuExpress[0], out DateTime number1);
                    var flag2 = DateTime.TryParse(calcuExpress[2], out DateTime number2);
                    //计算时间间隔，只能相减
                    if (!flag1 || !flag2) throw new ValidationException($"时间类的运算表达式，参数{calcuExpress[0]},{calcuExpress[1]}转化失败！".L10N());
                    switch (unit)
                    {
                        case IndicatorUnit.Day:
                            res = (number1 - number2).TotalDays;
                            break;
                        case IndicatorUnit.Hour:
                            res = (number1 - number2).TotalHours;
                            break;
                        case IndicatorUnit.minutes:
                            res = (number1 - number2).TotalMinutes;
                            break;
                    }
                    express = $"{number1}-{number2}";
                }
                else
                {
                    express = string.Join("", calcuExpress);
                    IArithmetricCalculator calculator = ExpressionTreeArithmetricCalculator.Create(express);
                    calculator.TryCalculate(out res);
                }
                return (res, express);
            }
            catch (Exception ex)
            {
                throw new ValidationException($"指标计算异常:计算表达式{express}，计算结果{res}！".L10nFormat());
            }
        }
        #endregion

        #region 指标-比较
        /// <summary>
        /// 检验指标结果
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="calcuValues"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        public virtual int CheckIndicatorCalcuResult(AbnormalDecisionRule rule, List<double> calcuValues)
        {
            var indicatorUnit = rule.IndicatorUnit;
            var flag = calcuValues
                .Select(calcuValue =>
                {
                    if (indicatorUnit == IndicatorUnit.Scale)
                    {
                        calcuValue *= 100;
                    }
                    return CompareIndicator(calcuValue, rule);
                })
                .Count(result => result);

            return flag;
        }

        /// <summary>
        /// 指标比较
        /// </summary>
        /// <param name="value"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool CompareIndicator(double value, AbnormalDecisionRule entity)
        {
            double.TryParse(entity.Value1, out double val);
            double.TryParse(entity.Value2, out double val2);
            switch (entity.Operator)
            {
                case Operator.Less:
                    return value < val;
                case Operator.LessEqual:
                    return value <= val;
                case Operator.Greater:
                    return value > val;
                case Operator.GreaterEqual:
                    return value >= val;
                case Operator.Equal:
                    return value == val;
                case Operator.between:
                    return value >= val && value <= val2;
            }

            return false;
        }

        /// <summary>
        /// 计算某一指标的数量
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        private int CalculationCount(IEnumerable<DataRow> Rows, LayerConditions layer)
        {
            return Rows
                .Count(row => {
                    var filed = GetColumnAsName(layer);
                    var val = row[filed];
                    return val.ToString() == layer?.Value1;
                });
        }

        #endregion

        #endregion

        #endregion

    }

}
