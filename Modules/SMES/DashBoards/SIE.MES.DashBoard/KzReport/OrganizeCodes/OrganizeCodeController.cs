using SIE.Common;
using SIE.Core.ApiModels;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.OrganizeCodes
{
    public class OrganizeCodeController:DomainController
    {
        /// <summary>
        /// 获取Mrp
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="plantName"></param>
        /// <returns></returns>
        public virtual List<DictionaryData> GetMrps(string productLine,string plantName)
        {
            var query = Query<OrganizeCode>()
                .WhereIf(!productLine.IsNullOrEmpty(), p => p.ProductLine.Contains($"{productLine}"))
                .WhereIf(!plantName.IsNullOrEmpty(), p => p.PlantName.Contains($"{plantName}"));
              var entityList = query.ToList().GroupBy(p => new { p.FactoryCode }).
                ToDictionary(p=>p.Select(p=>p.FactoryCode).FirstOrDefault().ToString(),p=>p.ToList().Select(p=>p.MrpController).ToList());

            return entityList.Select(p=> new DictionaryData () { DicKey=p.Key, DicValue=p.Value }).ToList<DictionaryData>();
        }

        /// <summary>
        /// 获取 工厂编码 ，Mrp控制者
        /// </summary>
        /// <param name="PlantName"></param>
        /// <param name="FactoryCodeList"></param>
        /// <returns></returns>
        public virtual List<DictionaryData> GetMrps2(string PlantName, List<string> FactoryCodeList)
        {
            var query = Query<OrganizeCode>();

            // PlantName 精确匹配
            if (!string.IsNullOrEmpty(PlantName))
                query = query.Where(p => p.PlantName == PlantName);

            // FactoryCodeList 非空且有元素时才加入条件
            if (FactoryCodeList != null && FactoryCodeList.Any())
                query = query.Where(p => FactoryCodeList.Contains(p.FactoryCode));

            // 执行查询并分组转为字典：Key = FactoryCode, Value = List<string>
            var grouped = query.ToList()
                .GroupBy(p => p.FactoryCode)
                .ToDictionary(g => g.Key.ToString(), g => g.Select(p => p.MrpController).ToList());

            // 返回 DictionaryData 列表
            return grouped.Select(p => new DictionaryData { DicKey = p.Key, DicValue = p.Value }).ToList();
        }


        /// <summary>
        /// 获取组织代码
        /// </summary>
        /// <param name="productLine"></param>
        /// <param name="plantName"></param>
        /// <returns></returns>
        public virtual List<DictionaryObjData> GetOrganizeCodeList(string productLine, string plantName)
        {
            var query = Query<OrganizeCode>()
                .WhereIf(!productLine.IsNullOrEmpty(), p => p.ProductLine.Contains($"{productLine}"))
                .WhereIf(!plantName.IsNullOrEmpty(), p => p.PlantName.Contains($"{plantName}"));
            var entityList = query.ToList().GroupBy(p => new { p.FactoryCode }).
              ToDictionary(p => p.Select(p => p.FactoryCode).FirstOrDefault().ToString(), p => p.AsEntityList());
            return entityList.Select(p => new DictionaryObjData() { DicKey = p.Key, DicValue = p.Value.ToList<object>()}).ToList<DictionaryObjData>();
        }
    }
}
