using MimeKit;
using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.DataQuery
{
    /// <summary>
    /// 安灯管理数据请求
    /// </summary>
    public class AndonManageDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取选择安灯的停线和叫料
        /// </summary>
        /// <param name="andonId"></param>
        /// <returns></returns>
        public List<bool> GetLineStopAndAskMaterial(double andonId)
        {
            return RT.Service.Resolve<AndonManageController>().GetLineStopAndAskMaterial(andonId);
        }

        /// <summary>
        /// 根据工位获取在制工单
        /// </summary>
        public EntityList<WorkOrder> GetMakingWorkOrder(double stationId)
        {
            return RT.Service.Resolve<AndonManageController>().GetMakingWorkOrder(stationId);
        }

        /// <summary>
        /// 获取转派员工
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<Employee> GetReassignEmployee(string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<AndonManageController>().GetReassignEmployee(keyword, pagingInfo);
        }

        /// <summary>
        /// 安灯管理触发上传图片
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string AndonManageSaveImage(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1 && fileName.Split('.').Length > 1)
            {
                var exts = new List<string> { "png", "jpg", "jpeg", "bmp", "gif", "webp", "psd", "svg", "tiff", "jfif" };
                var fileSplitLength = fileName.Split('.').Length;
                var fileType = fileName.Split('.')[fileSplitLength - 1];
                if (!exts.Contains(fileType))
                {
                    throw new ValidationException("只能上传图片格式的文件".L10N());
                }
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "AndonManageImage";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                return $"{path}/{fileName}";
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }

        }

        /// <summary>
        /// 安灯管理上传附件
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string AndonManageSaveAttachment(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1 && fileName.Split('.').Length > 1)
            {
                var fileSplitLength = fileContent.Split(',').Length;
                var bytes = Convert.FromBase64String(fileContent.Split(',')[fileSplitLength - 1]);
                const string prePath = "AndonManageAttachment";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                return $"{path}/{fileName}";
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }

        /// <summary>
        /// 获取安灯管理信息
        /// </summary>
        /// <param name="andonManageId"></param>
        /// <returns></returns>
        public AndonManage GetAndonManageInfo(double andonManageId)
        {
            var andonManage = RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
            return andonManage;
        }
    }
}
