using SIE.Domain;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Resources.Enterprises
{
    /// <summary>
    /// 企业层级 控制器
    /// </summary>
    public class EnterpriseLevelTestController : DomainController
    {
        /// <summary>
        /// 根据企业层级名称获取相关层级的数据
        /// </summary>
        /// <param name="name">企业层级名称</param>
        /// <returns>返回企业层级相关数据</returns>
        public virtual List<EnterpriseLevel> GetEnterpriseLevel(string name)
        {
            List<EnterpriseLevel> result = new List<EnterpriseLevel>();
            EntityList<EnterpriseLevel> enterpriseLevels = Query<EnterpriseLevel>().Where(p => p.InvOrgId == RT.InvOrg || p.InvOrgId == 0).ToList();
            EnterpriseLevel currLevel = enterpriseLevels.FirstOrDefault(p => p.Name == name);
            if (currLevel != null)
            {
                // key:企业层级ID， value：企业层级信息
                Dictionary<double, EnterpriseLevel> dicTopLevel = enterpriseLevels.ToDictionary(p => p.Id);
                // key:企业层级ID， value：子企业层级信息
                Dictionary<double, List<EnterpriseLevel>> dicBottomLevel = enterpriseLevels.Where(p => p.TreePId.HasValue).GroupBy(p => p.TreePId.Value).ToDictionary(p => p.Key, p => p.ToList());
                List<EnterpriseLevel> topLevels = GetTopEnterpriseLevel(currLevel, dicTopLevel);
                List<EnterpriseLevel> bottomLevels = GetBottomEnterpriseLevel(currLevel, dicBottomLevel);
                result.AddRange(topLevels);
                result.AddRange(bottomLevels);
                result.Add(currLevel);
            }

            return result;
        }

        /// <summary>
        /// 获取相关的上层的企业层级信息
        /// </summary>
        /// <param name="currLevel">指定企业层级</param>
        /// <param name="dicLevel">企业层级字典信息key:企业层级ID， value：企业层级信息</param>
        /// <returns>返回相关的上层的企业层级信息</returns>
        private List<EnterpriseLevel> GetTopEnterpriseLevel(EnterpriseLevel currLevel, Dictionary<double, EnterpriseLevel> dicLevel)
        {
            List<EnterpriseLevel> levels = new List<EnterpriseLevel>();
            EnterpriseLevel tmpLevel = currLevel;
            while (tmpLevel.TreePId.HasValue && dicLevel.ContainsKey(tmpLevel.TreePId.Value))
            {
                tmpLevel = dicLevel[tmpLevel.TreePId.Value];
                levels.Add(tmpLevel);
            }

            return levels;
        }

        /// <summary>
        /// 获取相关的下层的企业层级信息
        /// </summary>
        /// <param name="currLevel">指定企业层级</param>
        /// <param name="dicBottomLevel">企业层级字典信息企业层级ID， value：子企业层级信息</param>
        /// <returns>返回相关的下层的企业层级信息</returns>
        private List<EnterpriseLevel> GetBottomEnterpriseLevel(EnterpriseLevel currLevel, Dictionary<double, List<EnterpriseLevel>> dicBottomLevel)
        {
            List<EnterpriseLevel> levels = new List<EnterpriseLevel>();
            List<EnterpriseLevel> tmpLevels = new List<EnterpriseLevel>() { currLevel };
            while (tmpLevels.Count > 0)
            {
                tmpLevels = dicBottomLevel.Where(p => tmpLevels.Any(q => q.Id == p.Key)).SelectMany(p => p.Value).ToList();
                levels.AddRange(tmpLevels);
            }

            return levels;
        }

        /// <summary>
        /// 创建企业层级
        /// </summary>
        /// <param name="isDefault">是否默认的企业层级</param>
        /// <returns>返回企业层级数据</returns>
        public virtual List<EnterpriseLevel> CreateEnterpriseLevel(bool isDefault)
        {
            string groupName = "集团";
            string factoryCode = "Factory";
            string factoryName = "工厂";
            string workShopCode = "WorkShop";
            string workShopName = "_车间";
            string lineCode = "Line";
            string lineName = "线体";
            List<EnterpriseLevel> levels = null;
            if (isDefault)
            {
                levels = GetEnterpriseLevel(factoryName);
            }

            if (levels == null || levels.Count == 0)
            {
                levels = new List<EnterpriseLevel>();
                EnterpriseLevel groupLevel = Query<EnterpriseLevel>().Where(p => p.InvOrgId == 0).FirstOrDefault();
                // 创建集团
                if (groupLevel == null)
                {
                    groupLevel = new EnterpriseLevel();
                    groupLevel.GenerateId();
                    groupLevel.Code = "Group";
                    groupLevel.Name = groupName;
                    groupLevel.InvOrgId = 0;
                    groupLevel.Type = EnterpriseType.Group;
                }

                levels.Add(groupLevel);

                // 创建工厂
                EnterpriseLevel factoryLevel = new EnterpriseLevel();
                factoryLevel.GenerateId();
                factoryLevel.Code = isDefault ? factoryCode : (factoryCode + factoryLevel.Id);
                factoryLevel.Name = isDefault ? factoryName : (factoryName + factoryLevel.Id);
                factoryLevel.InvOrgId = RT.InvOrg;
                factoryLevel.Type = EnterpriseType.Plant;
                factoryLevel.TreePId = groupLevel.Id;
                levels.Add(factoryLevel);

                // 创建车间
                EnterpriseLevel workShopLevel = new EnterpriseLevel();
                workShopLevel.GenerateId();
                workShopLevel.Code = isDefault ? workShopCode : (workShopCode + workShopLevel.Id);
                workShopLevel.Name = isDefault ? workShopName : (workShopName + workShopLevel.Id);
                workShopLevel.InvOrgId = RT.InvOrg;
                workShopLevel.Type = EnterpriseType.Shop;
                workShopLevel.TreePId = factoryLevel.Id;
                levels.Add(workShopLevel);

                // 创建线体
                EnterpriseLevel lineLevel = new EnterpriseLevel();
                lineLevel.GenerateId();
                lineLevel.Code = isDefault ? lineCode : (lineCode + lineLevel.Id);
                lineLevel.Name = isDefault ? lineName : (lineName + lineLevel.Id);
                lineLevel.InvOrgId = RT.InvOrg;
                lineLevel.Type = EnterpriseType.Line;
                lineLevel.TreePId = workShopLevel.Id;
                levels.Add(lineLevel);

                lineCode = "A" + lineCode;
                lineName = "A" + lineName;
                EnterpriseLevel lineLevel1 = new EnterpriseLevel();
                lineLevel1.GenerateId();
                lineLevel1.Code = isDefault ? lineCode : (lineCode + lineLevel1.Id);
                lineLevel1.Name = isDefault ? lineName : (lineName + lineLevel1.Id);
                lineLevel1.InvOrgId = RT.InvOrg;
                lineLevel1.Type = EnterpriseType.Line;
                lineLevel1.TreePId = workShopLevel.Id;
                levels.Add(lineLevel1);
            }

            return levels;
        }
    }
}