using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards;
using SIE.ERPInterface.Common.Enums;

namespace SIE.ERPInterface.Smom.Download.QmsStandards
{
    /// <summary>
    /// QMS检验标准控制器
    /// </summary>
    public class DownloadQmsStandardController : DomainController
    {
        #region 物料检验标准
        /// <summary>
        /// 从API下载物料检验标准到业务表
        /// </summary>
        /// <param name="itemInspStandards"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadItemInspStandardsToBusiness(List<ItemInspStandardData> itemInspStandards, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ItemInspStandardData>(
                itemInspStandards,
                p => this.SaveItemInspStandards(p.OrderByLastUpdateDate()),
                JobType.ItemInspStandard,
                invOrg);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveItemInspStandards(List<ItemInspStandardData> datas)
        {
            var errors = new List<ErpErrorData>();

            var nameList = datas.Select(p => p.Name).Distinct().ToList();
            //var standardList = RT.Service.Resolve<StandardsController>().GetItemInspectionStandardsByNames(nameList);

            var tran = new ItemInspStandardTransfer();

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    //var oldStandard = standardList.FirstOrDefault(s => s.Name == p.Name && s.Version == p.Version && s.InspectionType.ToLabel() == p.InspectionType);
                    //删除
                    //if (p.IsDelete && oldStandard != null)
                    //{
                    //    error.ErrMsg = $"删除失败，物料检验标准不可删除。";
                    //    errors.Add(error);
                    //    continue;
                    //}
                    //else if (oldStandard != null)
                    //{
                    //    //已存在，不可新增
                    //    error.ErrMsg = $"新增失败，物料检验标准[{p.Name}+{p.InspectionType}+{p.Version}]已存在。";
                    //    errors.Add(error);
                    //    continue;
                    //}
                    //else
                    //{
                    //    //新增
                    //    var newEntity = tran.Transfer(p);
                    //    RF.SaveIngoreValidations(newEntity); //不跑实体默认的验证规则。提高性能
                    //}
                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }


        #endregion

        #region 分类检验标准
        /// <summary>
        /// 从API下载分类检验标准到业务表
        /// </summary>
        /// <param name="categoryInspStandards"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadCategoryInspStandardsToBusiness(List<CategoryInspStandardData> categoryInspStandards, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<CategoryInspStandardData>(
                categoryInspStandards,
                p => this.SaveCategoryInspStandards(p.OrderByLastUpdateDate()),
                JobType.CategoryInspStandard,
                invOrg);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveCategoryInspStandards(List<CategoryInspStandardData> datas)
        {
            var errors = new List<ErpErrorData>();

            var nameList = datas.Select(p => p.Name).Distinct().ToList();
            //var standardList = RT.Service.Resolve<StandardsController>().GetCategoryInspectionStandardsByNames(nameList);

            var tran = new CategoryInspStandardTransfer();

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    //var oldStandard = standardList.FirstOrDefault(s => s.Name == p.Name && s.Version == p.Version && s.InspectionType.ToLabel() == p.InspectionType);
                    //删除
                    //if (p.IsDelete && oldStandard != null)
                    //{
                    //    error.ErrMsg = $"删除失败，分类检验标准不可删除。";
                    //    errors.Add(error);
                    //    continue;
                    //}
                    //else if (oldStandard != null)
                    //{
                    //    //已存在，不可新增
                    //    error.ErrMsg = $"新增失败，分类检验标准[{p.Name}+{p.InspectionType}+{p.Version}]已存在。";
                    //    errors.Add(error);
                    //    continue;
                    //}
                    //else
                    //{
                    //    //新增
                    //    var newEntity = tran.Transfer(p);
                    //    RF.SaveIngoreValidations(newEntity); //不跑实体默认的验证规则。提高性能
                    //}
                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }


        #endregion
    }
}
