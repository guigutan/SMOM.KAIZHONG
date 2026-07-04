using SIE.AbnormalInfo.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Dao
{
    /// <summary>
    /// 属性树
    /// </summary>
    [Serializable]
    public class QuerySelectTree
    {
        /// <summary>
        /// 允许拖曳
        /// </summary>
        public bool allowDrag { get; set; }
        /// <summary>
        /// 允许删除
        /// </summary>
        public bool allowDrop { get; set; } = true;

        /// <summary>
        /// 子树
        /// </summary>
        public List<QuerySelectTree> children { get; set; } = new List<QuerySelectTree>();
        /// <summary>
        /// cls
        /// </summary>
        public string cls { get; set; } = "";


        /// <summary>
        /// 能否展开
        /// </summary>
        public bool expandable { get; set; } = true;
        /// <summary>
        /// 展开状态
        /// </summary>
        public bool expanded { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TabName { get; set; }

        /// <summary>
        /// isFirst
        /// </summary>
        public bool isFirst { get; set; } = true;
        /// <summary>
        /// isLast
        /// </summary>
        public bool isLast { get; set; } = true;
        /// <summary>
        /// leaf
        /// </summary>
        public bool leaf { get; set; }
     
        /// <summary>
        /// 是否root
        /// </summary>
        public bool root { get; set; } = true;
        /// <summary>
        /// text
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// visible
        /// </summary>
        public bool visible { get; set; } = true;
        /// <summary>
        /// 字段
        /// </summary>

        public string field { get; set; }

        /// <summary>
        /// 一对多关联字段
        /// </summary>
        public string oneToMoreRelationField { get; set; }

        public string type { get; set; }

        public FieldProp editType { get; set; }

        /// <summary>
        /// 下拉数据源
        /// </summary>
        public List<string> comboxSource { get; set; } = new List<string>();
    }
}
