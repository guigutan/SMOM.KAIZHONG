using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.ESop.EngDocuments.Daos;
using SIE.ESop.EngDocuments.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Services
{
    /// <summary>
    /// Service
    /// </summary>
    public class FileUseDetailService : DomainService
    {
        private readonly FileUseDetailDao _fileUseDetailDao;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="fileUseDetailDao"></param>
        public FileUseDetailService(FileUseDetailDao fileUseDetailDao)
        {
            _fileUseDetailDao = fileUseDetailDao;
        }

        /// <summary>
        /// 工程文件保存命令
        /// </summary>
        /// <param name="data"></param>
        public virtual void FileUseSave(EntityList data)
        {
            var saveHandle = new FileUseSaveHandle(data);
            saveHandle.ParentNotRequired();
            saveHandle.WebUseTypeIsRepeat();
            saveHandle.DBUseTypeIsRepeat();
        }

        /// <summary>
        /// 根据使用类型获取
        /// </summary>
        /// <param name="useTypes"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<FileUseDetail> GetFileUseDetails(List<string> useTypes, List<double> ids)
        {
            return _fileUseDetailDao.GetFileUseDetails(useTypes, ids);
        }

        /// <summary>
        /// 根据使用类型获取配置项文件夹
        /// </summary>
        /// <param name="useType"></param>
        /// <returns></returns>
        public virtual double? GetConfigUseTypeFolderId(string useType)
        {
            return _fileUseDetailDao.GetConfigUseTypeFolderId(useType);
        }
    }
}
