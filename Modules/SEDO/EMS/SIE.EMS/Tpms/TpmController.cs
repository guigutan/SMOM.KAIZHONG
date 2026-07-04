using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Tpms.Configs;
using SIE.EMS.Tpms.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// Tpm控制器
    /// </summary>
    public partial class TpmController : DomainController
    {
        /// <summary>
        /// 获取Tmp评分单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetTpmScoreNo()
        {
            var config = ConfigService.GetConfig(new TpmScoreNoConfig(), typeof(TpmRecord));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到Tpm评分单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 用于判断同类型不能有项目名称
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="scoreType">类型</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public virtual bool GetProjectName(string projectName, ScoreType scoreType, double id)
        {
            return Query<TpmWeekInspectScore>()
                .Where(p => p.ProjectName == projectName && p.ScoreType == scoreType && p.Id != id).Count() > 0;
        }

        /// <summary>
        /// 获取TPM周工作评分检查项
        /// </summary>
        /// <returns>周工作评分检查项列表</returns>
        public virtual EntityList<TpmWeekInspectScore> GetTpmWeekInspectScores()
        {
            return Query<TpmWeekInspectScore>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取TPM检查评分项
        /// </summary>
        /// <param name="scoreType">类型</param>
        /// <returns></returns>
        public virtual EntityList<TpmWeekInspectScore> GetTpmWeekInspectScores(ScoreType scoreType)
        {
            return Query<TpmWeekInspectScore>().Where(p => p.ScoreType == scoreType).ToList();
        }

        /// <summary>
        /// 保存Tmp评分操作记录
        /// </summary>
        /// <param name="mainInfo">TPM操作日志</param>
        /// <param name="details">TPM评分明细列表</param>
        /// <returns>TPM操作日志Id</returns>
        public virtual double SaveTempScoreRecord(TpmRecord mainInfo, EntityList<TpmRecordDetail> details)
        {
            using (var trans = DB.TransactionScope(EMS.EmsEntityDataProvider.ConnectionStringName))
            {
                mainInfo.DetailList.Clear();

                details.ForEach(e =>
                {
                    mainInfo.DetailList.Add(e);
                });
                RF.Save(mainInfo);
                trans.Complete();
            }
            return mainInfo.Id;
        }

        /// <summary>
        /// 验证百分比总和为100%
        /// </summary>
        /// <param name="newData">新的周工作检查评分项列表</param>
        /// <param name="updatedData">更新的周工作检查评分项列表</param>
        /// <param name="removedData">删除的周工作检查评分项列表</param>
        /// <returns>true/false</returns>
        public virtual bool Validation100(EntityList<TpmWeekInspectScore> newData, EntityList<TpmWeekInspectScore> updatedData, EntityList<TpmWeekInspectScore> removedData)
        {
            var data = Query<TpmWeekInspectScore>().ToList();

            if (newData.Count > 0)
            {
                data.AddRange(newData);
            }

            if (updatedData.Count > 0)
            {
                foreach (var item in updatedData)
                {
                    data.FirstOrDefault(p => p.Id == item.Id).ScoreType = item.ScoreType;//2019-05-30,钟南盛
                    //data.FirstOrDefault(p => p.Id == item.Id).ScoreRate = item.ScoreRate;
                }
            }

            if (removedData.Count > 0)
            {
                foreach (var item in removedData)
                {
                    data.Remove(data.FirstOrDefault(p => p.Id == item.Id));
                }
            }

            //if (data.Where(p => p.ScoreType == ScoreType.Check).Sum(p => p.ScoreRate) == 100)//2019-05-30,钟南盛
            //{
            //    return true;
            //}
            return false;
        }

        /// <summary>
        /// 通过查询实体获取TPM操作记录列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>TPM操作记录列表</returns>
        public virtual EntityList<TpmRecord> GetTpmRecords(TpmRecordCriteria criteria)
        {
            var query = Query<TpmRecord>();
            if (criteria.EquipCode.IsNotEmpty() || criteria.MachineNo.IsNotEmpty() || criteria.WorkshopId.HasValue || criteria.ProcessId.HasValue)
            {
                query.Exists<EquipAccount>((a, b) => b.Where(p => p.Id == a.EquipmentId)
                 .WhereIf(criteria.EquipCode.IsNotEmpty(), p => p.Code.Contains(criteria.EquipCode))
                 .WhereIf(criteria.MachineNo.IsNotEmpty(), p => p.Name.Contains(criteria.MachineNo))
                 .WhereIf(criteria.WorkshopId.HasValue, p => p.WorkShopId == criteria.WorkshopId)
                 .WhereIf(criteria.ProcessId.HasValue, p => p.ProcessId == criteria.ProcessId));
            }

            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据TPM操作记录Id获取Tpm评分明细列表
        /// </summary>
        /// <param name="tpmRecordId">TPM操作记录Id</param>
        /// <returns>Tpm评分明细列表</returns>
        public virtual EntityList<TpmRecordDetail> GetTpmRecordDetails(double tpmRecordId)
        {
            return Query<TpmRecordDetail>().Where(p => p.TpmRecordId == tpmRecordId).ToList();
        }

        /// <summary>
        /// 获取TPM评分明细列表
        /// </summary>
        /// <returns>TPM评分明细列表</returns>
        public virtual EntityList<TpmRecordDetail> GetTpmRecordDetails()
        {
            var scoreList = RF.GetAll<TpmWeekInspectScore>();
            EntityList<TpmRecordDetail> recordDetails = new EntityList<TpmRecordDetail>();
            foreach (var score in scoreList)
            {
                var recordDetail = new TpmRecordDetail();
                recordDetail.WeekInspectScoreId = score.Id;
                recordDetail.ProjectName = score.ProjectName;
                recordDetail.ProjectType = score.ScoreType;//2019-05-30,ScoreType改为枚举。钟南盛
                //recordDetail.ScoreRate = score.ScoreRate;
                //recordDetail.CheckStandard = score.CheckStandard;
                recordDetails.Add(recordDetail);
            }

            return recordDetails;
        }

        /// <summary>
        /// 创建TPM操作记录信息
        /// </summary>
        /// <returns>TPM操作记录信息</returns>
        public virtual TpmRecordInfo CreateTpmRecordInfo()
        {
            TpmRecordInfo recordInfo = new TpmRecordInfo();
            recordInfo.ErrMsg = string.Empty;
            try
            {
                var now = RF.Find<TpmRecord>().GetDbTime();
                TpmRecord bill = new TpmRecord();
                bill.TpmNo = RT.Service.Resolve<TpmController>().GetTpmScoreNo();
                bill.CreateDate = now;
                bill.UpdateDate = now;
                bill.ExecutionTime = now;
                bill.GenerateId();
                recordInfo.Data = bill;
            }
            catch (Exception ex)
            {
                recordInfo.ErrMsg = ex.Message;
            }

            return recordInfo;
        }

        /// <summary>
        /// 保存新增TPM操作记录
        /// </summary>
        /// <param name="addRecordInfo">TPM操作记录信息</param>
        /// <returns>执行结果</returns>
        public virtual string SaveTpmRecordInfo(AddRecordInfo addRecordInfo)
        {
            string errMsg = string.Empty;
            EntityList<TpmRecordDetail> recordDetailList = new EntityList<TpmRecordDetail>();
            EntityList<TpmRecordDetail> deleteDetailList = new EntityList<TpmRecordDetail>();

            try
            {
                var tpmRecord = addRecordInfo.TpmRecord;
                var recordDetails = addRecordInfo.TpmRecordDetailList;
                ValidateTpmRecord(tpmRecord, recordDetails);
                deleteDetailList.AddRange(GetOldRecordDetails(tpmRecord));

                tpmRecord.ScorerName = RT.Identity.Name;
                int sumScore = 0;
                foreach (var recordDetail in recordDetails)
                {
                    TpmRecordDetail detail = CreateRecordDetail(recordDetail);
                    detail.TpmRecordId = tpmRecord.Id;
                    sumScore += detail.DeductScore;
                    recordDetailList.Add(detail);
                }

                var totalScore = 100 - sumScore;
                tpmRecord.TotalScore = totalScore < 0 ? 0 : totalScore;
                SaveTpmRelateInfo(tpmRecord, recordDetailList, deleteDetailList);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 保存TPM相关信息
        /// </summary>
        /// <param name="tpmRecord">Tpm操作记录</param>
        /// <param name="recordDetailList">Tpm评分明细列表</param>
        /// <param name="deleteDetailList">旧的Tpm评分明细列表</param>
        private void SaveTpmRelateInfo(TpmRecord tpmRecord, EntityList<TpmRecordDetail> recordDetailList, EntityList<TpmRecordDetail> deleteDetailList)
        {
            using (var trans = DB.TransactionScope(EMS.EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(tpmRecord);
                if (deleteDetailList.Any())
                    RF.Save(deleteDetailList);
                RF.Save(recordDetailList);
                trans.Complete();
            }
        }

        /// <summary>
        /// 创建Tpm评分明细
        /// </summary>
        /// <param name="recordDetail">Tpm评分明细原型</param>
        /// <returns>Tpm评分明细</returns>
        private TpmRecordDetail CreateRecordDetail(TpmRecordDetail recordDetail)
        {
            var detail = new TpmRecordDetail
            {
                WeekInspectScoreId = recordDetail.WeekInspectScoreId,
                DeductScore = recordDetail.DeductScore,
                Remark = recordDetail.Remark
            };
            //if (recordDetail.ProjectType == ScoreType.Specification && recordDetail.DeductScore > 0)
            //    detail.DeductScore = recordDetail.WeekInspectScore.ScoreRate;
            return detail;
        }

        /// <summary>
        /// 获取旧的Tpm评分明细列表
        /// </summary>
        /// <param name="tpmRecord">Tpm操作记录</param>
        /// <returns>旧的Tpm评分明细列表</returns>
        private List<TpmRecordDetail> GetOldRecordDetails(TpmRecord tpmRecord)
        {
            List<TpmRecordDetail> deleteDetailList = new List<TpmRecordDetail>();
            var oldRecord = RF.GetById<TpmRecord>(tpmRecord.Id);
            if (oldRecord == null)
                tpmRecord.PersistenceStatus = PersistenceStatus.New;
            else
                tpmRecord.PersistenceStatus = PersistenceStatus.Modified;

            var oldDetails = GetTpmRecordDetails(tpmRecord.Id);
            oldDetails.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            deleteDetailList.AddRange(oldDetails);
            return deleteDetailList;
        }

        /// <summary>
        /// 验证TPM相关信息
        /// </summary>
        /// <param name="tpmRecord">Tpm操作记录</param>
        /// <param name="recordDetails">Tpm评分明细</param>
        private void ValidateTpmRecord(TpmRecord tpmRecord, List<TpmRecordDetail> recordDetails)
        {
            if (tpmRecord.WorkGroup == null)
                throw new ValidationException("请选择班组！".L10N());
            if (HasLatestWeekScore(tpmRecord.EquipmentId))
                throw new ValidationException("该设备在本周已经评分!".L10N());
            //if (recordDetails.Any(p => p.ProjectType == ScoreType.Check && p.DeductScore > p.WeekInspectScore.ScoreRate))
            //    throw new ValidationException("扣分不能大于上限！".L10N());
            if (recordDetails.Any(p => p.DeductScore > 0 && p.Remark.IsNullOrEmpty()))
                throw new ValidationException("有扣分项必须填写备注！".L10N());
        }

        /// <summary>
        /// 检查该设备本周有无做过TPM评分
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <returns>true/false</returns>
        public virtual bool HasLatestWeekScore(double equipId)
        {
            var now = RF.Find<TpmRecord>().GetDbTime();
            var lastRecord = GetTpmRecordByEquipId(equipId);
            if (lastRecord == null)
                return false;
            var lastDate = lastRecord.CreateDate;
            var startOfWeek = lastDate.AddDays(1 - Convert.ToInt32(lastDate.DayOfWeek.ToString("d"))).Date;  //本周周一 
            var endOfWeek = startOfWeek.AddDays(6).Date.AddDays(1).AddSeconds(-1);  //本周周日 

            if (now >= startOfWeek && now <= endOfWeek)
                return true;

            return false;
        }

        /// <summary>
        /// 获取最新创建的TPM操作记录
        /// </summary>
        /// <param name="equipId">设备Id</param>
        /// <returns>最新创建的TPM操作记录</returns>
        public virtual TpmRecord GetTpmRecordByEquipId(double equipId)
        {
            return Query<TpmRecord>().Where(p => p.EquipmentId == equipId).OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 根据部门ID获取班组
        /// </summary>
        /// <param name="workshopId"></param>
        /// <param name="Code"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WorkGroup> QueryWorkGroupByWorkshopId(double workshopId, string Code, PagingInfo pagingInfo)
        {
            var q = Query<WorkGroup>().Where(p => p.DepartmentId == workshopId);
            if (Code.IsNotEmpty())
            {
                q.Where(o => o.Name.Contains(Code));
            }

            return q.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

    }
}
