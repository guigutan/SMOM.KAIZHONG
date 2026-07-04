using SIE.Tech.Processs;
using SIE.Tech.VictoryStandards;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Tech.Routings.ApiModels
{
    /// <summary>
    /// 工序明细
    /// </summary>
    [Serializable]
    public class RoutingProcessDetai
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规则ID
        /// </summary>
        public string RuleId
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum
        {
            get;
            set;
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否批次工序
        /// </summary>
        public bool IsBatch
        {
            get;
            set;
        }

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType
        {
            get;
            set;
        }

        /// <summary>
        /// 序列
        /// </summary>
        public int SortOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 返回序列
        /// </summary>
        public int SortOrderBack
        {
            get;
            set;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public ResultTypeForDesign Result
        {
            get;
            set;
        }

        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 工序参数ID
        /// </summary>
        public double ParameterId
        {
            get;
            set;
        }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool? CanChoose
        {
            get;
            set;
        }
        /// <summary>
        /// 是否重复
        /// </summary>
        public bool? IsRepeat
        {
            get;
            set;
        }

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public bool? IsCreateSku
        {
            get;
            set;
        }
        /// <summary>
        /// 是否计产工序
        /// </summary>
        public bool? IsCalculate
        {
            get;
            set;
        }

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsGenerateTask
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool? IsRequirementTask
        {
            get;
            set;
        }
        

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool? IsBuckleMaterial
        {
            get;
            set;
        }

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? NormalVictoryId
        {
            get;
            set;
        }


        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter
        {
            get;
            set;
        }

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime
        {
            get;
            set;
        }

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool? IsPassRate
        {
            get;
            set;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        public bool? IsBinding
        {
            get;
            set;
        }

        /// <summary>
        /// 解绑
        /// </summary>
        public bool? IsUnBinding
        {
            get;
            set;
        }

        /// <summary>
        /// 层级
        /// </summary>
        public int? Level
        {
            get;
            set;
        }

        /// <summary>
        /// 已走过
        /// </summary>
        public bool IsPass
        {
            get;
            set;
        }
        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ActivityId{get;set;}
    }
}
