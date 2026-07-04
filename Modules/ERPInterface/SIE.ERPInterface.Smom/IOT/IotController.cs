using Newtonsoft.Json;
using SIE.Andon.Andons;
using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.EventMessages.IOT;
using SIE.MES.LineAndon;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.IOT.Data;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.IOT
{
    /// <summary>
    /// IOT 控制器
    /// </summary>
    public class IotController : DomainController, IIotTaskReport
    {

        /// <summary>
        /// 设置IOT设备初始任务信息
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        public virtual void SetIotTaskInfo(IotTaskInfo taskInfo)
        {
            var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(taskInfo.ResourceCode);
            if (andonLine == null || andonLine.AndonEntity.IsNullOrEmpty() || andonLine.AndonOrder.IsNullOrEmpty())
            {
                throw new ValidationException("请先维护资源[{0}]对应的IOT实体与IOT指令".L10nFormat(taskInfo.ResourceCode));
            }
            var token = RT.Service.Resolve<AndonManageController>().IotGetToken();
            if (token.IsNullOrEmpty())
                throw new ValidationException("获取 IOT Token 失败");

            var ret = RT.Service.Resolve<AndonManageController>().IotGetWorkWrite(token, andonLine.AndonEntity, andonLine.AndonOrder, taskInfo.TaskNo, taskInfo.ResourceCode, (int)taskInfo.InitQty);

            RT.Logger.Info($"IOT指令下发: IotGetWorkWrite: {JsonConvert.SerializeObject(taskInfo)}, 返回结果: [{ret}]");

            if (ret.IsNotEmpty())
                throw new ValidationException("IOT实体[{0}]指令[{1}]下发失败: \r\n{2}".L10nFormat(andonLine.AndonEntity, andonLine.AndonOrder, ret));
        }

        /// <summary>
        /// 获取IOT设备任务信息
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        public virtual IotTaskInfo GetIotTaskInfo(IotTaskInfo taskInfo)
        {
            var data = new IotTaskInfo();
            var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(taskInfo.ResourceCode);
            if (andonLine == null || andonLine.AndonEntity.IsNullOrEmpty() || andonLine.AndonOrder.IsNullOrEmpty())
            {
                throw new ValidationException("请先维护资源[{0}]对应的IOT实体与IOT指令".L10nFormat(taskInfo.ResourceCode));
            }
            var token = RT.Service.Resolve<AndonManageController>().IotGetToken();
            if (token.IsNullOrEmpty())
                throw new ValidationException("获取 IOT Token 失败");

            var iotRet = RT.Service.Resolve<AndonManageController>().IOTGetWorkRead(token, andonLine.AndonEntity);

            //RT.Logger.Info($"IOT指令下发: IOTGetWorkRead: {JsonConvert.SerializeObject(taskInfo)}, 返回结果: [{JsonConvert.SerializeObject(iotRet)}]");

            if (iotRet == null)
                return data;

            data.TaskNo = iotRet.WorkOrder;
            data.ResourceCode = iotRet.DeviceCode;
            data.OutPutQty = iotRet.OutPutNum;

            return data;
        }

        /// <summary>
        /// 下发多任务单至IOT
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="resourceCode"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SetIotTaskInfos(List<IotTaskInfo> taskInfo, string resourceCode)
        {
            if (taskInfo.Count == 0)
                throw new ValidationException("没有要下发的数据");
            var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(resourceCode);
            if (andonLine == null || andonLine.AndonEntity.IsNullOrEmpty() || andonLine.AndonOrder.IsNullOrEmpty())
            {
                throw new ValidationException("请先维护资源[{0}]对应的IOT实体与IOT指令".L10nFormat(resourceCode));
            }
            var token = RT.Service.Resolve<AndonManageController>().IotGetToken();
            if (token.IsNullOrEmpty())
                throw new ValidationException("获取 IOT Token 失败");
            taskInfo = taskInfo.OrderBy(p => p.TaskNo).ToList();
            var jsonObj = new Dictionary<string, object>();
            jsonObj.Add("DeviceCode", resourceCode);
            jsonObj.Add("PutNum", 0);
            var k = 0;
            for (var i = 0; i < taskInfo.Count; i++)
            {
                var info = taskInfo[i];
                jsonObj.Add($"WorkOrder{i + 1}", info.TaskNo);
                jsonObj.Add($"OutPutNum{i + 1}", info.InitQty);
                k = i + 1;
            }
            //最大支持16个任务单, 需补齐16对参数
            while (k < 16)
            {
                jsonObj.Add($"WorkOrder{k + 1}", " ");  //无任务单时,不给空值 (如果给空值,IOT无法清除数据)
                jsonObj.Add($"OutPutNum{k + 1}", 0);
                k++;
            }
            //var iotData = jsonObj.CastTo<IotPunchData>();
            var ret = RT.Service.Resolve<AndonManageController>().IotBatchWorkWrite(token, andonLine.AndonEntity, andonLine.AndonOrder, jsonObj, taskInfo.Select(p => p.TaskNo).Concat(","));

            RT.Logger.Info($"IOT指令下发: IotBatchWorkWrite: {JsonConvert.SerializeObject(taskInfo)}, 返回结果: [{ret}]");

            if (ret.IsNotEmpty())
                throw new ValidationException("IOT实体[{0}]指令[{1}]下发失败: \r\n{2}".L10nFormat(andonLine.AndonEntity, andonLine.AndonOrder, ret));

        }

        /// <summary>
        /// 从IOT读取多任务单数据
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<IotTaskInfo> GetIotTaskInfos(string resourceCode)
        {
            var datas = new List<IotTaskInfo>();
            //string resourceCode = taskInfo.FirstOrDefault()?.ResourceCode;
            var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(resourceCode);
            if (andonLine == null || andonLine.AndonEntity.IsNullOrEmpty() || andonLine.AndonOrder.IsNullOrEmpty())
            {
                throw new ValidationException("请先维护资源[{0}]对应的IOT实体与IOT指令".L10nFormat(resourceCode));
            }
            var token = RT.Service.Resolve<AndonManageController>().IotGetToken();
            if (token.IsNullOrEmpty())
                throw new ValidationException("获取 IOT Token 失败");

            var iotRet = RT.Service.Resolve<AndonManageController>().IotBatchWorkRead(token, andonLine.AndonEntity);

            //RT.Logger.Info($"IOT指令下发: IotBatchWorkRead: {JsonConvert.SerializeObject(taskInfo)}, 返回结果: [{JsonConvert.SerializeObject(iotRet)}]");

            if (iotRet == null)
                return datas;
            datas = iotRet.Select(p => new IotTaskInfo()
            {
                TaskNo = p.WorkOrder.Trim(),
                ResourceCode = p.DeviceCode,
                OutPutQty = p.OutPutNum
            }).ToList();

            return datas;

        }

        /// <summary>
        /// 保存IOT押出换轴记录
        /// </summary>
        /// <param name="datas"></param>
        [ApiService("保存IOT押出换轴记录")]
        [AllowAnonymous]
        public virtual void SaveAxisChangeRecords([ApiParameter("换轴信息")] List<AxisChangeInfo> datas)
        {

            if (datas == null || datas.Count == 0)
                throw new ValidationException("提交数据为空");

            if (datas.Any(p => p.IotEntity.IsNullOrEmpty()))
                throw new ValidationException("IOT实体不能为空");
            if (datas.Any(p => p.TaskNo.IsNullOrEmpty()))
                throw new ValidationException("任务单号不能为空");

            //RT.Logger.Info($"保存IOT押出换轴记录 SaveAxisChangeRecords: {JsonConvert.SerializeObject(datas)}");

            var taskNos = datas.Select(p => p.TaskNo).Distinct().ToList();
            var records = taskNos.SplitContains(temp =>
            {
                return Query<AxisChangeRecord>().Where(p => temp.Contains(p.TaskNo) && !p.IsReport).ToList();
            });
            EntityList<AxisChangeRecord> htSaveList = new EntityList<AxisChangeRecord>();

            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                foreach (var item in datas.OrderBy(p => p.CollectionTime))
                {
                    var record = records.OrderByDescending(p => p.CollectionTime).FirstOrDefault(p => p.IotEntity == item.IotEntity && p.TaskNo == item.TaskNo && !p.ChangeFlag);
                    if (record == null)
                    {
                        record = new AxisChangeRecord()
                        {
                            IotEntity = item.IotEntity,
                            Factory = item.Factory,
                            TaskNo = item.TaskNo,
                        };
                    }
                    record.MeterCount = item.MeterCount;
                    record.CollectionTime = item.CollectionTime ?? DateTime.Now;
                    record.AxisQty = item.AxisQty;
                    record.ChangeFlag = item.ChangeFlag;

                    RF.Save(record);
                }

                trans.Complete();
            }
        }
    }
}
