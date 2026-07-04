using SIE.AbnormalInfo.AbnormalInfos.Pushers;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Common.Sender;
using SIE.Core.AnomalyMonitors;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Defects;
using SIE.Defects.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.PDCA;
using SIE.Items;
using SIE.Items.Items;
using SIE.Logging;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息控制器
    /// </summary>
    public partial class AbnormalInfoController : DomainController
    {
        /// <summary>
        /// 获取来源类型为预警平台的异常定义数据
        /// </summary>
        /// <param name="abnormalSource">异常来源</param>
        /// <param name="alertId">预警id</param>
        /// <param name="abnormalCategoryId">异常分类id</param>
        /// <returns>异常信息定义数据</returns>
        public virtual EntityList<AbnormalInfoDefinition> GetAlertAbnormalDefinitions(AbnormalSource abnormalSource, double alertId, double? abnormalCategoryId)
        {
            return Query<AbnormalInfoDefinition>()
                .Where(p => p.AbnormalSource == abnormalSource && p.AlerterId == alertId)
                .WhereIf(abnormalCategoryId.HasValue, p => p.AbnormalCategoryId == abnormalCategoryId)
                .ToList();
        }

        /// <summary>
        /// 根据异常来源获取异常定义信息
        /// </summary>
        /// <param name="abnormalSource">异常来源</param>
        /// <returns>异常信息定义数据</returns>
        public virtual EntityList<AbnormalInfoDefinition> GetAbnormalDefinitions(AbnormalSource abnormalSource)
        {
            return Query<AbnormalInfoDefinition>().Where(p => p.AbnormalSource == abnormalSource).ToList();
        }

        /// <summary>
        /// 根据异常来源获取异常定义信息
        /// </summary>
        /// <param name="abnormalSource">异常来源</param>
        /// <returns>异常信息定义数据</returns>
        public virtual AbnormalInfoDefinition GetAbnormalDefinition(AbnormalSource abnormalSource)
        {
            return Query<AbnormalInfoDefinition>().Where(p => p.AbnormalSource == abnormalSource).FirstOrDefault();
        }

        /// <summary>
        /// 根据异常分类id获取不同的单位类型
        /// </summary>
        /// <param name="abnormalInfoCategoryId">异常分类id</param>
        /// <param name="sendUpgradeSetIds">推送升级设置id集合</param>
        /// <returns></returns>
        public virtual EntityList<SenderUpgradeSettings> GetDisUnitTypeData(double abnormalInfoCategoryId, List<double> sendUpgradeSetIds)
        {
            var oldUnitTypeData = Query<SenderUpgradeSettings>().Where(p => p.AbnormalInfoCategoryId == abnormalInfoCategoryId && !sendUpgradeSetIds.Contains(p.Id)).Select(p => p.UnitType).Distinct().ToList();
            return oldUnitTypeData;
        }

        /// <summary>
        /// 根据异常定义id获取不在id范围内的设置
        /// </summary>
        /// <param name="abnormalInfoDefinitionId">异常定义id</param>
        /// <param name="senderSetIds">推送升级设置id集合</param>
        /// <returns></returns>
        public virtual EntityList<DefinitionSenderSettings> GetDefinitionSenderSettingsExceptIds(double abnormalInfoDefinitionId, List<double> senderSetIds)
        {
            var settings = Query<DefinitionSenderSettings>().Where(p => p.AbnormalDefinitionId == abnormalInfoDefinitionId && !senderSetIds.Contains(p.Id)).ToList();
            return settings;
        }

        /// <summary>
        /// 根据异常分类id获取不在id范围内的设置
        /// </summary>
        /// <param name="abnormalInfoCategoryId">异常分类id</param>
        /// <param name="sendUpgradeSetIds">推送升级设置id集合</param>
        /// <returns></returns>
        public virtual EntityList<SenderUpgradeSettings> GetSenderUpgradeSettingsExceptIds(double abnormalInfoCategoryId, List<double> sendUpgradeSetIds)
        {
            var settings = Query<SenderUpgradeSettings>().Where(p => p.AbnormalInfoCategoryId == abnormalInfoCategoryId && !sendUpgradeSetIds.Contains(p.Id)).ToList();
            return settings;
        }

        /// <summary>
        /// 根据异常分类id获取推送升级设置数据
        /// </summary>
        /// <param name="abnormalInfoCategoryId">异常分类id</param>
        /// <returns></returns>
        public virtual EntityList<SenderUpgradeSettings> GetSenderUpgrades(double abnormalInfoCategoryId)
        {
            var senderUpgrades = Query<SenderUpgradeSettings>().Where(p => p.AbnormalInfoCategoryId == abnormalInfoCategoryId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return senderUpgrades;
        }

        /// <summary>
        /// 根据异常定义id获取不同的单位类型
        /// </summary>
        /// <param name="abnormalDefinitionId">异常定义id</param>
        /// <param name="sendUpgradeSetIds">推送升级设置id集合</param>
        /// <returns></returns>
        public virtual EntityList<DefinitionSenderSettings> GetDefDisUnitTypeData(double abnormalDefinitionId, List<double> sendUpgradeSetIds)
        {
            var oldUnitTypeData = Query<DefinitionSenderSettings>().Where(p => p.AbnormalDefinitionId == abnormalDefinitionId && !sendUpgradeSetIds.Contains(p.Id)).Select(p => p.UnitType).Distinct().ToList();
            return oldUnitTypeData;
        }

        /// <summary>
        /// 获取推送的异常信息定义
        /// </summary>
        /// <param name="source">信息来源</param>
        /// <returns></returns>
        public virtual double GetAbnormalInfoDefinition(AbnormalSource? source)
        {
            var q = Query<AbnormalInfoDefinition>();
            q.WhereIf(source.HasValue, p => p.AbnormalSource == source);
            var list = q.ToList<double>();
            if (list?.Count > 0)
                return list.First();
            else
                throw new ValidationException("异常信息定义未维护".L10N());
        }

        /// <summary>
        /// 获取异常信息关联的推送方式
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<Pusher> GetAbnormalPushers()
        {
            var q = Query<Pusher>().Join<PushPlug>((pusher, plug) => pusher.PushPlugId == plug.Id).Where<PushPlug>((pusher, plug) => plug.Name == "异常信息邮件推送插件");
            return q.ToList();
        }

        #region 异常信息管理
        /// <summary>
        /// 查询异常信息
        /// </summary>
        /// <param name="criteria">异常信息查询实体</param>
        /// <returns></returns>
        public virtual EntityList QueryAbnormalInfos(AbnormalInforCriteria criteria)
        {
            Check.NotNull(criteria, "异常信息查询实体不能为空".L10N());
            var q = Query<AbnormalInfor>();
            q.WhereIf(criteria.No.IsNotEmpty(), p => p.No.Contains(criteria.No));
            q.WhereIf(criteria.AbnormalInfoDefinitionId.HasValue, p => p.AbnormalInfoDefinitionId == criteria.AbnormalInfoDefinitionId);
            if (criteria.ProcessId.HasValue)    //工序
            {
                var process = RF.GetById<Process>(criteria.ProcessId);
                if (process != null)
                    q.Where(p => p.JoinProcessNames.Contains("%" + process.Name + "%"));
            }
            q.WhereIf(criteria.WorkShopId.HasValue, p => p.WorkShopId == criteria.WorkShopId);
            q.WhereIf(criteria.LineId.HasValue, p => p.LineId == criteria.LineId);
            q.WhereIf(criteria.ItemId.HasValue, p => p.ItemId == criteria.ItemId);
            q.WhereIf(criteria.AbnormalStatus.HasValue, p => p.AbnormalStatus == criteria.AbnormalStatus);
            if (criteria.CreateDate != null)
            {
                q.WhereIf(criteria.CreateDate.BeginValue.HasValue, p => p.CreateDate >= criteria.CreateDate.BeginValue);
                q.WhereIf(criteria.CreateDate.EndValue.HasValue, p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            if (criteria.OrderInfoList != null && criteria.OrderInfoList.Count > 0)
                q.OrderBy(criteria.OrderInfoList);

            var list = q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            list = GetDefectInfo(list);
            list = GetHandlersDisplay(list);
            var types = AnomalyEventListener.Instance.ReadAnomalyModule();
           // RT.Service.Resolve<CollectionService>().GetBillNoPass(types[0]);
            return list;
        }

        /// <summary>
        /// 获取缺陷信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual EntityList<AbnormalInfor> GetDefectInfo(EntityList<AbnormalInfor> list)
        {
            if (list == null || list.Count == 0) return list;
            var defectIds = new List<double>();
            foreach (var abnormal in list)
            {
                if (abnormal.DefectIds.IsNotEmpty())
                {
                    var ids = Array.ConvertAll(abnormal.DefectIds.Split(','), d => double.Parse(d));
                    defectIds = defectIds.Union(ids).ToList();
                }
            }
            if (defectIds?.Count > 0)
            {
                var defects = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
                foreach (var abnormal in list)
                {
                    if (abnormal.DefectIds.IsNullOrEmpty()) continue;
                    var ids = abnormal.DefectIds.Split(',');
                    var abnormalDefects = defects.Where(p => ids.Contains(p.Id.ToString())).ToList();
                    abnormal.JoinDefectCodes = string.Join(",", abnormalDefects.Select(p => p.Code).ToList());
                    abnormal.JoinDefectCodeDescriptions = string.Join(",", abnormalDefects.Select(p => p.Description).ToList());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取异常处理人信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual EntityList<AbnormalInfor> GetHandlersDisplay(EntityList<AbnormalInfor> list)
        {
            if (list == null || list.Count == 0) return list;
            var defIds = list.Select(p => p.AbnormalInfoDefinitionId).ToList();
            if (defIds.IsNullOrEmpty()) return list;
            //查询异常ID关联处理人姓名
            var q = Query<AbnormalInfoDefinition>()
                .Join<AbnormalInfoDefinitionEmployee>((def, rel) => def.Id == rel.AbnormalInfoDefinitionId)
                .Join<AbnormalInfoDefinitionEmployee, Employee>((rel, emp) => rel.HandlerId == emp.Id)
                .Where(p => defIds.Contains(p.Id))
                .Select<Employee>((def, emp) => new
                {
                    Id = def.Id,    //异常定义ID
                    Value = emp.Name    //员工名称
                });
            var nameList = q.ToList<BaseDataStringInfo>();
            if (nameList.IsNullOrEmpty()) return list;
            //聚合得出异常ID和处理人姓名清单的字典
            var dic = nameList.GroupBy(p => p.Id).Select(p => new BaseDataStringInfo()
            {
                Id = p.Key,
                Value = string.Join(StringCommon.SplitStr, p.Select(k => k.Value))
            }).ToList();
            if (dic.IsNullOrEmpty()) return list;
            //把处理人姓名赋值到异常管理列表
            list.ForEach(p => p.HandlersDisplay = dic.FirstOrDefault(k => k.Id == p.AbnormalInfoDefinitionId)?.Value);

            return list;
        }

        /// <summary>
        /// 获取缺陷信息
        /// </summary>
        /// <param name="abnormal"></param>
        /// <returns></returns>
        public virtual AbnormalInfor GetDefectInfo(AbnormalInfor abnormal)
        {
            if (abnormal == null) return abnormal;

            if (abnormal.DefectIds.IsNotEmpty())
            {
                var defectIds = Array.ConvertAll(abnormal.DefectIds.Split(','), d => double.Parse(d)).ToList<double>();

                var abnormalDefects = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
                if (abnormalDefects != null && abnormalDefects?.Count > 0)
                {
                    abnormal.JoinDefectCodes = string.Join(",", abnormalDefects.Select(p => p.Code).ToList());
                    abnormal.JoinDefectCodeDescriptions = string.Join(",", abnormalDefects.Select(p => p.Description).ToList());
                }
            }
            return abnormal;
        }

        /// <summary>
        /// 保存异常信息
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveNewAbnormalInfo(AbnormalInfor info)
        {
            Check.AssertNotNull(info, "异常信息不能为空".L10N());
            RF.Save(info);
        }

        /// <summary>
        /// 生成新的异常信息单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetNewAbnormalInfoNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AbnormalInfor));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到异常信息单号生成规则,请检查规则配置".L10N());

            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 提交异常信息
        /// </summary>
        /// <param name="abnormal"></param>
        public virtual void SumbitAbnormalInfo(AbnormalInfor abnormal)
        {
            using (var trans = DB.TransactionScope(AbnormalInfoDataProvider.ConnectionStringName))
            {
                abnormal.AbnormalStatus = AbnormalStatus.Close;
                RF.Save(abnormal);
                if (abnormal.IsSendPdca && !abnormal.IsRectificationTask)
                {
                    SendPdca(abnormal);
                }
                SendEmails(abnormal);
                trans.Complete();
            }
        }

        /// <summary>
        /// 推送邮件
        /// </summary>
        /// <param name="abnormal"></param>
        protected virtual void SendEmails(AbnormalInfor abnormal)
        {
            Check.AssertNotNull(abnormal, "异常信息不能为空。".L10N());
            //推送邮件
            var pusher = GetPusher(abnormal);
            if (pusher != null)
            {
                AbnormalInfoAlertResult result = CreateAlertResult(abnormal);
                try
                {
                    RT.Service.Resolve<PushPlugController>().Send(pusher, result);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.Error("发送邮件失败。".L10N() + ex.GetExceptionDetail());
                    throw new ValidationException("异常邮件推送配置错误，请检查。原因：{0}".L10nFormat(ex.Message));
                }
            }
        }

        /// <summary>
        /// 生成异常信息推送信息
        /// </summary>
        /// <param name="abnormal"></param>
        /// <returns></returns>
        protected virtual AbnormalInfoAlertResult CreateAlertResult(AbnormalInfor abnormal)
        {
            var result = new AbnormalInfoAlertResult() { AlertInfoList = new List<AbnormalInfoPusher>() };
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息单号".L10N(),
                Value = abnormal.No
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息描述".L10N(),
                Value = abnormal.AbnormalInfoDefinition.Desc
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息分类".L10N(),
                Value = abnormal.AbnormalInfoDefinition.AbnormalCategory.Desc
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常任务发生时间".L10N(),
                Value = abnormal.CreateDate.ToString("yyyy年MM月dd日 HH:mm")
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常任务处理完成时间".L10N(),
                Value = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常任务状态".L10N(),
                Value = abnormal.AbnormalStatus.ToLabel()
            });
            return result;
        }

        /// <summary>
        /// 获取异常信息的推送方式
        /// </summary>
        /// <param name="abnormal"></param>
        /// <returns></returns>
        protected virtual Pusher GetPusher(AbnormalInfor abnormal)
        {
            Pusher pusher = null;
            Check.AssertNotNull(abnormal, "异常信息不能为空。".L10N());
            var settings = Query<DefinitionSenderSettings>().Where(p => p.AbnormalDefinitionId == abnormal.AbnormalInfoDefinitionId).OrderByDescending(p => p.TimeType).ToList();  //按时间降序
            var sendLogs = Query<SendLog>().Where(p => p.AbnormalInforId == abnormal.Id).ToList();
            if (sendLogs == null || sendLogs.Count == 0)
            {
                pusher = settings.LastOrDefault()?.Pusher;
            }
            else
            {
                var set = settings.FirstOrDefault(p => sendLogs.Any(k => k.SenderSettingId == p.Id));//最后一次推送通知
                pusher = set?.Pusher;
            }
            return pusher;
        }

        /// <summary>
        /// 推送PDCA
        /// </summary>
        /// <param name="abnormal"></param>
        protected virtual void SendPdca(AbnormalInfor abnormal)
        {
            CreatePdcaReportEvent reportEvent = CreatePdcaReportSubmittedEvent(abnormal);
            RT.Service.Resolve<ICreatePdcaReportEvent>().GeneratePdcaReport(reportEvent);
        }

        /// <summary>
        /// 发起改善
        /// </summary>
        /// <param name="abnormal">异常信息</param>
        /// <returns>异常信息发起改善事件</returns>
        CreatePdcaReportEvent CreatePdcaReportSubmittedEvent(AbnormalInfor abnormal)
        {
            Defect highLevelDefect = null;
            List<DefectInfo> defects = new List<DefectInfo>();

            var defectDesc = abnormal.AbnormalInfoDefinitionDesc.IsNullOrEmpty() ? abnormal.AbnormalInfoDefinition.Desc : abnormal.AbnormalInfoDefinitionDesc;  //问题描述

            if (abnormal.DefectIds.IsNullOrEmpty())
                throw new ValidationException("请选择缺陷代码或取消发起PDCA再提交报告。".L10N());

            // 查找缺陷等级最高的排序
            if (!abnormal.DefectIds.IsNullOrEmpty())
            {
                var defectIds = Array.ConvertAll(abnormal.DefectIds.Split(','), d => double.Parse(d)).ToList();
                var defectList = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
                if (defectList.IsNotEmpty())
                {
                    highLevelDefect = GetHighLevelDefect(defectList);

                    // 将检验单据的所有检验项目的缺陷、缺陷等级记录推送到PDCA改善报告的问题列表中
                    defects = TransferToDefectInfoList(defectList);
                }
            }

            // 根据物料获取对应的质量分类id  
            double? categoryId = null;
            if (abnormal.ItemId.HasValue)
            {
                var category = RT.Service.Resolve<ItemController>().GetQualityCategory(abnormal.ItemId.Value, CategoryType.Quality);
                categoryId = category?.ItemCategoryId;
            }
            if (highLevelDefect == null)
                throw new ValidationException("找不到缺陷等级最高的缺陷".L10N());

            // 创建PDCA改善报告
            return new CreatePdcaReportEvent
            {
                BillId = abnormal.Id,
                BillNo = abnormal.No,
                ItemId = abnormal.ItemId,
                QualityCategoryId = categoryId,
                DefectId = highLevelDefect.Id,
                DefectLevel = highLevelDefect.DefectLevel,
                DefectDescription = defectDesc,
                WorkShopId = abnormal.WorkShopId,
                LineId = abnormal.LineId,
                DefectInfoList = defects.DistinctBy(item => item.DefectId).ToList(), //去掉重复的问题
            };
        }

        /// <summary>
        /// 取最高等级的缺陷等级名称
        /// </summary>
        /// <param name="defectIds"></param>
        /// <returns></returns>
        public virtual string GetHighestDefectLevel(List<double> defectIds)
        {
            // 查找缺陷等级最高的排序
            if (defectIds.IsNullOrEmpty())
                return null;
            var defectList = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
            if (defectList.IsNotEmpty())
            {
                var highLevelDefect = GetHighLevelDefect(defectList);
                return highLevelDefect.DefectGrade.Name;
            }
            return default;
        }

        /// <summary>
        /// 计算最高等级缺陷
        /// </summary>
        /// <param name="defectList"></param>
        /// <returns></returns>
        protected virtual Defect GetHighLevelDefect(EntityList<Defect> defectList)
        {
            Defect highLevelDefect = null;

            if (defectList.IsNullOrEmpty())
            {
                throw new ValidationException("缺陷清单为空，不能计算最高等级缺陷".L10N());
            }
            else
            {
                highLevelDefect = defectList.First();
            }

            // 查找缺陷等级最高的排序
            DefectSeverity highLevel = DefectSeverityHelper.LowestDefectSeverity;

            foreach (var item in defectList)
            {
                DefectSeverity curLevel = item.DefectSeverity;
                if (DefectSeverityHelper.IsHigherThan(curLevel, highLevel))
                {
                    highLevel = curLevel;
                    highLevelDefect = item;
                }
            }

            // 取检验单据所有检验项目中缺陷等级最高的检验项目的缺陷、缺陷等级作为PDCA改善清单的缺陷、缺陷等级
            if (highLevelDefect == null)
                throw new ValidationException("找不到缺陷等级最高的缺陷".L10N());
            return highLevelDefect;
        }

        /// <summary>
        /// 缺陷信息转换
        /// </summary>
        /// <param name="defectList"></param>
        /// <returns></returns>
        protected virtual List<DefectInfo> TransferToDefectInfoList(EntityList<Defect> defectList)
        {
            var defects = new List<DefectInfo>();
            foreach (var item in defectList)
            {
                if (defects.All(p => p.DefectId != item.Id))
                    defects.Add(new DefectInfo() { DefectCode=item.Code, DefectId = item.Id, DefectLevel = item.DefectLevel, QualityType = EnumViewModel.EnumToLabel(item.QualityType).L10N() });
            }
            return defects;
        }

        /// <summary>
        /// 保存异常信息
        /// </summary>
        /// <param name="abnormal"></param>
        public virtual void SaveAbnormalInfo(AbnormalInfor abnormal)
        {
            if (abnormal.AbnormalStatus == AbnormalStatus.ToProcess)
                abnormal.AbnormalStatus = AbnormalStatus.Processing;
            RF.Save(abnormal);
        }

        /// <summary>
        /// 获取未完成单据及其关联的未推送升级设置
        /// </summary>
        /// <returns></returns>
        public virtual List<AbnormalInfoAndSetting> GetNotClosedAbnormalInfos()
        {
            var settingList = GetNotCloseSettings();

            if (settingList?.Count > 0)
            {
                var logList = GetNotCloseSendLogs();
                var list = settingList.Where(p => logList.All(k => k.SenderSettingId != p.SettingId || k.AbnormalInforId != p.AbnormalInfoId)).ToList();//未有推送记录
                LoadAbnormalAndSetting(list);
                return list;
            }
            else
                return new List<AbnormalInfoAndSetting>();
        }

        /// <summary>
        /// 加载异常信息和设置
        /// </summary>
        /// <param name="list"></param>
        private void LoadAbnormalAndSetting(List<AbnormalInfoAndSetting> list)
        {
            if (list?.Count > 0)
            {
                //先加载相关异常信息和设置
                var abnormalIDs = list.Select(p => p.AbnormalInfoId).Distinct().ToList();
                var setIDs = list.Select(p => p.SettingId).Distinct().ToList();
                var abnormals = Query<AbnormalInfor>().Where(p => abnormalIDs.Contains(p.Id)).ToList();
                var settings = Query<DefinitionSenderSettings>().Where(p => setIDs.Contains(p.Id)).ToList();

                list.ForEach(p =>
                {
                    if (abnormals?.Count > 0)
                        p.AbnormalInfo = abnormals.FirstOrDefault(k => k.Id == p.AbnormalInfoId);
                    if (settings?.Count > 0)
                        p.Setting = settings.FirstOrDefault(k => k.Id == p.SettingId);
                });
            }
        }

        /// <summary>
        /// 获取未关闭信息关联的推送设置
        /// </summary>
        private IList<AbnormalInfoAndSetting> GetNotCloseSettings()
        {
            var q = Query<AbnormalInfor>();
            q.Join<AbnormalInfoDefinition>((info, def) => info.AbnormalStatus != AbnormalStatus.Close && info.AbnormalInfoDefinitionId == def.Id).Join<AbnormalInfoDefinition, DefinitionSenderSettings>((def, set) => def.Id == set.AbnormalDefinitionId);
            var list = q.Select<DefinitionSenderSettings>((info, set) => new
            {
                AbnormalInfoId = info.Id,
                SettingId = set.Id,
            }).ToList<AbnormalInfoAndSetting>();
            return list;
        }

        /// <summary>
        /// 获取未关闭信息关联的推送记录
        /// </summary>
        private EntityList<SendLog> GetNotCloseSendLogs()
        {
            var q = Query<SendLog>();
            q.Join<AbnormalInfor>((log, info) => info.AbnormalStatus != AbnormalStatus.Close && info.Id == log.AbnormalInforId);
            return q.ToList();
        }

        /// <summary>
        /// 根据资源获取异常管理信息
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>异常管理信息</returns>
        public virtual EntityList<AbnormalInfor> GetAbnormalInfors(double resourceId)
        {
            var elo = new EagerLoadOptions();
            elo.LoadWith(AbnormalInfor.AbnormalInfoDefinitionProperty);
            elo.LoadWithViewProperty();
            var query = Query<AbnormalInfor>().Where(p => p.AbnormalStatus != AbnormalStatus.Close);
            if (resourceId > 0)
                query.Where(p => p.LineId == resourceId);
            return query.ToList(null, elo);
        }
        #endregion
    }

    /// <summary>
    /// 异常信息关联推送升级设置
    /// </summary>
    [Serializable]
    public class AbnormalInfoAndSetting
    {
        #region 异常信息
        /// <summary>
        /// 异常信息ID
        /// </summary>
        public double AbnormalInfoId { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public AbnormalInfor AbnormalInfo { get; set; }
        #endregion

        #region 异常发送设置
        /// <summary>
        /// 异常发送设置ID
        /// </summary>
        public double SettingId { get; set; }

        /// <summary>
        /// 异常发送设置
        /// </summary>
        public DefinitionSenderSettings Setting { get; set; }
        #endregion
    }
}
