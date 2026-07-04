using SIE.Common.Configs;
using SIE.Core.Common.Service;
using SIE.Domain.Validation;
using SIE.LES.MaterialReceptions.APIModels;
using SIE.LES.MaterialReceptions.Configs;
using SIE.LES.MaterialReceptions.Enums;
using System;
using System.Linq;

namespace SIE.LES.MaterialReceptions.Services
{
    public partial class MaterialReceptionServices : DomainService
    {

        /// <summary>
        /// 获取配置项的接收方式
        /// </summary>
        /// <returns></returns>
        public virtual ConfigValues GetReceiveConfig()
        {
            var receiveConfig = ConfigService.GetConfig(new MaterialReceptionQtyConfig(), typeof(MaterialReception));
            if (receiveConfig == null)
            {
                throw new ValidationException("未找到接收方式配置,请检查配置项".L10N());
            }
            return receiveConfig.OrderQty;
        } 
    }
}
