using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 查询关键字
    /// </summary>
    [Serializable]
    public class BaseKeywordData
    {
        /// <summary>
        /// 静态实例化
        /// </summary>
        public static readonly BaseKeywordData Empty = new BaseKeywordData();

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// Lpn
        /// </summary>
        private string lpn = string.Empty;

        /// <summary>
        /// LPN编号
        /// </summary>
        public string LpnCode
        {
            get { return lpn.IsNullOrEmpty() ? "*" : lpn; }
            set { lpn = value; }
        }

        /// <summary>
        /// 货主
        /// </summary>
        private string storerCode = string.Empty;

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return storerCode.IsNullOrEmpty() ? "*" : storerCode; }
            set { storerCode = value; }
        }
        /// <summary>
        /// 项目号
        /// </summary>
        string projectNo = string.Empty;

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectNo
        {
            get { return projectNo.IsNullOrEmpty() ? "*" : projectNo; }
            set { projectNo = value; }
        }

        /// <summary>
        /// 任务号
        /// </summary>
        string taskNo = string.Empty;

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return taskNo.IsNullOrEmpty() ? "*" : taskNo; }
            set { taskNo = value; }
        }

        /// <summary>
        /// 物料编码或物料名称
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState State { get; set; }
    }
}
