using Newtonsoft.Json;
using SIE.Common;
using SIE.Domain;
using SIE.EventMessages.AbnormalInfo;
using SIE.RedCardManagment.RedCards;
using SIE.RedCardManagment.RedCards.Config;
using System;
using System.Linq;

namespace SIE.RedCardManagment.RedcardEvent
{
	/// <summary>
	/// 来料检验单EventBus监听控制器
	/// </summary>
	public class RedcardEventsController : DomainController
    {
		/// <summary>
		/// 发布事件通知-红牌状态该表改变
		/// </summary>
		/// <param name="entity">红牌管理</param>
		/// <param name="list"></param>
		/// <param name="ApplyTime"></param>
		/// <param name="ApplicantName"></param>
		public virtual void OnRedCardStatusChange(RedCard entity, EntityList<ProductRetroactive> list, DateTime? ApplyTime, string ApplicantName)
        {
            //当单据检验结果为不合格时，不回传至WMS。当不合格审核完成后，再回传至WMS
            const string timeFormat = "yyyy-MM-dd HH:mm:ss";
            var products= list.Where(x => x.Status == RedCardState.Enable).GroupBy(c=> new {c.ProductCode,c.WorkNo,c.ApplicantName,c.ApplyTime }).Select(c => new {
                ProductCode=c.Key.ProductCode,
                WorkNo = c.Key.WorkNo,
                ApplicantName = c.Key.ApplicantName,
                ApplyTime = c.Key.ApplyTime
            }).ToList();
            var pubEvent = new PubTaskEvent()
            {
                PubKey = entity.Id.ToString(),
                EntityType=typeof(RedCard).GetQualifiedName(),
                TaskDescription =
                $"红牌单号：{entity.No}\r\n" +
                //$"申请单号：{entity.ApplyBillNo}\r\n" +
                $"物料编码:{entity.Item?.Code}\r\n"+
                $"物料批次：{entity.ItemBatch}\r\n" +
                $"生产周期：{entity.ProductDateStart?.ToString(timeFormat)}---{entity.ProductDateEnd?.ToString(timeFormat)}\r\n" +
                $"供应商：{entity.Supplier?.Name}\r\n" +
                $"红牌启用时间：{ApplyTime?.ToString(timeFormat)}\r\n" +
                $"执行人：{ApplicantName}\r\n" +
                $"[关联产品红牌启用清单]：\r\n" +
                string.Join("\r\n", products.Select(product =>
                {
                    return $"产品编码:{product.ProductCode};工单：{product.WorkNo};启用时间：{product.ApplyTime?.ToString(timeFormat)};执行人：{product.ApplicantName}";
                })
                ),
            };
            try
            {
                RT.EventBus.Publish(pubEvent);
            }
            catch (Exception ex)
            {
                RT.Logger.Error($"红牌发起异常任务失败,异常信息：{ex.Message}".L10N());
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="call"></param>
        public virtual void SysRedcardData(TaskCallEvent call)
        {
            if (call.PubKey.IsNullOrEmpty()) return;
            double.TryParse(call.PubKey, out var value);
            DB.Update<RedCard>().Set(c => c.AbnormalTaskNo, call.TaskNo).Where(c => c.Id == value).Execute();
        }

        /// <summary>
        /// 任务管理-禁用红牌管理
        /// </summary>
        /// <param name="call"></param>
        public virtual void RedcardTaskHandel(TaskhandleEvent call)
        {
            if (typeof(RedCard).GetQualifiedName()!= call.EntityType) return;
            double.TryParse(call.PubKey, out double redCardId);
            var config = JsonConvert.DeserializeObject<RecardTaskExtConfigValue>(call.HandelContent);
            if (null == config) return;
            if (!config.IsDisabled) return;
            RT.Service.Resolve<RedCardService>().DisabledRedCardState(redCardId);
        }

    }
}
