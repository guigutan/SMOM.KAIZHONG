using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Scripting.Utils;
using SIE.Andon.Andons.Enum;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Api;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MES.WIP.Products;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform
{
    /// <summary>
    /// 
    /// </summary>
    public class WinFormAndonApiController : AndonManageController
    {
        /// <summary>
        /// 获取工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="stationId">工位Id</param>
        [ApiService("获取工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）")]
        [return: ApiReturn("获取工位为工作单元选择的工位或者触发人为当前用户的安灯事件（状态为待响应、处理中、待验收）")]
        public virtual List<XPAndonManage> GetXPAndonManages([ApiParameter("工单Id")] double employeeId, [ApiParameter("工位Id")] double stationId)
        {
            List<XPAndonManage> result = new List<XPAndonManage>();

            if (employeeId == 0 || stationId == 0)
                return result;

            var andonManages = base.GetAndonManages(employeeId, stationId);

            andonManages.OrderBy(x => x.CreateDate).ForEach(p => result.Add(XPAndonManage.Gen(p)));

            return result;
        }

        /// <summary>
        /// 获取用户有权限且启用的安灯类型下启用的安灯
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        [ApiService("获取用户有权限且启用的安灯类型下启用的安灯")]
        [return: ApiReturn("获取用户有权限且启用的安灯类型下启用的安灯")]
        public virtual List<XPAndon> GetXPAndons([ApiParameter("工单Id")] double employeeId)
        {
            List<XPAndon> result = new List<XPAndon>();

            if (employeeId == 0)
                return result;

            var andons = base.GetAndonsByEmployeeId(employeeId);

            andons.ForEach(p => result.Add(XPAndon.Gen(p)));

            return result;
        }

        /// <summary>
        /// 获取指定安灯管理ID的操作日志
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        [ApiService("获取指定安灯管理ID的操作日志")]
        [return: ApiReturn("获取指定安灯管理ID的操作日志")]
        public virtual List<XPAndonManageOperateLog> GetXPAndonManageOperateLog([ApiParameter("安灯管理ID")] double andonManageId)
        {
            List<XPAndonManageOperateLog> result = new List<XPAndonManageOperateLog>();

            if (andonManageId == 0)
                return result;

            var operationLogs = Query<AndonManageOperateLog>().Where(p => p.AndonManageId == andonManageId).OrderByDescending(p => p.OperateTime).ToList();

            operationLogs.ForEach(p => result.Add(XPAndonManageOperateLog.Gen(p)));

            return result;
        }

        /// <summary>
        /// 获取指定安灯管理ID的消息推送信息
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        [ApiService("获取指定安灯管理ID的消息推送信息")]
        [return: ApiReturn("获取指定安灯管理ID的消息推送信息")]
        public virtual List<XPAndonManageMessageSend> GetXPAndonManageMessageSend([ApiParameter("安灯管理ID")] double andonManageId)
        {
            List<XPAndonManageMessageSend> result = new List<XPAndonManageMessageSend>();

            if (andonManageId == 0)
                return result;

            var operationLogs = Query<AndonManageMessageSend>().Where(p => p.AndonManageId == andonManageId).OrderByDescending(p => p.MessageSendTime).ToList();

            operationLogs.ForEach(p => result.Add(XPAndonManageMessageSend.Gen(p)));

            return result;
        }


        /// <summary>
        /// 安灯管理-取消
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        /// <param name="remark">取消原因</param>
        [ApiService("安灯管理-取消")]
        [return: ApiReturn("安灯管理-取消")]
        public virtual XPApiResultAndon AndonManageCancel([ApiParameter("安灯管理ID")] double andonManageId, [ApiParameter("取消原因")] string remark)
        {
            if (remark.IsNullOrEmpty())
                throw new ArgumentException("【取消原因】必须输入！".L10N());

            XPApiResultAndon result = new XPApiResultAndon();

            RT.Service.Resolve<AndonManageController>().AndonManageCancel(andonManageId, AndonManageOperateType.Cancel, remark);

            var andonManage = RF.GetById<AndonManage>(andonManageId);
            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 安灯管理-响应
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        [ApiService("安灯管理-响应")]
        [return: ApiReturn("安灯管理-响应")]
        public virtual XPApiResultAndon AndonManageResponse([ApiParameter("安灯管理ID")] double andonManageId)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            var andonManage = RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
            var nowHandler = RT.Identity;
            var oldHandler = andonManage.Handler;
            var reason = "";
            if (oldHandler != null)
            {
                reason = "处理人由" + andonManage.Handler.Name + "变更为" + nowHandler.Name;
            }
            else
            {
                reason = "处理人更新为" + nowHandler.Name;
            }
            RT.Service.Resolve<AndonManageController>().AndonManageResponse(andonManageId, AndonManageOperateType.Response, reason.L10N());
           
            andonManage.State = AndonManageState.Processing;

            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 安灯管理-处理完成
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        [ApiService("安灯管理-处理完成")]
        [return: ApiReturn("安灯管理-处理完成")]
        public virtual XPApiResultAndon AndonManageHandleAsync([ApiParameter("安灯管理ID")] double andonManageId)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            var andonManage = RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
            RT.Service.Resolve<AndonManageController>().AndonManageHandleAsync(andonManageId, AndonManageOperateType.Handle, "");

            andonManage.State = AndonManageState.ToAccepted;

            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 安灯管理-处理完成
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        [ApiService("安灯管理-处理完成")]
        [return: ApiReturn("安灯管理-处理完成")]
        public virtual XPApiResultAndon AndonManageHandle([ApiParameter("安灯管理ID")] double andonManageId)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            var andonManage = RF.GetById<AndonManage>(andonManageId, new EagerLoadOptions().LoadWithViewProperty());
            RT.Service.Resolve<AndonManageController>().AndonManageHandle(andonManageId, AndonManageOperateType.Handle, "");

            andonManage.State = AndonManageState.ToAccepted;

            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 安灯管理-驳回
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        /// <param name="remark">驳回原因</param>
        [ApiService("安灯管理-驳回")]
        [return: ApiReturn("安灯管理-驳回")]
        public virtual XPApiResultAndon AndonManageReject([ApiParameter("安灯管理ID")] double andonManageId, [ApiParameter("驳回原因")] string remark)
        {
            if (remark.IsNullOrEmpty())
                throw new ArgumentException("【驳回原因】必须输入！".L10N());

            XPApiResultAndon result = new XPApiResultAndon();

            RT.Service.Resolve<AndonManageController>().AndonManageReject(andonManageId, AndonManageOperateType.Reject, remark);

            var andonManage = RF.GetById<AndonManage>(andonManageId);
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Processing;

            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 安灯管理-验收
        /// </summary>
        /// <param name="andonManageId">安灯管理ID</param>
        /// <param name="actualTime">实际影响时间</param>
        [ApiService("安灯管理-验收")]
        [return: ApiReturn("安灯管理-验收")]
        public virtual XPApiResultAndon AndonManageCheck([ApiParameter("安灯管理ID")] double andonManageId, [ApiParameter("实际影响时间")] double actualTime)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            RT.Service.Resolve<AndonManageController>().AndonManageCheck(andonManageId, AndonManageOperateType.Check, "验收成功", actualTime);

            var andonManage = RF.GetById<AndonManage>(andonManageId);
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Closed;

            result.AndonManage = (XPAndonManage.Gen(andonManage));
            result.Logs = GetXPAndonManageOperateLog(andonManageId);
            result.Messages = GetXPAndonManageMessageSend(andonManageId);

            return result;
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="fileUrl">文件路径</param>
        [ApiService("下载附件")]
        [return: ApiReturn("下载附件")]
        public virtual XPAttach FileDownload([ApiParameter("文件路径")] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                throw new ArgumentException("要下载的文件不能为空".L10N());

            XPAttach result = new XPAttach();
            result.FileUrl = fileUrl;
            result.FileName = fileUrl.Split('/').LastOrDefault();
            result.Contents = RT.Service.Resolve<AttachmentController>().FileDownload(fileUrl, result.FileName);

            return result;
        }

        /// <summary>
        /// 获取安灯
        /// </summary>
        /// <param name="andonId">安灯维护Id</param>
        /// <param name="stationId">工位Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="resourceId">资源Id</param>
        /// <returns></returns>
        [ApiService("获取安灯")]
        [return: ApiReturn("获取安灯")]
        public virtual XPAndonManage CreateAndonManageByAndon([ApiParameter("安灯维护Id")] object andonId, [ApiParameter("工位Id")] double stationId,
            [ApiParameter("工序Id")] double processId, [ApiParameter("资源Id")] double resourceId)
        {
            var andonManage = base.CreateAndonManage(andonId, stationId, processId, resourceId);

            return XPAndonManage.Gen(andonManage);
        }

        /// <summary>
        /// 根据产线获取设备台账列表
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns></returns>
        [ApiService("根据产线获取设备台账列表")]
        [return: ApiReturn("根据产线获取设备台账列表")]
        public virtual List<XPEquipAccount> GetXPEquipAccountInfos([ApiParameter("资源Id")] double resourceId, [ApiParameter("查询关键字")] string keyword)
        {
            var equips = base.GetEquipAccountsByResourceId(resourceId, keyword, null);
            if (equips.Count <= 0)
                equips = base.GetEquipAccounts(null, keyword);

            var result = new List<XPEquipAccount>();
            equips.ForEach(p =>
            {
                result.Add(XPEquipAccount.Gen(p));
            });
            return result;
        }

        /// <summary>
        /// 安灯管理添加BOM物料命令
        /// </summary>
        /// <param name="andonManage"></param>
        /// <returns></returns>
        [ApiService("安灯管理添加BOM物料命令")]
        [return: ApiReturn("安灯管理添加BOM物料命令")]
        public virtual XPAndonManageCallMaterial AddXPCallMaterial([ApiParameter("资源Id")] XPAndonManage andonManage)
        {
            var callMaterial = new AndonManageCallMaterial
            {
                FactoryId = andonManage.FactoryId,
                WipId = (double)andonManage.WipResourceId,
                WorkShopId = andonManage.WorkShopId,
                WorkOrderId = andonManage.WorkOrderId,
                ProcessId = andonManage.ProcessId,
                AndonManageId = andonManage.Id,
            };
            callMaterial = base.AddCallMaterial(callMaterial);
            return XPAndonManageCallMaterial.Gen(callMaterial); 
        }

        /// <summary>
        /// 叫料选择物料
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="wipId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [ApiService("安灯触发叫料选择物料")]
        [return: ApiReturn("物料 List<CallMaterialInfo>")]
        public virtual List<XPItem> ChoseXPItems([ApiParameter("工单Id")] double? woId, [ApiParameter("资源Id")] double? wipId, [ApiParameter("查询关键字")] string keyword)
        {
            AndonManageCallMaterial andonManageCallMaterial = new AndonManageCallMaterial
            {
                WorkOrderId = woId,
                ProcessId = wipId,
            };
            var itemEntityList = ChoseItems(andonManageCallMaterial, null, keyword);
            List<XPItem> result = new List<XPItem>();
            itemEntityList.ForEach(p => result.Add(XPItem.Gen(p)));

            return result;
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="fileName">文件名称，不带路径</param>
        /// <param name="bytesBase64">文件内容Base64字符</param>
        /// <returns></returns>
        [ApiService("附件上传")]
        [return: ApiReturn("附件上传")]
        public virtual string UploadAndoPhoto([ApiParameter("文件名称，不带路径")] string fileName, [ApiParameter("文件内容")] string bytesBase64)
        {
            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            var fileExtesion = System.IO.Path.GetExtension(fileName);

            if (!exts.Contains(fileExtesion))
            {
                throw new ArgumentException("只能上传图片格式的文件".L10N());
            }

            byte[] bytes = Convert.FromBase64String(bytesBase64);

            const string prePath = "AndonManageImage";
            var path = $"{prePath}/{Guid.NewGuid()}";
            RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(fileName, bytes, path);
            var fliePath = $"{path}/{fileName}";
            return fliePath;
        }

        /// <summary>
        /// 保存安灯管理
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="listMaterial"></param>
        /// <returns></returns>
        [ApiService("保存安灯管理")]
        [return: ApiReturn("保存安灯管理")]
        public virtual XPApiResultAndon SaveAndonAndItemDetailAsync([ApiParameter("安灯管理")] XPAndonManage andonManage,
            [ApiParameter("安灯管理叫料列表")] List<XPAndonManageCallMaterial> listMaterial)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            EntityList<AndonManageCallMaterial> list = new EntityList<AndonManageCallMaterial>();
            listMaterial.ForEach(p => list.Add(p.ToAndonManageCallMaterial()));

            AndonManage am = andonManage.ToAndonManage();
            base.SaveAndonAndItemDetailAsync(am, list);

            am = base.GetAndonManage(andonManage.Id);
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Processing;

            result.AndonManage = XPAndonManage.Gen(am);
            result.Logs = GetXPAndonManageOperateLog(am.Id);
            result.Messages = GetXPAndonManageMessageSend(am.Id);

            return result;
        }


        /// <summary>
        /// 保存安灯管理
        /// </summary>
        /// <param name="andonManage"></param>
        /// <param name="listMaterial"></param>
        /// <returns></returns>
        [ApiService("保存安灯管理")]
        [return: ApiReturn("保存安灯管理")]
        public virtual XPApiResultAndon SaveAndonAndItemDetail([ApiParameter("安灯管理")] XPAndonManage andonManage,
            [ApiParameter("安灯管理叫料列表")] List<XPAndonManageCallMaterial> listMaterial)
        {
            XPApiResultAndon result = new XPApiResultAndon();

            EntityList<AndonManageCallMaterial> list = new EntityList<AndonManageCallMaterial>();
            listMaterial.ForEach(p => list.Add(p.ToAndonManageCallMaterial()));

            AndonManage am = andonManage.ToAndonManage();
            base.SaveAndonAndItemDetail(am, list);

            am = base.GetAndonManage(andonManage.Id);
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Processing;

            result.AndonManage = XPAndonManage.Gen(am);
            result.Logs = GetXPAndonManageOperateLog(am.Id);
            result.Messages = GetXPAndonManageMessageSend(am.Id);

            return result;
        }
    }
}
