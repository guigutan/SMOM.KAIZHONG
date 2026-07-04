using Org.BouncyCastle.Crypto;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.EngDocuments.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Handles
{
    /// <summary>
    /// 工程文件使用明细保存帮助类
    /// </summary>
    public class FileUseSaveHandle
    {
        #region 属性
        /// <summary>
        /// Id
        /// </summary>
        private List<double> Ids { get; set; }

        /// <summary>
        /// 使用类型
        /// </summary>
        private List<string> UseTypeList { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        private EntityList<FileUseDetail> FileUseDetails { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 重复校验
        /// </summary>
        /// <returns></returns>
        public bool ParentNotRequired()
        {
            if (FileUseDetails == null) return false;
            if (FileUseDetails.Any(p => p.UseType.IsNullOrEmpty()))
            {
                throw new ValidationException("使用类型必填！".L10N());
            }
            if (FileUseDetails.Any(p => p.FolderId == null || p.FolderId == 0))
            {
                throw new ValidationException("文件夹必填！".L10N());
            }
            return false;
        }

        /// <summary>
        /// 验证前端是否重复
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public bool WebUseTypeIsRepeat()
        {
            foreach(var item in FileUseDetails)
            {
                var same = FileUseDetails.Where(p => p.UseType == item.UseType).ToList();
                if (same != null && same.Count > 1)
                {
                    throw new ValidationException("已经存在使用类型是[{0}]的数据".L10nFormat(item.UseType));
                }
            }
            return false;
        }

        /// <summary>
        /// 验证与数据库后端是否重复
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public bool DBUseTypeIsRepeat()
        {
            var dbData = RT.Service.Resolve<FileUseDetailService>().GetFileUseDetails(UseTypeList, Ids);
            foreach(var item in FileUseDetails)
            {
                var same = dbData.Where(p => p.UseType == item.UseType).ToList();
                if (same != null &&  same.Count > 0)
                {
                    throw new ValidationException("已经存在使用类型是[{0}]的数据".L10nFormat(item.UseType));
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="data"></param>
        public FileUseSaveHandle(EntityList data)
        {
            if (data == null || (data.Count == 0 && data.DeletedList.Count == 0))
            {
                throw new ValidationException("保存数据异常！".L10N());
            }
            FileUseDetails = data as EntityList<FileUseDetail>;
            UseTypeList = new List<string>();
            Ids = new List<double>();
            UseTypeList.AddRange(FileUseDetails.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.UseType).ToList());
            Ids.AddRange(FileUseDetails.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).Select(p => p.Id).ToList());
        }
    }
}
