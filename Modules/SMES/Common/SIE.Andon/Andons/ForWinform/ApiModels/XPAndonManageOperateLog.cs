using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Resources.Employees;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 安灯管理操作记录
    /// </summary>
    [Serializable]
    public class XPAndonManageOperateLog
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public AndonManageOperateType OperateType { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperaterId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee Operater { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 距离上一次操作时长(小时)
        /// </summary>
        public double LastOperate { get; set; }

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId { get; set; }

        /// <summary>
        /// 安灯管理
        /// </summary>
        public AndonManage AndonManage { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperaterName { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>
        public string UpdateByName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public static XPAndonManageOperateLog Gen(AndonManageOperateLog log)
        {
            return new XPAndonManageOperateLog()
            {
                Id = log.Id,
                OperateTime = log.OperateTime,
                OperateType = log.OperateType,
                OperaterId = log.OperaterId,
                //Operater = log.Operater,
                Remark = log.Remark,
                LastOperate = log.LastOperate,
                AndonManageId = log.AndonManageId,
                //AndonManage = log.AndonManage,
                OperaterName = log.Operater?.Name,
                CreateByName = log.CreateByName,
                CreateDate = log.CreateDate,
                UpdateByName = log.UpdateByName,
                UpdateDate = log.UpdateDate
            };
        }
    }
}
