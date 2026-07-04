using SIE.AbnormalInfo.Common;
using SIE.Configuration;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Service
{
	/// <summary>
	/// Sql辅助类
	/// </summary>
	public static class AbnormalRuleSqlHelper
	{
		#region Sql组装
		/// <summary>
		/// 
		/// </summary>
		/// <param name="columns">查询列</param>
		/// <param name="tabName">表名</param>
		/// <returns></returns>
		public static string GetTabSelect(IEnumerable<string> columns)
		{
			string select = columns.Any() ? string.Join(",\r\n", columns.Distinct().ToList()) : "*";
			return string.Format("SELECT {0}", select);
		}
		#endregion

		#region Where 组装
		public static string AddWhere(string column, FieldProp columnEnum, string value1, string value2, string tabName, LogicalOperator? logicOpt)
		{

			if (columnEnum == FieldProp.Text)
				value1 = $"'{value1}'";
			var opt = logicOpt == LogicalOperator.AND ? "AND" : (logicOpt == LogicalOperator.OR ? "OR" : "");
			//时间格式
			if (columnEnum == FieldProp.DateTime)
			{
				return DateTimeCondition(column, columnEnum, value1, value2, tabName, opt);
			}
			else if (columnEnum == FieldProp.EnumTime)
			{
				var val =GetEnumByType(typeof(DateType), value1);
				DateType dateType = (DateType)val;
				var str = GetTimeByDateType(dateType, column, tabName);
				return $"{str}{opt}";
			}
			else
			{
				return $"{tabName}.{column} = {value1} {opt}";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="column"></param>
		/// <param name="columnEnum"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="tabName"></param>
		/// <param name="opt"></param>
		/// <returns></returns>
		public static string DateTimeCondition(string column, FieldProp columnEnum, string value1, string value2, string tabName, string opt)
		{
			var cfg = RT.Config.GetConnectionString(AbnormalInfoDataProvider.ConnectionStringName);
			if (cfg.ProviderName.Contains("Oracle")|| cfg.ProviderName.Contains("Dm"))
			{
				return $"{tabName}.{column} >= to_date('{value1}' ,'yyyy-mm-dd hh24:mi:ss') AND {tabName}.{column} <= to_date('{value2}','yyyy-mm-dd hh24:mi:ss') {opt}";
			}
			else if (cfg.ProviderName.Contains("SqlServer"))
			{
				return $"{tabName}.{column} >= CONVERT(DATETIME,'{value1}' , 120) AND {tabName}.{column} <= CONVERT(DATETIME,'{value2}',120) {opt}";
			}
            else if (cfg.ProviderName.Contains("MySql"))
            {
                return $"{tabName}.{column} >= '{value1}' AND {tabName}.{column} <= '{value2}'";
            }
            return "";
		}

		public static string GetTimeByDateType(DateType type,string column,  string tabName)
		{
		    string st=string.Empty;
			string end=string.Empty;
			DateTime today = DateTime.Today;
			switch (type)
			{
				case DateType.Day:
					// 获取当天的时间区域
					DateTime startOfDay = today.Date;
					st = startOfDay.ToString("yyyy-MM-dd HH:mm:ss");
					end = startOfDay.AddDays(1).AddTicks(-1).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DateType.Week:
					DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
					st = today.AddDays(-(int)today.DayOfWeek).ToString("yyyy-MM-dd HH:mm:ss");
					end = startOfWeek.AddDays(7).AddTicks(-1).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DateType.Month:
					DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
					st = new DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd HH:mm:ss");
					end = startOfMonth.AddMonths(1).AddTicks(-1).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				case DateType.Year:
					DateTime startOfYear = new DateTime(today.Year, 1, 1);
					st = new DateTime(today.Year, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
					end = startOfYear.AddYears(1).AddTicks(-1).ToString("yyyy-MM-dd HH:mm:ss");
					break;
				default:
					break;
			}
			return $"{tabName}.{column} >= to_date('{st}' ,'yyyy-mm-dd hh24:mi:ss') AND {tabName}.{column} <= to_date('{end}','yyyy-mm-dd hh24:mi:ss')";
		}
		#endregion

		#region Join
		//只支持三级联表
		public static string GenerateJoinSql(IEnumerable<dynamic> tabs, string tab)
		{
			IEnumerable<dynamic> subTabs = tabs.Where(t => t.parentTabName == tab && t.TabName != tab);
			if (!subTabs.Any() || subTabs == null)
			{
				return "";
			}

			var joinSql = string.Join("\r\n", subTabs.Select(t => {
				//一对一：来料引用物料
				if (string.IsNullOrEmpty(t.RefPColumnName))
				{
					return $" LEFT JOIN {t.TabName} ON {tab}.{t.SuperRefColumnName} = {t.TabName}.Id";
				}
				else
				{
					return $" LEFT JOIN {t.TabName} ON {tab}.Id = {t.TabName}.{t.RefPColumnName}";
				}
			}));

			return $" \r\n{joinSql} \r\n{string.Join("\r\n", subTabs.Select(t => GenerateJoinSql(tabs, t.TabName)))}";
		}
		#endregion

		#region Group 分组
		/// <summary>
		/// 数据查询后，分组
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="groupFields"></param>
		/// <returns></returns>
		public static IEnumerable<dynamic> DataGoup(DataTable dataTable, List<string> groupFields)
		{
			var groupedData = dataTable.AsEnumerable()
			.GroupBy(row => new
			{
				GroupFields = string.Join(",", groupFields.Select(field => row[field])) // 将分组字段值连接为字符串
			})
			.Select(group => new
			{
				Key = group.Key,
				Rows = group.ToList<DataRow>()
			});
			return groupedData;
		}
		#endregion

		#region 枚举/字段类型
		/// <summary>
		/// 获取字段的编辑类型
		/// </summary>
		/// <param name="propType"></param>
		/// <returns></returns>
		public static FieldProp GetPropEditType(Type propType)
		{
			FieldProp editType = FieldProp.Text;
			if (propType.IsEnum)
			{
				editType = FieldProp.EnumField;
			}
			else if (propType == typeof(System.DateTime) || propType==typeof(System.DateTime?))
			{
				editType = FieldProp.DateTime;
			}
			else if (propType == typeof(System.Int32) || propType == typeof(System.Double))
			{
				editType = FieldProp.Numerical;
			}
			return editType;

		}

		#region 枚举处理
		public static string GetEnumDescription(Enum value)
		{
			FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
			var attributes = (LabelAttribute[])fieldInfo.GetCustomAttributes(typeof(LabelAttribute), false);
			return attributes.Length > 0 ? attributes[0].Label : value.ToString();
		}

		/// <summary>
		/// 获取枚举值
		/// </summary>
		/// <param name="propType"></param>
		/// <param name="label"></param>
		/// <returns></returns>
		public static int GetEnumByType(Type propType, string label)
		{
			if (propType.IsEnum)
			{
				Array values = Enum.GetValues(propType);
				foreach (var value in values)
				{
					if (label == GetEnumDescription((Enum)value))
					{
						return (int)value;
					}
				}
			}
			return -1;
		}
		#endregion

		#endregion

		#region 运算表达式
		/// <summary>
		/// 分解运算表达式
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>

		public static List<string> DecomposeExpression(string expression)
		{
			var tokens = new List<string>();
			var currentToken = "";
			int i = 0;
			while (i < expression.Length)
			{
				char c = expression[i];
				if (char.IsDigit(c) || char.IsLetter(c))
				{
					currentToken += c;
				}
				else
				{
					if (currentToken != "")
					{
						tokens.Add(currentToken);
						currentToken = "";
					}
					if (c == '+' || c == '-' || c == '*' || c == '/')
					{
						tokens.Add(c.ToString());
					}
					else if (c == '(')
					{
						tokens.Add("(");
						int j = FindMatchingParenthesis(expression, i);
						var subExpression = expression.Substring(i + 1, j - i - 1);
						var subTokens = DecomposeExpression(subExpression);
						tokens.AddRange(subTokens);
						i = j;
						tokens.Add(")");
					}
				}
				i++;
			}
			if (currentToken != "")
			{
				tokens.Add(currentToken);
			}
			return tokens;
		}

		private static int FindMatchingParenthesis(string expression, int startIndex)
		{
			int count = 0;
			for (int i = startIndex; i < expression.Length; i++)
			{
				char c = expression[i];
				if (c == '(')
				{
					count++;
				}
				else if (c == ')')
				{
					count--;
					if (count == 0)
					{
						return i;
					}
				}
			}
			throw new ValidationException("表达式异常，没有找到匹配的括号.".L10N());
		}

		#endregion
	}
}
