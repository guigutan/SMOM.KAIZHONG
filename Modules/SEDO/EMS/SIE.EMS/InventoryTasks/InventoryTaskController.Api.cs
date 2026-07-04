using SIE.Api;
using SIE.Domain.Validation;
using SIE.EMS.InventoryTasks.ApiModels;
using System.Collections.Generic;
using System;
using SIE.EMS.Enums;
using SIE.Domain;
using SIE.EMS.InventoryPlans;
using System.Linq;
using SIE.Equipments.EquipAccounts;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Common.Attachments;
using SIE.Equipments.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Common.Import;
using SIE.Fixtures;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SIE.Fixtures.Models;
using SIE.Common;
using DocumentFormat.OpenXml.EMMA;
using SIE.Core.Common.Controllers;
using SIE.EMS.Common.ApiModels;
using SIE.EMS.Logs;
using System.Text.Json;
using SIE.Common.Catalogs;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务API控制器
    /// </summary>
    public partial class InventoryTaskController : DomainController
    {
        #region 盘点任务-设备
        /// <summary>
        /// 获取设备盘点任务
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <returns>设备盘点任务</returns>
        [ApiService("获取设备盘点任务")]
        [return: ApiReturn("设备盘点任务")]
        public virtual List<InventoryTaskInfo> GetEquipInventoryTaskInfos([ApiParameter("盘点PDA类型")] int pdaType)
        {
            var list = new List<InventoryTaskInfo>();
            var curId = RT.IdentityId;
            var query = Query<InventoryTask>().Where(p => p.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Equipment);
            if (pdaType == 1)
            {
                //只能查询创建人为当前用户或者盘点人页签存在当前用户且有初盘权限的数据
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing);
                query.Exists<InventoryTaskCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.First)) && a.Id == b.InventoryTaskId));
            }
            else if (pdaType == 2)
            {
                //只能查询创建人为当前用户或者盘点人页签存在当前用户且有复盘权限的数据
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.FirstDone || p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing);
                query.Exists<InventoryTaskCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.Second)) && a.Id == b.InventoryTaskId));
            }
            else
            {
                throw new ValidationException("获取设备盘点任务失败，请输入正确的参数".L10N());
            }
            var tasks = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var taskIds = tasks.Select(p => p.Id).ToList();
            var planIds = tasks.Select(p => p.InventoryPlanId).ToList();
            var ranges = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanEquipments(planIds);
            var allTaskEquips = GetInventoryEquipByTaskIds(taskIds);
            var allCounters = GetTaskCountersByTaskIds(taskIds);
            var now = RF.Find<InventoryTask>().GetDbTime();
            foreach (var task in tasks)
            {
                var info = new InventoryTaskInfo();
                info.InventoryTaskId = task.Id;
                info.TaskNo = task.TaskNo;
                info.PlanEndDate = task.PlanEndDate;
                info.InventoryType = task.InventoryType;
                info.ManageDeptName = task.ManageDeptName;
                info.ManageDeptId = task.ManageDeptId ?? 0;
                info.FactoryId = task.FactoryId;
                info.NeedPhoto = task.NeedPhoto;
                info.Remark = task.Remark;
                info.InventoryTaskStatus = task.InventoryTaskStatus;
                info.InventoryTaskStatusName = task.InventoryTaskStatus.ToLabel().L10N();
                info.Progress = task.Percentage.ToString();
                info.IsOverdue = now.Date > task.PlanEndDate;
                var range = ranges.FirstOrDefault(p => p.InventoryPlanId == task.InventoryPlanId);
                if (range != null)//盘点范围
                {
                    info.UseDeptName = range.UseDeptName;
                    info.TypeCategory = range.TypeCategory;
                    info.WarehouseName = range.WarehouseName;
                }
                /*  if (!task.PhotoFilePath.IsNullOrWhiteSpace())
                  {
                      var strs = task.PhotoFilePath.Split('/');
                      var fileName = strs[strs.Length - 1];
                      var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(task.PhotoFilePath, fileName);
                      info.Picture = Convert.ToBase64String(fileBytes);
                  }*/
                var counter = allCounters.FirstOrDefault(p => p.EmployeeId == curId && p.InventoryTaskId == task.Id);
                var taskEquips = allTaskEquips.Where(p => p.InventoryTaskId == task.Id).ToList();
                //计算盘点数量
                GetInventoryCount(info, task, counter, taskEquips, pdaType);
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 计算盘点数量
        /// </summary>
        /// <param name="info">盘点任务信息</param>
        /// <param name="task">盘点任务</param>
        /// <param name="counter">盘点人</param>
        /// <param name="taskEquips">盘点任务设备</param>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        private void GetInventoryCount(InventoryTaskInfo info, InventoryTask task, InventoryTaskCounter counter, List<InventoryTaskEquipment> taskEquips, int pdaType)
        {
            var curId = RT.IdentityId;
            //当前用户为创建人或盘点范围为【全部设备】 
            if (curId == task.CreateBy || (counter != null && counter.InventoryScope == InventoryScope.All))
            {
                if (pdaType == 1)
                {
                    //设备清单中来源为【账内资产】的数量
                    info.NeedInventory = taskEquips.Count(p => p.InventoryAssetSource == InventoryAssetSource.Account);
                    info.NoInventory = taskEquips.Count(p => p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstInventoryResult == null);
                }
                else
                {
                    //设备清单的数量
                    info.NeedInventory = taskEquips.Count;
                    info.NoInventory = taskEquips.Count(p => p.SecondInventoryResult == null);
                }
            }
            else
            {
                //盘点范围为【自有管理】
                if (counter != null && counter.InventoryScope == InventoryScope.Own)
                {
                    if (pdaType == 1)
                    {
                        //设备清单中来源为【账内资产】且【使用责任人】为当前用户的数量
                        info.NeedInventory = taskEquips.Count(p => p.InventoryAssetSource == InventoryAssetSource.Account && (p.OldUserId == curId || p.UserId == curId));
                        info.NoInventory = taskEquips.Count(p => p.InventoryAssetSource == InventoryAssetSource.Account && (p.OldUserId == curId || p.UserId == curId)
                        && p.FirstInventoryResult == null);
                    }
                    else
                    {
                        //设备清单中【使用责任人】或者【初盘人】为当前用户的数量
                        info.NeedInventory = taskEquips.Count(p => p.OldUserId == curId || p.UserId == curId || p.FirstCounterId == curId);
                        info.NoInventory = taskEquips.Count(p => (p.OldUserId == curId || p.UserId == curId || p.FirstCounterId == curId) && p.SecondInventoryResult == null);
                    }
                }
            }
        }

        /// <summary>
        /// 设备盘点执行-扫描设备编码/RFID
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <param name="code">扫描条码</param>
        /// <param name="taskId">盘点任务id</param>
        [ApiService("设备盘点执行-扫描设备编码/RFID")]
        [return: ApiReturn("设备盘点执行扫描结果")]
        public virtual InventoryEquipInfo EquipInventoryExecute([ApiParameter("盘点PDA类型")] int pdaType, [ApiParameter("扫描条码")] string code,
            [ApiParameter("盘点任务id")] double taskId)
        {
            if (code == null || code.Length <= 0)
            {
                throw new ValidationException("请输入设备编码".L10N().FormatArgs(code));
            }
            var info = new InventoryEquipInfo();
            info.PdaType = pdaType;
            info.InventoryTaskId = taskId;
            info.EquipCode = code;
            EquipAccount equip = null;
            //扫描的编码作为设备编码或RFID获取设备台账
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                equip = Query<EquipAccount>().Where(p => p.Code == code || p.RFID == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            }
            if (equip == null)
            {
                throw new ValidationException("设备编码不存在【{0}】".L10N().FormatArgs(code));
            }
            //盘点任务的设备清单
            var taskEquipment = Query<InventoryTaskEquipment>().Where(p => p.InventoryTaskId == taskId && p.EquipmentCode == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //

            if (pdaType == 1)//初盘
            {
                if (taskEquipment != null && taskEquipment.FirstInventoryResult != null)
                {
                    throw new ValidationException("当前设备【{0}】已盘点，不能重复盘点".L10N().FormatArgs(code));
                }
                //获取不到设备台账时，跳转到【新增盘盈】页面；能够获取到设备台账时，将设备台账字段展示在界面
                if (equip == null)
                {
                    info.IsHaveEquip = false;
                }
                else
                {
                    SetInventoryAccountInfo(info, equip, taskEquipment);
                }
            }
            else if (pdaType == 2)//复盘
            {
                if (taskEquipment != null && taskEquipment.SecondInventoryResult != null)
                {
                    throw new ValidationException("当前设备【{0}】已盘点，不能重复盘点".L10N().FormatArgs(code));
                }
                //获取不到设备台账时，跳转到【新增盘盈】页面，传入扫描的编码和设备清单的信息（如果设备编码存在于盘点任务的设备清单）
                if (equip == null)
                {
                    info.IsHaveEquip = false;
                    if (taskEquipment != null)
                    {
                        SetInventoryEquipInfo(info, taskEquipment);
                    }
                }
                else
                {
                    //能够获取到设备台账时，如果设备在盘点任务的设备清单里且【初盘结果】不为【盘亏】，界面展示的字段值来源设备清单的【实盘XX】字段
                    if (taskEquipment != null && taskEquipment.FirstInventoryResult != InventoryResult.Loss)
                    {
                        info.IsHaveEquip = true;
                        SetInventoryEquipInfo(info, taskEquipment);
                    }
                    //能够获取到设备台账时，如果设备不在盘点任务的设备清单，或者初盘结果为【盘亏】，则获取设备台账字段展示在界面
                    if (taskEquipment == null || taskEquipment.FirstInventoryResult == InventoryResult.Loss)
                    {
                        SetInventoryAccountInfo(info, equip, taskEquipment);
                    }
                }
            }
            else
            {
                throw new ValidationException("设备盘点执行失败，请输入正确的参数".L10N());
            }
            return info;
        }

        /// <summary>
        /// 设置盘点任务设备信息
        /// </summary>
        /// <param name="info">盘点任务设备信息</param>
        /// <param name="equip">设备台账</param>
        /// <param name="taskEquipment">设备清单</param>
        private void SetInventoryAccountInfo(InventoryEquipInfo info, EquipAccount equip, InventoryTaskEquipment taskEquipment)
        {
            info.IsHaveEquip = true;
            info.EquipId = equip.Id;
            info.EquipName = equip.Name;
            info.TypeCategory = equip.EquipTypeCategory;
            info.EquipTypeId = equip.EquipTypeViewId;
            info.EquipModelId = equip.EquipModelId;
            info.UseLevel = equip.UseLevel;
            info.ManageDeptId = equip.ManageDepartmentId;
            info.ManageDeptName = equip.ManageDepartmentName;
            info.UseDeptId = equip.UseDepartmentId;
            info.UseDeptName = equip.UseDepartmentName;
            info.UserId = equip.UserId;
            info.UserName = equip.User?.Name;
            info.AccountUseState = (int)equip.UseState;
            info.AccountUseStateDisplay = equip.UseState.ToLabel().L10N();
            info.AccountState = (int)equip.State;
            info.AccountStateDisplay = equip.State.ToLabel().L10N();
            info.WorkShopId = equip.WorkShopId;
            info.WorkShopName = equip.WorkShopName;
            info.ResourceId = equip.ResourceId;
            info.ResourceName = equip.ResourceName;
            info.WarehouseId = equip.WarehouseId;
            info.WarehouseName = equip.WarehouseName;
            info.StorageLocationId = equip.StorageLocationId;
            info.StorageLocationName = equip.StorageLocationName;
            info.Location = equip.InstallationLocation;
            if (taskEquipment != null && !taskEquipment.PhotoFilePath.IsNullOrWhiteSpace())
            {
                var strs = taskEquipment.PhotoFilePath.Split('/');
                var fileName = strs[strs.Length - 1];
                var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(taskEquipment.PhotoFilePath, fileName);
                info.PictureFileName = fileName;
                info.Picture = Convert.ToBase64String(fileBytes);
            }
        }

        /// <summary>
        /// 设置盘点任务设备信息
        /// </summary>
        /// <param name="info">盘点任务设备信息</param>
        /// <param name="taskEquipment">设备清单</param>
        private void SetInventoryEquipInfo(InventoryEquipInfo info, InventoryTaskEquipment taskEquipment)
        {
            //info.IsHaveEquip = true;
            info.EquipId = taskEquipment.EquipAccountId ?? 0;
            info.EquipName = taskEquipment.EquipmentName;
            info.TypeCategory = taskEquipment.TypeCategory;
            info.EquipTypeId = taskEquipment.EquipTypeId;
            info.EquipModelId = taskEquipment.EquipModelId;
            info.ManageDeptId = taskEquipment.RealManageDeptId;
            info.ManageDeptName = taskEquipment.RealManageDeptName;
            info.UseDeptId = taskEquipment.RealUseDeptId;
            info.UseDeptName = taskEquipment.RealUseDeptName;
            info.UserId = taskEquipment.UserId;
            info.UserName = taskEquipment.UserName;
            info.AccountUseState = (int?)taskEquipment.AccountUseState;
            info.AccountUseStateDisplay = taskEquipment.AccountUseState.ToLabel().L10N();
            info.AccountState = (int?)taskEquipment.AccountState;
            info.AccountStateDisplay = taskEquipment.AccountState.ToLabel().L10N();
            info.WorkShopId = taskEquipment.RealWorkShopId;
            info.WorkShopName = taskEquipment.RealWorkShopName;
            info.ResourceId = taskEquipment.RealResourceId;
            info.ResourceName = taskEquipment.RealResourceName;
            info.WarehouseId = taskEquipment.RealWarehouseId;
            info.WarehouseName = taskEquipment.RealWarehouseName;
            info.StorageLocationId = taskEquipment.StorageLocationId;
            info.StorageLocationName = taskEquipment.StorageLocationName;
            info.Location = taskEquipment.RealLocation;
            if (!taskEquipment.PhotoFilePath.IsNullOrWhiteSpace())
            {
                var strs = taskEquipment.PhotoFilePath.Split('/');
                var fileName = strs[strs.Length - 1];
                var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(taskEquipment.PhotoFilePath, fileName);
                info.PictureFileName = fileName;
                info.Picture = Convert.ToBase64String(fileBytes);
            }
        }

        /// <summary>
        /// 设备盘点执行-提交
        /// </summary>
        /// <param name="info">设备盘点执行扫描结果</param>
        [ApiService("设备盘点执行-提交")]
        public virtual void EquipInventorySubmit([ApiParameter("设备盘点执行扫描结果")] InventoryEquipInfo info)
        {
            if (info.EquipId <= 0)
            {
                throw new ValidationException("数据异常，设备id不正确".L10N());
            }
            if (info.EquipCode.IsNullOrWhiteSpace())
            {
                throw new ValidationException("数据异常，设备编码为空".L10N());
            }
            var model = SetAddProfitViewModel(info);
            AddProfit(model);
        }

        /// <summary>
        /// 设备新增盘盈-提交
        /// </summary>
        /// <param name="info">设备新增盘盈信息</param>
        [ApiService("设备新增盘盈-提交")]
        public virtual void EquipProfitSubmit([ApiParameter("设备新增盘盈信息")] InventoryEquipInfo info)
        {
            //if (info.Picture.IsNullOrWhiteSpace())
            //{
            //    throw new ValidationException("上传的图片不能为空".L10N());
            //}
            //if (info.PictureFileName.IsNullOrWhiteSpace())
            //{
            //    throw new ValidationException("上传的图片文件名不能为空".L10N());
            //}
            var model = SetAddProfitViewModel(info);
            AddProfit(model);
        }

        /// <summary>
        /// 生成新增盘盈信息
        /// </summary>
        /// <param name="info">盘点任务设备信息</param>
        /// <returns>新增盘盈信息</returns>
        private AddProfitViewModel SetAddProfitViewModel(InventoryEquipInfo info)
        {
            var model = new AddProfitViewModel();
            model.InventoryTaskId = info.InventoryTaskId;
            model.AddProfitUIState = info.EquipId > 0 ? AddProfitUIState.A : AddProfitUIState.B;
            model.NoHaveCode = info.EquipCode.IsNullOrWhiteSpace();
            model.EquipmentCode = info.EquipCode;
            model.EquipmentName = info.EquipName;
            model.AccountUseState = (Core.Enums.AccountUseState?)info.AccountUseState;
            model.AccountState = (Core.Enums.AccountState?)info.AccountState;
            model.EquipModelId = info.EquipModelId;
            model.EquipTypeId = info.EquipTypeId;
            model.TypeCategory = info.TypeCategory;
            model.UseLevel = info.UseLevel;
            model.UseDeptId = info.UseDeptId;
            model.UserId = info.UserId;
            model.RealWorkShopId = info.WorkShopId;
            model.RealResourceId = info.ResourceId;
            model.RealWarehouseId = info.WarehouseId;
            model.StorageLocationId = info.StorageLocationId;
            model.RealLocation = info.Location;
            if (!info.Picture.IsNullOrWhiteSpace())
            {
                var bytes = Convert.FromBase64String(info.Picture);
                const string prePath = "InventoryPlanPhoto";
                var path = $"{prePath}/{Guid.NewGuid()}";
                RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(info.PictureFileName, bytes, path);
                model.PhotoFilePath = $"{path}/{info.PictureFileName}";
            }
            return model;
        }

        /// <summary>
        /// 获取盘点任务的设备清单
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <param name="taskId">盘点任务id</param>
        /// <returns>盘点任务的设备清单</returns>
        [ApiService("获取盘点任务的设备清单")]
        [return: ApiReturn("盘点任务的设备清单")]
        public virtual List<InventoryTaskEquipInfo> GetInventoryTaskEquipments([ApiParameter("盘点PDA类型")] int pdaType, [ApiParameter("盘点任务id")] double taskId)
        {
            if (pdaType != 1 && pdaType != 2)
            {
                throw new ValidationException("获取盘点任务的设备清单失败，请输入正确的参数".L10N());
            }
            var task = GetById<InventoryTask>(taskId);
            if (task == null)
            {
                throw new ValidationException("获取设备清单失败，盘点任务不存在".L10N());
            }
            var allTaskEquips = RT.Service.Resolve<InventoryTaskController>().GetTaskEquipments(task, null, null);
            var list = new List<InventoryTaskEquipInfo>();
            foreach (var item in allTaskEquips)
            {
                var info = new InventoryTaskEquipInfo();
                info.InventoryTaskEquipmentId = item.Id;
                info.EquipCode = item.EquipmentCode;
                info.EquipName = item.EquipmentName;
                if (pdaType == 1)//初盘
                {
                    info.IsFinishInventory = item.FirstInventoryResult.HasValue;
                    info.InventoryResult = item.FirstInventoryResult.HasValue ? item.FirstInventoryResult.ToLabel() : "未盘点".L10N();
                    info.InventoryResultValue = item.FirstInventoryResult.HasValue ? (int)item.FirstInventoryResult.Value : -1;
                }
                else//复盘
                {
                    info.IsFinishInventory = item.SecondInventoryResult.HasValue;
                    if (item.SecondInventoryResult == null)
                    {
                        info.InventoryResult = item.FirstInventoryResult.HasValue ? item.FirstInventoryResult.ToLabel() : "未盘点".L10N();
                        info.InventoryResultValue = item.FirstInventoryResult.HasValue ? (int)item.FirstInventoryResult.Value : -1;

                    }
                    else
                    {
                        info.InventoryResult = item.SecondInventoryResult.ToLabel();
                        info.InventoryResultValue = (int)item.SecondInventoryResult;
                    }
                }
                //执行类型为【盲盘】时，待盘点不展示数据
                if (task.InventoryExecuteType == InventoryExecuteType.Blind && pdaType == 1 && !info.IsFinishInventory)
                {
                    continue;
                }
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 获取盘点任务设备清单的更多信息
        /// </summary>
        /// <param name="taskEquipId">设备清单id</param>
        /// <returns>盘点任务设备清单的更多信息</returns>
        [ApiService("获取盘点任务设备清单的更多信息")]
        [return: ApiReturn("盘点任务设备清单的更多信息")]
        public virtual InventoryTaskEquipMoreInfo GetTaskEquipMoreInfo([ApiParameter("设备清单id")] double taskEquipId)
        {
            var taskEquipment = Query<InventoryTaskEquipment>().Where(p => p.Id == taskEquipId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (taskEquipment == null)
            {
                throw new ValidationException("盘点任务设备清单不存在".L10N());
            }
            //状态为【未盘点】的数据；查看更多的字段取当前XXX的字段值
            //状态为【已盘点】的数据；查看更多页面的字段取实盘XXX的字段值
            var result = taskEquipment.SecondInventoryResult.HasValue ? taskEquipment.SecondInventoryResult : taskEquipment.FirstInventoryResult;
            var info = new InventoryTaskEquipMoreInfo();
            info.InventoryResult = result.HasValue ? result.ToLabel() : "未盘点".L10N();
            info.EquipCode = taskEquipment.EquipmentCode;
            info.EquipName = taskEquipment.EquipmentName;
            if (taskEquipment.InventoryStatus == InventoryStatus.Done)
            {
                info.UseDeptName = taskEquipment.RealManageDeptName;
                info.UserName = taskEquipment.UserName;
                if (taskEquipment.AccountUseState.HasValue)
                {
                    info.AccountUseStateDisplay = taskEquipment.AccountUseState.ToLabel().L10N();
                }
                if (taskEquipment.AccountState.HasValue)
                {
                    info.AccountStateDisplay = taskEquipment.AccountState.ToLabel().L10N();
                }
                info.WorkShopName = taskEquipment.RealWorkShopName;
                info.WarehouseName = taskEquipment.RealWarehouseName;
                info.ResourceName = taskEquipment.RealResourceName;
                info.StorageLocationName = taskEquipment.StorageLocationName;
                info.Location = taskEquipment.RealLocation;
            }
            else
            {
                info.UseDeptName = taskEquipment.OldUseDeptName;
                info.UserName = taskEquipment.OldUserName;
                info.AccountUseStateDisplay = taskEquipment.OldAccountUseState.ToLabel();
                info.AccountStateDisplay = taskEquipment.OldAccountState.ToLabel();
                info.WorkShopName = taskEquipment.OldWorkShopName;
                info.WarehouseName = taskEquipment.OldWarehouseCode;
                info.ResourceName = taskEquipment.OldResourceName;
                info.StorageLocationName = taskEquipment.OldStorageLocationCode;
                info.Location = taskEquipment.OldLocation;
            }
            return info;
        }

        /// <summary>
        /// 设备清单-盘亏
        /// </summary>
        /// <param name="taskEquipId">设备清单id</param>
        [ApiService("设备清单-盘亏")]
        public virtual void InventoryTaskEquipLoss([ApiParameter("设备清单id")] double taskEquipId)
        {
            var taskEquipment = GetById<InventoryTaskEquipment>(taskEquipId);
            if (taskEquipment == null)
            {
                throw new ValidationException("盘点任务设备清单不存在".L10N());
            }
            var task = GetById<InventoryTask>(taskEquipment.InventoryTaskId);
            if (task == null)
            {
                throw new ValidationException("盘点任务不存在".L10N());
            }
            //更新对应设备清单字段
            taskEquipment.InventoryStatus = InventoryStatus.Done;
            taskEquipment.RealManageDeptId = null;
            taskEquipment.RealUseDeptId = null;
            taskEquipment.AccountUseState = null;
            taskEquipment.AccountState = null;
            taskEquipment.UserId = null;
            taskEquipment.RealWorkShopId = null;
            taskEquipment.RealResourceId = null;
            taskEquipment.RealWarehouseId = null;
            taskEquipment.StorageLocationId = null;
            taskEquipment.RealLocation = string.Empty;
            var now = RF.Find<InventoryTask>().GetDbTime();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    taskEquipment.FirstInventoryResult = InventoryResult.Loss;
                    taskEquipment.FirstCounterId = RT.IdentityId;
                    taskEquipment.InventoryDateTime = now;

                    //更新盘点任务和盘点计划的盘点进度
                    UpdatePercentage(new EntityList<InventoryTask> { task });
                }
                else
                {
                    taskEquipment.SecondInventoryResult = InventoryResult.Loss;
                    taskEquipment.SecondCounterId = RT.IdentityId;
                    taskEquipment.SecondDateTime = now;
                }
                RF.Save(taskEquipment);
                trans.Complete();
            }
        }
        #endregion

        #region 盘点任务-工治具
        /// <summary>
        /// 获取工治具盘点任务
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <returns>设备盘点任务</returns>
        [ApiService("获取工治具盘点任务")]
        [return: ApiReturn("工治具盘点任务")]
        public virtual List<FixtureTaskInfo> GetFixtureInventoryTaskInfos([ApiParameter("盘点PDA类型")] int pdaType)
        {
            var list = new List<FixtureTaskInfo>();
            //var curId = RT.IdentityId;
            var query = Query<InventoryTask>().Where(p => p.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Fixture && p.InventoryTaskStatus != InventoryTaskStatus.Completed);
            if (pdaType == 1)
            {
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing);
            }
            else if (pdaType == 2)
            {
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing || p.InventoryTaskStatus == InventoryTaskStatus.FirstDone);
            }
            //if (pdaType == 1)
            //{
            //    //只能查询创建人为当前用户或者盘点人页签存在当前用户且有初盘权限的数据
            //    query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing);
            //    query.Exists<InventoryTaskCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.First)) && a.Id == b.InventoryTaskId));
            //}
            //else if (pdaType == 2)
            //{
            //    //只能查询创建人为当前用户或者盘点人页签存在当前用户且有复盘权限的数据
            //    query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.FirstDone || p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing);
            //    query.Exists<InventoryTaskCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.Second)) && a.Id == b.InventoryTaskId));
            //}
            //else
            //{
            //    throw new ValidationException("获取设备盘点任务失败，请输入正确的参数".L10N());
            //}
            var tasks = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //var taskIds = tasks.Select(p => p.Id).ToList();
            //var planIds = tasks.Select(p => p.InventoryPlanId).ToList();
            var now = RF.Find<InventoryTask>().GetDbTime();
            foreach (var task in tasks)
            {
                var info = new FixtureTaskInfo();
                info.InventoryTaskId = task.Id;
                info.TaskNo = task.TaskNo;
                info.PlanEndDate = task.PlanEndDate;
                info.InventoryType = task.InventoryType;
                info.FactoryId = task.FactoryId;
                info.NeedPhoto = task.NeedPhoto;
                info.Remark = task.Remark;
                info.InventoryTaskStatus = task.InventoryTaskStatus;
                info.InventoryTaskStatusName = task.InventoryTaskStatus.ToLabel().L10N();
                info.IsOverdue = now.Date > task.PlanEndDate;
                info.Percentage = task.Percentage;
                info.InventoryExecuteType = task.InventoryExecuteType;
                var range = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanFixture(task.InventoryPlanId);
                if (range != null)//盘点范围(工治具)
                {
                    info.FixtureTypes = range.FixtureTypes;
                    info.FixtureModels = range.FixtureModels;
                    info.FixtureEncodes = range.FixtureEncodes;
                }
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 工治具盘点执行-扫描设备编码/RFID
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <param name="code">扫描条码</param>
        /// <param name="taskId">盘点任务id</param>
        [ApiService("工治具盘点执行-扫描设备编码/RFID")]
        [return: ApiReturn("工治具盘点执行扫描结果")]
        public virtual List<InventoryFixtureExcuteInfo> FixtureInventoryExecute([ApiParameter("盘点PDA类型")] int pdaType, [ApiParameter("扫描条码")] string code,
            [ApiParameter("盘点任务id")] double taskId)
        {
            List<InventoryFixtureExcuteInfo> result = new List<InventoryFixtureExcuteInfo>();
            var task = RF.GetById<InventoryTask>(taskId, new EagerLoadOptions().LoadWithViewProperty());
            if (task == null)
            {
                throw new ValidationException("盘点任务信息丢失，请确认".L10N());
            }
            //扫描的编码作为工治具编码
            EntityList<InventoryTaskFixtureEncode> fixtureEncodes;
            //扫描的编码作为工治具ID
            EntityList<InventoryTaskFixtureIdAccount> fixtureIdAccounts;
            if (!code.IsNullOrEmpty())
            {
                //包含工治具编码和工治具ID的
                fixtureEncodes = Query<InventoryTaskFixtureEncode>().Where(p => (p.FixtureEncode.Code == code)
             && p.InventoryTaskId == taskId)
                   .WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus != InventoryStatus.Done)
                   .WhereIf(pdaType == 2, m => m.InventoryStatus == InventoryStatus.Done)
                   .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var fixcodeIds = fixtureEncodes.Where(p => p.ManageMode == ManageMode.Number).Select(p => p.FixtureEncodeId).ToList();
                var querFixAccounId = Query<InventoryTaskFixtureIdAccount>().Where(p => p.InventoryTaskId == taskId)
                  .WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus != InventoryStatus.Done)
                 .WhereIf(pdaType == 2, m => m.SecondResult == null && m.InventoryStatus == InventoryStatus.Done);
                if (fixcodeIds.Count >0)
                {
                    querFixAccounId.Where(p => fixcodeIds.Contains(p.FixtureEncodeId) || p.Sn == code);
                }
                else
                {
                    querFixAccounId.Where(p => p.Sn == code);
                }
                //获取工治具ID
                fixtureIdAccounts = querFixAccounId
                  .WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus != InventoryStatus.Done)
                 .WhereIf(pdaType == 2, m => m.SecondResult == null && m.InventoryStatus == InventoryStatus.Done)
                 .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                if (!fixtureEncodes.Any() && !fixtureIdAccounts.Any())
                {
                    throw new ValidationException("扫描条码无相关信息，请重新输入".L10N());
                }
            }
            else
            {
                fixtureEncodes = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == taskId).WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus != InventoryStatus.Done)
                   .WhereIf(pdaType == 2, m => m.InventoryStatus == InventoryStatus.Done).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                fixtureIdAccounts = Query<InventoryTaskFixtureIdAccount>().WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus != InventoryStatus.Done)
                   .WhereIf(pdaType == 2, m => m.SecondResult == null && m.InventoryStatus == InventoryStatus.Done).Where(p => p.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }


            //盘点任务的设备清单
            if (pdaType == 1)//初盘
            {
                var firstInvList = fixtureEncodes.Where(m => !m.FirstResult.HasValue || m.InventoryStatus != InventoryStatus.Done).ToList();
                //工治具编码设备清单
                foreach (var firstInv in firstInvList)
                {
                    //工治具ID管控
                    List<InventoryTaskFixtureIdAccount> fixtureIdFirAccounts = new List<InventoryTaskFixtureIdAccount>();
                    if (firstInv.ManageMode == ManageMode.Number)
                    {
                        fixtureIdFirAccounts = fixtureIdAccounts.Where(p => p.FixtureEncodeId == firstInv.FixtureEncodeId && !p.FirstResult.HasValue || p.InventoryStatus != InventoryStatus.Done).ToList();
                    }
                    SetDetailInfo(pdaType, task, result, firstInv, fixtureIdAccounts.ToList());
                }
                //if (fixtureIdAccounts.Count > 0)
                //{
                //    var fixtureEncodeId = fixtureIdAccounts.FirstOrDefault().FixtureEncodeId;
                //    var fixtureCode = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == taskId && p.FixtureEncodeId == fixtureEncodeId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                //    SetDetailInfo(pdaType, task, result, fixtureCode, fixtureIdAccounts.ToList());
                //}

            }
            else if (pdaType == 2)//复盘
            {
                var secondInvList = fixtureEncodes.Where(m => m.FirstResult.HasValue).ToList();
                foreach (var secondInv in secondInvList)
                {
                    //工治具ID管控
                    List<InventoryTaskFixtureIdAccount> fixtureIdSecAccounts = new List<InventoryTaskFixtureIdAccount>();
                    if (secondInv.ManageMode == ManageMode.Number)
                    {
                        fixtureIdSecAccounts = fixtureIdAccounts.Where(p => p.FixtureEncodeId == secondInv.FixtureEncodeId && !p.SecondResult.HasValue).ToList();
                    }
                    else
                    {
                        if (secondInv.SecondResult.HasValue)
                        {
                            continue;
                        }
                    }
                    SetDetailInfo(pdaType, task, result, secondInv, fixtureIdSecAccounts);
                }
            }
            else
            {
                throw new ValidationException("备件盘点执行失败，请输入正确的参数".L10N());
            }
            return result;
        }


        /// <summary>
        /// 设置盘点明细数据
        /// </summary>
        /// <param name="pdaType"></param>
        /// <param name="task"></param>
        /// <param name="result"></param>
        /// <param name="firstInv"></param>
        /// <param name="inventoryTaskFixtureIds">工治具序列号ID</param>
        private void SetDetailInfo(int pdaType, InventoryTask task, List<InventoryFixtureExcuteInfo> result, InventoryTaskFixtureEncode firstInv, List<InventoryTaskFixtureIdAccount> inventoryTaskFixtureIds)
        {
            if (firstInv.ManageMode == ManageMode.Number)
            {
                foreach (var item in inventoryTaskFixtureIds)
                {
                    var info = new InventoryFixtureExcuteInfo(true);
                    info.Id = firstInv.Id;
                    info.AccountId = item.Id;
                    info.PdaType = pdaType;
                    info.InventoryTaskId = task.Id;
                    info.Code = item.FixtureEncode?.Code;
                    info.ModelName = item.FixtureEncode?.FixtureModel?.Name;
                    info.IsBlind = task.InventoryExecuteType == InventoryExecuteType.Blind;
                    info.Sn = item.Sn;
                    //info.StockQty = 1;
                    //info.OnlineQty = item.OnlineQty;
                    info.ManageMode = (int)firstInv.ManageMode;
                    info.CountStatus = 2;
                    if (firstInv.ManageMode == ManageMode.Number)
                    {
                        info.QualityState = (int)(item.FixtureStatus == FixtureStatus.InStorage ? PdaQualityState.Pass : PdaQualityState.NG);
                    }
                    info.InventoryType = ((int)task.InventoryExecuteType).ToString();
                    result.Add(info);
                }
            }
            else
            {
                var info = new InventoryFixtureExcuteInfo(true);
                info.Id = firstInv.Id;
                info.PdaType = pdaType;
                info.InventoryTaskId = task.Id;
                info.Code = firstInv.FixtureEncode?.Code;
                info.ModelName = firstInv.FixtureEncode?.FixtureModel?.Name;
                info.IsBlind = task.InventoryExecuteType == InventoryExecuteType.Blind;
                info.Sn = firstInv.Sn;
                info.StockQty = firstInv.StockQty ?? 0;
                info.StockPassQty = firstInv.StockPassQty ?? 0;
                info.StockNgQty = firstInv.StockNgQty ?? 0;
                info.OnlineQty = firstInv.OnlineQty;
                info.ManageMode = (int)firstInv.ManageMode;
                info.CountStatus = 1;
                info.InventoryType = ((int)task.InventoryExecuteType).ToString();
                result.Add(info);
            }

        }


        /// <summary>
        /// 获取工治具设备清单界面待盘点列表
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        [ApiService("获取工治具清单列表")]
        [return: ApiReturn("返回工治具清单")]
        public virtual List<InventoryFixtureListInfo> GetInventoryFixtureList([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("任务Id")] double taskId)
        {
            var result = new List<InventoryFixtureListInfo>();
            if (PdaType <= 0 || taskId <= 0)
            {
                throw new ValidationException("获取备件清单列表失败,请输入正确的参数".L10N());
            }
            //未盘点
            GetNotInvFixtureInfoList(PdaType, taskId, result);
            //已盘点
            GetInvFixtureDoneInfo(PdaType, taskId, result);

            return result;
        }


        /// <summary>
        /// 获取未盘点的工治具清单
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        /// <param name="result"></param>
        private void GetNotInvFixtureInfoList(int PdaType, double taskId, List<InventoryFixtureListInfo> result)
        {
            if (PdaType == 1)//初盘
            {
                //取所有未盘点的明细
                var allInvFixtures = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == taskId && p.InventoryStatus != InventoryStatus.Done)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (allInvFixtures.Any())
                {
                    GetFixtureListInfo(taskId, PdaType, result, allInvFixtures);
                }
            }
            if (PdaType == 2)//复盘
            {
                //取所有未盘点的明细
                var inventoryTaskFixtures = Query<InventoryTaskFixtureEncode>().Where(p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.SecondResult == null)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (inventoryTaskFixtures.Any())
                {
                    GetFixtureListInfo(taskId, PdaType, result, inventoryTaskFixtures);
                }
            }
        }


        /// <summary>
        /// 获取工治具列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="PdaType">1初盘 2复盘</param>
        /// <param name="result"></param>
        /// <param name="fixtureEncodes"></param>
        private void GetFixtureListInfo(double taskId, int PdaType, List<InventoryFixtureListInfo> result, EntityList<InventoryTaskFixtureEncode> fixtureEncodes)
        {
            var fixtureEncodeIds = fixtureEncodes.Select(p => p.FixtureEncodeId).ToList();
            var fixtureAccounts = fixtureEncodeIds.SplitContains(ids =>
            {
                return Query<InventoryTaskFixtureIdAccount>().Where(p => ids.Contains(p.FixtureEncodeId) && p.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions()
                   .LoadWithViewProperty());
            });
            fixtureEncodes.ForEach(
                 item =>
                 {
                     var res = new InventoryFixtureListInfo()
                     {
                         PdaType = PdaType,
                         ManageMode = (int)item.ManageMode,
                         Id = item.Id,
                         InventoryTaskId = taskId,
                         FixtureEncodeId = item.FixtureEncodeId,
                         FixtureEncodeCode = item.FixtureEncode?.Code,
                         Sn = item.Sn,
                         ModelName = item.FixtureEncode?.FixtureModel?.Name,
                         Total = item.Total,
                         NotInvQty = PdaType == 1 ? fixtureAccounts.Where(p => p.FixtureEncodeId == item.FixtureEncodeId && p.FirstResult == null).Count() : fixtureAccounts.Where(p => p.FixtureEncodeId == item.FixtureEncodeId && p.SecondResult == null).Count()
                     };
                     result.Add(res);
                 }
                );
        }


        /// <summary>
        /// 获取工治具清单列表(已盘点)
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private void GetInvFixtureDoneInfo(int PdaType, double taskId, List<InventoryFixtureListInfo> result)
        {
            //取所有已有初盘/复盘结果的明细
            var fixtureEncodes = Query<InventoryTaskFixtureEncode>()
                .WhereIf(PdaType == 1, p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.FirstResult != null)
                .WhereIf(PdaType == 2, p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.SecondResult != null)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            fixtureEncodes.ForEach(item =>
            {
                var res = new InventoryFixtureListInfo()
                {
                    ManageMode = (int)item.ManageMode,
                    IsFinishInventory = true,
                    Id = item.Id,
                    InventoryTaskId = taskId,
                    InvResult = item.FirstResult.Value.ToLabel(),
                    PdaType = PdaType,
                    Sn = item.Sn,
                    FixtureEncodeCode = item.FixtureEncode.Code,
                    FixtureEncodeId = item.FixtureEncodeId,
                    ModelName = item.ModelName
                };
                result.Add(res);
            });
        }


        /// <summary>
        /// 工治具新增盘盈扫描
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="code"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [ApiService("工治具新增盘盈扫描")]
        [return: ApiReturn("")]
        public virtual AddFixtureProfitViewModel ScandInvFixtureAddInfo([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("扫描的条码")] string code, [ApiParameter("扫描的条码")] double taskId)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ValidationException("请输入工治具编码/工治具ID！".L10N());
            }
            if (PdaType <= 0)
            {
                throw new ValidationException("参数异常，请检查页面参数".L10N());
            }
            if (taskId <= 0)
            {
                throw new ValidationException("盘点任务ID参数异常，请检查页面参数".L10N());
            }

            var result = new AddFixtureProfitViewModel();
            result.PdaType = PdaType;
            result.InventoryTaskId = taskId;
            var task = RF.GetById<InventoryTask>(taskId);
            //获取盘点范围
         /*   var inventoryPlanFixture = Query<InventoryPlanFixture>().Where(m => m.InventoryPlanId == task.InventoryPlanId).FirstOrDefault();
            //查询工治具编码存不存在
            var fixtureAccounts = RT.Service.Resolve<InventoryPlanController>().GetInventoryFixtureAccount(inventoryPlanFixture);*/
         var fixtureAccount = Query<InventoryTaskFixtureEncode>().Where(p => p.FixtureEncode.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (fixtureAccount == null)
            {
                throw new ValidationException("盘点范围获取不到工治具数据".L10N());
            }
            else if(fixtureAccount.ManageMode == ManageMode.Code)
            {
                throw new ValidationException("【{0}】管控方式为编码管控，不允许手动新增盘盈".FormatArgs(code));
            }
            else
            {

            }
          
            if (fixtureAccount != null)
            {
                result.FixtureEncode = fixtureAccount.FixtureEncode;
                result.ModelName = fixtureAccount.ModelName;
                result.Encode = fixtureAccount.FixtureEncode.Code;
                result.ManageMode = fixtureAccount.ManageMode;
                return result;
            }
            else
            {
                throw new ValidationException("扫描的条码【{0}】获取不到数据".L10nFormat(code));
            }

        }


        /// <summary>
        /// 生成序列号
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [ApiService("生成序列号")]
        [return: ApiReturn("")]
        public virtual string GetBatchNumberNoOrSn()
        {
             var sn = RT.Service.Resolve<CommonController>().GetNo<FixtureIDAccount>("工治具ID");

            var exsitedSn = Query<FixtureAccount>().Where(m => m.Code == sn).FirstOrDefault();
            if (exsitedSn != null)
            {
                throw new ValidationException("序列号{0}已存在".L10nFormat(exsitedSn.Code));
            }
            return sn;
        }

        /// <summary>
        /// 提交备件盘盈新增
        /// </summary>
        /// <param name="model"></param>
        [ApiService("提交备件盘盈新增")]
        [return: ApiReturn("")]
        public virtual void FixtureInventoryProfitSubmit([ApiParameter("盘盈新增数据")] AddFixtureProfitViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("新增盘盈执行失败，请输入正确的参数".L10N());
            }
            /*if (model.ManageMode == ManageMode.Number)
            {
                if ((model.GoodQty == 1 && model.NgQty == 1) || (model.GoodQty == 0 && model.NgQty == 0))
                {
                    throw new ValidationException("序列号管控时，实盘良品数、实盘不良品数只能一个值为0,另一个值为1".L10N());
                }
                model.IsGood = model.GoodQty == 1;
                model.IsNg = model.NgQty == 1;
            }*/
           AddFixtureProfit(model);
        }


        /// <summary>
        /// 获取工治具盘点清单(未盘点)查看更多
        /// </summary>
        /// <param name="PdaType">1-初盘 2复盘</param>
        /// <param name="taskId">盘点任务Id</param>
        /// <param name="fixtureId">工治具Id</param>
        /// <returns></returns>
        [ApiService(" 获取工治具盘点清单(未盘点)-查看更多列表-ID管理")]
        [return: ApiReturn("返回未盘点清单的更多信息")]
        public virtual List<InventoryFixtureListInfo> GetInvenFixtureMoreInfo([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("任务Id")] double taskId, [ApiParameter("工治具Id")] double fixtureId)
        {
            var result = new List<InventoryFixtureListInfo>();
            if (PdaType <= 0 || taskId <= 0)
            {
                throw new ValidationException("获取工治具清单详细列表失败,请输入正确的参数".L10N());
            }
            var details = Query<InventoryTaskFixtureIdAccount>().Where(p => p.InventoryTaskId == taskId && p.FixtureEncodeId == fixtureId)
                .WhereIf(PdaType == 1, p => p.FirstResult == null)
                .WhereIf(PdaType == 2, p => p.FirstResult != null && p.SecondResult == null)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            details.ForEach(item =>
            {
                var res = new InventoryFixtureListInfo();
                res.FixtureEncodeCode = item.FixtureEncode.Code;
                res.ModelName = item.FixtureEncode.FixtureModel.Name;
                res.Sn = item.Sn;
                res.PdaType = PdaType;
                res.FixtureEncodeId = item.FixtureEncodeId;
                res.InventoryTaskId = item.InventoryTaskId;
                res.ManageMode = 5;
                res.Id = item.Id;
                res.QualityList.ForEach(it =>
                {
                    if (it.Value == 5 && item.FixtureStatus == FixtureStatus.InStorage)
                    { it.isBoole = true; }
                    if (it.Value == 10 && item.FixtureStatus == FixtureStatus.OnLine)
                    {
                        it.isBoole = true;
                    }
                });
                result.Add(res);
            });

            return result;
        }

        /// <summary>
        /// 工治具盘点提交
        /// </summary>
        /// <param name="pdaResultInfo"></param>
        /// <returns></returns>
        [ApiService("工治具盘点执行-提交")]
        [return: ApiReturn("")]
        public virtual void FixtureInventorySubmit([ApiParameter("提交结果")] List<InventoryFixtureExcuteInfo> pdaResultInfo)
        {
            if (!pdaResultInfo.Any())
            {
                throw new ValidationException("工治具盘点执行失败，请输入正确的参数".L10N());
            }
            var taskId = pdaResultInfo.FirstOrDefault().InventoryTaskId;
            var ids = pdaResultInfo.Select(m => m.Id).ToList();
            //编码明细(所有的包含编码管控和ID管控)
            var fixtureEncodes = Query<InventoryTaskFixtureEncode>().Where(m => ids.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //ID管控
            var encodeIds = pdaResultInfo.Select(t => t.AccountId).ToList();
            var fixtureEncodeIds = Query<InventoryTaskFixtureIdAccount>().Where(m => encodeIds.Contains(m.Id) && m.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var hadResult = new EntityList<InventoryTaskFixtureEncode>();
            var hadEncodeIds = new EntityList<InventoryTaskFixtureIdAccount>();
            var now = RF.Find<InventoryTaskFixtureEncode>().GetDbTime();

            foreach (var saveInfo in pdaResultInfo)
            {
                var detail = fixtureEncodes.Where(p => p.Id == saveInfo.Id).FirstOrDefault();
                //序列号类型看ProduceState ，编码看CountStr 为结果
                if ((ManageMode)saveInfo.ManageMode == ManageMode.Number && saveInfo.ProduceState.Any(m => m.isBoole) && saveInfo.QualityList.Any(m => m.isBoole))//序列号看List
                {
                    var fixtureIdAccout = fixtureEncodeIds.Where(p => p.FixtureEncodeId == detail.FixtureEncodeId && p.InventoryTaskId == saveInfo.InventoryTaskId && p.Id == saveInfo.AccountId).FirstOrDefault();
                    fixtureIdAccout.InventoryStatus = InventoryStatus.Done;
                    var resultType = saveInfo.ProduceState.FirstOrDefault(m => m.isBoole).Value;
                    SetResult(fixtureIdAccout, resultType, saveInfo);
                    hadEncodeIds.Add(fixtureIdAccout);
                    if (!hadResult.Any(p => p.Id == detail.Id))
                    {
                        hadResult.Add(detail);
                    }
                }
                else
                {
                    if (saveInfo.CountStr != "" && detail != null && saveInfo.InputOnlineQty != null && saveInfo.InputNgQty != null && saveInfo.InputPassQty != null)
                    {
                        detail.InventoryStatus = InventoryStatus.Done;
                        SetQtyInfo(now, saveInfo, detail);
                        hadResult.Add(detail);
                    }
                }
            }

            if (hadResult.Any() || hadEncodeIds.Any())
            {
                SaveFixtureTaskListPda(taskId, hadResult, hadEncodeIds);
            }
            else
            {
                throw new ValidationException("未提交任何数据,请确认盘点数据是否填写完整".L10N());
            }
        }


        /// <summary>
        /// 设置盘点结果
        /// </summary>
        /// <param name="fixtureIdAccount"></param>
        /// <param name="resultType"></param>
        /// <param name="info"></param>
        private void SetResult(InventoryTaskFixtureIdAccount fixtureIdAccount, int resultType, InventoryFixtureExcuteInfo info)
        {
            switch (resultType)
            {
                case 0: //"正常":
                    if (info.PdaType == 1) //初盘
                    {
                        fixtureIdAccount.FirstResult = InventoryResult.Normal;
                        fixtureIdAccount.FirstStatus = FixtureStatus.InStorage;

                    }
                    else
                    {
                        fixtureIdAccount.SecondResult = InventoryResult.Normal;
                        fixtureIdAccount.SecondStatus = FixtureStatus.InStorage;
                    }
                    break;
                case 1: // "信息变动":
                    if (info.PdaType == 1)//初盘
                    {
                        fixtureIdAccount.FirstResult = InventoryResult.InfoChange;
                        fixtureIdAccount.FirstStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    else
                    {
                        fixtureIdAccount.SecondResult = InventoryResult.InfoChange;
                        fixtureIdAccount.SecondStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    break;
                case 2: // "盘亏":
                    if (info.PdaType == 1)//初盘
                    {
                        fixtureIdAccount.FirstResult = InventoryResult.Loss;
                        fixtureIdAccount.FirstStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    else
                    {
                        fixtureIdAccount.SecondResult = InventoryResult.Loss;
                        fixtureIdAccount.SecondStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    break;
                case 3: // "盘盈":
                    if (info.PdaType == 1)//初盘
                    {
                        fixtureIdAccount.FirstResult = InventoryResult.Profit;
                        fixtureIdAccount.FirstStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    else
                    {
                        fixtureIdAccount.SecondResult = InventoryResult.Profit;
                        fixtureIdAccount.SecondStatus = info.QualityList.First(m => m.Value == 5).isBoole ? FixtureStatus.InStorage : FixtureStatus.OnLine;
                    }
                    break;
            }
        }


        /// <summary>
        /// 设置盘点结果数量信息
        /// </summary>
        /// <param name="now"></param>
        /// <param name="saveInfo"></param>
        /// <param name="detail"></param>
        private void SetQtyInfo(DateTime now, InventoryFixtureExcuteInfo saveInfo, InventoryTaskFixtureEncode detail)
        {
            if (saveInfo.PdaType == 1)
            {
                detail.InventoryDateTime = now;

                detail.FirstStockPassQty = int.Parse(saveInfo.InputPassQty);
                detail.FirstStockNgQty = int.Parse(saveInfo.InputNgQty);
                detail.FirstOnline = int.Parse(saveInfo.InputOnlineQty);
                detail.FirstCounterId = RT.IdentityId;
                detail.FirstStock = detail.FirstStockPassQty + detail.FirstStockNgQty;
                detail.FirstTotal = detail.FirstStock + detail.FirstOnline;
                detail.FirstDiff = detail.FirstTotal - detail.Total;
            }
            if (saveInfo.PdaType == 2)
            {
                detail.SecondDateTime = now;

                detail.SecStockPassQty = Convert.ToInt32(saveInfo.InputPassQty);
                detail.SecStockNgQty = Convert.ToInt32(saveInfo.InputNgQty);
                detail.SecondOnline = Convert.ToInt32(saveInfo.InputOnlineQty);
                detail.SecondCounterId = RT.IdentityId;
                detail.SecondStock = detail.SecStockPassQty + detail.SecStockNgQty + detail.SecondOnline;
                detail.SecondTotal = detail.SecondStock + detail.SecondOnline;
                detail.SecondDiff = detail.SecondTotal - detail.Total;
            }
        }


        #endregion

        #region 离线盘点
        /// <summary>
        /// 离线设备盘点获取任务和设备清单
        /// </summary>
        /// <returns></returns>
        [ApiService("离线设备盘点获取任务和设备清单")]
        [return: ApiReturn("离线设备盘点获取任务和设备清单")]
        public virtual List<OffLineEqpTaskInfo> GetOffLineInvTaskAndDtl()
        {
            //var TaskController = RT.Service.Resolve<InventoryTaskController>();
            var rst = new List<OffLineEqpTaskInfo>();
            var list = GetOffLineInvTask(new EagerLoadOptions().LoadWithViewProperty());
            var planIds = list.Select(x => x.InventoryPlanId).Distinct().ToList();
            var taskRange = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanEquipmentByIds(planIds);
            var epqIdsList = list.SelectMany(p => p.InventoryTaskEquipmentList).Where(p => p.EquipAccountId.HasValue).Select(a => a.EquipAccountId).ToList();
            var eqpAchList = RT.Service.Resolve<EquipAccountController>().GetEquipAccountAttachmentsByIds(epqIdsList);
            foreach (var item in list)
            {
                //盘点范围
                var range = taskRange.FirstOrDefault(x => x.InventoryPlanId == item.InventoryPlanId);
                var task = new OffLineEqpTaskInfo();
                task.TaskId = item.Id;
                task.PlanId = item.InventoryPlanId;
                task.TaskNo = item.TaskNo;
                task.PlanEndDate = item.PlanEndDate;
                task.InventoryType = item.InventoryType;
                task.Remark = item.Remark;
                task.InventoryAssetObject = item.InventoryAssetObject;
                task.InventoryAssetObjectName = item.InventoryAssetObject.ToLabel().L10N();
                task.InventoryTaskStatusName = item.InventoryTaskStatus.ToLabel().L10N();
                task.InventoryTaskStatus = item.InventoryTaskStatus;
                task.IsOverdue = DateTime.Now > item.PlanEndDate;
                task.NeedPhoto = item.NeedPhoto;
                task.ManageDeptId = range.ManageDeptId;
                task.ManageDeptCode = range.ManageDeptCode;
                task.ManageDeptName = range.ManageDeptName;
                if (range != null)
                {
                    task.UseDeptId = range.UseDeptId;
                    task.UseDeptCode = range.UseDeptCode;
                    task.UseDeptName = range.UseDeptName;
                }
                item.InventoryTaskEquipmentList.Where(a => a.InventoryStatus == InventoryStatus.Not).ForEach(a =>
                {
                    var eqItem = new EqpTaskList();
                    eqItem.BillNo = item.TaskNo;
                    eqItem.InvTaskId = item.Id;
                    eqItem.EqpId = a.Id;
                    eqItem.AccountState = a.OldAccountState;
                    eqItem.AccountStateName = a.OldAccountState.ToLabel().L10N();
                    eqItem.EquipmentCode = a.EquipmentCode;
                    eqItem.EquipmentName = a.EquipmentName;
                    eqItem.UserId = a.OldUserId;
                    eqItem.UserName = a.OldUserName;
                    eqItem.AccountUseState = a.OldAccountUseState;
                    eqItem.AccountUseStateName = a.OldAccountUseState.ToLabel().L10N();
                    eqItem.RealWorkShopId = a.OldWorkShopId;
                    eqItem.RealWorkShopName = a.OldWorkShopName;
                    eqItem.RealResourceId = a.OldResourceId;
                    eqItem.RealResourceName = a.OldResourceName;
                    eqItem.RealWarehouseId = a.OldWarehouseId;
                    //OldWarehouseCode为仓库名称
                    eqItem.RealWarehouseName = a.OldWarehouseCode;
                    eqItem.StorageLocationId = a.OldStorageLocationId;
                    eqItem.StorageLocationCode = a.OldStorageLocationCode;
                    eqItem.RealLocation = a.OldLocation;
                    eqItem.RFID = a.EqRFID;
                    eqItem.InventoryStatus = a.InventoryStatus;
                    eqItem.InventoryStatusName = a.InventoryStatus.ToLabel().L10N();
                    eqItem.RealManageDeptId = a.OldManageDeptId;
                    eqItem.RealManageDeptName = a.OldManageDept;
                    eqItem.RealUseDeptId = a.OldUseDeptId;
                    eqItem.RealUseDeptName = a.OldUseDeptName;
                    if (a.EquipAccountId.HasValue)
                    {
                        var eqpAchItem = eqpAchList.Where(x => x.OwnerId.Value == a.EquipAccountId.Value).OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                        if (eqpAchItem != null)
                        {
                            //如果有多个附件取最新上传的附件,附件名称取设备编号 编号不能重复
                            eqItem.FileName = a.EquipmentCode;
                            eqItem.FilePath = eqpAchItem.FilePath;
                            eqItem.FileExtesion = eqpAchItem.FileExtesion;
                        }
                    }
                    if (!a.PhotoFilePath.IsNullOrEmpty())
                    {
                        var strs = a.PhotoFilePath.Split('/');
                        var fileName = strs[strs.Length - 1];
                        var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(a.PhotoFilePath, fileName);
                        eqItem.PictureFileName = fileName;
                        eqItem.Picture = Convert.ToBase64String(fileBytes);
                        //
                    }
                    task.EqpTaskLists.Add(eqItem);
                });
                rst.Add(task);
            }
            return rst;
        }

        /// <summary>
        /// 离线工治具盘点获取任务和设备清单
        /// </summary>
        /// <returns></returns>
        [ApiService("离线工治具盘点获取任务和设备清单")]
        [return: ApiReturn("离线工治具盘点获取任务和设备清单")]
        public virtual InvFixtureAllData GetOffLineInvFixtureTaskAndDtl()
        {
            var rst = new InvFixtureAllData();
            var list = GetOffLineFixtureInvTask(new EagerLoadOptions().LoadWithViewProperty());
            var planIds = list.Select(x => x.InventoryPlanId).Distinct().ToList();
            var taskRange = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanFixtureByIds(planIds);
            var encodes = list.SelectMany(p => p.InventoryTaskFixtureEncodeList).Select(p => p.FixtureEncodeId).Distinct().ToList();
            List<FixtureFile> allfileList = new List<FixtureFile>();
            foreach (var item in list)
            {
                //盘点范围
                var range = taskRange.FirstOrDefault(x => x.InventoryPlanId == item.InventoryPlanId);
                var task = new InvFixtureTaskList();
                task.InvFixtureDtlTaskList = new List<InvFixtureDtlList>();
                task.TaskId = item.Id;
                task.PlanId = item.InventoryPlanId;
                task.TaskNo = item.TaskNo;
                task.PlanEndDate = item.PlanEndDate;
                task.InventoryType = item.InventoryType;
                task.Remark = item.Remark;
                task.InventoryAssetObject = item.InventoryAssetObject;
                task.InventoryAssetObjectName = item.InventoryAssetObject.ToLabel().L10N();
                task.InventoryTaskStatusName = item.InventoryTaskStatus.ToLabel().L10N();
                task.InventoryTaskStatus = item.InventoryTaskStatus;
                task.IsOverdue = DateTime.Now > item.PlanEndDate;
                if (range != null)
                {
                    task.ManageMode = range.ManageMode;
                    task.FixtureTypes = range.FixtureTypes;
                    task.FixtureModels = range.FixtureModels;
                    task.FixtureEncodes = range.FixtureEncodes;
                }
                //编码管控-只有是编码管控且是未盘点的明细才能盘点
                item.InventoryTaskFixtureEncodeList.Where(p => p.ManageMode == Fixtures.ManageMode.Code && p.InventoryStatus == InventoryStatus.Not).ForEach(f =>
                {
                    //编码管控
                    var encode = new InvFixtureDtlList();
                    encode.InvTaskId = item.Id;
                    encode.BillNo = item.TaskNo;
                    encode.FixtureEncodeId = f.Id;
                    encode.StockQty = f.StockQty;
                    encode.Online = f.Online;
                    encode.Total = f.Total;
                    encode.ManageMode = f.ManageMode;
                    encode.FixtureEncodeCode = f.FixtureEncodeCode;
                    encode.ModelName = f.ModelName;
                    encode.ModelCode = f.ModelCode;
                    var fileData = allfileList.FirstOrDefault(x => x.FileName == f.FixtureEncodeCode);
                    if (fileData != null)
                    {
                        encode.FileName = fileData.FileName;
                        encode.FileExtesion = fileData.FileExtesion;
                    }
                    task.InvFixtureDtlTaskList.Add(encode);
                });
                item.InventoryTaskFixtureIdAccountList.Where(f => f.InventoryStatus == InventoryStatus.Not).ForEach(f =>
                {
                    //序列号管控
                    var encode = new InvFixtureDtlList();
                    encode.InvTaskId = item.Id;
                    encode.BillNo = item.TaskNo;
                    encode.FixtureSnId = f.Id;
                    encode.Sn = f.Sn;
                    encode.FixtureEncodeCode = f.FixtureEncodeCode;
                    encode.FixtureStatus = f.FixtureStatus;
                    encode.ModelName = f.ModelName;
                    encode.ModelCode = f.ModelCode;
                    //序列号管理管控方式一定是序列号
                    encode.ManageMode = ManageMode.Number;
                    var fileData = allfileList.FirstOrDefault(x => x.FileName == f.FixtureEncodeCode);
                    if (fileData != null)
                    {
                        encode.FileName = fileData.FileName;
                        encode.FileExtesion = fileData.FileExtesion;
                    }
                    task.InvFixtureDtlTaskList.Add(encode);
                });
                if (task.InvFixtureDtlTaskList.Count > 0)
                {
                    rst.InvFixtureTaskLists.Add(task);
                }
            }
            rst.InvFixtureFile.AddRange(allfileList);
            return rst;
        }
        
        /// <summary>
        /// 离线盘点下载位置快码
        /// </summary>
        [ApiService("离线盘点下载位置快码")]
        [return: ApiReturn("离线盘点下载位置快码")]
        public virtual List<LocationFastCode> DownLoadLocationFastCode()
        {
            var rst = new List<LocationFastCode>();
            var locationCode = "LOCATION_TYPE";
            var catalogList = Query<Catalog>().Join<CatalogType>((x, y) => x.CatalogTypeId == y.Id && y.Code == locationCode).ToList();
            catalogList.ForEach(p =>
            {
                var item = new LocationFastCode();
                item.Code = p.Code;
                item.Name = p.Name;
                rst.Add(item);
            });
            return rst;
        }

        /// <summary>
        /// 离线设备盘点提交
        /// </summary>
        /// <param name="submitDatas">提交数据</param>
        /// <returns></returns>
        [ApiService("离线设备盘点提交")]
        [return: ApiReturn("离线设备盘点提交")]
        public virtual List<UploadLoadReturnData> SubmitOffLineEqpCount(List<SubmitDtlEqp> submitDatas)
        {
            var rst = new List<UploadLoadReturnData>();
            var logData = new EntityList<EdoOutlineUploadLog>();
            var taskIds = submitDatas.Select(p => p.InvTaskId).Distinct().ToList();
            var invTasks = GetInventoryTasksByIds(taskIds);
            foreach (var item in submitDatas)
            {
                var rstItem = new UploadLoadReturnData();
                var logItem = new EdoOutlineUploadLog();
                logItem.UploadType = UploadType.Machine;
                logItem.Remark = item.Remark;
                try
                {
                    var invtask = invTasks.FirstOrDefault(x => x.Id == item.InvTaskId);
                    rstItem.LineNo = item.EqpId.ToString();
                    if (invtask == null)
                    {
                        throw new ValidationException("盘点任务不存在".L10N());
                    }
                    rstItem.BillNo = invtask.TaskNo;
                    logItem.BillNo = invtask.TaskNo;
                    var eqpItem = invtask.InventoryTaskEquipmentList.FirstOrDefault(a => a.Id == item.EqpId);
                    if (eqpItem == null)
                    {
                        throw new ValidationException("盘点任务:[{0}]设备清单下无设备编码:[{1}的设备]".L10nFormat(invtask.TaskNo, item.EquipmentCode));
                    }
                    logItem.MachineCode = item.EquipmentCode;
                    logItem.MachineName = item.EquipmentName;
                    if (item.InventoryResult == InventoryResult.Normal && invtask.NeedPhoto && item.Picture.IsNullOrEmpty())
                    {
                        throw new ValidationException("盘点任务:[{0}]需要强制拍照,设备:[{1}]无对应的照片信息".L10nFormat(invtask.TaskNo, item.EquipmentCode));
                    }
                    if (item.InventoryResult == InventoryResult.Normal && item.RealWarehouseId.HasValue && item.RealWorkShopId.HasValue)
                    {
                        throw new ValidationException("盘点任务:[{0}]下,设备:[{1}]仓库和车间只能选择一个".L10nFormat(invtask.TaskNo, item.EquipmentCode));
                    }
                    if (invtask.InventoryTaskStatus != InventoryTaskStatus.Doing && invtask.InventoryTaskStatus != InventoryTaskStatus.FirstDone && invtask.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
                    {
                        throw new ValidationException("盘点任务:[{0}]盘点状态不为【盘点中、复盘中、初盘完成】".L10nFormat(invtask.TaskNo, item.EquipmentCode));
                    }
                    SetEqpDetailValue(item, invtask, eqpItem);
                    rstItem.IsSuccess = true;
                    logItem.UploadState = UploadState.Success;
                }
                catch (ValidationException ex)
                {
                    rstItem.FailReason = ex.Message;
                    rstItem.IsSuccess = false;
                    logItem.FailReason = ex.Message;
                    logItem.UploadState = UploadState.Fail;
                }
                rst.Add(rstItem);
                logData.Add(logItem);
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(invTasks);
                var firstDoneTask = invTasks.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing).AsEntityList();
                UpdatePercentage(firstDoneTask);
                //保存离线上传日志
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(logData);
                trans.Complete();
            }
            return rst;
        }

        /// <summary>
        /// 更新盘点结果
        /// </summary>
        /// <param name="submitDtlEqp">提交的盘点结果(设备)</param>
        /// <param name="task">盘点任务</param>
        /// <param name="eqpItem">设备清单</param>
        private void SetEqpDetailValue(SubmitDtlEqp submitDtlEqp, InventoryTask task, InventoryTaskEquipment eqpItem)
        {
            var now = RF.Find<InventoryTask>().GetDbTime();
            //盘点任务状态为【初盘完成】时，更新为【复盘中】
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
            {
                task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
            }
            //离线盘点只有正常和盘亏
            if (submitDtlEqp.InventoryResult == InventoryResult.Normal)
            {
                eqpItem.InventoryStatus = InventoryStatus.Done;
                eqpItem.RealManageDeptId = submitDtlEqp.RealManageDeptId;
                eqpItem.RealUseDeptId = submitDtlEqp.RealUseDeptId;
                eqpItem.AccountUseState = submitDtlEqp.AccountUseState;
                eqpItem.AccountState = submitDtlEqp.AccountState;
                eqpItem.UserId = submitDtlEqp.UserId;
                eqpItem.RealWorkShopId = submitDtlEqp.RealWorkShopId;
                eqpItem.RealResourceId = submitDtlEqp.RealResourceId;
                eqpItem.RealWarehouseId = submitDtlEqp.RealWarehouseId;
                eqpItem.StorageLocationId = submitDtlEqp.StorageLocationId;
                eqpItem.RealLocation = submitDtlEqp.RealLocation;
                if (!submitDtlEqp.Picture.IsNullOrWhiteSpace())
                {
                    var bytes = Convert.FromBase64String(submitDtlEqp.Picture);
                    const string prePath = "InventoryPlanPhoto";
                    var path = $"{prePath}/{Guid.NewGuid()}";
                    RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(submitDtlEqp.PictureFileName, bytes, path);
                    eqpItem.PhotoFilePath = $"{path}/{submitDtlEqp.PictureFileName}";
                }
                //来源为【账内资产】时，全部值一样时取值【正常】，有不一样时取值【信息变动】
                UpdateInventoryResult(eqpItem, task);
            }
            else
            {
                eqpItem.InventoryStatus = InventoryStatus.Done;
                eqpItem.RealManageDeptId = null;
                eqpItem.RealUseDeptId = null;
                eqpItem.AccountUseState = null;
                eqpItem.AccountState = null;
                eqpItem.UserId = null;
                eqpItem.RealWorkShopId = null;
                eqpItem.RealResourceId = null;
                eqpItem.RealWarehouseId = null;
                eqpItem.StorageLocationId = null;
                eqpItem.RealLocation = string.Empty;
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    eqpItem.FirstInventoryResult = InventoryResult.Loss;
                    eqpItem.FirstCounterId = RT.IdentityId;
                    eqpItem.InventoryDateTime = now;
                }
                else
                {
                    eqpItem.SecondInventoryResult = InventoryResult.Loss;
                    eqpItem.SecondCounterId = RT.IdentityId;
                    eqpItem.SecondDateTime = now;
                }
            }

        }

        /// <summary>
        /// 离线工治具盘点提交
        /// </summary>
        /// <param name="submitDatas"></param>
        /// <returns></returns>
        [ApiService("离线工治具盘点提交")]
        [return: ApiReturn("离线工治具盘点提交")]
        public virtual List<UploadLoadReturnData> SubmitOffLineJigsCount(List<SubmitFixtureTaskData> submitDatas)
        {
            var rst = new List<UploadLoadReturnData>();
            var logData = new EntityList<EdoOutlineUploadLog>();
            var taskIds = submitDatas.Select(p => p.InvTaskId).Distinct().ToList();
            var invTasks = GetInventoryTasksByIds(taskIds);
            foreach (var item in submitDatas)
            {
                var rstItem = new UploadLoadReturnData();
                var logItem = new EdoOutlineUploadLog();
                rstItem.BillNo = item.BillNo;
                logItem.UploadType = UploadType.Encode;
                logItem.EncodeCode = item.FixtureEncodeCode;
                logItem.EncodeName = item.FixtureEncodeCode;
                logItem.Sn = item.Sn;
                logItem.BillNo = item.BillNo;
                logItem.Remark = item.Remark;
                rstItem.LineNo = item.ID.ToString();
                try
                {
                    var invtask = invTasks.FirstOrDefault(x => x.Id == item.InvTaskId);
                    rstItem.LineNo = item.ID.ToString();
                    if (invtask == null)
                    {
                        throw new ValidationException("盘点任务不存在".L10N());
                    }
                    SetFixDtlData(item, invtask);
                    if (item.ManageMode == ManageMode.Number)
                    {
                        SetFixDtlDataByFixId(invtask, item.Sn);
                    }
                    rstItem.IsSuccess = true;
                    logItem.UploadState = UploadState.Success;
                }
                catch (ValidationException ex)
                {
                    rstItem.FailReason = ex.Message;
                    rstItem.IsSuccess = false;
                    logItem.FailReason = ex.Message;
                    logItem.UploadState = UploadState.Fail;
                }
                rst.Add(rstItem);
                logData.Add(logItem);
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(invTasks);
                var firstDoneTask = invTasks.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing).AsEntityList();
                UpdateFixturePercentage(firstDoneTask);
                //保存离线上传日志
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(logData);
                trans.Complete();
            }
            return rst;
        }

        /// <summary>
        /// 设置明细数据
        /// </summary>
        /// <param name="submitFixtureTaskData"></param>
        /// <param name="task"></param>
        private void SetFixDtlData(SubmitFixtureTaskData submitFixtureTaskData, InventoryTask task)
        {
            var now = RF.Find<InventoryTask>().GetDbTime();
            //盘点任务状态为【初盘完成】时，更新为【复盘中】
            if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
            {
                task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
            }
            if (submitFixtureTaskData.ManageMode == ManageMode.Number)
            {
                var fixItem = task.InventoryTaskFixtureIdAccountList.FirstOrDefault(x => x.Sn == submitFixtureTaskData.Sn);
                fixItem.InventoryStatus = InventoryStatus.Done;
                if (submitFixtureTaskData.InventoryResult == InventoryResult.Loss)
                {
                    if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                    {
                        fixItem.FirstResult = InventoryResult.Loss;
                        fixItem.FirstCounterId = RT.IdentityId;
                        fixItem.FirstDateTime = now;
                    }
                    else
                    {
                        fixItem.SecondResult = InventoryResult.Loss;
                        fixItem.SecondCounterId = RT.IdentityId;
                        fixItem.SecondDateTime = now;
                    }
                }
                else
                {
                    if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                    {
                        fixItem.FirstResult = submitFixtureTaskData.InventoryResult;
                        fixItem.FirstStatus = submitFixtureTaskData.FixtureStatus;
                        fixItem.FirstCounterId = RT.IdentityId;
                        fixItem.FirstDateTime = now;
                    }
                    else
                    {
                        fixItem.SecondResult = submitFixtureTaskData.InventoryResult;
                        fixItem.SecondStatus = submitFixtureTaskData.FixtureStatus;
                        fixItem.SecondCounterId = RT.IdentityId;
                        fixItem.SecondDateTime = now;
                    }
                }
            }
            if (submitFixtureTaskData.ManageMode == ManageMode.Code)
            {
                var CodeItem = task.InventoryTaskFixtureEncodeList.FirstOrDefault(x => x.FixtureEncodeCode == submitFixtureTaskData.FixtureEncodeCode);
                int stockQty = submitFixtureTaskData.RealStockQty;
                int lineQty = submitFixtureTaskData.RealOnlineQty;
                CodeItem.InventoryStatus = InventoryStatus.Done;
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    //初盘
                    SetFirstFixtureEncodeCount(stockQty, lineQty, CodeItem);
                }
                else
                {
                    //复盘
                    SetSecondFixtureEncodeCount(stockQty, lineQty, CodeItem);
                }
            }
        }


        /// <summary>
        /// 设置初盘编码类数据
        /// </summary>
        /// <param name="stockQty">盘点在库数</param>
        /// <param name="lineQty">盘点在线数</param>
        /// <param name="inventoryTaskFixtureEncode"></param>
        private void SetFirstFixtureEncodeCount(int stockQty, int lineQty, InventoryTaskFixtureEncode inventoryTaskFixtureEncode)
        {
            inventoryTaskFixtureEncode.FirstStock = stockQty;
            inventoryTaskFixtureEncode.FirstOnline = lineQty;
            inventoryTaskFixtureEncode.FirstTotal = stockQty + lineQty;
            inventoryTaskFixtureEncode.FirstDiff = inventoryTaskFixtureEncode.FirstTotal - inventoryTaskFixtureEncode.Total;
            inventoryTaskFixtureEncode.FirstCounterId = RT.IdentityId;
            inventoryTaskFixtureEncode.InventoryDateTime = DateTime.Now;
            if (inventoryTaskFixtureEncode.Total > inventoryTaskFixtureEncode.FirstTotal)
            {
                inventoryTaskFixtureEncode.FirstResult = InventoryResult.Loss;
            }
            if (inventoryTaskFixtureEncode.Total < inventoryTaskFixtureEncode.FirstTotal)
            {
                inventoryTaskFixtureEncode.FirstResult = InventoryResult.Profit;
            }
            if (inventoryTaskFixtureEncode.Total == inventoryTaskFixtureEncode.FirstTotal)
            {
                inventoryTaskFixtureEncode.FirstResult = inventoryTaskFixtureEncode.FirstStock == inventoryTaskFixtureEncode.StockQty && inventoryTaskFixtureEncode.FirstOnline == inventoryTaskFixtureEncode.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
            }
        }

        /// <summary>
        /// 设置复盘编码类数据
        /// </summary>
        /// <param name="stockQty">盘点在库数</param>
        /// <param name="lineQty">盘点在线数</param>
        /// <param name="inventoryTaskFixtureEncode"></param>
        private void SetSecondFixtureEncodeCount(int stockQty, int lineQty, InventoryTaskFixtureEncode inventoryTaskFixtureEncode)
        {
            inventoryTaskFixtureEncode.SecondStock = stockQty;
            inventoryTaskFixtureEncode.SecondOnline = lineQty;
            inventoryTaskFixtureEncode.SecondTotal = stockQty + lineQty;
            inventoryTaskFixtureEncode.SecondDiff = inventoryTaskFixtureEncode.SecondTotal - inventoryTaskFixtureEncode.Total;
            inventoryTaskFixtureEncode.SecondCounterId = RT.IdentityId;
            inventoryTaskFixtureEncode.SecondDateTime = DateTime.Now;
            if (inventoryTaskFixtureEncode.Total > inventoryTaskFixtureEncode.SecondTotal)
            {
                inventoryTaskFixtureEncode.SecondResult = InventoryResult.Loss;
            }
            if (inventoryTaskFixtureEncode.Total < inventoryTaskFixtureEncode.SecondTotal)
            {
                inventoryTaskFixtureEncode.SecondResult = InventoryResult.Profit;
            }
            if (inventoryTaskFixtureEncode.Total == inventoryTaskFixtureEncode.SecondTotal)
            {
                inventoryTaskFixtureEncode.SecondResult = inventoryTaskFixtureEncode.SecondStock == inventoryTaskFixtureEncode.StockQty && inventoryTaskFixtureEncode.SecondOnline == inventoryTaskFixtureEncode.Online ? InventoryResult.Normal : InventoryResult.InfoChange;
            }
        }

        /// <summary>
        /// 根据复盘       
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="Sn"></param>
        private void SetFixDtlDataByFixId(InventoryTask task, string Sn)
        {
            var enCodes = task.InventoryTaskFixtureEncodeList;
            var snItem = task.InventoryTaskFixtureIdAccountList.FirstOrDefault(x => x.Sn == Sn);
            var enCode = snItem.FixtureEncodeCode;
            var enCodeItem = enCodes.FirstOrDefault(x => x.FixtureEncodeCode == enCode);
            int stockQty = 0;
            int onlineQty = 0;
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                stockQty = task.InventoryTaskFixtureIdAccountList.Count(p => p.FixtureEncodeCode == enCode && p.FirstStatus == FixtureStatus.InStorage);
                onlineQty = task.InventoryTaskFixtureIdAccountList.Count(p => p.FixtureEncodeCode == enCode && p.FirstStatus == FixtureStatus.OnLine);
                var IdCount = task.InventoryTaskFixtureIdAccountList.Count(p => p.InventoryStatus == InventoryStatus.Done && p.FixtureEncodeCode == enCode);
                enCodeItem.InventoryStatus = IdCount == enCodeItem.Total ? InventoryStatus.Done : InventoryStatus.Part;
                SetFirstFixtureEncodeCount(stockQty, onlineQty, enCodeItem);
            }
            else
            {
                stockQty = task.InventoryTaskFixtureIdAccountList.Count(p => p.FixtureEncodeCode == enCode && p.SecondStatus == FixtureStatus.InStorage);
                onlineQty = task.InventoryTaskFixtureIdAccountList.Count(p => p.FixtureEncodeCode == enCode && p.SecondStatus == FixtureStatus.OnLine);
                SetSecondFixtureEncodeCount(stockQty, onlineQty, enCodeItem);

            }


        }
        #endregion
    }
}
