using SIE.Domain.Validation;
using SIE.EMS.InventoryPlans;
using SIE.Equipments.Configs;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划查询器
    /// </summary>
    public class InventoryPlanDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的盘点计划
        /// </summary>
        /// <returns>新的盘点计划</returns>
        public InventoryPlan GetNewInventoryPlan()
        {
            return RT.Service.Resolve<InventoryPlanController>().GetNewInventoryPlan();
        }

        ///<summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public ApprovalConfigValue GetInventoryPlanApproval()
        {
            return RT.Service.Resolve<InventoryPlanController>().GetApprovalConfigValue();
        }

        /// <summary>
        /// 保存盘点计划图片
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="type"></param>
        ///<param name="dataId">要保存的数据</param> 
        /// <returns>路径</returns>
        public string SaveInventoryPlanPhoto(string fileContent, string fileName,string type, double dataId)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1 && fileName.Split('.').Length > 1)
            {
                var exts = new List<string> { "png", "jpg", "jpeg", "bmp", "gif", "webp", "psd", "svg", "tiff", "jfif" };
                if (!exts.Contains(fileName.Split('.')[1]))
                {
                    throw new ValidationException("只能上传图片格式的文件".L10N());
                }
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "InventoryPlanPhoto";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                var returnFilePath = $"{path}/{fileName}";
                RT.Service.Resolve<InventoryPlanController>().SaveInventoryPlanImage(dataId, returnFilePath, type);
                return returnFilePath;
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }

        /// <summary>
        /// 重写保存盘点计划图片方法
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string SaveInventoryPlanPhotoNoSave(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1 && fileName.Split('.').Length > 1)
            {
                var exts = new List<string> { "png", "jpg", "jpeg", "bmp", "gif", "webp", "psd", "svg", "tiff", "jfif" };
                if (!exts.Contains(fileName.Split('.')[1]))
                {
                    throw new ValidationException("只能上传图片格式的文件".L10N());
                }
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "InventoryPlanPhoto";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
                return $"{path}/{fileName}";
            }
            else
            {
                throw new ValidationException("文件内容异常，不能上传。".L10N());
            }
        }
    }
}
