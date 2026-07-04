using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.Web.Data;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试查询器
    /// </summary>
    public class EquipmentSetupDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的安装调试
        /// </summary>
        /// <returns>新的安装调试</returns>
        public EquipmentSetup GetNewEquipmentSetup()
        {
            return RT.Service.Resolve<EquipmentSetupController>().GetNewEquipmentSetup();
        }

        /// <summary>
        /// 保存安装调试附件
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <returns>路径</returns>
        public string SaveSetupAttachment(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1)
            {
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "SaveSetupAttachment";
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
        /// 领料申请
        /// </summary>
        /// <param name="model">领料申请</param>
        /// <param name="list">领料申请明细</param>
        public void MaterialApply(MaterialApplyViewModel model, List<MaterialApplyDetailViewModel> list)
        {
            RT.Service.Resolve<EquipmentSetupController>().MaterialApply(model, list);
        }

        /// <summary>
        /// 根据备件+仓库查询可用库存数
        /// </summary>
        /// <param name="sparePartId">备件</param>
        /// <param name="warehouseId">仓库</param>
        /// <returns>可用库存数</returns>
        public decimal GetWarehouseQty(double sparePartId, double warehouseId)
        {
            return RT.Service.Resolve<EquipmentSetupController>().GetWarehouseQty(sparePartId, warehouseId);
        }
    }
}
