using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录保存前校验批次号是否重复
    /// </summary>
    [System.ComponentModel.DisplayName("报工记录保存前校验批次号是否重复")]
    [System.ComponentModel.Description("报工记录保存前校验批次号是否重复")]
    public class ReportRecordSubmitting : OnSubmitting<ReportRecord>
    {
        /// <summary>
        /// 保存报工记录前执行
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="e">实体提交参数</param>
        protected override void Invoke(ReportRecord entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                ReportRecord record = entity;
                if (record.BatchNo.IsNotEmpty()) 
                {
                    if (RT.Service.Resolve<ReportController>().ExistReportBatchNo(record.BatchNo)) 
                    {
                        throw new ValidationException("批次号已存在".L10N());
                    }
                }
            }
        }
    }
}
