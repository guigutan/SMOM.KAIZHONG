using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.Skills;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Resources
{
    public partial class ResTestController : DomainController
    {
        #region 企业层级、企业模型
        /// <summary>
        /// 创建企业模型
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> CreateEnterprises(List<EnterpriseType> types)
        {
            var groupLevel = Query<EnterpriseLevel>().Where(p => p.Type == EnterpriseType.Group).FirstOrDefault();
            EntityList<EnterpriseLevel> levels = new EntityList<EnterpriseLevel>();
            if (groupLevel == null)
            {
                groupLevel = new EnterpriseLevel()
                {
                    Type = EnterpriseType.Group,
                    Code = "Group",
                    Name = "集团",
                    IsByHand = YesNo.No,
                    InvOrgId = 0
                };
                groupLevel.GenerateId();
                RF.Save(groupLevel);
            }
            double parentLevelId = groupLevel.Id;
            for (int i = 0; i < types.Count; i++)
            {
                var level = new EnterpriseLevel();
                level.GenerateId();
                level.Type = types[i];
                level.Code = $"{types[i]}{level.Id}";
                level.Name = $"{types[i].ToLabel()}{level.Id}";
                level.IsByHand = YesNo.Yes;
                level.InvOrgId = RT.InvOrg;
                level.TreePId = parentLevelId;
                level.IsResource = true;
                levels.Add(level);
                parentLevelId = level.Id;
            }
            RF.Save(levels);
            //组织模型
            EntityList<Enterprise> enterprises = new EntityList<Enterprise>();
            var group = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Group).FirstOrDefault();
            if (group == null)
            {
                group = new Enterprise()
                {
                    Level = groupLevel,
                    Code = "Group",
                    Name = "集团",
                    IsByHand = YesNo.No,
                    InvOrgId = RT.InvOrg,
                    IsResource = false
                };
                group.GenerateId();
                enterprises.Add(group);
            }
            double parentEnterpriseId = group.Id;
            levels.ForEach(level =>
            {
                var enterprise = new Enterprise();
                enterprise.GenerateId();
                enterprise.Level = level;
                enterprise.Code = $"{level.Type.ToLabel()}{level.Id}";
                enterprise.Name = $"{level.Type.ToLabel()}{level.Id}";
                enterprise.IsByHand = YesNo.Yes;
                enterprise.InvOrgId = 1;
                enterprise.IsResource = level.IsResource;
                enterprise.TreePId = parentEnterpriseId;
                parentEnterpriseId = enterprise.Id;
                enterprises.Add(enterprise);
            });
            RF.Save(enterprises);
            return enterprises;
        }

        /// <summary>
        /// 获取或创建企业模型
        /// 企业层级：集团-车间-产线
        /// 对应模型：集团-MES生产车间-MES生产产线
        /// </summary> 
        /// <returns></returns>
        public virtual EntityList<Enterprise> GetOrCreateEnterprises()
        {
            string shopLevelCode = $"MES-{EnterpriseType.Shop.ToString()}";
            string lineLevelCode = $"MES-{EnterpriseType.Line.ToString()}";
            var controller = RT.Service.Resolve<CommonController>();
            var groupLevel = controller.GetData<EnterpriseLevel>(p => p.Type == EnterpriseType.Group);
            EntityList<EnterpriseLevel> levels = new EntityList<EnterpriseLevel>();
            if (groupLevel == null)
            {
                groupLevel = new EnterpriseLevel()
                {
                    Type = EnterpriseType.Group,
                    Code = "Group",
                    Name = "集团",
                    IsByHand = YesNo.No,
                    InvOrgId = 0
                };
                groupLevel.GenerateId();
                RF.Save(groupLevel);
            }
            var shopLevel = controller.GetData<EnterpriseLevel>(p => p.Type == EnterpriseType.Shop && p.Code == shopLevelCode && p.Name == shopLevelCode && p.TreePId == groupLevel.Id);
            if (shopLevel == null)
            {
                shopLevel = new EnterpriseLevel();
                shopLevel.GenerateId();
                shopLevel.Type = EnterpriseType.Shop;
                shopLevel.Code = $"MES-{EnterpriseType.Shop.ToString()}";
                shopLevel.Name = $"MES-{EnterpriseType.Shop.ToString()}";
                shopLevel.IsByHand = YesNo.Yes;
                shopLevel.InvOrgId = RT.InvOrg;
                shopLevel.TreePId = groupLevel.Id;
                shopLevel.IsResource = true;
                levels.Add(shopLevel);
            }
            var lineLevel = controller.GetData<EnterpriseLevel>(p => p.Type == EnterpriseType.Line && p.Code == lineLevelCode && p.Name == lineLevelCode && p.TreePId == shopLevel.Id);
            if (lineLevel == null)
            {
                lineLevel = new EnterpriseLevel();
                lineLevel.GenerateId();
                lineLevel.Type = EnterpriseType.Line;
                lineLevel.Code = $"MES-{EnterpriseType.Line.ToString()}";
                lineLevel.Name = $"MES-{EnterpriseType.Line.ToString()}";
                lineLevel.IsByHand = YesNo.Yes;
                lineLevel.InvOrgId = RT.InvOrg;
                lineLevel.TreePId = shopLevel.Id;
                lineLevel.IsResource = true;
                levels.Add(lineLevel);
            }
            RF.Save(levels);
            //组织模型
            EntityList<Enterprise> enterprises = new EntityList<Enterprise>();
            var group = controller.GetData<Enterprise>(p => p.Level.Type == EnterpriseType.Group);
            if (group == null)
            {
                group = new Enterprise()
                {
                    Level = groupLevel,
                    Code = EnterpriseType.Group.ToString(),
                    Name = "集团",
                    IsByHand = YesNo.No,
                    InvOrgId = RT.InvOrg,
                    IsResource = false
                };
                group.GenerateId();
            }
            enterprises.Add(group);
            var shop = controller.GetData<Enterprise>(p => p.Level.Type == EnterpriseType.Shop && p.Code == "MES生产车间" && p.Name == "MES生产车间" && p.TreePId == group.Id);
            if (shop == null)
            {
                shop = new Enterprise();
                shop.GenerateId();
                shop.Level = shopLevel;
                shop.Code = "MES生产车间";
                shop.Name = "MES生产车间";
                shop.IsByHand = YesNo.Yes;
                shop.InvOrgId = 1;
                shop.IsResource = shopLevel.IsResource;
                shop.TreePId = group.Id;
            }
            enterprises.Add(shop);
            var line = controller.GetData<Enterprise>(p => p.Level.Type == EnterpriseType.Line && p.Code == "MES生产产线" && p.Name == "MES生产产线" && p.TreePId == shop.Id);
            if (line == null)
            {
                line = new Enterprise();
                line.GenerateId();
                line.Level = lineLevel;
                line.Code = "MES生产产线";
                line.Name = "MES生产产线";
                line.IsByHand = YesNo.Yes;
                line.InvOrgId = 1;
                line.IsResource = lineLevel.IsResource;
                line.TreePId = shop.Id;
            }
            enterprises.Add(line);
            RF.Save(enterprises);
            return enterprises;
        }
        #endregion

        #region 生产资源
        public virtual WipResource GetOrCreateWipResource()
        {
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                //创建车间产线
                var enterprise = GetOrCreateEnterprises();
                //创建默认日历方案
                GetOrCreateCalendarScheme("Unit MES采集测试日历方案");
                //同步车间产线
                RT.Service.Resolve<WipResourceController>().RunSync();
                var line = enterprise.FirstOrDefault(p => p.Level.Type == EnterpriseType.Line);
                if (line == null)
                    throw new ValidationException("企业模型数据异常");
                var wipResource = RT.Service.Resolve<WipResourceController>().GetScheResourceByResCode(line.Code);
                if (wipResource == null)
                    throw new ValidationException("企业模型同步失败");
                tran.Complete();
                return wipResource;
            }
        }
        #endregion

        #region 班制 班次
        public virtual ShiftType GetOrCreateShiftType(string keywork)
        {
            var shiftType = RT.Service.Resolve<CommonController>().GetData<ShiftType>(p => p.Code == keywork && p.Name == keywork);
            if (shiftType == null)
            {
                shiftType = new ShiftType()
                {
                    Code = keywork,
                    Name = keywork,
                };
                shiftType.GenerateId();
                //6:00-13:59  14:00-21:59  22:00-5:59
                shiftType.ShiftList.Add(new Shift()
                {
                    Code = "早班",
                    Name = "早班",
                    BeginTime = Convert.ToDateTime("06:00"),
                    EndTime = Convert.ToDateTime("13:59")
                });
                shiftType.ShiftList.Add(new Shift()
                {
                    Code = "中班",
                    Name = "中班",
                    BeginTime = Convert.ToDateTime("14:00"),
                    EndTime = Convert.ToDateTime("21:59")
                });
                shiftType.ShiftList.Add(new Shift()
                {
                    Code = "晚班",
                    Name = "晚班",
                    BeginTime = Convert.ToDateTime("22:00"),
                    EndTime = Convert.ToDateTime("05:59")
                });
            }
            RF.Save(shiftType);
            return shiftType;
        }
        #endregion

        #region 周方案 日历方案
        public virtual CalendarScheme GetOrCreateCalendarScheme(string name)
        {
            var shiftType = GetOrCreateShiftType("Unit MES采集班制");
            string weekName = "Unit MES采集测试周方案";
            var scheme = RT.Service.Resolve<CalendarSchemeController>().GetCalendarScheme(name);
            if (scheme == null)
            {
                scheme = new CalendarScheme()
                {
                    Name = name,
                };
                scheme.GenerateId();
                scheme.SchemeWeeks.Add(new CalendarSchemeWeek()
                {
                    Name = weekName,
                    ActiveDate = DateTime.Today.AddDays(1),
                    Scheme = scheme,
                    Mon = true,
                    Tue = true,
                    Wed = true,
                    Thu = true,
                    Fri = true,
                    Sat = true,
                    Sun = true,
                    ShiftType = shiftType
                });
            }
            else
            {
                var weekScheme = scheme.SchemeWeeks.FirstOrDefault(p => p.Name == weekName);
                if (weekScheme == null)
                {
                    weekScheme = new CalendarSchemeWeek()
                    {
                        Name = weekName,
                        ActiveDate = DateTime.Today.AddDays(1),
                        Scheme = scheme,
                        Mon = true,
                        Tue = true,
                        Wed = true,
                        Thu = true,
                        Fri = true,
                        Sat = true,
                        Sun = true,
                        ShiftType = shiftType
                    };
                    scheme.SchemeWeeks.Add(weekScheme);
                }
                else
                {
                    weekScheme.ActiveDate = DateTime.Today.AddDays(1);
                }
            }
            scheme.IsEnable = YesNo.Yes;
            scheme.IsDefault = YesNo.Yes;
            RF.Save(scheme);
            return scheme;
        }
        #endregion

        #region 技能
        public virtual EntityList<Skill> CreateSkill(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            EntityList<Skill> results = new EntityList<Skill>();
            var skillCategory = CreateSkillCategory();
            for (int i = 0; i < count; i++)
            {
                var entity = new Skill();
                entity.GenerateId();
                entity.Code = $"Code{entity.Id}";
                entity.Name = $"Name{entity.Id}";
                entity.Category = skillCategory;
                results.Add(entity);
            }
            RF.Save(results);
            return results;
        }

        public virtual SkillCategory CreateSkillCategory()
        {
            var entity = new SkillCategory();
            entity.GenerateId();
            entity.Code = $"Code{entity.Id}";
            entity.Name = $"Name{entity.Id}";
            RF.Save(entity);
            return entity;
        }

        public virtual EmployeeSkill CreateEmployeeSkill(double employeeId, double skillId)
        {
            var entity = new EmployeeSkill();
            entity.GenerateId();
            entity.AuthDate = DateTime.Now;
            entity.ExamRequired = ExamRequired.NoMatter;
            entity.EmployeeId = employeeId;
            entity.AuthStatus = AuthStatus.Valid;
            entity.SkillId = skillId;
            entity.OperationRequired = OperationRequired.NoMatter;
            entity.TrainingRequired = TrainingRequired.NoMatter;
            RF.Save(entity);
            return entity;
        }
        #endregion
    }
}