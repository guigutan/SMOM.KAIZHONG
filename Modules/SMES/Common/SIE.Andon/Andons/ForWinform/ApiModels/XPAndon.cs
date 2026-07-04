using SIE.Andon.Andons.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 安灯维护
    /// </summary>
    [Serializable]
    public class XPAndon
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode { get; set; }

        /// <summary>
        /// 安灯名称
        /// </summary>
        public string AndonName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 安灯类型Id
        /// </summary>
        public double? AndonTypeId { get; set; }

        /// <summary>
        /// 安灯类型
        /// </summary>
        public AndonType AndonType { get; set; }

        /// <summary>
        /// 安灯大类
        /// </summary>
        public AndonTypeClass AndonClass { get; set; }

        /// <summary>
        /// 通用解决方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SIE.Domain.State State { get; set; }

        /// <summary>
        /// 限制重复触发
        /// </summary>
        public bool RepeatTrigger { get; set; }

        /// <summary>
        /// 停线
        /// </summary>
        public AndonYesOrNo LineStop { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string DefaultType { get; set; }

        /// <summary>
        /// 叫料
        /// </summary>
        public AndonYesOrNo AskMaterial { get; set; }

        /// <summary>
        /// 负责部门Id
        /// </summary>
        public double DepartmentId { get; set; }

        /// <summary>
        /// 负责部门
        /// </summary>
        //public Organization Department { get; set; }

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double ChargerId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        //public Employee Charger { get; set; }

        /// <summary>
        /// 消息推送
        /// </summary>
        //public List<AndonMessageSend> MessageSendList { get; set; }

        #region 视图属性

        /// <summary>
        /// 负责部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName { get; set; }

        #endregion

        public static XPAndon Gen(Andon andon)
        {
            return new XPAndon()
            {
                Id = andon.Id,
                AndonCode = andon.AndonCode,
                AndonName = andon.AndonName,
                Desc = andon.Desc,
                AndonTypeId = andon.AndonTypeId,
                AndonType = andon.AndonType,
                AndonClass = andon.AndonClass,
                Solution = andon.Solution,
                Priority = andon.Priority,
                OrderNo = andon.OrderNo,
                State = andon.State,
                RepeatTrigger = andon.RepeatTrigger,
                LineStop = andon.LineStop,
                DefaultType = andon.DefaultType,
                AskMaterial = andon.AskMaterial,
                DepartmentId = (double)andon.DepartmentId,
                //Department = andon.Department,
                ChargerId = (double)andon.ChargerId,
                //Charger = andon.Charger,
                //MessageSendList = andon.MessageSendList,
                DepartmentName = andon.DepartmentName,
                AndonTypeName = andon.AndonTypeName
            };
        }
    }
}
