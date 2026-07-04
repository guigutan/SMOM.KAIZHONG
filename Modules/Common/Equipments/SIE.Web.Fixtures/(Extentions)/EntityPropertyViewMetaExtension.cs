using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;

namespace SIE.Web.Fixtures._Extentions_
{
    /// <summary>
    /// 实体属性扩展编辑器
    /// </summary>
    public static class EntityPropertyViewMetaExtension
    {
        /// <summary>
        /// 异常类型-异常现象-编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseAbnormalTypeUnusualEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action) where T : new()
        {
            return GetAbnormalTypeMeta(meta, AbnormalType.Unusual, action);
        }

        /// <summary>
        /// 异常类型-故障类型-编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static WebEntityPropertyViewMeta<T> UseAbnormalTypeFaultEditor<T>(this WebEntityPropertyViewMeta<T> meta, Action<PagingLookUpBaseConfig, T> action) where T : new()
        {
            return GetAbnormalTypeMeta(meta, AbnormalType.Fault, action);
        }

        /// <summary>
        /// 异常现象元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="meta"></param>
        /// <param name="abnormalType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static WebEntityPropertyViewMeta<T> GetAbnormalTypeMeta<T>(this WebEntityPropertyViewMeta<T> meta, AbnormalType abnormalType, Action<PagingLookUpBaseConfig, T> action) where T : new()
        {
            meta.UseDataSource((source, pagingInfo, keyword) =>
            {
                var fixtureRepairDetail = source as SIE.Fixtures.Repairs.FixtureRepairDetail;
                var fixtureAbnormals = new EntityList<FixtureAbnormal>();
                if (fixtureRepairDetail == null || string.IsNullOrEmpty(fixtureRepairDetail.FixtureAccountCode))
                    return fixtureAbnormals;
                return RT.Service.Resolve<CoreFixtureController>().GetFixtureAbnormals(abnormalType, fixtureRepairDetail.FixtureModelType, pagingInfo, null);
            }).UsePagingLookUpEditor(action);
            return meta;
        }

        /// <summary>
        /// 单元格值导入前必填写验证执行函数
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static WebEntityPropertyViewMeta<T> BeforeImportRequireFunc<T>(this WebEntityPropertyViewMeta<T> meta, string fieldName)
        {
            meta.ViewMeta.BeforeImportFunc = (v) =>
            {
                var str = v.ToString();
                if (str.IsNullOrEmpty())
                {
                    throw new ValidationException("【{0}】必须填写".L10nFormat(fieldName));
                }

                return v;
            };

            return meta;
        }

        /// <summary>
        /// 单元格值导入前必填写验证执行函数
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static WebEntityPropertyViewMeta<T> BeforeImportNonnegativeFunc<T>(this WebEntityPropertyViewMeta<T> meta, string fieldName)
        {
            meta.ViewMeta.BeforeImportFunc = (v) =>
            {
                bool isNum = decimal.TryParse(v.ToString(), out decimal num);
                if (isNum)
                {
                    if (num < 0)
                    {
                        throw new ValidationException("【{0}】必须为非负数".L10nFormat(fieldName));
                    }   
                }
                else
                {
                    throw new ValidationException("【{0}】必须填数字".L10nFormat(fieldName));
                }
                return v;
            };

            return meta;
        }
    }
}
