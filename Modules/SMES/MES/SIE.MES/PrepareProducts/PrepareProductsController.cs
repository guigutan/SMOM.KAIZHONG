using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.MES.ProcessPrepareRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备控制器
    /// </summary>
    public partial class PrepareProductsController : DomainController
    {
        /// <summary>
        /// 创建产品准备记录
        /// </summary>
        /// <param name="prepareRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<PrepareRecordDetail> CreatePrepareRecordDetail(double prepareRecordId)
        {
            return RT.Service.Resolve<PrepareRecordService>().CreatePrepareRecordDetail(prepareRecordId);
        }

        /// <summary>
        ///确认按钮
        /// </summary>
        /// <param name="prepareRecordDetails"></param>
        public virtual void Comfrim(EntityList<PrepareRecordDetail> prepareRecordDetails)
        {
            var newPrepareRecordDetailList = new EntityList<PrepareRecordDetail>();
            foreach (var projectInfo in prepareRecordDetails)
            {
                PrepareRecordDetail prepareRecordDetail = new PrepareRecordDetail();
                prepareRecordDetail.ProcessId = projectInfo.ProcessId;
                prepareRecordDetail.PrepareRecordId = projectInfo.PrepareRecordId;
                prepareRecordDetail.PrepareProjectId = projectInfo.PrepareProjectId;
                prepareRecordDetail.ProjectCode = projectInfo.ProjectCode;
                prepareRecordDetail.ProjectDesc = projectInfo.ProjectDesc;
                prepareRecordDetail.ProjectName = projectInfo.ProjectName;
                prepareRecordDetail.Remark = projectInfo.Remark;
                prepareRecordDetail.ConfirmerId = RT.IdentityId;
                prepareRecordDetail.Result = projectInfo.Result;
                prepareRecordDetail.ProjectType = projectInfo.ProjectType;
                newPrepareRecordDetailList.Add(prepareRecordDetail);
            }
            RT.Service.Resolve<PrepareRecordService>().Comfrim(newPrepareRecordDetailList);
        }
    }
}
