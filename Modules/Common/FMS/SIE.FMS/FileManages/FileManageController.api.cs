using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.FMS.FileManages.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.FMS.FileManages
{
    /// <summary>
    /// 文件控制器-接口
    /// </summary>
    public partial class FileManageController : DomainController
    {
        /// <summary>
        /// 获取待审核文件信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("获取待审核文件信息")]
        [return: ApiReturn("待审核文件信息")]
        public virtual List<FileInfor> GetToAuditFile([ApiParameter("查询信息:{AuditorName:审核人名称,FileCode:文件编号,FileName:文件名,Version:版本号}")] FileQueryInfo queryInfo)
        {
            var fileManages = GetToAuditFiles(queryInfo.FileCode, queryInfo.FileName, queryInfo.Version);
            var infos = new List<FileInfor>();
            var ftpUrl = RT.Config.Get<string>(FtpPath);
            foreach (var fileManage in fileManages)
            {
                var info = new FileInfor();
                info.FileId = fileManage.Id;
                info.FileCode = fileManage.Code;
                info.FileName = fileManage.Name;
                info.Version = fileManage.VersionPrefix + fileManage.Version;
                info.FilePath = ftpUrl + fileManage.Path;
                info.Size = fileManage.Size;
            }
            return infos;
        }

        /// <summary>
        /// 根据编码、名称、版本获取待审核的文件
        /// </summary>
        /// <param name="code">文件编码</param>
        /// <param name="name">文件名称</param>
        /// <param name="version">文件版本</param>
        /// <returns>待审核的文件</returns>
        public virtual EntityList<FileManage> GetToAuditFiles(string code, string name, string version)
        {
            return Query<FileManage>().Where(p => p.Code == code && p.Name == name && (p.VersionPrefix + p.Version) == version
             && !p.IsHistory && (p.FileState == FileState.Audit || p.FileState == FileState.ToScrap)).ToList();
        }

        /// <summary>
        /// 审核文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="isPass">是否审核通过</param>
        /// <param name="rejectReason">驳回原因</param>
        /// <param name="auditorId">审核人ID</param>
        /// <returns></returns>
        [ApiService("审核文件")]
        public virtual void AuditFile([ApiParameter("文件Id")] double fileId, [ApiParameter("是否审核通过")] bool isPass, [ApiParameter("驳回原因")] string rejectReason, [ApiParameter("审核人ID")] double auditorId)
        {
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var file = RF.GetById<FileManage>(fileId);
                if (file == null)
                    throw new ValidationException("文件不存在".L10N());
                if (file.FileState != FileState.Audit && file.FileState != FileState.ToScrap)
                    throw new ValidationException("文件不是待审核状态！".L10N());
                CreateOAFileAudit(file, rejectReason, auditorId, isPass);
                if (isPass)
                {
                    if (file.FileState == FileState.ToScrap)
                        file.FileState = FileState.ScrapToRelease;
                    else
                        file.FileState = FileState.ToRelease;
                }
                else
                    file.FileState = file.PreFileState.Value;
                RF.Save(file);
                string op = isPass ? "OA审核通过".L10N() : "OA审核驳回".L10N();
                CreateLog(new List<double> { file.Id }, op);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建OA审批流数据
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="rejectReason">驳回原因</param>
        /// <param name="auditorId">审核人id</param>
        /// <param name="isPass">是否通过</param>
        private void CreateOAFileAudit(FileManage file, string rejectReason, double auditorId, bool isPass)
        {
            var fileAudit = new FileAudit();
            fileAudit.Operation = file.FileState == FileState.ToScrap ? OperationType.ToScrap : OperationType.ToPublish;
            fileAudit.Remark = "OA审核文件".L10N();
            fileAudit.RejectReason = rejectReason;
            fileAudit.AuditorId = auditorId;
            fileAudit.State = isPass ? FileAuditState.Pass : FileAuditState.Reject;
            fileAudit.FileId = file.Id;
            fileAudit.IsEnabled = true;
            RF.Save(fileAudit);
        }
    }
}
