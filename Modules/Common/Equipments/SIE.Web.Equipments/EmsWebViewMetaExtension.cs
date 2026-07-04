using SIE.Domain.Validation;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.Equipments
{
    /// <summary>
    /// 实体属性视图元数据-EMS扩展
    /// </summary>
    public static class EmsWebViewMetaExtension
    {
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
    }
}
