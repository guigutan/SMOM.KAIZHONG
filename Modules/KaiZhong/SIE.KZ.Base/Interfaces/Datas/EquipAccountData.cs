using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 设备台账信息
    /// </summary>
    [Serializable]
    public class EquipAccountData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string EQUNR { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SHTXT { get; set; }

        /// <summary>
        /// 种类（类型）(M:设备;P:模具)
        /// </summary>
        public string EQTYP { get; set; }

        /// <summary>
        /// 型号/图号
        /// </summary>
        public string TYPBZ { get; set; }

        /// <summary>
        /// 状态()
        /// </summary>
        /*
         * SAP-未投用对应MES-待验收;SAP-在用对应MES-使用中;SAP-闲置对应MES-闲置;SAP-报废对应MES-报废;SAP-盘亏、删除对应MES-已处理;
         E0001 未投用	
        E0002 在用	
        E0003 闲置	
        E0004 盘亏	
        E0005 报废
        E0006 删除
         */
        public string STTXT { get; set; }

        /// <summary>
        /// 购置日期
        /// </summary>
        public string ANSDT { get; set; }

        /// <summary>
        /// 车间
        /// </summary>
        public string INGRP { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string GEWRK { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string HERST { get; set; }

        /// <summary>
        /// 管理部门
        /// </summary>
        public string ZUSER_ID { get; set; }

        /// <summary>
        /// 成本中心代码
        /// </summary>
        public string KOSTL { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string ARBPL { get; set; }

        /// <summary>
        /// 系列号/穴位
        /// </summary>
        public string SERGE { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string SWERK { get; set; }

        /// <summary>
        /// 功能位置
        /// </summary>
        public string TPLNR { get; set; }
    }
}
