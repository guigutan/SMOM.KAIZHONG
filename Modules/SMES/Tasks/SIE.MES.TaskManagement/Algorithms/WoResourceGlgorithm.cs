using SIE.Common.Algorithm;
using SIE.Domain.Validation;
using SIE.EventMessages.Resources.WipResources;
using SIE.MES.TaskManagement.Dispatchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Algorithms
{
    /// <summary>
    /// 产线编码算法(取后两位)
    /// </summary>
    [Algorithm("产线编码算法(取后两位)", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    public class WoResourceGlgorithm : CodeAlgorithm<DispatchTask>
    {
        public override string GetCode(DispatchTask data)
        {
            if (data == null)
                throw new ValidationException("任务单不存在,无法生成编码规则".L10N());
            if (data.ResourceId == null)
                return string.Empty;
            var code = RT.Service.Resolve<IWipResources>().WipResourcesName(data.ResourceId.Value);                
            // 确保字符串长度至少为2
            if (code.Length >= 2)
            {
                // 从长度减2的位置开始，截取2个字符
                code = code.Substring(code.Length - 2, 2);
            }
            return code;
        }
    }

}
