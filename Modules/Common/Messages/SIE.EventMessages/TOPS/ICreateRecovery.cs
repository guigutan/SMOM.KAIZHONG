using SIE.Core.Inspections;
using SIE.EventMessages.PDCA;
using SIE.EventMessages.QMS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.TOPS
{

    /// <summary>
    /// 创建PDCA改善报告
    /// </summary>
    [Services.Service(FallbackType = typeof(CreateRecoveryDefault))]
    public interface ICreateRecovery
    {
        /// <summary>
        /// 生成8D
        /// </summary>
        /// <param name="reportEvent">8D参数</param>
        string GenerateRecovery(CreateRecoveryEvent reportEvent);

        /// <summary>
        /// 检查是否存在8D编码生成规则
        /// </summary>
        /// <returns></returns>
        bool CheckRecoveryCodeConfig();
    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class CreateRecoveryDefault : ICreateRecovery
    {
        public string GenerateRecovery(CreateRecoveryEvent reportEvent)
        {
            throw new NotImplementedException("缺少8D模块，无法创建8D");
        }

        /// <summary>
        /// 检查是否存在8D编码生成规则
        /// </summary>
        /// <returns></returns>
        public bool CheckRecoveryCodeConfig()
        {
            return false;
        }
    }

    /// <summary>
    /// 生成PDCA改善报告参数
    /// </summary>
    [Serializable]
    public class CreateRecoveryEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateRecoveryEvent()
        {
            DefectInfoList = new List<DefectInfo>();
        }

        /// <summary>
        /// 处理人
        /// </summary>
        public double HandlerId { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDescription { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public InspectionType? InspectionType { get; set; }

        /// <summary>
        /// 问题清单列表
        /// </summary>
        public List<DefectInfo> DefectInfoList { get; set; }

        /// <summary>
        /// 来源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 来源Ids
        /// </summary>
        public string SourceIds { get; set; }

        /// <summary>
        /// 改善来源模块
        /// </summary>
        public RecoveryType RecoveryType { get; set; }

    }


}
