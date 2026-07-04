using Elsa.Services.Models;
using SIE.Core.Common.IService;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.RedCardManagment.RedCards;
using SIE.RedCardManagment.WorkFlow.Activities.RedCardApplyBill;
using SIE.RedCardManagment.WorkFlow.CategoryConfig;
using SIE.RedCardManagment.WorkFlow.Variables;
using SIE.Resources.Employees;
using SIE.WorkFlow.Base.Common.Interfaces;
using SIE.WorkFlow.Base.FlowDefinitions;
using SIE.WorkFlow.Core;
using SIE.WorkFlow.Models;
using SIE.WorkFlow.WorkFlowInstances;
using System;
using System.Linq;
using System.Text;

namespace SIE.RedCardManagment.WorkFlow
{
    /// <summary>
    /// 红牌申请-流程-控制器
    /// </summary>
    public class RedCardApplyBillWorkflowController : DomainController
    {
        private readonly FlowInstanceController _flowInstanceController;
        private readonly FlowDefinitionController _flowDefinitionController;
        private readonly ISieWorkflow _sieWorkflow;
        private readonly IRepositoryFactoryService _rfs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowInstanceController"></param>
        /// <param name="flowDefinitionController"></param>
        /// <param name="sieWorkflow"></param>
        /// <param name="rfs"></param>
        public RedCardApplyBillWorkflowController(FlowInstanceController flowInstanceController, FlowDefinitionController flowDefinitionController, ISieWorkflow sieWorkflow, IRepositoryFactoryService rfs)
        {
            _flowInstanceController = flowInstanceController;
            _flowDefinitionController = flowDefinitionController;
            _sieWorkflow = sieWorkflow;
            _rfs = rfs;
        }

        #region 查询

        /// <summary>
        /// 是否能匹配到流程定义
        /// </summary>
        /// <returns></returns>
        public virtual bool HasRedCardApplyWorkflowDefinition(int applySource)
        {
            var flowDefinition = GetWorkflowDefinition();
            if (flowDefinition == null)
                return false;
            else
                return true;

        }

        #endregion

        #region 撤回工作流（撤回至首节点）

        /// <summary>
        ///  撤回工作流（撤回至首节点）
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual void WithDrawWorkflow(double billId, string reason)
        {
            var bill = _rfs.GetById<RedCardApplyBill>(billId);
            if (bill == null)
                throw new ValidationException("申请单不存在".L10N());
            if (bill.BillStatus == BillStatus.Done)
                throw new ValidationException("申请单已完成，无法撤回流程！".L10N());
            if (bill.FlowInstanceId == null)
                throw new ValidationException("工作流实例不存在。无法撤回".L10N());
            if (bill.WorkFlowFirstActivityId.IsNullOrEmpty())
                throw new ValidationException("未记录流程首节点Id，无法撤回流程".L10N());
            //流程实例或任务那边的撤销处理
            _sieWorkflow.CancelWorkflow(bill.FlowInstanceId.Value, reason);
            using (var tran = DB.TransactionScope(RedCardWorkFlowEntityDataProvider.ConnectionStringName))
            {
                bill.BillStatus = BillStatus.ToDo;
                bill.WithDrawReason = reason;
                bill.FlowInstanceId = null;
                bill.WorkFlowFirstActivityId = null;
                if (bill.PersistenceStatus == PersistenceStatus.Unchanged)
                    bill.PersistenceStatus = PersistenceStatus.Modified;
                _rfs.Save(bill);
                tran.Complete(); //提交事务
            }
        }

        #endregion

        #region 启动工作流（全新或恢复）


        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public virtual void StartWorkflow(RedCardApplyBill bill)
        {

            ValidationBill(bill, true);

            using (var tran = DB.TransactionScope(RedCardManagmentDataProvider.ConnectionStringName))
            {
                bill.WorkflowStarterId = RT.IdentityId;
                bill.BillStatus = BillStatus.Doing;
                RF.Save(bill);
                tran.Complete(); //提交事务

            }

            if (bill.FlowInstanceId.HasValue)
            {
                //单据已被驳回，恢复工作流
                ResumeWorkflow(bill);
            }
            else
            {   //启动新工作流
                NewWorkflow(bill);
            }
        }

        private void ValidationBill(RedCardApplyBill bill, bool isStart = false)
        {
            StringBuilder errorMsg = new StringBuilder();
            if (!bill.ItemId.HasValue)
                errorMsg.AppendLine("物料不能为空".L10N());

            if (bill.ProblemDescription.IsNullOrEmpty())
                errorMsg.AppendLine("问题描述不能为空".L10N());

            if (bill.ProblemDescription.Length > 3000)
                errorMsg.AppendLine("问题描述长度不能超过3000字符".L10N());

            if (isStart)
            {
                if ((bill.ProductDateStart == null && bill.ProductDateEnd == null) && bill.JoinBarcodes.IsNullOrEmpty() && bill.JoinProductBatchs.IsNullOrEmpty())
                    errorMsg.AppendLine("追溯条件不能为空".L10N());

                if (bill.ProductDateStart == null && bill.ProductDateEnd != null)
                {
                    errorMsg.AppendLine("生产周期起始时间不能为空".L10N());
                }

                if (bill.ProductDateStart != null && bill.ProductDateEnd == null)
                {
                    errorMsg.AppendLine("生产周期截止时间不能为空".L10N());
                }

                if (bill.ProductDateStart.HasValue && bill.ProductDateEnd.HasValue && bill.ProductDateStart >= bill.ProductDateEnd)
                    errorMsg.AppendLine("生产周期开始日期不能大于或等于结束日期".L10N());
                if (bill.BillStatus == BillStatus.ToDo)
                {
                    var hasDefinition = HasRedCardApplyWorkflowDefinition((int)bill.ApplySource);
                    if (!hasDefinition)
                        errorMsg.AppendLine("该申请来源无法匹配流程，请先添加正确的流程定义".L10N());
                }
                //检查红牌管理编码生成规则
                RT.Service.Resolve<RedCardService>().CheckRedCardCodeConfig();

            }
            if (errorMsg.ToString().IsNotEmpty())
                throw new ValidationException(errorMsg.ToString());
        }



        /// <summary>
        /// 恢复工作流
        /// </summary>
        /// <param name="bill"></param>
        public virtual void ResumeWorkflow(RedCardApplyBill bill)
        {

            if (bill.FlowInstanceId == null)
                throw new ValidationException("工作流实例不存在。请撤销流程后重新发起".L10N());
            if (bill.WorkFlowFirstActivityId.IsNullOrEmpty())
                throw new ValidationException("流程不是由正常工作流驳回，节点Id无记录，无法恢复流程".L10N());
            var result = _flowInstanceController.ExecutePendingWorkflowAsync(bill.FlowInstance.WorkflowInstanceId, bill.WorkFlowFirstActivityId, null);
            if (result.Executed && result.WorkflowInstance.Fault != null)
            {
                throw new ValidationException(result.WorkflowInstance.Fault.Message);
            }
        }

        /// <summary>
        /// 开始工作流
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public virtual void NewWorkflow(RedCardApplyBill bill)
        {
            //1.验证此单据是否已存在流程
            string sourceType = typeof(RedCardApplyBill).FullName;
            string sourceId = bill.Id.ToString();
            if (_flowInstanceController.ExitsDoingFlowInstance(sourceType, sourceId))
            {
                throw new ValidationException("申请单已有运行中的工作流，不能重复发起".L10N());
            }

            //2.匹配流程定义
            var flowDefinition = GetWorkflowDefinition();
            if (flowDefinition == null)
                throw new ValidationException("匹配不了[物料红牌申请]的流程定义，请检查".L10nFormat());

            //3.创建工作流实例
            string remark = "【申请单号{0}，物料编码{1}】红牌申请流程".L10nFormat(bill.No, bill.Item.Code);
            double starterId = RT.IdentityId;

            var startableWorkflow = _sieWorkflow.FindStartableWorkflowAsync(flowDefinition.DefinitionId, starterId, flowDefinition.Id, remark, "红牌申请".L10N(), sourceType, sourceId, true);
            if (startableWorkflow == null)
                throw new ValidationException("未找到对应的且已发布的工作流定义，创建实例失败".L10N());

            //4.保存信息
            using (var tran = DB.TransactionScope(RedCardWorkFlowEntityDataProvider.ConnectionStringName))
            {
                bill.FlowInstanceId = startableWorkflow.FlowInstanceId;
                bill.BillStatus = BillStatus.Doing;
                bill.WorkFlowFirstActivityId = startableWorkflow.ActivityId;
                if (bill.PersistenceStatus == PersistenceStatus.Unchanged)  //修改持久化状态，触发保存时的验证规则
                    bill.PersistenceStatus = PersistenceStatus.Modified;
                _rfs.Save(bill);
                tran.Complete(); //提交事务
            }

            //5.开始流程
            RedCardApplyBillActivityInput input = new RedCardApplyBillActivityInput();
            input.Variable = new Variables.RedCardApplyVariable();
            input.Variable.BillId = bill.Id;
            input.Variable.InvOrgId = RT.InvOrg;
            input.Variable.StarterId = starterId;
            input.Variable.FlowInstanceId = startableWorkflow.FlowInstanceId;
            input.Variable.Starter = RF.GetById<Employee>(RT.IdentityId)?.Name;
            _sieWorkflow.TriggerWorkflowAsync(startableWorkflow.InstanceGuid, startableWorkflow.ActivityId, input);
        }

        #endregion

        #region 匹配流程定义

        /// <summary>
        /// 匹配流程定义
        /// </summary>
        /// <returns></returns>
        private FlowDefinition GetWorkflowDefinition()
        {
            //注释：红牌申请流程根据【申请来源】来匹配流程定义。

            FlowDefinition flowDefinition = null;
            //1.获取红牌申请的流程定义。
            var deflist = _flowDefinitionController.GetWorkflowDefinitionsByCategory(RedCardApplyCategory.CategoryName, State.Enable);

            //2.匹配
            //说明：标品只匹配默认流程，如果项目需要精准匹配，可以扩展分类配置之后，在这里写逻辑
            //foreach (var def in deflist)
            //{
            //    if (def.CategoryConfig.IsNullOrEmpty()) continue;
            //    var config = JsonConvert.DeserializeObject<RedCardApplyCategoryConfig>(def.CategoryConfig);
            //    if (config.ApplySource == applySource)
            //    {
            //        flowDefinition = def;
            //        break;
            //    }
            //}
            flowDefinition = deflist.FirstOrDefault();
            return flowDefinition;
        }
        #endregion

        #region 驳回至【申请信息】节点，即首节点

        /// <summary>
        /// 驳回至【申请信息】节点，即首节点
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        public virtual void RejectArriveRedCardApplyBillActivity(ActivityExecutionContext context, RejectArriveInput input)
        {
            //1.获取变量
            RedCardApplyVariable variable = context.GetSieVariable<RedCardApplyVariable>();
            //2.设置运行时的库存组织
            RT.InvOrg = variable.InvOrgId;
            //3.修改单据
            var bill = _rfs.GetById<RedCardApplyBill>(variable.BillId);
            if (bill != null)
            {
                bill.BillStatus = BillStatus.Reject;
                bill.RejectReason = input.Reason;
                _rfs.Save(bill);
            }
        }

        #endregion

        #region 结束节点

        #region 结束节点执行逻辑

        /// <summary>
        /// 结束节点执行逻辑
        /// </summary>
        /// <param name="context"></param>
        public virtual void FinishApply(ActivityExecutionContext context)
        {
            //1.获取变量
            RedCardApplyVariable variable = context.GetSieVariable<RedCardApplyVariable>();
            //2.设置运行时的库存组织
            RT.InvOrg = variable.InvOrgId;

            //3.修改单据
            var bill = _rfs.GetById<RedCardApplyBill>(variable.BillId);
            if (bill != null)
            {
                bill.BillStatus = BillStatus.Done;
                RedCard redCard = RT.Service.Resolve<RedCardService>().GenerateRedCard(bill);
                _rfs.Save(redCard);
                _rfs.Save(bill);
            }

            //4.关闭流程
            _flowInstanceController.FinishFlowInstance(variable.FlowInstanceId);
        }
        #endregion

        #endregion

        #region 订阅[终止工作流]事件的处理

        /// <summary>
        /// 订阅[终止工作流]事件的处理、终止操作在[流程任务]端
        /// </summary>
        /// <param name="e"></param>
        public virtual void CancelFlowInstanceEventHandle(CancelFlowInstanceEvent e)
        {
            var bill = _rfs.GetById<RedCardApplyBill>(e.SourceId);
            if (bill != null)
            {
                bill.BillStatus = BillStatus.ToDo;
                bill.WithDrawReason = e.CancelReason;
                bill.FlowInstanceId = null;
                bill.WorkFlowFirstActivityId = null;
                _rfs.Save(bill);
            }
        }

        #endregion
    }
}
