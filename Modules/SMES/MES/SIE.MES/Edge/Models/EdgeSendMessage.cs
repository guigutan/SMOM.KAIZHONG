using SIE.MQueue;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 消息定义
    /// </summary>
    public class EdgeSendMessage
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type（1：工单基础数据(工单、工序BOM、包装规则) 2、工单工艺路线 3、工单条码打印和导入 4、工单条码关联 5、包装号）
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public string InvOrg { get; set; }

    }
}
