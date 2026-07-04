using SIE.Common;
using SIE.Core;
using SIE.Dock.Datas;
using SIE.Dock.DockMaintains.Service;
using SIE.Dock.YardZones.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Dock.DockAppoints.Service
{
    /// <summary>
    /// 月台预约Service
    /// </summary>
    public partial class DockAppointService
    {
        /// <summary>
        /// 预约图保存数据
        /// </summary>
        /// <param name="appoints">预约数据</param>
        public virtual DockMapSaveReturnData SaveAppoints(List<DockAppoint> appoints)
        {
            using (var tran = DB.TransactionScope(DockEntityDataProvider.ConnectionStringName))
            {
                var rst = MapValidate(appoints);
                if (rst.ErrorMsg.IsNotEmpty())
                    return rst;
                var delIds = appoints.Where(f => f.PersistenceStatus == PersistenceStatus.Deleted).Select(f => f.Id).ToList();
                delIds.SplitDataExecute(sons =>
                {
                    DB.Update<DockAppoint>().Set(f => f.CancelAppointBy, RT.IdentityId)
                    .Set(f => f.CancelAppointDate, DateTime.Now)
                    .Set(f => f.IsCancelAppoint, true).Where(f => sons.Contains(f.Id)).Execute();
                });
                appoints.RemoveAll(f => f.PersistenceStatus == PersistenceStatus.Deleted || f.PersistenceStatus == PersistenceStatus.Unchanged);
                var dockIds = appoints.Select(f => f.DockMaintainId).Distinct().ToList();
                var yzoneIds = appoints.Select(f => f.YardZoneId).Distinct().ToList();
                var handles = RT.Service.Resolve<DockHandlingService>().GetDockHandlings(yzoneIds);
                var dbAps = GetDockAppointByDockIds(dockIds, DateTime.Now.Date);//当前存在的预约
                var dbApsId = dbAps.Select(f => f.Id).ToList();
                var changeDocksAppointIds = appoints.Where(f => f.PersistenceStatus == PersistenceStatus.Modified && !dbApsId.Contains(f.Id)).Select(f => f.Id).ToList();
                var changeDocksAppoints = RT.Service.Resolve<DockAppointService>().GetDockAppointList(changeDocksAppointIds);
                var editEntitys = appoints.OrderBy(f => f.PersistenceStatus).AsEntityList();
                if (editEntitys.Any(f => f.PersistenceStatus == PersistenceStatus.New && f.AppointStartDate <= DateTime.Now))
                {
                    var err = editEntitys.FirstOrDefault(f => f.PersistenceStatus == PersistenceStatus.New && f.AppointStartDate <= DateTime.Now);
                    throw new ValidationException("预约时段[{0}至{1}]已经开始，不能进行预约".L10nFormat(err.AppointStartDate, err.AppointEndDate));
                }
                //修改的在前面先保存
                editEntitys.ForEach(f =>
                  {
                      f.AppointDate = f.AppointDate.Date;

                      if (f.PersistenceStatus == PersistenceStatus.Modified)
                      {
                          var ap = dbAps.FirstOrDefault(a => a.Id == f.Id);
                          if (ap == null)
                          {
                              ap = changeDocksAppoints.FirstOrDefault(a => a.Id == f.Id);
                              if (ap == null)
                                  throw new ValidationException("修改的数据不存在[预约号{0},公司{1}]".L10nFormat(f.No, f.CompanyName));
                          }
                          ap.Clone(f);
                      }
                      else if (f.PersistenceStatus == PersistenceStatus.New)
                      {
                          dbAps.Add(f);
                      }
                  });
                dbAps.GroupBy(f => new { f.DockMaintainId, f.AppointStartDate, f.AppointEndDate }).ForEach(f =>
                    {
                        var h = f.Sum(a => a.UseHours);
                        if (f.Key.AppointStartDate.AddHours(h) > f.Key.AppointEndDate)
                            throw new ValidationException("月台[{2}]预约时段[{0}至{1}]预约时长已满".L10nFormat(f.Key.AppointStartDate, f.Key.AppointEndDate, f.First().DockMaintain.Name));
                    });
                handles.GroupBy(f => f.YardZoneId).ForEach(f =>
                {
                    dbAps.Where(a => a.YardZoneId == f.Key).GroupBy(a => new { a.AppointStartDate, a.AppointEndDate }).ForEach(a =>
                     {
                         var hand = f.FirstOrDefault(b => b.BeginTime == a.Key.AppointStartDate.ToString(DateTimeFormat.HHmm) && b.EndTime == a.Key.AppointEndDate.ToString(DateTimeFormat.HHmm));
                         if (hand == null)
                         {
                             throw new ValidationException("园片区[{2}]月台装卸能力时间段[{0}-{1}]已不存在".L10nFormat(a.Key.AppointStartDate.ToString("HH:mm"), a.Key.AppointEndDate.ToString("HH:mm"), f.First().YardZone.Name));
                         }
                         if (hand.ReceiveAppoNum == 0)
                         {
                             var pickCount = a.Count(c => c.AppointType == AppointType.PickUp);
                             if (pickCount > 0)
                                 throw new ValidationException("保存的数据已超过园片区[{2}]月台装卸能力时间段[{0}-{1}]提货能力".L10nFormat(a.Key.AppointStartDate.ToString("HH:mm"), a.Key.AppointEndDate.ToString("HH:mm"), f.First().YardZone.Name, hand.ReceiveAppoNum));
                         }
                         if (hand.ShipAppoNum == 0)
                         {
                             var deliCount = a.Count(c => c.AppointType == AppointType.Delivery);
                             if (deliCount > 0)
                                 throw new ValidationException("保存的数据已超过园片区[{2}]月台装卸能力时间段[{0}-{1}]送货能力".L10nFormat(a.Key.AppointStartDate.ToString("HH:mm"), a.Key.AppointEndDate.ToString("HH:mm"), f.First().YardZone.Name, hand.ShipAppoNum));
                         }
                     });
                });

                RF.Save(editEntitys);
                tran.Complete();
                return rst;
            }
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="appoints"></param>
        /// <returns></returns>
        private DockMapSaveReturnData MapValidate(List<DockAppoint> appoints)
        {
            DockMapSaveReturnData rst = new DockMapSaveReturnData();

            var config = _dockAppointDao.GetDockAppointNumberRule();

            //预约结束时间-预约开始时间≤配置项的最大预约时间
            if (config != null && config.MaxAppointTime.HasValue)
            {
                var errNos = appoints.Where(f => f.UseHours > (double)(config.MaxAppointTime.Value)).Select(f => f.No).ToList();
                if (errNos.Any())
                {
                    if (rst.ErrorMsg.IsNotEmpty())
                        rst.ErrorMsg += "<br/>";
                    rst.ErrorMsg = "预计占用时长 必须小于等于配置项的最大预约时间:[{0}]".L10nFormat(config?.MaxAppointTime ?? 0);
                    rst.Codes.AddRange(errNos);
                    return rst;
                }
            }
            return rst;
        }

        /// <summary>
        /// 检查时间
        /// </summary>
        /// <param name="dockAppoint"></param>
        public virtual void NewCheckAppointDatas(DockAppoint dockAppoint)
        {
            var config = _dockAppointDao.GetDockAppointNumberRule();

            if (config != null && config.MaxAppointTime.HasValue)
            {
                if (dockAppoint.UseHours > (double)(config.MaxAppointTime.Value))
                    throw new ValidationException("预计占用时长 必须小于等于配置项的最大预约时间:[{0}]".L10nFormat(config?.MaxAppointTime ?? 0));
            }
        }
    }
}
