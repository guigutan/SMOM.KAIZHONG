using SIE.Common.Attachments;
using SIE.Common.Employees;
using SIE.Common.Sender;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.FMS.FileManages.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.FMS.FileManages
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public partial class FileManageController : DomainController
    {

        private const string requireInParam = "传入参数不能为空";

        /// <summary>
        /// ftp附件上传下载地址
        /// </summary>
        public static string FtpPath { get { return "ftp.path"; } }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<FileManage> GetFiles(FileManageCriteria criteria)
        {
            var query = Query<FileManage>().Where(p => !p.IsHistory);
            if (!string.IsNullOrEmpty(criteria.Code))
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!string.IsNullOrEmpty(criteria.KeyWord))
                query.Where(p => p.Name.Contains(criteria.KeyWord));
            if (criteria.FileState.HasValue)
                query.Where(p => p.FileState == criteria.FileState);
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据文件ID获取此文件的历史版本数据
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns>文件的历史版本数据</returns>
        public virtual EntityList<FileManage> GetHistoryVersion(double fileId)
        {
            var oldFile = RF.GetById<FileManage>(fileId);
            if (oldFile == null)
                throw new ValidationException("找不到选择的文件".L10N());
            return Query<FileManage>().Where(p => p.FolderId == oldFile.FolderId && p.Name == oldFile.Name && p.IsHistory).ToList();
        }

        /// <summary>
        /// 根据文件ID获取文件操作记录(包括历史版本)
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件操作记录</returns>
        public virtual EntityList<FileLog> GetFileLogs(double fileId)
        {
            var oldFile = RF.GetById<FileManage>(fileId);
            if (oldFile == null)
                throw new ValidationException("找不到选择的文件".L10N());
            var allFiles = Query<FileManage>().Where(p => p.FolderId == oldFile.FolderId && p.Name == oldFile.Name).ToList();
            var allFileIds = allFiles.Select(p => p.Id).ToList<double>();
            return Query<FileLog>().Where(p => allFileIds.Contains(p.FileManageId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件夹下的文件（不包含历史版本）
        /// </summary>
        /// <param name="foldId">文件夹</param>
        /// <returns>文件</returns>
        public virtual EntityList<FileManage> GetFilesByFolderId(double? foldId)
        {
            return Query<FileManage>().Where(p => p.FolderId == foldId && !p.IsHistory).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件夹下发布的文件（不包含历史版本）
        /// </summary>
        /// <param name="foldId">文件夹</param>
        /// <returns>文件</returns>
        public virtual EntityList<FileManage> GetReleaseFilesByFolderId(double? foldId)
        {
            return Query<FileManage>().Where(p => p.FolderId == foldId && p.FileState == FileState.Release && !p.IsHistory).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取同一个文件夹下面同样名字的文件数量(非历史版本非作废)
        /// </summary>
        /// <param name="folderId">文件夹ID</param>
        /// <param name="name">文件名</param>
        /// <returns>文件数量</returns>
        public virtual int CountSameFile(double? folderId, string name)
        {
            return Query<FileManage>().Where(p => p.FolderId == folderId && p.Name == name && !p.IsHistory && p.FileState != FileState.Scrap).Count();
        }

        /// <summary>
        /// 根据名称列表获取非历史非作废版本的文件
        /// </summary>
        /// <param name="fileNames">名称列表</param>
        /// <returns>文件列表</returns>
        public virtual EntityList<FileManage> GetSameNameFiles(List<string> fileNames)
        {
            return Query<FileManage>().Where(o => fileNames.Contains(o.Name) && !o.IsHistory && o.FileState != FileState.Scrap).ToList();
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>文件</returns>
        public virtual EntityList<FileManage> GetFilesById(List<double> ids)
        {
            var exp = ids.CreateContainsExpression<FileManage>("x", "Id");
            return Query<FileManage>().Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件by编码
        /// </summary>
        /// <param name="codes">编码</param>
        /// <returns>文件</returns>
        public virtual EntityList<FileManage> GetFilesByCodes(List<string> codes)
        {
            var exp = codes.CreateContainsExpression<FileManage>("x", "Code");
            return Query<FileManage>().Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前文件夹的文件by名称
        /// </summary>
        /// <param name="names">文件名称</param>
        /// <param name="folderId">文件夹ID</param>
        /// <returns>当前文件夹的同名文件</returns>
        public virtual EntityList<FileManage> GetCurFolderFilesByNames(List<string> names, double? folderId)
        {
            var exp = names.CreateContainsExpression<FileManage>("x", "Name");
            return Query<FileManage>().Where(p => p.FolderId == folderId).Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件夹by父id
        /// </summary>
        /// <param name="parIds"></param>
        /// <returns></returns>
        public virtual EntityList<Folder> GetFoldersByParIds(List<double> parIds)
        {
            List<double?> preFolderIds = new List<double?>();
            parIds.ForEach(p => preFolderIds.Add(p));
            return Query<Folder>().Where(p => p.PreFolderId > 0 && preFolderIds.Contains(p.PreFolderId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件夹下的文件夹
        /// </summary>
        /// <param name="parId">当前文件夹Id</param>
        /// <returns>文件夹</returns>
        public virtual EntityList<Folder> GetFolders(double? parId)
        {
            return Query<Folder>().Where(p => p.PreFolderId == parId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <param name="ids">文件夹id</param>
        /// <returns>文件夹</returns>
        public virtual EntityList<Folder> GetFolders(List<double> ids)
        {
            if (ids.Count == 0)
                return new EntityList<Folder>();
            var exp = ids.CreateContainsExpression<Folder>("x", "Id");
            return Query<Folder>().Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取父下面所有文件夹（递归）
        /// </summary>
        /// <param name="parId">父Id</param>      
        /// <returns>文件夹</returns>
        public virtual EntityList<Folder> GetAllFolders(double? parId)
        {
            EntityList<Folder> folders = new EntityList<Folder>();
            var fos = GetFolders(parId);
            if (fos.Count == 0)
                return folders;
            else
            {
                fos.ForEach(p =>
                {
                    folders.Add(p);
                    folders.AddRange(GetAllFolders(p.Id));
                });
            }
            return folders;
        }

        /// <summary>
        /// 获取所有上级目录（包括当前目录）
        /// </summary>
        /// <param name="folderId">当前目录Id</param>
        /// <returns>所有上级目录</returns>
        public virtual EntityList<Folder> GetRootFolders(double folderId)
        {
            EntityList<Folder> folders = new EntityList<Folder>();
            var fo = RF.GetById<Folder>(folderId);
            if (fo != null)
            {
                folders.Add(fo);
                if (fo.PreFolderId.HasValue)
                    folders.AddRange(GetRootFolders(fo.PreFolderId.Value));
                else
                    return folders;
            }
            return folders;
        }

        /// <summary>
        /// 获取有包含文件的文件夹
        /// </summary>
        /// <param name="ids">Id</param>
        /// <returns>文件夹</returns>
        public virtual EntityList<Folder> GetExistContentFolders(List<double> ids)
        {
            var exp = ids.CreateContainsExpression<Folder>("x", "Id");
            return Query<Folder>().Where(exp).Exists<FileManage>((x, y) => y.Where(p => p.FolderId == x.Id))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 修改文件夹
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="name">名称</param>
        public virtual void EditNewFolder(double folderId, string name)
        {
            var folder = RF.GetById<Folder>(folderId);
            if (folder == null)
                throw new ValidationException("文件夹已经不存在,请刷新数据后操作！".L10N());
            else
            {
                folder.Name = name;
                RF.Save(folder);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folderId">文件夹</param>
        public virtual void DeleteFolder(double folderId)
        {
            if (folderId == 0)
                throw new ValidationException(requireInParam.L10N());
            DeleteFolders(new List<double>() { folderId });
        }

        /// <summary>
        /// 批量删除文件夹和文件
        /// </summary>
        /// <param name="folderIds">文件夹Id</param>
        /// <param name="fileIds">文件Id</param>
        public virtual void DeleteFoldersAndFiles(List<double> folderIds, List<double> fileIds)
        {
            if (fileIds.Count == 0 && folderIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                if (folderIds.Count > 0)
                {
                    DeleteFolders(folderIds);
                }
                if (fileIds.Count > 0)
                {
                    var files = GetFilesById(fileIds);
                    ValidationPermission(files, PermissionType.Delete);
                    if (files.Any(p => p.FileState != FileState.Created))
                        throw new ValidationException("只能删除草稿状态的文件！".L10N());
                    files.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
                    RF.Save(files);
                    CreateLog(files.Select(p => p.Id).ToList(), "文件删除");
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="selFolderIds">选择的文件夹</param>
        private void DeleteFolders(List<double> selFolderIds)
        {
            if (selFolderIds.Count > 0)
            {
                var folderIdNull = new List<double?>();
                var selectFolders = GetFolders(selFolderIds);
                //先找到要删除的文件夹的上一级文件夹，判断是否有父文件夹下的删除权限，
                //如果在BS正常操作是只会传一个父文件夹下的所选文件夹进来，但如果是接口像测试组的操作，可能会把其他文件夹下的文件夹传入
                //空则上一级是首页               
                selectFolders.ForEach(p =>
                {
                    folderIdNull.Add(p.PreFolderId);
                });
                ValidationPermission(folderIdNull, PermissionType.Delete);

                var folders = GetFoldersByParIds(selFolderIds);
                if (folders.Count > 0)
                    throw new ValidationException("文件夹下有内容，不允许删除！".L10N());
                var files = GetExistContentFolders(selFolderIds);
                if (files.Count > 0)
                    throw new ValidationException("文件夹下有内容，不允许删除！".L10N());
                DB.Delete<Folder>().Where(p => selFolderIds.Contains(p.Id)).Execute();
            }
        }

        /// <summary>
        /// 作废文件
        /// </summary>
        /// <param name="fileIds">文件</param>
        /// <param name="url">url</param>
        public virtual void ScarpFiles(List<double> fileIds, string url)
        {
            if (fileIds == null || fileIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var files = RT.Service.Resolve<FileManageController>().GetFilesById(fileIds);
                ValidationPermission(files, PermissionType.Scrap);
                if (files.Any(p => p.FileState != FileState.Release && p.FileState != FileState.Edit))
                    throw new ValidationException("只能作废发布或修订状态的文件".L10N());
                var set = ValidatFileSetting();
                var receivers = GetReceivers(set.PusherId);
                if (!set.IsOA && receivers.Count == 0)
                    throw new ValidationException("未配置文件审核人，请检查文件设置！".L10N());
                files.ForEach(p =>
                {
                    p.PreFileState = p.FileState;
                    p.FileState = FileState.ToScrap;
                    p.FlowCreateBy = RT.IdentityId;
                    if (!set.IsOA)
                        CreateFileAudits(receivers, p.Id, OperationType.ToScrap);
                });
                RF.Save(files);
                CreateLog(files.Select(p => p.Id).ToList(), "文件作废");
                tran.Complete();
                if (set.IsOA)
                {
                    //调用OA接口                   
                }
                else
                {
                    SendEmail(files, set.PusherId, 0, url);
                }
            }
        }

        /// <summary>
        /// 获取文件设置
        /// </summary>
        /// <returns>文件设置</returns>
        public virtual FileSetting GetFileSetting()
        {
            return Query<FileSetting>().FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证文件设置
        /// </summary>
        /// <returns>文件设置</returns>
        public virtual FileSetting ValidatFileSetting()
        {
            var set = GetFileSetting();
            if (set == null)
                throw new ValidationException("无文件设置信息，请先在【菜单-设置】中配置！".L10N());
            return set;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileDatas">文件</param>
        /// <param name="SuccessData">成功上传的数据</param>
        public virtual void UploadFiles(List<FileData> fileDatas, List<FileData> SuccessData)
        {
            var folderIds = fileDatas.Select(p => p.FolderId).ToList();
            ValidationPermission(folderIds, PermissionType.Upload);
            // var set = ValidatFileSetting();
            fileDatas.ForEach(p =>
            {
                var codes = p.FileName.Split('_');
                if (codes.Length > 1)
                {
                    p.Code = codes[0];
                    p.FileName = p.FileName.Remove(0, codes[0].Length + 1);
                }
                else p.Code = p.FileName;
            });
            var nameFiles = GetCurFolderFilesByNames(fileDatas.Select(f => f.FileName).ToList(), fileDatas.FirstOrDefault()?.FolderId);
            fileDatas.ForEach(p =>
            {
                using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
                {
                    if (nameFiles.Count > 0 && nameFiles.Any(f => f.Name == p.FileName))
                        throw new ValidationException("当前文件夹中已经存在文件[{0}]！".L10nFormat(p.FileName));
                    string path = "FMS";
                    if (p.FolderId.HasValue) path += "/" + Guid.NewGuid().ToString("N");
                    FileManage file = new FileManage();
                    //file.VersionPrefix = set.VersionHead;
                    file.Version = "1.0";
                    file.Name = p.FileName;
                    file.Code = p.Code;
                    file.FileState = FileState.Created;
                    file.FolderId = p.FolderId;
                    file.Size = p.FileSize;
                    file.FileExtesion = p.FileExtesion;
                    file.Path = path;
                    var serverFileName = file.Name.Replace(p.FileExtesion, "") + Guid.NewGuid().ToString("N") + p.FileExtesion;
                    file.ServerFileName = serverFileName;
                    RF.Save(file);
                    CreateLog(new List<double>() { file.Id }, "上传文件");
                    FileStorage(p.Content, serverFileName, path);
                    tran.Complete();
                    SuccessData.Add(new FileData() { FolderId = file.Id });
                }
            });
        }

        /// <summary>
        /// 上传修订的文件
        /// </summary>
        /// <param name="fileData">文件内容</param>
        /// <param name="oldFileId">原文件ID</param>
        public virtual void UploadEditFile(FileData fileData, double oldFileId)
        {
            var oldFile = ValidateEditFile(oldFileId);
            ValidationPermission(new List<double?> { oldFile.FolderId }, PermissionType.Modify);
            var newFile = new FileManage();
            var codes = fileData.FileName.Split('_');
            if (codes.Length > 1)
            {
                newFile.Code = codes[0];
                newFile.Name = fileData.FileName.Remove(0, codes[0].Length + 1);
            }
            else
            {
                newFile.Code = fileData.FileName;
                newFile.Name = fileData.FileName;
            }
            newFile.FileState = FileState.Edit;
            newFile.Size = fileData.FileSize;
            newFile.VersionPrefix = oldFile.VersionPrefix;
            if (oldFile.FileState == FileState.Release)
            {
                var maxVersion = GetMaxFileVersion(oldFile);
                newFile.Version = (float.Parse(maxVersion) + 0.1).ToString("###0.0##");
            }
            else
            {
                newFile.Version = oldFile.Version;
                oldFile.PersistenceStatus = PersistenceStatus.Deleted;
            }
            string path = "FMS";
            if (oldFile.FolderId.HasValue) path += "/" + Guid.NewGuid().ToString("N");
            newFile.Path = path;
            newFile.FolderId = oldFile.FolderId;
            newFile.FileExtesion = fileData.FileExtesion;
            var serverFileName = newFile.Name.Replace(fileData.FileExtesion, "") + Guid.NewGuid().ToString("N") + fileData.FileExtesion;
            newFile.ServerFileName = serverFileName;
            newFile.PreFileState = FileState.Release;
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(oldFile);
                RF.Save(newFile);
                CreateLog(new List<double>() { oldFile.Id, newFile.Id }, "上传修订文件");
                FileStorage(fileData.Content, serverFileName, path);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证修订的原文件
        /// </summary>
        /// <param name="oldFileId">原文件ID</param>
        /// <returns>原文件</returns>
        private FileManage ValidateEditFile(double oldFileId)
        {
            var oldFile = RF.GetById<FileManage>(oldFileId);
            if (oldFile == null)
                throw new ValidationException("找不到修订的原文件".L10N());
            if (oldFile.FileState != FileState.Release && oldFile.FileState != FileState.Edit)
                throw new ValidationException("只能修订【发布】和【修订】状态的文件".L10N());
            if (oldFile.FileState == FileState.Edit && oldFile.CreateBy != RT.IdentityId)
                throw new ValidationException("您不是该文件的上传人，无法修订此文件".L10N());
            if (oldFile.FileState == FileState.Release)
            {
                var oldEditFileCount = CountSameFile(oldFile.FolderId, oldFile.Name);
                if (oldEditFileCount > 1)
                    throw new ValidationException("此文件已经在修订中，不能再次修订".L10N());
            }
            return oldFile;
        }

        /// <summary>
        /// 获取此文件在此目录下的最大版本
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>最大版本</returns>
        public virtual string GetMaxFileVersion(FileManage file)
        {
            var maxFile = Query<FileManage>().Where(p => p.FolderId == file.FolderId && p.Name == file.Name)
                .OrderByDescending(p => p.Version).FirstOrDefault();
            return maxFile?.Version;
        }

        /// <summary>
        /// 保存文件到服务器
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <param name="serverFileName">文件在服务器名称</param>
        /// <param name="path">路径</param>
        /// <remarks>服务器路径见webConfig文件</remarks>
        private void FileStorage(string content, string serverFileName, string path)
        {
            // 解析出base64的字符，格式形如：data:application/zip;base64,UEsDBBQAAAAIAKJUU02Zjai********
            const string base64Discriminator = "base64,";
            var base64Index = content.IndexOf(base64Discriminator) + base64Discriminator.Length;
            var base64Str = "";
            //支持空文件上传
            if (content.Length > base64Index)
            {
                base64Str = content.Substring(base64Index);
            }

            // 转换为byte[]
            var contentByte = Convert.FromBase64String(base64Str);
            RT.Service.Resolve<AttachmentController>().FileStorage(serverFileName, contentByte, path);
        }

        /// <summary>
        /// 获取推送人
        /// </summary> 
        /// <returns>推送人</returns>
        public virtual EntityList<Pusher> GetPusher(PagingInfo pagingInfo, string keyWord)
        {
            var query = Query<Pusher>().Join<PushPlug>((x, y) => x.PushPlugId == y.Id && y.PushClass == typeof(FileManageSender).ToString());
            if (!keyWord.IsNullOrEmpty())
            {
                query.Where(p => p.Name.Contains(keyWord));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取接收人
        /// </summary>
        /// <param name="pusherId">推送方式</param>
        /// <returns>接收人</returns>
        public virtual EntityList<Receiver> GetReceivers(double pusherId)
        {
            return Query<Receiver>().Where(p => p.PusherId == pusherId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 启用审批流
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="url">链接</param>
        public virtual void StartFlow(List<double> fileIds, string url)
        {
            if (fileIds == null || fileIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var files = GetFilesById(fileIds);
                if (files.Any(p => p.CreateBy != RT.IdentityId))
                    throw new ValidationException("只能选择自己上传的文件！".L10N());
                if (files.Any(p => p.FileState != FileState.Created && p.FileState != FileState.Edit))
                    throw new ValidationException("只能选择草稿或修订状态的文件！".L10N());
                var set = ValidatFileSetting();
                var receivers = GetReceivers(set.PusherId);
                if (!set.IsOA && receivers.Count == 0)
                    throw new ValidationException("未配置文件审核人，请检查文件设置！".L10N());
                files.ForEach(p =>
                {
                    p.PreFileState = p.FileState;
                    p.FileState = FileState.Audit;
                    p.VersionPrefix = set.VersionHead;//设置版本前缀
                    p.FlowCreateBy = RT.IdentityId;
                    if (!set.IsOA)
                        CreateFileAudits(receivers, p.Id, OperationType.ToPublish);
                });
                RF.Save(files);
                CreateLog(files.Select(p => p.Id).ToList(), "启用审批流".L10N());
                tran.Complete();
                if (set.IsOA)
                {
                    //调用OA接口                   
                }
                else
                {
                    SendEmail(files, set.PusherId, 0, url);
                }
            }
        }

        /// <summary>
        /// 创建文件审批流信息
        /// </summary>
        /// <param name="receivers">审核人列表</param>
        /// <param name="fileId">文件Id</param>
        /// <param name="type">文件流审核操作类型</param>
        private void CreateFileAudits(EntityList<Receiver> receivers, double fileId, OperationType type)
        {
            foreach (var receiver in receivers)
            {
                var fileAudit = new FileAudit();
                fileAudit.Operation = type;
                fileAudit.AuditorId = receiver.ReceiverId;
                fileAudit.State = FileAuditState.ToAuidt;
                fileAudit.FileId = fileId;
                fileAudit.IsEnabled = true;
                RF.Save(fileAudit);
            }
        }

        /// <summary>
        /// 撤回审批流
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        public virtual void ReturnFlow(List<double> fileIds)
        {
            if (fileIds == null || fileIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var files = GetFilesById(fileIds);
                if (files.Any(p => p.FlowCreateBy != RT.IdentityId))
                    throw new ValidationException("只有流程发其人才可以撤回当前流程！".L10N());
                if (files.Any(p => p.FileState != FileState.Audit && p.FileState != FileState.ToScrap))
                    throw new ValidationException("请选择审核中的文件！".L10N());
                files.ForEach(p =>
                {
                    p.FileState = p.PreFileState.Value;
                });
                RF.Save(files);
                var allFileAudits = GetFileAudits(fileIds);
                allFileAudits.ForEach(p => { p.IsEnabled = false; p.Remark = "此审批流已被撤回".L10N(); });
                RF.Save(allFileAudits);
                CreateLog(files.Select(p => p.Id).ToList(), "撤回审批流".L10N());
                tran.Complete();
                var set = ValidatFileSetting();
                if (set.IsOA)
                {
                    //撤回OA流程接口                   
                }
                else
                {
                    SendEmail(files, set.PusherId, 2);
                }
            }
        }

        /// <summary>
        /// 根据文件ID列表获取待审核、有效的文件流审核
        /// </summary>
        /// <param name="fileIds">文件ID列表</param>
        /// <returns>待审核、有效的文件流审核</returns>
        public virtual EntityList<FileAudit> GetFileAudits(List<double> fileIds)
        {
            return Query<FileAudit>().Where(p => p.IsEnabled && p.State == FileAuditState.ToAuidt && fileIds.Contains(p.FileId)).ToList();
        }

        /// <summary>
        /// 审核文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="reason">驳回原因</param>
        /// <param name="url">当前网址</param>
        public virtual void AuditFiles(List<double> fileIds, string reason, string url)
        {
            if (fileIds == null || fileIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var files = GetFilesById(fileIds);
                if (files.Any(p => p.FileState != FileState.Audit && p.FileState != FileState.ToScrap))
                    throw new ValidationException("文件不是待审核状态！".L10N());
                List<FileDataEmail> fileDataEmails = new List<FileDataEmail>();
                var allFileAudits = GetFileAudits(fileIds);
                var invalidFileAudits = new EntityList<FileAudit>();//失效的文件流审核
                files.ForEach(p =>
                {
                    var opType = p.FileState == FileState.ToScrap ? OperationType.ToScrap : OperationType.ToPublish;
                    var fileAudit = allFileAudits.FirstOrDefault(f => f.FileId == p.Id && f.Operation == opType && f.AuditorId == RT.IdentityId);
                    if (fileAudit == null)
                        throw new ValidationException("您没有审核文件：{0}的权限".L10nFormat(p.Name));
                    var otherFileAudits = allFileAudits.Where(f => f.FileId == p.Id && f.Operation == opType && f.AuditorId != RT.IdentityId);
                    invalidFileAudits.AddRange(otherFileAudits);
                    if (reason.IsNullOrEmpty())
                    {
                        fileAudit.State = FileAuditState.Pass;
                        if (p.FileState == FileState.ToScrap)
                            p.FileState = FileState.ScrapToRelease;
                        else
                            p.FileState = FileState.ToRelease;
                    }
                    else
                    {
                        p.FileState = p.PreFileState.Value;
                        fileAudit.State = FileAuditState.Reject;
                        fileAudit.RejectReason = reason;
                        setFileDataEmail(fileDataEmails, reason, p);
                    }
                    RF.Save(fileAudit);
                });
                invalidFileAudits.ForEach(p => { p.IsEnabled = false; p.Remark = "此审批流已失效".L10N(); });
                RF.Save(invalidFileAudits);
                RF.Save(files);
                string op = reason.IsNotEmpty() ? "审核驳回".L10N() : "审核通过".L10N();
                CreateLog(files.Select(p => p.Id).ToList(), op);
                tran.Complete();
                fileDataEmails.ForEach(p =>
                {
                    RT.Service.Resolve<EmailController>().SendEmail(p);
                });
            }
        }

        /// <summary>
        /// 设置邮件文件信息
        /// </summary>
        /// <param name="fileDataEmails">邮件文件数据</param>
        /// <param name="reason">驳回原因</param>
        /// <param name="file">文件</param>
        private void setFileDataEmail(List<FileDataEmail> fileDataEmails, string reason, FileManage file)
        {
            var fileData = fileDataEmails.FirstOrDefault(f => f.EmployeeIds.Contains(file.CreateBy));
            var ftpUrl = RT.Config.Get<string>(FtpPath);
            if (fileData == null)
            {
                fileData = new FileDataEmail();
                fileData.EmployeeIds = new List<double>() { file.CreateBy };
                fileData.FileDatas = new List<FileData>();
            }
            fileData.FileDatas.Add(new FileData() { Code = file.Code, FileName = file.Name, FilePath = ftpUrl + file.Path });
            fileData.EmailType = 1;
            fileData.ReturnReason = reason;
            fileDataEmails.Add(fileData);
        }

        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="pushId">推送方式Id</param>
        /// <param name="type">类型0-审核 1-驳回 2-撤回 3-发布</param>
        /// <param name="url">审核地址</param>
        /// <param name="receiverIds">发布接收人Id</param>
        private void SendEmail(EntityList<FileManage> files, double pushId, int type, string url = "", List<double> receiverIds = null)
        {
            if (receiverIds == null)
            {
                var receivers = GetReceivers(pushId);
                if (receivers.Count == 0) return;
                receiverIds = receivers.Select(p => p.ReceiverId).ToList();
            }
            else
                receiverIds.AddRange(files.Select(p => p.CreateBy).ToList());

            var ftpUrl = RT.Config.Get<string>(FtpPath);
            var fileData = new FileDataEmail();
            fileData.EmployeeIds = receiverIds;
            fileData.FileDatas = new List<FileData>();
            fileData.EmailType = type;
            fileData.AuditUrl = url;
            files.ForEach(p =>
            {
                fileData.FileDatas.Add(new FileData() { Code = p.Code, FileName = p.Name, FilePath = ftpUrl + p.Path });
            });
            RT.Service.Resolve<EmailController>().SendEmail(fileData);
        }

        /// <summary>
        /// 写操作日志
        /// </summary>
        public virtual void CreateLog(List<double> fileId, string operateDesc)
        {
            fileId.ForEach(p =>
            {
                FileLog fileLog = new FileLog()
                {
                    FileManageId = p,
                    Operation = operateDesc
                };
                RF.Save(fileLog);
            });
        }

        /// <summary>
        /// 发布文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="receiverIds">接收人Id</param>
        public virtual void PublishFiles(List<double> fileIds, List<double> receiverIds)
        {
            if (fileIds == null || fileIds.Count == 0 || receiverIds == null || receiverIds.Count == 0)
                throw new ValidationException(requireInParam.L10N());
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var files = GetFilesById(fileIds);
                var set = ValidatFileSetting();
                if (files.Any(p => p.FileState != FileState.ToRelease && p.FileState != FileState.ScrapToRelease))
                    throw new ValidationException("文件不是待发布状态，不能发布！".L10N());
                var oldFileManages = GetSameNameFiles(files.Select(p => p.Name).ToList<string>());
                files.ForEach(p =>
                {
                    p.PreFileState = p.FileState;
                    if (p.FileState == FileState.ScrapToRelease)
                        p.FileState = FileState.Scrap;
                    else
                    {
                        p.FileState = FileState.Release;
                        var oldFile = oldFileManages.FirstOrDefault(o => o.FolderId == p.FolderId && o.Name == p.Name && !o.IsHistory && o.Id != p.Id);
                        if (oldFile != null)
                        {
                            oldFile.IsHistory = true;
                            RF.Save(oldFile);
                        }
                    }
                });
                RF.Save(files);
                CreateLog(files.Select(f => f.Id).ToList(), "发布");
                tran.Complete();
                SendEmail(files, set.PusherId, 3, string.Empty, receiverIds);
            }
        }

        /// <summary>
        /// 根据文件用户组id获取文件用户组和用户关系
        /// </summary>
        /// <param name="fileUserGroupId">文件用户组id</param>
        /// <param name="sortInfo">排序参数</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <returns>文件用户组和用户关系</returns>
        public virtual EntityList<UserInFileUserGroup> GetUserInFileUserGroups(double fileUserGroupId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<UserInFileUserGroup>()
                .Where(p => p.FileUserGroupId == fileUserGroupId)
                .OrderBy(sortInfo)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据员工ID获取用户组ID列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>用户组ID列表</returns>
        public virtual IList<double> GetUserGroupsByEmployeeId(double employeeId)
        {
            return Query<UserInFileUserGroup>().Join<FileUserGroup>((a,b)=>a.FileUserGroupId==b.Id).Where(a => a.EmployeeId == employeeId).Select(a => a.FileUserGroupId).Distinct().ToList<double>();
        }

        /// <summary>
        /// 验证文件夹权限by文件Id
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="permissionType">权限类型</param>
        public virtual void ValidationPermission(EntityList<FileManage> files, PermissionType permissionType)
        {
            var folders = files.Select(p => p.FolderId).ToList();
            ValidationPermission(folders, permissionType);
        }

        /// <summary>
        /// 验证文件夹权限by文件夹Id
        /// </summary>
        /// <param name="folderIds">文件夹Id</param>
        /// <param name="permissionType">权限类型</param>
        public virtual void ValidationPermission(List<double?> folderIds, PermissionType permissionType)
        {
            var per = GetPermissionsByFolderIdsByCurrentUser(folderIds);
            if (folderIds.Any(f => f == null) && per.FirstOrDefault(f => f.FolderId == null && (f.Permissions & permissionType) == permissionType) == null)
            {
                throw new ValidationException("你所在的用户组在文件夹[首页]没有{0}权限！".L10nFormat(permissionType.ToLabel()));
            }
            var parFolders = GetFolders(folderIds.Where(p => p.HasValue).Select(f => f.Value).ToList());
            parFolders.ForEach(p =>
            {
                if (per.FirstOrDefault(f => f.FolderId == p.Id && (f.Permissions & permissionType) == permissionType) == null)
                    throw new ValidationException("你所在的用户组在文件夹[{0}]没有{1}权限！".L10nFormat(p.Name, permissionType.ToLabel()));
            });
        }

        #region 文件权限管理
        /// <summary>
        /// 根据文件夹ID获取文件权限
        /// </summary>
        /// <param name="folderId">文件夹ID</param>
        /// <returns>文件权限</returns>
        public virtual EntityList<UserGroupPermission> GetPermissionsByFolderId(double? folderId)
        {
            return Query<UserGroupPermission>().Where(p => p.FolderId == folderId).ToList();
        }

        /// <summary>
        /// 根据Id列表获取文件权限
        /// </summary>
        /// <param name="permissionIds">权限id列表</param>
        /// <returns>文件权限</returns>
        public virtual EntityList<UserGroupPermission> GetPermissionsByIds(List<double?> permissionIds)
        {
            return Query<UserGroupPermission>().Where(p => permissionIds.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 根据文件夹ID列表获取文件权限
        /// </summary>
        /// <param name="folderIds">文件夹ID列表</param>
        /// <returns>文件权限</returns>
        public virtual EntityList<UserGroupPermission> GetPermissionsByFolderIds(List<double?> folderIds)
        {
            return Query<UserGroupPermission>().Where(p => folderIds.Contains(p.FolderId)).ToList();
        }

        /// <summary>
        /// 获取登陆用户文件夹权限
        /// </summary>
        /// <param name="folderIds">文件夹</param>
        /// <returns>权限</returns>
        public virtual EntityList<UserGroupPermission> GetPermissionsByFolderIdsByCurrentUser(List<double?> folderIds)
        {
            return Query<UserGroupPermission>().Join<UserInFileUserGroup>((x, y) => x.FileUserGroupId == y.FileUserGroupId)
                .Join<UserInFileUserGroup, Employee>((y, z) => y.EmployeeId == z.Id && z.Id == RT.IdentityId)
                .WhereIf(folderIds.All(p => p != null), p => folderIds.Contains(p.FolderId))
                .WhereIf(folderIds.Any(p => p == null), p => p.FolderId == null || folderIds.Contains(p.FolderId)).Distinct().ToList();
        }

        /// <summary>
        /// 根据当前文件夹和用户组列表获取文件权限
        /// </summary>
        /// <param name="folderId">文件夹ID</param>
        /// <param name="fileUserGroupIds">用户组ID列表</param>
        /// <returns>文件权限列表</returns>
        public virtual EntityList<UserGroupPermission> GetPermissionsByFolderAndUser(double? folderId, List<double> fileUserGroupIds)
        {
            return Query<UserGroupPermission>().Where(p => p.FolderId == folderId && fileUserGroupIds.Contains(p.FileUserGroupId)).ToList();
        }

        /// <summary>
        /// 文件权限保存
        /// </summary>
        /// <param name="curFolderId">当前文件夹ID</param>
        /// <param name="permissionInfos">文件权限信息</param>
        public virtual void SavePermission(double? curFolderId, List<FilePermissionInfo> permissionInfos)
        {
            var permissionIds = permissionInfos.Where(p => p.PermissionId != null).Select(p => p.PermissionId).ToList<double?>();
            var oldPermissions = GetPermissionsByIds(permissionIds);
            var childFolders = GetAllFolders(curFolderId);
            var childFolderIds = new List<double?>();
            childFolders.ForEach(p => childFolderIds.Add(p.Id));
            var childPermissions = GetPermissionsByFolderIds(childFolderIds);
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var permission in permissionInfos)
                {
                    foreach (var childFolderId in childFolderIds)
                    {
                        var childPermission = childPermissions.FirstOrDefault(p => p.FileUserGroupId == permission.FileUserGroupId && p.FolderId == childFolderId);
                        if (childPermission == null)
                            CreatePermission(permission, childFolderId);
                        else
                            UpdataChildPermission(permission, childPermission, oldPermissions);
                    }
                    if (permission.PermissionId == null)
                        CreatePermission(permission, curFolderId);
                    else
                    {
                        var oldPermission = oldPermissions.FirstOrDefault(p => p.Id == permission.PermissionId);
                        Check.NotNull(oldPermission, nameof(UserGroupPermission));
                        oldPermission.Permissions = 0;
                        PermissionsCalculate(permission, oldPermission);
                        RF.Save(oldPermission);
                    }
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建一个文件权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="curFolderId">当前文件夹</param>
        private void CreatePermission(FilePermissionInfo permission, double? curFolderId)
        {
            var newPermission = new UserGroupPermission();
            newPermission.GenerateId();
            newPermission.FileUserGroupId = permission.FileUserGroupId;
            newPermission.FolderId = curFolderId;
            PermissionsCalculate(permission, newPermission);
            RF.Save(newPermission);
        }

        /// <summary>
        /// 更新子文件夹权限
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="childPermission">子权限</param>
        /// <param name="oldPermissions">原文件权限列表</param>
        private void UpdataChildPermission(FilePermissionInfo permission, UserGroupPermission childPermission, EntityList<UserGroupPermission> oldPermissions)
        {
            if (permission.PermissionId == null)
                PermissionsCalculate(permission, childPermission);
            else
            {
                var oldPermission = oldPermissions.FirstOrDefault(p => p.Id == permission.PermissionId);
                Check.NotNull(oldPermission, nameof(UserGroupPermission));
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Upload, permission.Upload);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Modify, permission.Modify);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Scrap, permission.Scrap);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Download, permission.Download);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Preview, permission.Preview);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Publish, permission.Publish);
                CalculateChildPermission(oldPermission, childPermission, PermissionType.Delete, permission.Delete);
            }
            RF.Save(childPermission);
        }

        /// <summary>
        /// 添加或者减少权限数值
        /// </summary>
        /// <param name="oldPermission">原文件权限</param>
        /// <param name="childPermission">子权限</param>
        /// <param name="type">权限类型</param>
        /// <param name="isAuthoriz">是否有授权</param>
        private void CalculateChildPermission(UserGroupPermission oldPermission, UserGroupPermission childPermission, PermissionType type, bool isAuthoriz)
        {
            if ((oldPermission.Permissions & type) == type)
            {
                if (!isAuthoriz)
                    childPermission.Permissions ^= type;
            }
            else
            {
                if (isAuthoriz)
                    childPermission.Permissions |= type;
            }
        }

        /// <summary>
        /// 计算权限数值
        /// </summary>
        /// <param name="permission">权限信息</param>
        /// <param name="userGroupPermission">文件权限</param>
        public virtual void PermissionsCalculate(FilePermissionInfo permission, UserGroupPermission userGroupPermission)
        {
            if (permission.Upload)
                userGroupPermission.Permissions |= PermissionType.Upload;
            if (permission.Modify)
                userGroupPermission.Permissions |= PermissionType.Modify;
            if (permission.Scrap)
                userGroupPermission.Permissions |= PermissionType.Scrap;
            if (permission.Download)
                userGroupPermission.Permissions |= PermissionType.Download;
            if (permission.Preview)
                userGroupPermission.Permissions |= PermissionType.Preview;
            if (permission.Publish)
                userGroupPermission.Permissions |= PermissionType.Publish;
            if (permission.Delete)
                userGroupPermission.Permissions |= PermissionType.Delete;
        }

        /// <summary>
        /// 获取当前登录用户此文件夹的权限
        /// </summary>
        /// <param name="folderId">当前文件夹ID</param>
        /// <returns>文件权限</returns>
        public virtual FilePermissionInfo GetCommandsPermission(double? folderId)
        {
            var data = new FilePermissionInfo();
            var fileUserGroupIds = GetUserGroupsByEmployeeId(RT.IdentityId);
            var userGroupPermissions = GetPermissionsByFolderAndUser(folderId, fileUserGroupIds.ToList());
            foreach (var permission in userGroupPermissions)
            {
                data.Upload = data.Upload || ((permission.Permissions & PermissionType.Upload) == PermissionType.Upload);
                data.Modify = data.Modify || ((permission.Permissions & PermissionType.Modify) == PermissionType.Modify);
                data.Scrap = data.Scrap || ((permission.Permissions & PermissionType.Scrap) == PermissionType.Scrap);
                data.Download = data.Download || ((permission.Permissions & PermissionType.Download) == PermissionType.Download);
                data.Preview = data.Preview || ((permission.Permissions & PermissionType.Preview) == PermissionType.Preview);
                data.Publish = data.Publish || ((permission.Permissions & PermissionType.Publish) == PermissionType.Publish);
                data.Delete = data.Delete || ((permission.Permissions & PermissionType.Delete) == PermissionType.Delete);
            }
            return data;
        }

        /// <summary>
        /// 当前用户是否有审核权限
        /// </summary>
        /// <returns>是否有审核权限</returns>
        public virtual bool IsHaveAuditPermission()
        {
            var set = GetFileSetting();
            if (set == null)
                return false;
            if (set.IsOA)
                return false;
            var mans = RT.Service.Resolve<FileManageController>().GetReceivers(set.PusherId);
            if (mans.FirstOrDefault(p => p.ReceiverId == RT.IdentityId) != null)
                return true;
            return IsHaveToAuditFile();
        }

        /// <summary>
        /// 判断当前用户是否是文件管理员
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFileAdmin()
        {
            var fileUserGroup = Query<FileUserGroup>()
                 .Join<UserInFileUserGroup>((x, y) => x.Id == y.FileUserGroupId && y.EmployeeId == RT.IdentityId).ToList();
            var isAdmin = fileUserGroup.Any(c => c.IsAdmin);
            return isAdmin;
        }

        /// <summary>
        /// 当前用户是否有待审核文件
        /// </summary>
        /// <returns>是否有待审核文件</returns>
        public virtual bool IsHaveToAuditFile()
        {
            return Query<FileAudit>().Where(p => p.AuditorId == RT.IdentityId && p.State == FileAuditState.ToAuidt && p.IsEnabled).Count() > 0;
        }

        /// <summary>
        /// 设为管理员
        /// </summary>
        /// <param name="fileUserGroup">文件用户组</param>
        public virtual void SetAdmin(FileUserGroup fileUserGroup)
        {
            using (var tran = DB.TransactionScope(FmsEntityDataProvider.ConnectionStringName))
            {
                var fileUserGroups = Query<FileUserGroup>().Where(p => p.IsAdmin).ToList();
                fileUserGroups.ForEach(p => p.IsAdmin = false);
                RF.Save(fileUserGroups);
                fileUserGroup.IsAdmin = true;
                RF.Save(fileUserGroup);
                tran.Complete();
            }
        }
        #endregion
    }
}
