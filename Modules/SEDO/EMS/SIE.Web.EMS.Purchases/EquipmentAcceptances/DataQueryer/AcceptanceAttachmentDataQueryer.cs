using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances.DataQueryers
{
    /// <summary>
    /// 设备开箱验收附件查询器
    /// </summary>
    public class AcceptanceAttachmentDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取设备开箱验收附件查询器的视图模型
        /// </summary>
        /// <param name="equipmentAcceptanceId"></param>
        /// <returns></returns>
        public Tuple<EquipmentAcceptanceAttachment> GetAcceptanceAttachmentDataForUpload(double equipmentAcceptanceId)
        {
            return new Tuple<EquipmentAcceptanceAttachment>(new EquipmentAcceptanceAttachment()
            {
                OwnerId = equipmentAcceptanceId
            });
        }


        /// <summary>
        /// 保存设备开箱验收附件
        /// </summary>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string SaveEquipmentAcceptanceAttachment(string fileContent, string fileName)
        {
            if (fileContent.Length <= 0)
            {
                throw new ValidationException("文件内容为空，不能上传。".L10N());
            }

            if (fileContent.Split(',').Length > 1)
            {
                var bytes = Convert.FromBase64String(fileContent.Split(',')[1]);
                const string prePath = "EquipmentAcceptanceAttachment";

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
