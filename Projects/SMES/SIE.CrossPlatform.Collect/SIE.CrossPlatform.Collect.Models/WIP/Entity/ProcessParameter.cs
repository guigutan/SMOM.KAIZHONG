using SIE.CrossPlatform.Collect.Models.Attributes;
using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    [Serializable]
    public class ProcessParameter
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultTypeForDesign Type
        {
            get;
            set;
        }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
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
        /// 跳转条件
        /// </summary>
        public string Condition
        {
            get;
            set;
        }
    }
    public enum ResultTypeForDesign
    {
        /// <summary>
        /// 任意
        /// </summary>
        [Label("任意")]
        [Category("Common")]
        Any = ResultType.Pass | ResultType.Fail,

        /// <summary>
        /// 通过
        /// </summary>
        [Label("通过")]
        [Category("Common")]
        Pass = ResultType.Pass,

        /// <summary>
        /// 失败
        /// </summary>
        [Label("失败")]
        [Category("Common")]
        Fail = ResultType.Fail,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        [Category("Custom")]
        Custom = ResultType.Custom,

        /// <summary>
        /// 可选路径
        /// </summary>
        [Label("可选路径")]
        Optional = ResultType.Optional,
    }
}
