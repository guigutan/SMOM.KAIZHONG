using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.FMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments.Daos
{
    /// <summary>
    /// Dao
    /// </summary>
    public class FileUseDetailDao : BaseDao<FileUseDetail>
    {
        /// <summary>
        /// 根据使用类型获取
        /// </summary>
        /// <param name="useTypes"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<FileUseDetail> GetFileUseDetails(List<string> useTypes, List<double> ids)
        {
            return useTypes.SplitContains(tempTypes =>
            {
                return Query().Where(p => tempTypes.Contains(p.UseType) && !ids.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 根据使用类型获取配置项文件夹
        /// </summary>
        /// <param name="useType"></param>
        /// <returns></returns>
        public double? GetConfigUseTypeFolderId(string useType)
        {
            var fileUseDetail = Query().Where(p => p.UseType == useType).FirstOrDefault();
            //double? upFolderId = null;
            //if (fileUseDetail == null)
            //{
            //    var upFolder = RF.GetById<Folder>(fileUseDetail.FolderId);
            //    upFolderId = upFolder?.Id;
            //}
            return fileUseDetail?.FolderId;
        }
    }
}
