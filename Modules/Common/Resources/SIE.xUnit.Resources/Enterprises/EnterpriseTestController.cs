using SIE.Domain;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Resources.Enterprises
{
    /// <summary>
    /// 企业模型 控制器
    /// </summary>
    public class EnterpriseTestController : DomainController
    {
        /// <summary>
        /// 根据企业模型名称获取相关模型的数据
        /// </summary>
        /// <param name="name">企业模型名称</param>
        /// <returns>返回企业模型相关数据</returns>
        public virtual List<Enterprise> GetEnterprise(string name)
        {
            List<Enterprise> result = new List<Enterprise>();
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Enterprise.LevelProperty);
            EntityList<Enterprise> enterprises = Query<Enterprise>().Where(p => p.InvOrgId == RT.InvOrg || p.InvOrgId == 0).ToList(null, elo);
            Enterprise currEp = enterprises.FirstOrDefault(p => p.Name == name);
            if (currEp != null)
            {
                // key:企业模型ID， value：企业模型信息
                Dictionary<double, Enterprise> dicTopEp = enterprises.ToDictionary(p => p.Id);
                // key:企业模型ID， value：子企业模型信息
                Dictionary<double, List<Enterprise>> dicBottomEp = enterprises.Where(p => p.TreePId.HasValue).GroupBy(p => p.TreePId.Value).ToDictionary(p => p.Key, p => p.ToList());

                List<Enterprise> topEnterprises = GetTopEnterprise(currEp, dicTopEp);
                List<Enterprise> bottomEnterprises = GetBottomEnterprise(currEp, dicBottomEp);
                result.AddRange(topEnterprises);
                result.AddRange(bottomEnterprises);
                result.Add(currEp);
            }

            return result;
        }

        /// <summary>
        /// 获取相关的上层的企业模型信息
        /// </summary>
        /// <param name="currEp">指定企业层级</param>
        /// <param name="dicEnterprise">企业模型字典信息key:企业模型ID， value：企业模型信息</param>
        /// <returns>返回相关的上层的企业模型信息</returns>
        private List<Enterprise> GetTopEnterprise(Enterprise currEp, Dictionary<double, Enterprise> dicEnterprise)
        {
            List<Enterprise> levels = new List<Enterprise>();
            Enterprise tmpLevel = currEp;
            while (tmpLevel.TreePId.HasValue && dicEnterprise.ContainsKey(tmpLevel.TreePId.Value))
            {
                tmpLevel = dicEnterprise[tmpLevel.TreePId.Value];
                levels.Add(tmpLevel);
            }

            return levels;
        }

        /// <summary>
        /// 获取相关的下层的企业模型信息
        /// </summary>
        /// <param name="currEp">指定企业模型</param>
        /// <param name="dicBottomEnterprise">企业层级字典信息企业模型ID， value：子企业模型信息</param>
        /// <returns>返回相关的下层的企业模型信息</returns>
        private List<Enterprise> GetBottomEnterprise(Enterprise currEp, Dictionary<double, List<Enterprise>> dicBottomEnterprise)
        {
            List<Enterprise> levels = new List<Enterprise>();
            List<Enterprise> tmpEps = new List<Enterprise>() { currEp };
            while (tmpEps.Count > 0)
            {
                tmpEps = dicBottomEnterprise.Where(p => tmpEps.Any(q => q.Id == p.Key)).SelectMany(p => p.Value).ToList();
                levels.AddRange(tmpEps);
            }

            return levels;
        }

        /// <summary>
        /// 创建企业模型
        /// </summary>
        /// <param name="isDefault">是否默认的企业模型</param>
        /// <returns>返回企业模型数据</returns>
        public virtual List<Enterprise> CreateEnterprise(bool isDefault)
        {
            string groupName = "集团";
            string factoryCode = "*Factory";
            string factoryName = "*工厂";
            string workShopCode = "*WorkShop";
            string workShopName = "*车间";
            string lineCode = "*Line";
            string lineName = "*线体";

            List<Enterprise> enterprises = null;
            if (isDefault)
            {
                enterprises = GetEnterprise(factoryName);
            }

            if (enterprises == null || enterprises.Count == 0)
            {
                enterprises = new List<Enterprise>();
                List<EnterpriseLevel> levels = RT.Service.Resolve<EnterpriseLevelTestController>().CreateEnterpriseLevel(isDefault);
                levels.Where(p => p.Type == EnterpriseType.Line).ForEach(p => p.IsResource = true);

                // 创建集团
                Enterprise groupEnterprise = CreateGroupEnterprise(groupName,levels);
                enterprises.Add(groupEnterprise);

                // 创建工厂
                Enterprise factoryEp = CreateFactoryEp(isDefault, factoryCode, factoryName, groupEnterprise, levels);
                enterprises.Add(factoryEp);

                //// 创建车间
                var workShopCode1 = "SMT" + workShopCode;
                var workShopName1 = "SMT" + workShopName;
                Enterprise workShopEp = new Enterprise();
                workShopEp.GenerateId();
                workShopEp.Code = isDefault ? workShopCode : (workShopCode + workShopEp.Id);
                workShopEp.Name = isDefault ? workShopCode1 : (workShopName1 + workShopEp.Id);
                workShopEp.InvOrgId = RT.InvOrg;
                workShopEp.Level = levels.First(p => p.Type == EnterpriseType.Shop);
                workShopEp.LevelId = workShopEp.Level.Id;
                workShopEp.IsResource = workShopEp.Level.IsResource;
                workShopEp.TreePId = factoryEp.Id;
                enterprises.Add(workShopEp);

                // 创建线体
                var lineCode0 = "SMT1" + lineCode;
                var lineName0 = "SMT1" + lineName;
                Enterprise lineEp = new Enterprise();
                lineEp.GenerateId();
                lineEp.Code = isDefault ? lineCode0 : (lineCode0 + lineEp.Id);
                lineEp.Name = isDefault ? lineName0 : (lineName0 + lineEp.Id);
                lineEp.InvOrgId = RT.InvOrg;
                lineEp.Level = levels.First(p => p.Type == EnterpriseType.Line);
                lineEp.LevelId = lineEp.Level.Id;
                lineEp.IsResource = lineEp.Level.IsResource;
                lineEp.TreePId = workShopEp.Id;
                enterprises.Add(lineEp);

                var lineCode1 = "SMT2" + lineCode;
                var lineName1 = "SMT2" + lineName;
                Enterprise lineEp1 = new Enterprise();
                lineEp1.GenerateId();
                lineEp1.Code = isDefault ? lineCode1 : (lineCode1 + lineEp1.Id);
                lineEp1.Name = isDefault ? lineName1 : (lineName1 + lineEp1.Id);
                lineEp1.InvOrgId = RT.InvOrg;
                lineEp1.Level = levels.Last(p => p.Type == EnterpriseType.Line);
                lineEp1.LevelId = lineEp1.Level.Id;
                lineEp1.IsResource = lineEp1.Level.IsResource;
                lineEp1.TreePId = workShopEp.Id;
                enterprises.Add(lineEp1);

                // 创建车间
                var workShopCode2 = "DIP" + workShopCode;
                var workShopName2 = "DIP" + workShopName;
                Enterprise workShopEpA = new Enterprise();
                workShopEpA.GenerateId();
                workShopEpA.Code = isDefault ? workShopCode2 : (workShopCode2 + workShopEpA.Id);
                workShopEpA.Name = isDefault ? workShopName2 : (workShopName2 + workShopEpA.Id);
                workShopEpA.InvOrgId = RT.InvOrg;
                workShopEpA.Level = levels.First(p => p.Type == EnterpriseType.Shop);
                workShopEpA.LevelId = workShopEpA.Level.Id;
                workShopEpA.IsResource = workShopEpA.Level.IsResource;
                workShopEpA.TreePId = factoryEp.Id;
                enterprises.Add(workShopEpA);

                // 创建线体
                var lineCode2 = "DIP1" + lineCode;
                var lineName2 = "DIP1" + lineName;
                Enterprise lineEpA = new Enterprise();
                lineEpA.GenerateId();
                lineEpA.Code = isDefault ? lineCode2 : (lineCode2 + lineEpA.Id);
                lineEpA.Name = isDefault ? lineName2 : (lineName2 + lineEpA.Id);
                lineEpA.InvOrgId = RT.InvOrg;
                lineEpA.Level = levels.First(p => p.Type == EnterpriseType.Line);
                lineEpA.LevelId = lineEpA.Level.Id;
                lineEpA.IsResource = lineEpA.Level.IsResource;
                lineEpA.TreePId = workShopEpA.Id;
                enterprises.Add(lineEpA);

                var lineCode3 = "DIP2" + lineCode;
                var lineName3 = "DIP2" + lineName;
                Enterprise lineEpA1 = new Enterprise();
                lineEpA1.GenerateId();
                lineEpA1.Code = isDefault ? lineCode3 : (lineCode3 + lineEpA1.Id);
                lineEpA1.Name = isDefault ? lineName3 : (lineName3 + lineEpA1.Id);
                lineEpA1.InvOrgId = RT.InvOrg;
                lineEpA1.Level = levels.Last(p => p.Type == EnterpriseType.Line);
                lineEpA1.LevelId = lineEpA1.Level.Id;
                lineEpA1.IsResource = lineEpA1.Level.IsResource;
                lineEpA1.TreePId = workShopEpA.Id;
                enterprises.Add(lineEpA1);

                // 创建车间
                var workShopCode3 = "ASSY" + workShopCode;
                var workShopName3 = "ASSY" + workShopName;
                Enterprise workShopEpB = new Enterprise();
                workShopEpB.GenerateId();
                workShopEpB.Code = isDefault ? workShopCode3 : (workShopCode3 + workShopEpB.Id);
                workShopEpB.Name = isDefault ? workShopName3 : (workShopName3 + workShopEpB.Id);
                workShopEpB.InvOrgId = RT.InvOrg;
                workShopEpB.Level = levels.First(p => p.Type == EnterpriseType.Shop);
                workShopEpB.LevelId = workShopEpB.Level.Id;
                workShopEpB.IsResource = workShopEpB.Level.IsResource;
                workShopEpB.TreePId = factoryEp.Id;
                enterprises.Add(workShopEpB);

                // 创建线体
                var lineCode4 = "ASSY1" + lineCode;
                var lineName4 = "ASSY1" + lineName;
                Enterprise lineEpB = new Enterprise();
                lineEpB.GenerateId();
                lineEpB.Code = isDefault ? lineCode4 : (lineCode4 + lineEpB.Id);
                lineEpB.Name = isDefault ? lineName4 : (lineName4 + lineEpB.Id);
                lineEpB.InvOrgId = RT.InvOrg;
                lineEpB.Level = levels.First(p => p.Type == EnterpriseType.Line);
                lineEpB.LevelId = lineEpB.Level.Id;
                lineEpB.IsResource = lineEpB.Level.IsResource;
                lineEpB.TreePId = workShopEpB.Id;
                enterprises.Add(lineEpB);

                var lineCode5 = "ASSY2" + lineCode;
                var lineName5 = "ASSY2" + lineName;
                Enterprise lineEpB1 = new Enterprise();
                lineEpB1.GenerateId();
                lineEpB1.Code = isDefault ? lineCode5 : (lineCode5 + lineEpB1.Id);
                lineEpB1.Name = isDefault ? lineName5 : (lineName5 + lineEpB1.Id);
                lineEpB1.InvOrgId = RT.InvOrg;
                lineEpB1.Level = levels.Last(p => p.Type == EnterpriseType.Line);
                lineEpB1.LevelId = lineEpB1.Level.Id;
                lineEpB1.IsResource = lineEpB1.Level.IsResource;
                lineEpB1.TreePId = workShopEpB.Id;
                enterprises.Add(lineEpB1);
            }

            return enterprises;
        }

        private Enterprise CreateGroupEnterprise(string groupName, List<EnterpriseLevel> levels)
        {
            Enterprise groupEnterprise = Query<Enterprise>().Where(p => p.InvOrgId == 0).FirstOrDefault();
            // 创建集团
            if (groupEnterprise == null)
            {
                groupEnterprise = new Enterprise();
                groupEnterprise.GenerateId();
                groupEnterprise.InvOrgId = 0;
                groupEnterprise.Code = "Group";
                groupEnterprise.Name = groupName;
                groupEnterprise.LevelId = levels.First(p => p.Type == EnterpriseType.Group).Id;
            }
            return groupEnterprise;
        }

        private Enterprise CreateFactoryEp(bool isDefault,string factoryCode,string factoryName,  Enterprise groupEnterprise, List<EnterpriseLevel> levels)
        {
            Enterprise factoryEp = new Enterprise();
            factoryEp.GenerateId();
            factoryEp.InvOrgId = RT.InvOrg;
            factoryEp.Code = isDefault ? factoryCode : (factoryCode + factoryEp.Id);
            factoryEp.Name = isDefault ? factoryName : (factoryName + factoryEp.Id);
            factoryEp.Level = levels.First(p => p.Type == EnterpriseType.Plant);
            factoryEp.LevelId = factoryEp.Level.Id;
            factoryEp.IsResource = factoryEp.Level.IsResource;
            factoryEp.TreePId = groupEnterprise.Id;
            return factoryEp;
        }
    }
}
