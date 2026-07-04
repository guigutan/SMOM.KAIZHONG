using Amazon.Runtime.Internal.Transform;
using DocumentFormat.OpenXml.Office2010.Excel;
using NPOI.SS.UserModel;
using SIE.Common.Catalogs;
using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Core.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.ItemChecker;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.MetaModel.XmlConfig;
using SIE.Reflection;
using SIE.Web.Common.Export;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.ItemChecker.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class CheckerItemImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            batch.ForEach(p =>
            {
                var entity = p.Entity as CheckerItem;
                //entity.State = Domain.State.Enable;

                if (entity.DrawingNo.IsNullOrEmpty())
                {
                    entity.DrawingNo = entity.CheckerUphold?.DrawingNo;
                }
            });
            base.OnSave(batch);
        }

        protected override object ImportData(ImportViewArgs importViewArgs)
        {
            Tuple<FileType, byte[]> tuple = FileStreamHelper.Base64ToExcel(importViewArgs.Data);
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(tuple.Item2);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            new CheckerItemImportImporter().Import(ref importResult, ImportType, ImportView, memoryStream, OnRowDataRead, OnSave, delegate (string errors)
            {
                OnComplete(errors);
            });
            return importResult;
        }
    }

    public class CheckerItemImportImporter : CommonImporter
    {

        protected override Dictionary<string, object> GetRefData(CacheData cacheData, EntityPropertyViewMeta property)
        {
            Dictionary<string, object> refData = null;
            string name = property.Name;
            if (!cacheData.RefPropertyData.TryGetValue(name, out refData))
            {
                Type refType = property.SelectionViewMeta?.SelectionEntityType ?? property.Owner.EntityType;
                string refPropertyName = property?.XPath?.Substring(property.XPath.IndexOf(".") + 1) ?? property.Name;
                EntityPropertyMeta refProperty = CommonModel.Entities.Find(refType).Property(refPropertyName);
                EntityList all = RepositoryFactory.Find(refType).GetAll(new PagingInfo(1, 5000));
                refData = new Dictionary<string, object>(all.Count);
                (from p in all.OfType<Entity>()
                 select new
                 {
                     Key = p.GetProperty(refProperty.ManagedProperty).ToString(),
                     Value = p.GetId()
                 }).ForEach(p =>
                 {
                     try
                     {
                         if (p.Key.IsNotEmpty() && !refData.ContainsKey(p.Key))
                         {
                             refData.Add(p.Key, p.Value);
                         }
                     }
                     catch (ArgumentException)
                     {
                         throw new ValidationException("查找[{0}]属性[{1}]发现重复的值[{2}]".L10nFormat(refType.GetQualifiedName(), refPropertyName, p.Key));
                     }
                 });
                cacheData.RefPropertyData.Add(name, refData);
            }

            return refData;
        }
        protected override object GetRefId(Dictionary<string, object> refData, EntityPropertyViewMeta property, string value)
        {
            object value2 = null;
            if (!refData.TryGetValue(value, out value2))
            {
                Type refType = property.SelectionViewMeta?.SelectionEntityType ?? property.Owner.EntityType;
                string refPropertyName = property?.XPath?.Substring(property.XPath.IndexOf(".") + 1) ?? property.Name;
                EntityPropertyMeta refProperty = CommonModel.Entities.Find(refType).Property(refPropertyName);
                CommonQueryCriteria commonQueryCriteria = new CommonQueryCriteria();
                commonQueryCriteria.Add(refProperty.ManagedProperty, value);
                (from p in RepositoryFactory.Find(refType).GetBy(commonQueryCriteria).OfType<Entity>()
                 select new
                 {
                     Key = p.GetProperty(refProperty.ManagedProperty).ToString(),
                     Value = p.GetId()
                 }).ForEach(p =>
                 {
                     try
                     {
                         if (p.Key.IsNotEmpty() && !refData.ContainsKey(p.Key))
                         {
                             refData.Add(p.Key, p.Value);
                         }
                     }
                     catch (ArgumentException)
                     {
                         throw new ValidationException("查找[{0}]属性[{1}]发现重复的值[{2}]".L10nFormat(refType.GetQualifiedName(), refPropertyName, p.Key));
                     }
                 });
                refData.TryGetValue(value, out value2);
            }

            return value2;

        }
    }

}
