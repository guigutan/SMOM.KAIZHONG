using SIE.Domain;
using SIE.Resources.ProcessTechTypes;
using System.Collections.Generic;

namespace SIE.xUnit.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型 控制器
    /// </summary>
    public class ProcessTechTypeTestController : DomainController
    {
        /// <summary>
        /// 根据制程工艺类型编号获取制程工艺类型信息
        /// </summary>
        /// <param name="codes">制程工艺类型编号</param>
        /// <returns>制程工艺类型信息</returns>
        public virtual EntityList<ProcessTechType> GetProcessTechType(List<string> codes)
        {
            var query = Query<ProcessTechType>().Where(p => codes.Contains(p.Code));

            return query.ToList();
        }

        /// <summary>
        /// 创建制程工艺类型信息
        /// </summary>
        /// <param name="isDefault">是否新建默认的数据</param>
        /// <returns></returns>
        public virtual EntityList<ProcessTechType> CreateProcessTechType(bool isDefault)
        {
            string smtCode = "SMT";
            string dipCode = "DIP";
            string assyCode = "ASSY";
            EntityList<ProcessTechType> enterprises = new EntityList<ProcessTechType>();
            if (isDefault)
            {
                enterprises = GetProcessTechType(new List<string>() { smtCode, dipCode, assyCode });
            }

            if (enterprises == null || enterprises.Count == 0)
            {
                // 创建SMT制程工艺类型
                ProcessTechType smtType = new ProcessTechType();
                smtType.GenerateId();
                smtType.Code = isDefault ? smtCode : (smtCode + smtType.Id);
                smtType.Name = isDefault ? smtCode : (smtCode + smtType.Id);
                smtType.AlgorithmMarking = AlgorithmMarking.NORMAL;
                enterprises.Add(smtType);

                // 创建DIP制程工艺类型
                ProcessTechType dipType = new ProcessTechType();
                dipType.GenerateId();
                dipType.Code = isDefault ? dipCode : (dipCode + dipType.Id);
                dipType.Name = isDefault ? dipCode : (dipCode + dipType.Id);
                dipType.AlgorithmMarking = AlgorithmMarking.NORMAL;
                enterprises.Add(dipType);

                // 创建ASSY制程工艺类型
                ProcessTechType assyType = new ProcessTechType();
                assyType.GenerateId();
                assyType.Code = isDefault ? assyCode : (assyCode + assyType.Id);
                assyType.Name = isDefault ? assyCode : (assyCode + assyType.Id);
                assyType.AlgorithmMarking = AlgorithmMarking.NORMAL;
                enterprises.Add(assyType);
            }

            return enterprises;
        }
    }
}