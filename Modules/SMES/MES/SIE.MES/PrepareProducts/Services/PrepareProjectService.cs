using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PrepareProducts.Configs;
using SIE.MES.PrepareProducts.Daos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PrepareProducts.Services
{
    /// <summary>
    /// 产前准备项目维护服务类
    /// </summary>
    public class PrepareProjectService : DomainService
    {
        private readonly PrepareProjectDao _prepareProjectDao;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prepareProjectDao"></param>
        public PrepareProjectService(PrepareProjectDao prepareProjectDao)
        {
            _prepareProjectDao = prepareProjectDao;
        }

        /// <summary>
        /// 添加时根据编码获取数据库中的编码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual List<string> GetDataBasePreProByCode(List<string> codes)
        {
            return _prepareProjectDao.GetDataBasePreProByCode(codes);
        }

        /// <summary>
        /// 保存编码去重校验
        /// </summary>
        public virtual List<string> GetDataBasePreProByCode(List<double> ids)
        {
            return _prepareProjectDao.GetDataBasePreProByCode(ids);
        }

        /// <summary>
        /// 添加时根据名称获取数据库中的名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public virtual List<string> GetDataBasePreProByName(List<string> names)
        {
            return _prepareProjectDao.GetDataBasePreProByName(names);
        }

        /// <summary>
        /// 添加时根据名称获取数据库中的名称
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual List<string> GetDataBasePreProByName(List<double> ids)
        {
            return _prepareProjectDao.GetDataBasePreProByName(ids);
        }

        /// <summary>
        /// 添加命令生成编码
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual string GetPreProjectCodeWithAdd()
        {
            var config = ConfigService.GetConfig(new PrepareProjectCodeConfig(), typeof(PrepareProject));
            if (config == null || config.ProCodeRule == null)
            {
                throw new ValidationException("未找到订单编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.ProCodeRule, 1).FirstOrDefault();
        }

        /// <summary>
        /// 产前准备项目维护保存命令
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void PrepareProjectSave(EntityList data)
        {
            if (data == null || (data.Count == 0 && data.DeletedList.Count == 0))
            {
                throw new ValidationException("保存数据异常！".L10N());
            }
            // 新增数据
            var preProjectList = data as EntityList<PrepareProject>;
            var codes = preProjectList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.ProCode).ToList();
            var names = preProjectList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.ProName).ToList();
            var newIds = preProjectList.Select(p => p.Id).ToList();
            // 删除数据
            var deleteList = data.DeletedList.OfType<PrepareProject>().ToList();
            if (!deleteList.Any())
            {
                deleteList = new List<PrepareProject>();
            }
            var deleteIds = deleteList.Select(p => p.Id).ToList();
            // 数据库中的数据（除去删除的）
            var dataBaseCodes = RT.Service.Resolve<PrepareProjectService>().GetDataBasePreProByCode(newIds);
            var dataBaseNames = RT.Service.Resolve<PrepareProjectService>().GetDataBasePreProByName(newIds);
            codes.AddRange(dataBaseCodes);
            names.AddRange(dataBaseNames);
            if (preProjectList.Any(p => p.ProCode.Trim().Length == 0))
            {
                throw new ValidationException("项目编码不能为空！".L10N());
            }
            if (preProjectList.Any(p => p.ProName.Trim().Length == 0))
            {
                throw new ValidationException("项目名称不能为空！".L10N());
            }

            // deleteList 删除校验
            var preProductCount = RT.Service.Resolve<PrepareProductService>().GetPrepareProductDetailCount(deleteIds);
            if (preProductCount > 0)
            {
                throw new ValidationException("存在项目已被引用，请先删除产品产前准备设置后，再删除本项目！".L10N());
            }
            if (codes.Count != codes.Distinct().Count())
            {
                throw new ValidationException("项目编码不能为重复或项目编码已存在！".L10N());
            }
            //if (names.Count != names.Distinct().Count())
            //{
            //    throw new ValidationException("项目名称不能为重复或项目名称已存在！".L10N());
            //}
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="prepareProjectCriteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList QueryPrepareProjectList(PrepareProjectCriteria prepareProjectCriteria)
        {
            return _prepareProjectDao.QueryPrepareProjectList(prepareProjectCriteria);
        }
    }
}
