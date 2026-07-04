using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.WorkFlow;
using SIE.EMS.WorkFlow.PurchaseRequisition;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 工作流服务-使用内部工作流引擎
    /// </summary>
    public class PurchaseRequisitionInternalWorkFlowService : PurchaseRequisitionWorkFlowService
    {
        /// <summary>
        /// 工作流通用服务
        /// </summary>
        private readonly WorkflowService _workflowService = RT.Service.Resolve<WorkflowService>();

        /// <summary>
        /// 采购申请工作流服务
        /// </summary>
        private readonly PurchaseRequisitionWorkflowService _purchaseRequisitionWorkflowService
            = RT.Service.Resolve<PurchaseRequisitionWorkflowService>();

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        /// <exception cref="ValidationException">资产采购申请的列表为空，提交审核失败</exception>
        protected override void SubmitPurchaseRequisitions(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions == null)
            {
                throw new ValidationException("资产采购申请的列表为空，提交审核失败".L10N());
            }

            bool isError = false;

            foreach (PurchaseRequisition pr in purchaseRequisitions)
            {
                try
                {
                    var workflowInstanceId = CreateWorkFlowInstance(pr);

                    pr.WorkflowInstanceId = workflowInstanceId.ToString();
                    pr.ApprovalStatus = ApprovalStatus.UnderReview;
                    pr.WorkflowStartResult = "成功能创建【资产采购申请】工作流实例".L10N();
                }
                catch (Exception ex)
                {
                    pr.WorkflowStartResult = ex.GetExceptionMessage();
                    isError = true;
                }
            }

            // 保存数据到数据库
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(purchaseRequisitions);

                var now = RF.Find<PurchaseRequisition>().GetDbTime();

                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(purchaseRequisitions.Select(x => x.Id).ToList(),
                    typeof(PurchaseRequisition).FullName, ApprovalResult.Submit, now, "");

                trans.Complete();
            }

            if (isError)
            {
                throw new ValidationException("部份采购申请提交失败，请查询【流程启动结果】。".L10N());
            }
        }

        /// <summary>
        /// 创建内部工作流引擎的实例
        /// </summary>
        /// <param name="pr"></param>
        private double CreateWorkFlowInstance(PurchaseRequisition pr)
        {
            WorkFlowForm workFlowForm = new WorkFlowForm();
            workFlowForm.StarterId = RT.IdentityId;

            CreatePrMainInfo(pr, workFlowForm);

            workFlowForm.WorkFlowCustomLists = new List<WorkFlowCustomList>();

            //采购申请明细列表
            WorkFlowCustomList customList = new WorkFlowCustomList();
            customList.ListName = "采购申请明细列表".L10N();

            //创建采购申请明细的自定义列表的字段列表
            CreatePrDetailFields(customList);

            //创建采购申请明细的自定义列表的值列表
            CreatePrDetailValues(pr, customList);

            workFlowForm.WorkFlowCustomLists.Add(customList);

            //附件
            workFlowForm.Attachments = new List<WorkFlowAttachmentInfoModel>();

            foreach (var attachment in pr.AttachmentList)
            {
                workFlowForm.Attachments.Add(new WorkFlowAttachmentInfoModel()
                {
                    FileName = attachment.FileName,
                    FilePath = attachment.FilePath,
                    FileExtesion = attachment.FileExtesion,
                    FileSize = attachment.FileSize,
                });
            }

            return _purchaseRequisitionWorkflowService
                .CreatePurchaseRequisitionWorkFlow(workFlowForm, pr.PurchaseType, pr.PurchaseObjectType);
        }

        /// <summary>
        /// PR明细
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="customList"></param>
        private void CreatePrDetailValues(PurchaseRequisition pr, WorkFlowCustomList customList)
        {
            customList.CutomListValueList = new List<List<WorkFlowCutomListValue>>();
            foreach (var purchaseRequisitionItem in pr.DetailList)
            {
                List<WorkFlowCutomListValue> workFlowCutomListValues = new List<WorkFlowCutomListValue>();
                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "LineNo",
                    FieldValue = purchaseRequisitionItem.LineNo.ToString()
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "ObjectCode",
                    FieldValue = purchaseRequisitionItem.ObjectCode
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "Description",
                    FieldValue = purchaseRequisitionItem.Description
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "KeyItemDescription",
                    FieldValue = purchaseRequisitionItem.KeyItemDescription
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "Qty",
                    FieldValue = purchaseRequisitionItem.Qty.ToString()
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "UnitName",
                    FieldValue = purchaseRequisitionItem.UnitName
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "Price",
                    FieldValue = purchaseRequisitionItem.Price.ToString()
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "TotalAmount",
                    FieldValue = purchaseRequisitionItem.TotalAmount.ToString()
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "SupplierName",
                    FieldValue = purchaseRequisitionItem.SupplierName
                });

                workFlowCutomListValues.Add(new WorkFlowCutomListValue()
                {
                    FieldName = "DemandDate",
                    FieldValue = DateTimeFormat.ToShortFormat1(purchaseRequisitionItem.DemandDate)
                });

                customList.CutomListValueList.Add(workFlowCutomListValues);
            }
        }

        /// <summary>
        /// 创建采购申请明细字段列表
        /// </summary>
        /// <param name="customList"></param>
        private void CreatePrDetailFields(WorkFlowCustomList customList)
        {
            customList.CutomListFields = new List<WorkFlowCutomListField>();
            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "行号".L10N(),
                FieldName = "LineNo",
                FieldWidth = 50,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "采购对象编码".L10N(),
                FieldName = "ObjectCode",
                FieldWidth = 120,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "采购对象描述".L10N(),
                FieldName = "Description",
                FieldWidth = 200,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "规格型号".L10N(),
                FieldName = "Specification",
                FieldWidth = 120,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "项目事项".L10N(),
                FieldName = "KeyItemDescription",
                FieldWidth = 120,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "需求数量".L10N(),
                FieldName = "Qty",
                FieldWidth = 80,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "单位".L10N(),
                FieldName = "UnitName",
                FieldWidth = 80,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "参考单价".L10N(),
                FieldName = "Price",
                FieldWidth = 130,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "参考总金额".L10N(),
                FieldName = "TotalAmount",
                FieldWidth = 130,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "供应商".L10N(),
                FieldName = "SupplierName",
                FieldWidth = 200,
            });

            customList.CutomListFields.Add(new WorkFlowCutomListField()
            {
                FieldLabel = "需求日期".L10N(),
                FieldName = "DemandDate",
                FieldWidth = 150,
            });
        }

        /// <summary>
        /// 采购申请主表信息
        /// </summary>
        /// <param name="pr">采购申请</param>
        /// <param name="workFlowForm">自定义表单</param>
        private void CreatePrMainInfo(PurchaseRequisition pr, WorkFlowForm workFlowForm)
        {
            workFlowForm.FieldList = new List<WorkFlowFormField>();

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "申请单号".L10N(),
                Value = pr.No
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "工厂".L10N(),
                Value = pr.FactoryName
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "部门".L10N(),
                Value = pr.DepartmentName
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "采购类型".L10N(),
                Value = pr.PurchaseType.ToLabel()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "项目编号".L10N(),
                Value = pr.ProjectCode
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "项目名称".L10N(),
                Value = pr.ProjectName
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "采购对象".L10N(),
                Value = pr.PurchaseObjectType.ToLabel()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "品种数量".L10N(),
                Value = pr.VarietyQuantity.ToString()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "总数量".L10N(),
                Value = pr.TotalAmount.ToString()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "采购预算".L10N(),
                Value = pr.PurchaseBudget.ToString()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "币种".L10N(),
                Value = pr.Currency.ToLabel()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "金额单位".L10N(),
                Value = pr.AmountUnit.ToLabel()
            });

            workFlowForm.FieldList.Add(new WorkFlowFormField()
            {
                Name = "备注".L10N(),
                Value = pr.Remark
            });
        }

        /// <summary>
        /// 核准前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected override void CheckApprovalStatusBeforeApproved(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【审核中】的数据才能审核".L10N());
            }
        }

        /// <summary>
        /// 驳回前验证审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">资产采购申请的列表</param>
        protected override void CheckApprovalStatusBeforeReject(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【审核中】的数据才能驳回".L10N());
            }
        }

        /// <summary>
        /// 检查单据状态在撤回前
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        /// <exception cref="ValidationException">只有状态为【审核中】的数据才能操作</exception>
        protected override void CheckApprovalStatusBeforeRetract(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            if (purchaseRequisitions.Any(p => p.ApprovalStatus != ApprovalStatus.UnderReview))
            {
                throw new ValidationException("只有状态为【审核中】的数据才能操作".L10N());
            }
        }

        /// <summary>
        /// 撤回时更新审核状态
        /// </summary>
        /// <param name="purchaseRequisitions">采购申请单列表</param>
        protected override void UpdateApprovalStatusOnWithdraw(EntityList<PurchaseRequisition> purchaseRequisitions)
        {
            foreach (var pr in purchaseRequisitions)
            {
                try
                {
                    if (!pr.WorkflowInstanceId.IsNullOrWhiteSpace())
                    {
                        _workflowService.CancelWorkflow(double.Parse(pr.WorkflowInstanceId), "");
                    }

                    pr.ApprovalStatus = ApprovalStatus.Draft;
                    pr.WorkflowStartResult = "成功能撤消【资产采购申请】工作流实例".L10N();
                }
                catch (Exception ex)
                {
                    pr.WorkflowStartResult = ex.GetExceptionMessage();
                }
            }
        }
    }
}
