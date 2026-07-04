using SIE.Domain;
using SIE.Resources.Skills;
using System;
using System.Linq;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 人员技能控制器
    /// </summary>
    public partial class SkillAuthController : DomainController
    {
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>技能认证集合</returns>
        public virtual EntityList<SkillAuthentication> GetSkillAuths(SkillAuthenticationCriteria criteria)
        {
            var query = Query<SkillAuthentication>();
            if (criteria.SkillId.HasValue)
                query.Where(p => p.SkillId == criteria.SkillId);
            if (criteria.SkillCategoryId.HasValue)
                query.Where(p => p.Skill.CategoryId == criteria.SkillCategoryId);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据ID获取技能认证
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>返回技能认证实体</returns>
        public virtual SkillAuthentication GetSkillAuthentication(double id)
        {
            return RF.GetById<SkillAuthentication>(id);
        }

        /// <summary>
        /// 判断同一时间范围内是否存在培训记录
        /// </summary>
        /// <param name="record">编辑的培训记录</param>
        /// <returns>重复返回true，否则返回false</returns>
        public virtual bool IsExsitTraningRecord(TrainingRecord record)
        {
            return Query<TrainingRecord>()
                .Where(p => p.Id != record.Id
                    && p.SkillAuthId == record.SkillAuthId
                    && p.EmployeeId == record.EmployeeId
                    && ((p.BeginDate <= record.EndDate && p.EndDate >= record.EndDate)
                    || (p.BeginDate <= record.BeginDate && p.EndDate >= record.BeginDate)))
                .Count() > 0;
        }

        /// <summary>
        /// 判断同一时间内是否存在考试结果
        /// </summary>
        /// <param name="record">编辑的考试结果</param>
        /// <returns>重复返回true，否则返回false</returns>
        public virtual bool IsExsitExamResult(ExamResult record)
        {
            return Query<ExamResult>()
                .Where(p => p.Id != record.Id
                    && p.SkillAuthId == record.SkillAuthId
                    && p.EmployeeId == record.EmployeeId
                    && p.ExamTime == record.ExamTime)
                .Count() > 0;
        }

        /// <summary>
        /// 判断同一时间内是否存在实操记录
        /// </summary>
        /// <param name="record">编辑的实操记录</param>
        /// <returns>重复返回true，否则返回false</returns>
        public virtual bool IsExsitOperationRecord(OperationRecord record)
        {
            return Query<OperationRecord>()
                .Where(p => p.Id != record.Id
                    && p.SkillAuthId == record.SkillAuthId
                    && p.EmployeeId == record.EmployeeId
                    && p.AuditTime == record.AuditTime)
                .Count() > 0;
        }

        #region 自动更新员工维护下的技能授予
        /// <summary>
        /// 提交培训记录后触发检查技能授予
        /// </summary>
        /// <param name="trainingRecord">培训记录</param>
        public virtual void UpdateTrainingRecord(TrainingRecord trainingRecord)
        {
            UpdateEmploySkill(trainingRecord.SkillAuthId, trainingRecord.EmployeeId);
        }

        /// <summary>
        /// 提交考试结果后触发检查技能授予
        /// </summary>
        /// <param name="examResult">考试结果</param> 
        public virtual void UpdateExamResult(ExamResult examResult)
        {
            UpdateEmploySkill(examResult.SkillAuthId, examResult.EmployeeId);
        }

        /// <summary>
        /// 提交实操记录后触发检查技能授予
        /// </summary>
        /// <param name="operationRecord">实操记录</param>
        public virtual void UpdateOperationRecord(OperationRecord operationRecord)
        {
            UpdateEmploySkill(operationRecord.SkillAuthId, operationRecord.EmployeeId);
        }

        /// <summary>
        /// 更新员工技能
        /// </summary>
        /// <param name="skillAuthId">技能认证ID</param>
        /// <param name="employeeId">员工ID</param>
        public virtual void UpdateEmploySkill(double skillAuthId, double employeeId)
        {
            var skillAuth = RF.GetById<SkillAuthentication>(skillAuthId);
            ////培训结果验证
            var trainRecords = GetTrainingRecords(employeeId);
            TrainingRecord trainingRecord = null;
            if (!IsTrainingOk(skillAuth.TrainingRequired, trainRecords, ref trainingRecord))
                return;
            ////考试结果验证
            var examRecords = GetExamResults(employeeId);
            ExamResult examResult = null;
            if (!IsExamOk(skillAuth.ExamRequired, examRecords, ref examResult))
                return;
            ////实操结果验证
            var operationRecords = GetOperationRecords(employeeId);
            OperationRecord operationRecord = null;
            if (!IsOperationOk(skillAuth.OperationRequired, operationRecords, ref operationRecord))
                return;
            ////认证时间
            var trainDate = trainingRecord != null && trainingRecord.EndDate.HasValue ? trainingRecord.EndDate.Value : DateTime.MinValue;
            var examDate = examResult != null && examResult.ExamTime.HasValue ? examResult.ExamTime.Value : DateTime.MinValue;
            var operationDate = operationRecord != null && operationRecord.AuditTime.HasValue ? operationRecord.AuditTime.Value : DateTime.MinValue;
            DateTime authDate = GetLastDate(trainDate, examDate, operationDate);
            ////更新员工维护下的技能
            UpdateEmployeeSkill(employeeId, skillAuth, trainRecords, trainingRecord, examRecords, examResult, operationRecords, operationRecord, authDate);
        }

        /// <summary>
        /// 培训结果是否满足
        /// </summary> 
        /// <param name="target">培训要求</param>
        /// <param name="records">培训结果集</param>
        /// <param name="trainingRecord">最新培训记录</param>
        /// <returns>真：考试结果达标，假：考试结果未达标</returns>
        private bool IsTrainingOk(TrainingRequired target, EntityList<TrainingRecord> records, ref TrainingRecord trainingRecord)
        {
            if (target == TrainingRequired.NoMatter)
                return true;
            if (target == TrainingRequired.Finish) //培训要求是：完成，则只能在结果集里有完成才满足
            {
                var record = records.Where(p => p.Result == TrainingRequired.Finish).OrderByDescending(p => p.EndDate).FirstOrDefault();
                if (record == null)
                    return false;
                trainingRecord = record;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 考试结果是否满足
        /// </summary> 
        /// <param name="target">考试要求</param>
        /// <param name="records">考试记录集合</param> 
        /// <param name="examResult">最新考试结果</param>
        /// <returns>真：考试结果达标，假：考试结果未达标</returns>
        private bool IsExamOk(ExamRequired target, EntityList<ExamResult> records, ref ExamResult examResult)
        {
            if (target == ExamRequired.NoMatter)
                return true;
            if (target == ExamRequired.Pass) //考试要求是：及格，则只能在结果集里有及格和优秀才满足
            {
                var record = records.Where(p => (p.Result == ExamRequired.Pass || p.Result == ExamRequired.Excellent)).OrderByDescending(p => p.ExamTime).FirstOrDefault();
                if (record == null)
                    return false;
                examResult = record;
                return true;
            }

            if (target == ExamRequired.Excellent) //考试要求是：优秀，则只能在结果集里有优秀才满足
            {
                var record = records.Where(p => p.Result == ExamRequired.Excellent).OrderByDescending(p => p.ExamTime).OrderByDescending(p => p.ExamTime).FirstOrDefault();
                if (record == null)
                    return false;
                examResult = record;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 实操结果是否满足
        /// </summary> 
        /// <param name="target">实操要求</param>
        /// <param name="records">实操记录集合</param>
        /// <param name="operationRecord">最新实操记录</param> 
        /// <returns>真：实操结果达标，假：实操结果不达标</returns>
        private bool IsOperationOk(OperationRequired target, EntityList<OperationRecord> records, ref OperationRecord operationRecord)
        {
            if (target == OperationRequired.NoMatter)
                return true;
            if (target == OperationRequired.Pass) //实操要求是：通过，则只能在结果集里有通过和满意才满足
            {
                var record = records.Where(p => p.Result == OperationRequired.Pass || p.Result == OperationRequired.Satisfaction).OrderByDescending(p => p.AuditTime).FirstOrDefault();
                if (record == null)
                    return false;
                operationRecord = record;
                return true;
            }

            if (target == OperationRequired.Satisfaction) //实操要求是：满意，则只能在结果集里有满意才满足
            {
                var record = records.Where(p => p.Result == OperationRequired.Satisfaction).OrderByDescending(p => p.AuditTime).OrderByDescending(p => p.AuditTime).FirstOrDefault();
                if (record == null)
                    return false;
                operationRecord = record;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取三个日期最晚那个日期
        /// </summary>
        /// <param name="d1">第一个日期</param>
        /// <param name="d2">第二个日期</param>
        /// <param name="d3">第三个日期</param>
        /// <returns>最晚的日期</returns>
        private DateTime GetLastDate(DateTime d1, DateTime d2, DateTime d3)
        {
            DateTime d = d1;
            if (d < d2) d = d2;
            if (d < d3) d = d3;
            return d;
        }

        /// <summary>
        /// 更新员工维护下技能授予
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="skillAuth">技能认证</param>
        /// <param name="trainingRecords">员工培训记录</param>
        /// <param name="trainingRecord">最新培训记录</param>
        /// <param name="examResults">员工考试结果</param>
        /// <param name="examRecord">最新考试结果</param>
        /// <param name="operationRecords">员工实操记录</param>
        /// <param name="operationRecord">最新实操记录</param>
        /// <param name="authDate">认证日期</param> 
        private void UpdateEmployeeSkill(double employeeId, SkillAuthentication skillAuth, EntityList<TrainingRecord> trainingRecords, TrainingRecord trainingRecord, EntityList<ExamResult> examResults, ExamResult examRecord, EntityList<OperationRecord> operationRecords, OperationRecord operationRecord, DateTime authDate)
        {
            using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
            {
                DateTime? finalDate = null;
                if (skillAuth.Skill.Validity.HasValue)
                {
                    int validDay = skillAuth.Skill.Validity.Value;
                    finalDate = authDate.AddDays(validDay);
                }

                var examResult = examRecord == null ? ExamRequired.NoMatter : examRecord.Result;
                var trainResult = trainingRecord == null ? TrainingRequired.NoMatter : trainingRecord.Result;
                var operationResult = operationRecord == null ? OperationRequired.NoMatter : operationRecord.Result;
                var employeeSkill = RT.Service.Resolve<SkillController>().GetEmployeeSkill(employeeId, skillAuth.SkillId);
                if (employeeSkill == null)
                {
                    employeeSkill = new EmployeeSkill();
                    employeeSkill.EmployeeId = employeeId;
                    employeeSkill.SkillId = skillAuth.SkillId;
                    employeeSkill.GenerateId();
                }

                employeeSkill.AuthDate = authDate;
                employeeSkill.ExpireDate = finalDate?.Date;
                employeeSkill.ExamRequired = examResult;
                employeeSkill.TrainingRequired = trainResult;
                employeeSkill.OperationRequired = operationResult;
                employeeSkill.VerifierId = operationRecord?.VerifierId;
                employeeSkill.Validity = skillAuth.Skill.Validity;

                ////更新技能认证记录 authDate
                trainingRecords.ForEach(e =>
                {
                    if (e.EndDate.HasValue && e.EndDate <= authDate)
                        e.IsHistory = true;
                });
                examResults.ForEach(e =>
                {
                    if (e.ExamTime.HasValue && e.ExamTime <= authDate)
                        e.IsHistory = true;
                });
                operationRecords.ForEach(e =>
                {
                    if (e.AuditTime.HasValue && e.AuditTime <= authDate)
                        e.IsHistory = true;
                });
                RF.Save(employeeSkill);
                RF.Save(trainingRecords);
                RF.Save(examResults);
                RF.Save(operationRecords);
                tran.Complete();
            }
        }
        #endregion

        /// <summary>
        /// 返回两个日期间隔小时数
        /// </summary>
        /// <param name="d1">较晚日期</param>
        /// <param name="d2">较早日期</param>
        /// <returns>小时数</returns>
        [IgnoreProxy]
        public virtual decimal GetHourDiff(DateTime d1, DateTime d2)
        {
            TimeSpan ts1 = new TimeSpan(d1.Ticks);
            TimeSpan ts2 = new TimeSpan(d2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return decimal.Parse((ts.TotalMinutes / 60).ToString("###0.##"));
        }

        /// <summary>
        /// 获取员工培训记录
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="isHistory">是否包含历史数据，默认false</param>
        /// <returns>员工培训记录列表</returns>
        public virtual EntityList<TrainingRecord> GetTrainingRecords(double employeeId, bool? isHistory = false)
        {
            var query = Query<TrainingRecord>().Where(p => p.EmployeeId == employeeId);
            if (isHistory.HasValue)
                query.Where(p => p.IsHistory == isHistory);
            return query.ToList();
        }

        /// <summary>
        /// 获取员工考试结果
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="isHistory">是否包含历史数据，默认false</param>
        /// <returns>员工考试结果列表</returns>
        public virtual EntityList<ExamResult> GetExamResults(double employeeId, bool? isHistory = false)
        {
            var query = Query<ExamResult>().Where(p => p.EmployeeId == employeeId);
            if (isHistory.HasValue)
                query.Where(p => p.IsHistory == isHistory);
            return query.ToList();
        }

        /// <summary>
        /// 获取员工实操记录
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <param name="isHistory">是否包含历史数据，默认false</param>
        /// <returns>员工实操记录列表</returns>
        public virtual EntityList<OperationRecord> GetOperationRecords(double employeeId, bool? isHistory = false)
        {
            var query = Query<OperationRecord>().Where(p => p.EmployeeId == employeeId);
            if (isHistory.HasValue)
                query.Where(p => p.IsHistory == isHistory);
            return query.ToList();
        }
    }
}