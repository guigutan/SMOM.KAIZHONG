using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.RedCardManagment.RedCardApplyBills.Dao;
using System;
using System.Linq;
using SIE.Domain.Validation;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using System.Text;

namespace SIE.RedCardManagment.RedCardApplyBills.Service
{
    /// <summary>
    /// 红牌申请单 Service
    /// </summary>
    public class RedCardApplyBillService : DomainService
    {
        private readonly RedCardApplyBillDao _redCardApplyBillDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redCardApplyBillDao"></param>
        public RedCardApplyBillService(RedCardApplyBillDao redCardApplyBillDao)
        {
            _redCardApplyBillDao = redCardApplyBillDao;
        }

        /// <summary>
        /// 获取红牌申请单列表
        /// </summary>
        /// <param name="criteria">红牌申请单查询实体</param>
        /// <returns>红牌申请单列表</returns>
        public virtual EntityList<RedCardApplyBill> GetApplyBills(RedCardApplyBillCriteria criteria)
        {
            return _redCardApplyBillDao.GetApplyBills(criteria);
        }

        /// <summary>
        /// 取消申请
        /// </summary>
        /// <param name="redCardApplyBillId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual bool Cancel(double redCardApplyBillId, string reason)
        {
            var bill = _redCardApplyBillDao.GetById(redCardApplyBillId);
            if (bill == null)
                throw new SIE.Domain.Validation.ValidationException("申请单不存在".L10N());
            bill.CancelReason = reason;
            bill.BillStatus = BillStatus.Cancel;
            _redCardApplyBillDao.Save(bill);
            return true;
        }     

        /// <summary>
        /// 生成红牌申请单单号
        /// </summary>
        /// <returns>红牌申请单单号</returns>
        public virtual string GenerateNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(RedCardApplyBill));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到红牌申请单单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 检查红牌申请编码生成规则
        /// </summary>
        public virtual void CheckRedCardApplyCodeConfig()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(RedCardApplyBill));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到红牌申请单单号生成规则,请检查规则配置".L10N());
        }

        /// <summary>
        /// 创建保存单据
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public virtual void SaveBill(RedCardApplyBill bill)
        {
            using (var tran = DB.TransactionScope(RedCardManagmentDataProvider.ConnectionStringName))
            {
                ValidationBill(bill);
                bill.WorkflowStarterId = RT.IdentityId;
                bill.BillStatus = BillStatus.ToDo;
                RF.Save(bill);
                tran.Complete(); //提交事务
            }
        }

        private void ValidationBill(RedCardApplyBill bill)
        {
            StringBuilder errorMsg = new StringBuilder();
            if (!bill.ItemId.HasValue)
                errorMsg.AppendLine("物料不能为空".L10N());

            if (bill.ProblemDescription.IsNullOrEmpty())
                errorMsg.AppendLine("问题描述不能为空".L10N());

            if (bill.ProblemDescription.Length>3000)
                errorMsg.AppendLine("问题描述长度不能超过3000字符".L10N());

            
            if (errorMsg.ToString().IsNotEmpty())
                throw new ValidationException(errorMsg.ToString());
        }

        /// <summary>
        /// 不合格审核生成红牌申请单
        /// </summary>
        /// <param name="starterId"></param>
        /// <param name="itemId"></param>
        /// <param name="SupplierId"></param>
        /// <param name="problemDescription"></param>
        /// <param name="failedAuditWorkflowNo">不合格审核流程单号</param>
        /// <returns></returns>
        public virtual RedCardApplyBill GenerateApplyBillForFailedAudit(double starterId, double itemId, double? SupplierId, string problemDescription,string failedAuditWorkflowNo)
        {
            RedCardApplyBill redCardApplyBill = new RedCardApplyBill();
            redCardApplyBill.No = this.GenerateNo();
            redCardApplyBill.ProblemDescription = problemDescription;
            redCardApplyBill.WorkflowStarterId = starterId;
            redCardApplyBill.ItemId = itemId;
            redCardApplyBill.SupplierId = SupplierId;
            redCardApplyBill.ApplyType = ApplyType.Auto;
            redCardApplyBill.ApplySource = Core.RedCardManagments.ApplySource.DisqualificationAudit;
            redCardApplyBill.ApplySourceNo = failedAuditWorkflowNo;
            redCardApplyBill.BillStatus = BillStatus.ToDo;
            return redCardApplyBill;
        }
    }
}
