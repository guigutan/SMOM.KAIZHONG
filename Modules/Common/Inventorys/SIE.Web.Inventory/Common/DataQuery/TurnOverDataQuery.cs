using SIE.Inventory.Commom;
using SIE.Inventory.Strategy;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.Inventory.Common.DataQuery
{
    /// <summary>
    /// 周转规则变更数据
    /// </summary>
    public class TurnOverRuleChangeData
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField? SortField { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType? FieldType { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string EqualValue { get; set; }

        /// <summary>
        /// 下限天数
        /// </summary>
        public decimal? LowerLimitDay { get; set; }

        /// <summary>
        /// 上限天数
        /// </summary>
        public decimal? UpperLimitDay { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType? SortType { get; set; }
    }

    /// <summary>
    /// 周转规则查询
    /// </summary>
    public class TurnOverDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取周转规则数据
        /// </summary>
        /// <param name="field">排序（数值）</param>
        /// <returns>周转规则数据</returns>
        public TurnOverRuleChangeData GetTurnOverRuleDetailData(string field)
        {
            SortField? sortField = null;
            if (field != null)
            {
                int intSortField = int.Parse(field);
                sortField = (SortField)intSortField;
            }

            string[] strArrName = new string[] { "批次号","生产日期", "失效日期", "收货日期", "生产批次", "批次属性05"
                                                , "批次属性06", "批次属性07", "批次属性08", "批次属性09"
                                                , "批次属性10", "批次属性11", "批次属性12"};
            DataType[] arrDataType = new DataType[] { DataType.Text,DataType.Date, DataType.Date, DataType.Date,DataType.Text,DataType.Numerical,
                                                DataType.Numerical,DataType.Text,DataType.Text,DataType.Text,DataType.Text,DataType.Date,DataType.Date};

            Dictionary<string, DataType> dicSortField = new Dictionary<string, DataType>();
            for (int i = 0; i < strArrName.Length; i++)
            {
                if (!dicSortField.ContainsKey(strArrName[i]))
                {
                    dicSortField.Add(strArrName[i], arrDataType[i]);
                }
            }

            TurnOverRuleChangeData data = new TurnOverRuleChangeData();
            if (sortField != null && dicSortField.Count > 0)
            {
                data.FieldType = dicSortField[sortField.ToLabel()];
                if (data.FieldType.Value == DataType.Date)
                {
                    data.LowerLimit = null;
                    data.UpperLimit = null;
                    data.EqualValue = string.Empty;
                }
                else if (data.FieldType.Value == DataType.Text)
                {
                    data.LowerLimitDay = null;
                    data.UpperLimitDay = null;
                    data.LowerLimit = null;
                    data.UpperLimit = null;
                }
                else if (data.FieldType.Value == DataType.Numerical)
                {
                    data.LowerLimitDay = null;
                    data.UpperLimitDay = null;
                    data.EqualValue = string.Empty;
                }
            }
            else
            {
                data.FieldType = null;
                data.SortType = null;
                data.EqualValue = string.Empty;
                data.LowerLimit = null;
                data.UpperLimit = null;
                data.LowerLimitDay = null;
                data.UpperLimitDay = null;
            }

            return data;
        }
    }
}
