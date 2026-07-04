using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ApiInterfaces;
using SIE.MES.TeamManagement.RatedItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录控制器类
    /// </summary>
    public partial class ScoreRecordController
    {
        /// <summary>
        /// 获取班组成员信息集合
        /// </summary>
        /// <param name="workGourpId">班组Id</param>
        /// <returns>班组成员信息集合</returns>
        [ApiService("获取班组的成员信息")]
        [return: ApiReturn("班组成员信息. 参数类型: List<EmployeeInfo>")]
        public virtual List<EmployeeInfo> GetEmployeeByWorkGroupId([ApiParameter("班组Id")] double workGourpId)
        {
            CheckWorkGroupId(workGourpId);
            var employeeInfos = new List<EmployeeInfo>();
            var jobEmployees = GetJobEmployees(workGourpId);

            foreach (var curEmployee in jobEmployees)
            {
                var curEmpInfo = new EmployeeInfo();
                curEmpInfo.Id = curEmployee.Id;
                curEmpInfo.Code = curEmployee.Code;
                curEmpInfo.Name = curEmployee.Name;
                curEmpInfo.Sex = curEmployee.Sex;
                curEmpInfo.Phone = curEmployee.Phone;
                curEmpInfo.Email = curEmployee.Email;
                curEmpInfo.EmployeeType = curEmployee.EmployeeType;
                curEmpInfo.WorkGroupId = curEmployee.WorkGroupId;

                employeeInfos.Add(curEmpInfo);
            }

            return employeeInfos;
        }

        /// <summary>
        /// 获取评分项目分类信息集合
        /// </summary>
        /// <returns>评分项目分类信息集合</returns>
        [ApiService("获取评分项目分类信息")]
        [return: ApiReturn("评分项目分类信息. 参数类型:List<RatedItemCategoryInfo>")]
        public virtual List<RatedItemCategoryInfo> GetRatedCategoryInfos()
        {
            var ratedItemCategoryInfos = new List<RatedItemCategoryInfo>();
            var ratedItemCategorys = GetRatedItemCategories(); //获取所有的评分项目分类

            foreach (var curRatedItemCategory in ratedItemCategorys)
            {
                var curRatedCategoryInfo = new RatedItemCategoryInfo();
                curRatedCategoryInfo.Id = curRatedItemCategory.Id;
                curRatedCategoryInfo.Code = curRatedItemCategory.Code;
                curRatedCategoryInfo.Name = curRatedItemCategory.Name;
                ratedItemCategoryInfos.Add(curRatedCategoryInfo);
            }

            return ratedItemCategoryInfos;
        }

        /// <summary>
        /// 获取评分项目分类信息和评分项目信息
        /// isContainItem: true--评分项目分类信息和评分项目信息
        /// false--只包含评分项目分类信息
        /// </summary>
        /// <param name="isContainItem">是否包含评分项目</param>
        /// <returns>评分项目分类信息和评分项目信息</returns>
        [ApiService("获取评分项目分类信息和评分项目信息")]
        [return: ApiReturn("评分项目分类和评分项目信息. 参数类型:List<RatedItemCategoryInfo>")]
        public virtual List<RatedItemCategoryInfo> GetRatedItemCategoryInfos([ApiParameter("是否包含评分项目")] bool isContainItem)
        {
            var ratedItemCategoryInfos = new List<RatedItemCategoryInfo>();
            var ratedItemCategorys = GetRatedItemCategories(); //获取所有的评分项目分类
            EntityList<RatedItem> ratedItems = null;
            if (isContainItem)
                ratedItems = GetRatedItems(); //获取所有评分项目

            foreach (var curRatedItemCategory in ratedItemCategorys)
            {
                List<RatedItemInfo> ratedItemInfos = null;
                ratedItemInfos = CreateRatedItemInfos(ratedItems, curRatedItemCategory.Id);
                var curRatedCategoryInfo = CreateRatedItemCategoryInfo(curRatedItemCategory, ratedItemInfos);
                ratedItemCategoryInfos.Add(curRatedCategoryInfo);
            }

            return ratedItemCategoryInfos;
        }

        /// <summary>
        /// 班组长--评分填录
        /// </summary>
        /// <param name="scoreApplyInfo">评分填录API信息</param>
        [ApiService("班组长评分填录提交")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void CreateScoreRecord([ApiParameter("评分填录API信息")] ScoreApplyInfo scoreApplyInfo)
        {
            CheckScoreApplyInfoValid(scoreApplyInfo); //检查评分填录信息合法性       
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    var scoreRecords = CreateScoreRecords(scoreApplyInfo); //评分记录            
                    var scoreAttachments = CreateScoreAttachmentList(scoreApplyInfo.AttachmentList, scoreRecords); //评分记录附件

                    RF.Save(scoreRecords);
                    if (scoreAttachments != null && scoreAttachments.Any())
                    {
                        RF.Save(scoreAttachments);
                    }
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("班组长评分填录提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 班组长--班组员工评分查询
        /// 返回评分基本信息
        /// </summary>
        /// <param name="queryInfo">评分查询条件API</param>
        /// <returns>评分记录API信息集合</returns>
        [ApiService("班组成员评分查询")]
        [return: ApiReturn("评分记录API信息集合. 参数类型:List<ScoreRecordInfo>")]
        public virtual ScoreRecordInfos GetScoreRecordInfos([ApiParameter("评分查询条件API")] ScoreQueryInfo queryInfo)
        {
            CheckScoreQueryInfo(queryInfo);
            const bool isContainPetitions = false;
            PagingInfo pagingInfo = CreatePagingInfo(queryInfo.PageNumber, queryInfo.PageSize, true);
            var scoreRecords = GetScoreRecords(queryInfo, pagingInfo);
            if (scoreRecords == null || !scoreRecords.Any())
            {
                throw new ValidationException("未查询到员工评分信息!".L10N());
            }
            var scoreRecordInfos = CreateScoreRecordInfos(scoreRecords, isContainPetitions, pagingInfo);

            return scoreRecordInfos;
        }

        /// <summary>
        /// 班组长--员工评分明细查询
        /// 包含申诉信息、处理信息
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        /// <returns>评分记录API信息</returns>
        [ApiService("员工评分明细查询")]
        [return: ApiReturn("评分记录API信息. 参数类型: ScoreRecordInfo")]
        public virtual ScoreRecordInfo GetScoreRecordInfo([ApiParameter("评分记录Id")] double scoreRecordId)
        {
            var scoreRecord = CheckScoreRecordId(scoreRecordId);
            const bool isContainPetitions = true;
            var scoreRecordInfo = CreateScoreRecordInfo(scoreRecord, isContainPetitions);
            return scoreRecordInfo;
        }

        /// <summary>
        /// 班组长--员工评分申述处理
        /// </summary>
        /// <param name="processRecordInfo">处理记录API信息</param>
        [ApiService("评分申述处理提交")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void ProcessPetition([ApiParameter("处理记录API信息")] ProcessRecordInfo processRecordInfo)
        {
            var scoreRecord = CheckProcessRecordInfo(processRecordInfo);  //1.Check输入参数
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    ProcessRecord processRecord = null;
                    if (processRecordInfo.ProcessMode == (int)StateProcessMode.Adjust)
                        processRecord = CreateProcessRecord(scoreRecord, processRecordInfo); //10.生成处理记录
                    var curPetitionRecord = UpdatePetitionRecord(scoreRecord, processRecordInfo); //20.申诉记录属性修改                                        
                    ProcessPetitionUpdateScoreRecord(scoreRecord, ScoreState.Processed, processRecordInfo); //30.评分记录属性修改

                    if (processRecord != null)
                        RF.Save(processRecord);
                    RF.Save(curPetitionRecord);
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("员工评分申述处理提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 班组长--评分删除
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        [ApiService("班组长评分删除")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void DeleteScoreRecord([ApiParameter("评分记录Id")] double scoreRecordId)
        {
            var scoreRecord = CheckDeleteScoreRecord(scoreRecordId);
            try
            {
                DeleteUpdateScoreRecord(scoreRecord); //修改评分记录实体                
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("评分删除提交失败, 异常信息: [{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 员工个人评分查询
        /// </summary>
        /// <param name="queryInfo">评分查询条件API</param>
        /// <returns>员工个人评分API信息</returns>
        [ApiService("获取员工个人评分信息")]
        [return: ApiReturn("员工个人评分API信息. 参数类型:PersonalScoreInfo")]
        public virtual PersonalScoreInfo GetPersonalScoreInfos([ApiParameter("评分查询条件API")] ScoreQueryInfo queryInfo)
        {
            queryInfo.EmployeeId = RT.IdentityId;
            CheckPersonScoreQueryInfo(queryInfo);
            const bool isContainPetitions = true;
            PersonalScoreInfo personalScoreRecordInfo = null;
            PagingInfo pagingInfo = CreatePagingInfo(queryInfo.PageNumber, queryInfo.PageSize, true);
            var scoreRecords = GetScoreRecords(queryInfo, pagingInfo);
            if (scoreRecords == null || !scoreRecords.Any())
            {
                throw new ValidationException("未查询到员工评分信息!".L10N());
            }
            var scoreRecordInfos = CreateScoreRecordInfos(scoreRecords, isContainPetitions, pagingInfo);
            personalScoreRecordInfo = CreatePersonalScoreInfo(scoreRecordInfos);

            return personalScoreRecordInfo;
        }

        /// <summary>
        /// 评分记录员工申诉提交
        /// </summary>
        /// <param name="petitionInfo">申诉记录API信息</param>
        [ApiService("员工申诉提交")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void ScorePetition([ApiParameter("申诉记录API信息")] PetitionInfo petitionInfo)
        {
            var scoreRecord = CheckSubmitPetitionValid(petitionInfo); //1. Check传入的参数是否合法
            try
            {
                using (var tran = DB.TransactionScope(TeamManagementDataProvider.ConnectionStringName))
                {
                    var rowIndex = GetRowIndex(scoreRecord);
                    var curPetitionRecord = CreatePetitionRecord(petitionInfo, rowIndex); //10. 创建保存申诉记录                                        
                    var petitionAttachments = CreatePetitionAttachmentList(petitionInfo.AttachmentList, curPetitionRecord); //20. 创建保存申诉附件                                      
                    string updateMsg = string.Empty; //", 发起申诉!"
                    UpdateScoreRecord(scoreRecord, updateMsg, ScoreState.Stating); //30.修改评分记录的评分状态由"待申诉"-->"申诉中"

                    RF.Save(curPetitionRecord);
                    if (petitionAttachments != null && petitionAttachments.Any())
                    {
                        RF.Save(petitionAttachments);
                    }
                    tran.Complete();
                }
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("员工评分申诉提交失败, 异常信息:[{0}]".L10nFormat(exMsg.Message));
            }
        }

        /// <summary>
        /// 评分记录员工撤销申诉提交
        /// </summary>
        /// <param name="scoreRecordId">评分记录Id</param>
        [ApiService("员工撤销申诉")]
        [return: ApiReturn("提交成功时无返回值, 提交异常时抛出异常信息. 参数类型: 无")]
        public virtual void CancelPetition([ApiParameter("评分记录Id")] double scoreRecordId)
        {
            var scoreRecord = CheckCancelPetitionValid(scoreRecordId); //1. 验证是否能取消
            try
            {
                CancelUpdateScoreRecord(scoreRecord); //修改评分记录实体
            }
            catch (Exception exMsg)
            {
                throw new ValidationException("员工撤销申诉提交失败, 异常信息: [{0}]".L10nFormat(exMsg.Message));
            }
        }
    }
}
