using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPLinesideWarehouse
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public string SimpleCode { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFrozen { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType LibraryType { get; set; }

        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        public string SourceKey { get; set; }

        /// <summary>
        /// 是否线边仓
        /// </summary>
        public bool IsLineWarehouse { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

    }
}
