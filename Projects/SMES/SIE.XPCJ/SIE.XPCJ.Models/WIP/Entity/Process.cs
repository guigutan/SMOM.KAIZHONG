using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    public class Process
    {
        public double Id
        {
            get;
            set;
        }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 引用数量
        /// </summary>
        public int ReferenceTimes
        {
            get;
            set;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public ProcessType? Type
        {
            get;
            set;
        }

        /// <summary>
        /// 产品族小类Id
        /// </summary>
        public double ProductFamilyId
        {
            get;
            set;
        }


        /// <summary>
        /// 产品族小类
        /// </summary>
        public string ProductFamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? SegmentId
        {
            get;
            set;
        }


        /// <summary>
        /// 工段
        /// </summary>
        public string SegmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 启用入站控制
        /// </summary>
        public bool? EnableMoveInControl
        {
            get;
            set;
        }
        /// <summary>
        /// 交接类型
        /// </summary>
        public TransferType? TransferType
        {
            get;
            set;
        }

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool IsOutsourcing
        {
            get;
            set;
        }

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string CategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string ProductFamilyCode
        {
            get;
            set;
        }

        public List<ProcessCollectStep> CollectStepList
        {
            get;
            set;
        }
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<ProcessParameter> ParameterList {
            get;
            set;
        }
    }
}
