using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 物料分类下载控制器
    /// </summary>
    public class DownloadItemCateController : DomainController
    {
        /// <summary>
        /// 从API下载物料分类到业务表
        /// </summary>
        /// <param name="itemCateDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadItemCateToBusiness(List<ItemCategoryData> itemCateDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ItemCategoryData>(
                itemCateDatas,
                p => this.SaveItemCategorys(p.OrderByLastUpdateDate()),
                JobType.ItemCategory, 
                invOrg);
        }

        /// <summary>
        /// 从中间表下载物料分类到业务表
        /// </summary>
        public virtual ProcessResult DownloadItemCateToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ItemCategoryInf>(
                () => ctl.GetUnprocessedDatas<ItemCategoryInf>().OrderBy(p => p.CategoryLevelNum),  //物料分类中间表数据，按层次从小到大，父到子排序
                p =>
                {
                    var paras = this.GenerateItemCatePara(p);
                    return this.SaveItemCategorys(paras.OrderByLastUpdateDate());
                },
                JobType.ItemCategory, isManual);
        }


        /// <summary>
        /// 生成物料分类实体
        /// </summary>
        /// <param name="itemCateInfs">中间表数据</param>
        /// <returns></returns>
        private List<ItemCategoryData> GenerateItemCatePara(IEnumerable<ItemCategoryInf> itemCateInfs)
        {
            var paras = new List<ItemCategoryData>();

            itemCateInfs.ForEach(p =>
            {
                var data = new ItemCategoryData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.ItemType = (int)p.ItemType;
                data.LevelCode = p.Level;
                data.ParentCode = p.ParentCode;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// ERP保存数据到物料分类
        /// </summary>
        /// <param name="datas">通信数据类</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveItemCategorys(List<ItemCategoryData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //获取所有分类数据
            var itemCategorys = RF.GetAll<ItemCategory>(null, new EagerLoadOptions().LoadWith(ItemCategory.LevelProperty));
            //创建唯一键字典（编码+分类类型）
            var itemCategoryDic = itemCategorys.DistinctBy(p => "{0}_{1}".FormatArgs(p.Code, (int)p.Type)).ToDictionary(p => "{0}_{1}".FormatArgs(p.Code, (int)p.Type));

            //获取所有分类层级
            var levels = RF.GetAll<ItemCategoryLevel>();
            var levelsDic = levels.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveItemCategory(p, itemCategoryDic, levelsDic);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicLevel">分类层级数据字典</param>
        private void SaveItemCategory(ItemCategoryData data, Dictionary<string, ItemCategory> dic, Dictionary<string, ItemCategoryLevel> dicLevel)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            ItemCategory entity;
            ItemCategory parent = null;
            //编码+分类类型
            var key = "{0}_{1}".FormatArgs(data.Code, data.CategoryType);
            if (!dic.ContainsKey(key))
                dic.Add(key, new ItemCategory());
            entity = dic[key];
            //处理待删除数据
            if (data.IsDelete)
            {
                ctl.DeleteEntity(dic, key, entity);
                return;
            }
            //数据验证
            if (data.LevelCode.IsNullOrEmpty() || !dicLevel.ContainsKey(data.LevelCode))
                throw new ValidationException("分类层级编码[{0}]不存在".L10nFormat(data.LevelCode));
            var level = dicLevel[data.LevelCode];

            if (data.ParentCode == data.Code)
                throw new ValidationException("分类的编码[{0}]和父编码不能一样".L10nFormat(data.Code));
            if (data.ParentCode.IsNotEmpty())
            {
                var parentKey = "{0}_{1}".FormatArgs(data.ParentCode, data.CategoryType);
                if (!dic.ContainsKey(parentKey))
                    throw new ValidationException("类型[{1}]父编码[{0}]不存在".L10nFormat(data.ParentCode, data.CategoryType));
                parent = dic[parentKey];
            }

            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.Level = level;
            entity.Type = level.Type;
            entity.TreePId = parent?.Id;
            entity.SourceKey = data.ErpKey;

            RF.Save(entity);
        }


    }
}
