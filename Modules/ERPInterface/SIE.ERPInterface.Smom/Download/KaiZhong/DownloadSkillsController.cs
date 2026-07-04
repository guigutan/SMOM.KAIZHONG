using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.PersonnelSkills;
using SIE.Resources.Skills;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadSkillsController : DomainController
    {




        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveSkills(List<SkillData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<SkillData> list = new List<SkillData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Skill, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //技能编码(技能名称都是一样的)
                    var codes = datas.Select(p => p.ZJNMC).Distinct().ToList();
                    var skills = RT.Service.Resolve<SkillController>().GetSkills(codes);

                    //工号
                    var employeeCodes = datas.Select(p => p.PERNR).Distinct().ToList();

                    //获取员工
                    var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeList(employeeCodes);

                    //获取任意一个技能分类
                    SkillCategory skillCategory = RT.Service.Resolve<SkillController>().GetSkillCategoryFirst();
                    if (skillCategory == null)
                        throw new ValidationException("请任意维护一个技能分类".L10N());

                    //获取员工技能
                    var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
                    var employeeSkills = RT.Service.Resolve<SkillController>().GetEmployeeSkills(employeeIds);


                    foreach (var item in datas)
                    {
                        try
                        {
                            var employeeSkill = employeeSkills.FirstOrDefault(p => p.EmployeeCode == item.PERNR && p.SkillCode == item.ZJNMC);
                            var employee = employees.FirstOrDefault(p => p.Code == item.PERNR);
                            if (employee == null)
                                throw new ValidationException("员工[{0}]不存在!".L10nFormat(item.PERNR));
                            //创建新数据
                            employeeSkill = CreateSkill(employeeSkill, item, employee, skills, skillCategory);
                            if (employeeSkills.All(p => p.Id != employeeSkill.Id))
                                employeeSkills.Add(employeeSkill);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"员工{item.PERNR}技能{item.ZJNMC}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<SkillData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<SkillData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveSkills(List<SkillData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<SkillData> list = new List<SkillData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            //技能编码(技能名称都是一样的)
            var codes = datas.Select(p => p.ZJNMC).Distinct().ToList();
            var skills = RT.Service.Resolve<SkillController>().GetSkills(codes);

            //工号
            var employeeCodes = datas.Select(p => p.PERNR).Distinct().ToList();

            //获取员工
            var employees = RT.Service.Resolve<EmployeeController>().GetEmployeeList(employeeCodes);

            //获取任意一个技能分类
            SkillCategory skillCategory = RT.Service.Resolve<SkillController>().GetSkillCategoryFirst();
            //获取员工技能
            var employeeIds = employees.Select(p => p.Id).Distinct().ToList();
            var employeeSkills = RT.Service.Resolve<SkillController>().GetEmployeeSkills(employeeIds);

            try
            {
                if (datas != null || datas.Count > 0)
                {
                    if (skillCategory == null)
                        throw new ValidationException("请任意维护一个技能分类".L10N());

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                var employeeSkill = employeeSkills.FirstOrDefault(p => p.EmployeeCode == item.PERNR && p.SkillCode == item.ZJNMC);
                                var employee = employees.FirstOrDefault(p => p.Code == item.PERNR);
                                if (employee == null)
                                    throw new ValidationException("员工[{0}]不存在!".L10nFormat(item.PERNR));
                                //创建新数据
                                employeeSkill = CreateSkill(employeeSkill, item, employee, skills, skillCategory);
                                if (employeeSkills.All(p => p.Id != employeeSkill.Id))
                                    employeeSkills.Add(employeeSkill);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"员工{item.PERNR}技能{item.ZJNMC}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<SkillData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;
        }

        public virtual EmployeeSkill CreateSkill(EmployeeSkill employeeSkill, SkillData data, Employee employee, EntityList<Skill> skills, SkillCategory skillCategory)
        {
            //如果SMOM基础技能为空，就创建一个
            var skill = skills.FirstOrDefault(p => p.Code == data.ZJNMC);
            if (skill == null)
            {
                skill = new Skill();
                skill.Code = data.ZJNMC;
                skill.Name = data.ZJNMC;
                skill.CategoryId = skillCategory.Id;
                skill.Category = skillCategory;
                skill.PersistenceStatus = Domain.PersistenceStatus.New;
                RF.Save(skill);
                skills.Add(skill);
            }

            if (employeeSkill == null)
            {
                employeeSkill = new EmployeeSkill();
                employeeSkill.Skill = skill;
                employeeSkill.SkillId = skill.Id;
                employeeSkill.Employee = employee;
                employeeSkill.EmployeeId = employee.Id;
                employeeSkill.AuthDate = DateTime.Now;
                employeeSkill.ExamRequired = ExamRequired.NoMatter;
                employeeSkill.AuthStatus = AuthStatus.Valid;
                employeeSkill.OperationRequired = OperationRequired.NoMatter;
                employeeSkill.TrainingRequired = TrainingRequired.NoMatter;
                employeeSkill.PersistenceStatus = PersistenceStatus.New;
                RF.Save(employeeSkill);
            }

            return employeeSkill;


        }
    }
}
