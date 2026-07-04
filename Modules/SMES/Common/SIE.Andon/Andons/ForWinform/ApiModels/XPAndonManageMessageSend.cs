using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    /// <summary>
    /// 安灯管理消息推送子表
    /// </summary>
    [Serializable]
    public class XPAndonManageMessageSend
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 安灯管理Id
        /// </summary>
        public double AndonManageId { get; set; }

        /// <summary>
        /// 安灯管理
        /// </summary>
        //public AndonManage AndonManage { get; set; }

        /// <summary>
        /// 安灯维护消息推送Id
        /// </summary>
        public double AndonMessageSendId { get; set; }

        /// <summary>
        /// 安灯维护消息推送
        /// </summary>
        //public AndonMessageSend AndonMessageSend { get; set; }


        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime MessageSendTime { get; set; }

        /// <summary>
        /// 推送人Id
        /// </summary>
        public double MessageSendPersonId { get; set; }

        /// <summary>
        /// 推送人
        /// </summary>
        //public Employee MessageSendPerson { get; set; }

        /// <summary>
        /// 推送地址
        /// </summary>
        public string MessageSendAddress { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageSendTemplate { get; set; }

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


        /// <summary>
        /// 推送人名称
        /// </summary>
        public string MessageSendPersonName { get; set; }

        public static XPAndonManageMessageSend Gen(AndonManageMessageSend ms)
        {
            return new XPAndonManageMessageSend()
            {
                Id = ms.Id,
                AndonManageId = ms.AndonManageId,
                //AndonManage = ms.AndonManage,
                AndonMessageSendId = ms.AndonMessageSendId,
                //AndonMessageSend = ms.AndonManage,
                MessageSendTime = ms.MessageSendTime,
                //MessageSendPersonId = ms.MessageSendPersonId,
                //MessageSendPerson = ms.MessageSendPerson,
                MessageSendAddress = ms.MessageSendAddress,
                MessageSendTemplate = ms.MessageSendTemplate,
                CreateByName = ms.CreateByName,
                CreateDate = ms.CreateDate,
                UpdateByName = ms.UpdateByName,
                UpdateDate = ms.UpdateDate,
                MessageSendPersonName = ms.MessageSendPerson?.Name
            };
        }
    }
}
