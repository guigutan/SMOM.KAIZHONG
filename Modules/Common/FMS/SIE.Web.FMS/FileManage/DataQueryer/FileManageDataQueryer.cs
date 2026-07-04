using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.FMS;
using SIE.FMS.FileManages;
using SIE.FMS.FileManages.ApiModels;
using SIE.Resources;
using SIE.Security;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.FMS
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class FileManageDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查询命令获取文件数据
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>信息</returns>
        public EntityJson GetFileManageDatas(FileManageCriteria criter)
        {
            //if (criter.CreateDate.BeginValue == null && criter.CreateDate.EndValue == null && criter.Code.IsNullOrEmpty() && criter.KeyWord.IsNullOrEmpty() && criter.FileState == null)
            //    throw new ValidationException("请至少输入一个查询条件！".L10N());

            var ctl = RT.Service.Resolve<FileManageController>();

            var stores = ctl.GetFiles(criter);
            List<EntityJson> res = new List<EntityJson>();
            if (stores.Count > 0)
            {
                res = GetEntityJson(stores);
            }

            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            return resNode;
        }

        /// <summary>
        /// 根据文件ID获取此文件的历史版本数据
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns>文件的历史版本数据</returns>
        public EntityJson GetHistoryVersion(double fileId)
        {
            var stores = RT.Service.Resolve<FileManageController>().GetHistoryVersion(fileId);
            List<EntityJson> res = new List<EntityJson>();
            if (stores.Count > 0)
                res = GetEntityJson(stores);
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            return resNode;
        }

        /// <summary>
        /// 根据文件ID获取文件操作记录
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件操作记录</returns>
        public List<EntityJson> GetFileLogs(double fileId)
        {
            var logs = RT.Service.Resolve<FileManageController>().GetFileLogs(fileId);
            var res = new List<EntityJson>();
            foreach (var log in logs.OrderByDescending(p => p.CreateDate).OrderByDescending(p => p.FileVersion))
            {
                var resNode = new EntityJson();
                resNode.SetProperty("OperationDate", log.CreateDate);
                resNode.SetProperty("Operation", log.Operation);
                resNode.SetProperty("Version", log.FileVersion);
                resNode.SetProperty("OperationBy", log.CreateByName);
                res.Add(resNode);
            }
            return res;
        }

        /// <summary>
        /// 获取文件夹内容by文件夹Id
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <returns></returns>
        public EntityJson GetFileManageDatasByFolder(double? folderId)
        {
            var ctl = RT.Service.Resolve<FileManageController>();
            List<EntityJson> res = new List<EntityJson>();
            var folders = ctl.GetFolders(folderId);
            if (folders.Count > 0)
            {
                var folderIds = folders.Select(p => p.Id).ToList();
                var childDatas = ctl.GetFoldersByParIds(folderIds).Select(p => p.PreFolderId).ToList();
                var fileFolderDatas = ctl.GetExistContentFolders(folderIds).Select(p => p.Id).ToList();
                var hasChildren = folders.Where(p => childDatas.Contains(p.Id)).AsEntityList();
                res = GetEntityJson(hasChildren, null, true);
                var noChildren = folders.Where(p => !childDatas.Contains(p.Id)).AsEntityList();
                res.AddRange(GetEntityJson(noChildren, fileFolderDatas, false));
            }
            var stores = ctl.GetFilesByFolderId(folderId);
            res.AddRange(GetEntityJson(stores));
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            List<EntityJson> navData = new List<EntityJson>();
            if (folderId.HasValue)
            {
                var preFolders = ctl.GetRootFolders(folderId.Value);
                GetNavJsons(navData, preFolders, null, folderId.Value);
            }
            resNode.SetProperty("navData", navData);
            return resNode;
        }

        /// <summary>
        /// 获取文件夹内容by文件夹Id（发布文件）
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <returns></returns>
        public EntityJson GetReleaseFileManageDatasByFolder(double? folderId)
        {
            var ctl = RT.Service.Resolve<FileManageController>();
            List<EntityJson> res = new List<EntityJson>();
            var folders = ctl.GetFolders(folderId);
            if (folders.Count > 0)
            {
                var folderIds = folders.Select(p => p.Id).ToList();
                var childDatas = ctl.GetFoldersByParIds(folderIds).Select(p => p.PreFolderId).ToList();
                var fileFolderDatas = ctl.GetExistContentFolders(folderIds).Select(p => p.Id).ToList();
                var hasChildren = folders.Where(p => childDatas.Contains(p.Id)).AsEntityList();
                res = GetEntityJson(hasChildren, null, true);
                var noChildren = folders.Where(p => !childDatas.Contains(p.Id)).AsEntityList();
                res.AddRange(GetEntityJson(noChildren, fileFolderDatas, false));
            }
            var stores = ctl.GetReleaseFilesByFolderId(folderId);
            res.AddRange(GetEntityJson(stores));
            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            List<EntityJson> navData = new List<EntityJson>();
            if (folderId.HasValue)
            {
                var preFolders = ctl.GetRootFolders(folderId.Value);
                GetNavJsons(navData, preFolders, null, folderId.Value);
            }
            resNode.SetProperty("navData", navData);
            return resNode;
        }

        /// <summary>
        /// 获取树形数据
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <returns>返回数据</returns>
        public List<EntityJson> GetTreeDatas(double? folderId)
        {
            var ctl = RT.Service.Resolve<FileManageController>();
            List<EntityJson> rst = new List<EntityJson>();
            var folders = ctl.GetFolders(folderId);
            if (folderId == null || folderId == 0)
            {
                rst.Add(GetRootTreeDatas(folderId));
                return rst;
            }
            if (folders.Count > 0)
            {
                var children = folders.Where(p => p.PreFolderId == folderId).ToList();
                var hasChild = ctl.GetFoldersByParIds(children.Select(p => p.Id).ToList());
                children.OrderBy(p => p.Id).ForEach(p =>
                {
                    EntityJson json = new EntityJson();
                    bool isLeaf = true;
                    if (hasChild.Any(f => f.PreFolderId == p.Id))
                        isLeaf = false;
                    SetJson(json, p, isLeaf);
                    rst.Add(json);
                });
            }
            return rst;
        }

        /// <summary>
        /// 获取当前文件夹作为根节点展示
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <returns>返回数据</returns>
        public List<EntityJson> GetTargetTreeDatas(double? folderId)
        {
            var ctl = RT.Service.Resolve<FileManageController>();
            List<EntityJson> rstList = new List<EntityJson>();
            // 当前节点
            var rootFolder = ctl.GetFolders(new List<double> { folderId.Value }).FirstOrDefault();
            EntityJson rst = new EntityJson();
            var folders = ctl.GetFolders(folderId);
            rst.SetProperty("folderId", rootFolder.Id);
            rst.SetProperty("text", rootFolder.Name);
            rst.SetProperty("iconCls", "iconfont icon-Folder icon-blue");
            if (folders.Count > 0)
            {
                rst.SetProperty("leaf", false);
                rst.SetProperty("expanded", true);
                var children = folders.Where(p => p.PreFolderId == folderId).ToList();
                List<EntityJson> list = new List<EntityJson>();
                var hasChild = ctl.GetFoldersByParIds(children.Select(p => p.Id).ToList());
                children.OrderBy(p => p.Id).ForEach(p =>
                {
                    bool isLeaf = true;
                    if (hasChild.Any(f => f.PreFolderId == p.Id))
                        isLeaf = false;
                    EntityJson jsonitem = new EntityJson();
                    SetList(folders, p, jsonitem, isLeaf);
                    list.Add(jsonitem);

                });
                rst.SetProperty("children", list);
            }
            else
            {
                rst.SetProperty("leaf", true);
            }
            rstList.Add(rst);
            return rstList;
        }

        /// <summary>
        /// 递归寻找文件路径
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        public List<string> GetRootFolders(double folderId)
        {
            var fileManage = RT.Service.Resolve<FileManageController>().GetFilesById(new List<double> { folderId }).FirstOrDefault();
            if (fileManage == null)
            {
                throw new ValidationException("文件不存在！".L10N());
            }
            if (fileManage.FolderId == null)
            {
                throw new ValidationException("文件不存在文件夹下！".L10N());
            }
            var folderList = RT.Service.Resolve<FileManageController>().GetRootFolders(fileManage.FolderId.Value);
            var foderNames = folderList.Select(p => p.Name).ToList();
            foderNames.Reverse();
            return foderNames;
        }

        /// <summary>
        /// 新建保存文件夹
        /// </summary>
        /// <param name="folderId">父文件夹Id</param>
        /// <param name="name">名称</param>
        /// <returns>bool</returns>
        public EntityJson AddNewFolder(double? folderId, string name)
        {
            EntityJson json = new EntityJson();
            Folder folder = new Folder() { PreFolderId = folderId, Name = name };
            RF.Save(folder);
            json.SetProperty("errMsg", "");
            json.SetProperty("Id", folder.Id);
            json.SetProperty("Name", folder.Name);
            return json;
        }

        /// <summary>
        /// 修改文件夹
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="name">名称</param>
        public void EditNewFolder(double folderId, string name)
        {
            RT.Service.Resolve<FileManageController>().EditNewFolder(folderId, name);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <returns>string</returns>
        public void DeleteFolder(double folderId)
        {
            var folder = RF.GetById<Folder>(folderId);
            if (folder == null)
                throw new ValidationException("文件夹已经不存在,请刷新数据后操作！".L10N());
            RT.Service.Resolve<FileManageController>().DeleteFolder(folderId);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件Id</param>
        public void DeleteFile(double fileId)
        {
            var file = RF.GetById<SIE.FMS.FileManage>(fileId);
            if (file == null)
                throw new ValidationException("此文件不存在，请刷新界面重试！".L10N());
            if (file.FileState != FileState.Created)
                throw new ValidationException("只能删除草稿状态的文件！".L10N());
            RT.Service.Resolve<FileManageController>().ValidationPermission(new List<double?> { file.FolderId }, PermissionType.Delete);
            file.PersistenceStatus = PersistenceStatus.Deleted;
            RF.Save(file);
        }

        /// <summary>
        /// 批量删除文件和文件夹
        /// </summary>
        /// <param name="folderIds">文件夹Id</param>
        /// <param name="fileIds">文件Id</param>
        /// <returns></returns>
        public string DeleteFoldersAndFiles(List<double> folderIds, List<double> fileIds)
        {
            try
            {
                RT.Service.Resolve<FileManageController>().DeleteFoldersAndFiles(folderIds, fileIds);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// 作废文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="url">地址</param>
        /// <returns>返回信息</returns>
        public string ScarpFiles(List<double> fileIds, string url)
        {
            try
            {
                RT.Service.Resolve<FileManageController>().ScarpFiles(fileIds, url);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileDatas"></param>
        /// <returns></returns>
        public EntityJson UploadFiles(List<FileData> fileDatas)
        {
            EntityJson json = new EntityJson();
            var successData = new List<FileData>();
            try
            {
                RT.Service.Resolve<FileManageController>().UploadFiles(fileDatas, successData);
            }
            catch (Exception ex)
            {
                json.SetProperty("errMsg", ex.Message);
            }

            json.SetProperty("newFolderIds", successData.Select(p => p.FolderId).ToList());
            return json;
        }

        /// <summary>
        /// 上传修订的文件
        /// </summary>
        /// <param name="fileData">文件内容</param>
        /// <param name="oldFileId">原文件ID</param>
        public void UploadEditFile(FileData fileData, double oldFileId)
        {
            RT.Service.Resolve<FileManageController>().UploadEditFile(fileData, oldFileId);
        }

        /// <summary>
        /// 获取文件设置
        /// </summary>
        /// <returns>文件设置</returns>
        public FileSetting GetFileSetting()
        {
            var fileSetting = RT.Service.Resolve<FileManageController>().GetFileSetting();
            if (fileSetting != null)
            {
                var names = fileSetting.Pusher?.ReceiverList.Select(p => p.ReceiverName).ToList();
                if (names?.Count > 0)
                    fileSetting.AuditMans = string.Join(",", names);
                return fileSetting;
            }
            else
                return new FileSetting() { VersionHead = "V" };
        }

        /// <summary>
        /// 获取接收人
        /// </summary>
        /// <param name="pusherId">推送方式Id</param>
        /// <returns>string</returns>
        public string GetAuditMans(double pusherId)
        {
            var mans = RT.Service.Resolve<FileManageController>().GetReceivers(pusherId);
            if (mans.Count > 0)
                return string.Join(",", mans.Select(p => p.ReceiverName).ToList());
            else return string.Empty;
        }

        /// <summary>
        /// 保存文件设置
        /// </summary>
        /// <param name="fileSetting">文件设置</param>
        public bool SaveFileSetting(FileSetting fileSetting)
        {
            var id = fileSetting.Id;
            if (id > 0)
            {
                var setting = RF.GetById<FileSetting>(id);
                setting.PusherId = fileSetting.PusherId;
                setting.IsOA = fileSetting.IsOA;
                setting.VersionHead = fileSetting.VersionHead;
                RF.Save(setting);
            }
            else
            {
                fileSetting.GenerateId();
                fileSetting.PersistenceStatus = PersistenceStatus.New;
                RF.Save(fileSetting);
            }
            return true;
        }

        /// <summary>
        /// 启动审批流
        /// </summary>
        /// <param name="fileIds">文件</param>
        /// <param name="url">url</param>
        public void StartFlow(List<double> fileIds, string url)
        {
            RT.Service.Resolve<FileManageController>().StartFlow(fileIds, url);
        }

        /// <summary>
        /// 审核文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="reason">驳回原因</param>
        /// <param name="url">url</param>
        public void AuditFiles(List<double> fileIds, string reason, string url)
        {
            RT.Service.Resolve<FileManageController>().AuditFiles(fileIds, reason, url);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <returns></returns>
        public string DownLoadFiles(List<double> fileIds)
        {
            try
            {
                var files = RT.Service.Resolve<FileManageController>().GetFilesById(fileIds);
                RT.Service.Resolve<FileManageController>().ValidationPermission(files, PermissionType.Download);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取接收人
        /// </summary>
        /// <returns>json</returns>
        public List<EntityJson> GetReciveMans()
        {
            var jsons = new List<EntityJson>();
            var mans = RF.GetAll<Employee>();
            mans.ForEach(p =>
            {
                EntityJson json = new EntityJson();
                json.SetProperty("Name", p.Name);
                json.SetProperty("Id", p.Id);
                jsons.Add(json);
            });
            return jsons;
        }

        /// <summary>
        /// 发布文件
        /// </summary>
        /// <param name="fileIds">文件Id</param>
        /// <param name="receiverIds">接收人Id</param>
        public string PublishFiles(List<double> fileIds, List<double> receiverIds)
        {
            try
            {
                RT.Service.Resolve<FileManageController>().PublishFiles(fileIds, receiverIds);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        #region 私有方法

        /// <summary>
        /// 首次加载获取包括首页的树数据
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        private EntityJson GetRootTreeDatas(double? folderId)
        {
            var ctl = RT.Service.Resolve<FileManageController>();
            EntityJson rst = new EntityJson();
            var folders = ctl.GetFolders(folderId);
            rst.SetProperty("folderId", null);
            rst.SetProperty("text", "首页".L10N());
            rst.SetProperty("iconCls", "iconfont icon-Folder icon-blue");
            if (folders.Count > 0)
            {
                rst.SetProperty("leaf", false);
                rst.SetProperty("expanded", true);
                var children = folders.Where(p => p.PreFolderId == folderId).ToList();
                List<EntityJson> list = new List<EntityJson>();
                var hasChild = ctl.GetFoldersByParIds(children.Select(p => p.Id).ToList());
                children.OrderBy(p => p.Id).ForEach(p =>
                {
                    bool isLeaf = true;
                    if (hasChild.Any(f => f.PreFolderId == p.Id))
                        isLeaf = false;
                    EntityJson jsonitem = new EntityJson();
                    SetList(folders, p, jsonitem, isLeaf);
                    list.Add(jsonitem);

                });
                rst.SetProperty("children", list);
            }
            else
            {
                rst.SetProperty("leaf", true);
            }

            return rst;
        }

        /// <summary>
        /// 递归向下形成树形结构
        /// </summary>       
        private void SetList(EntityList<Folder> folders, Folder par, EntityJson json, bool isLeaf = true)
        {
            var children = folders.Where(p => p.PreFolderId == par.Id).ToList();
            if (children.Count > 0)
            {
                SetJson(json, par, false);
                json.SetProperty("expanded", false);
                List<EntityJson> list = new List<EntityJson>();
                children.OrderBy(p => p.Id).ForEach(p =>
                {
                    EntityJson jsonitem = new EntityJson();
                    SetList(folders, p, jsonitem);
                    list.Add(jsonitem);
                });

                json.SetProperty("children", list);
            }
            else
            {
                SetJson(json, par, isLeaf);
            }
        }

        /// <summary>
        /// 设置Json数据
        /// </summary>    
        private void SetJson(EntityJson json, Folder par, bool leaf)
        {
            json.SetProperty("folderId", par.Id);
            json.SetProperty("text", par.Name);
            json.SetProperty("leaf", leaf);
            json.SetProperty("iconCls", "iconfont icon-Folder icon-blue");
        }

        /// <summary>
        /// 根据文件设置前端的数据
        /// </summary>
        /// <param name="files">文件</param>
        /// <returns>前端的数据</returns>
        private List<EntityJson> GetEntityJson(EntityList<SIE.FMS.FileManage> files)
        {
            List<EntityJson> res = new List<EntityJson>();
            files.OrderByDescending(p => p.UpdateDate).ForEach(store =>
            {
                EntityJson node = new EntityJson();
                node.SetProperty("Code", store.Code);
                node.SetProperty("FId", store.Id);
                node.SetProperty("FileName", store.Name);
                node.SetProperty("Version", store.VersionPrefix + store.Version);
                node.SetProperty("FileState", (int)store.FileState);
                node.SetProperty("FileState_Display", store.FileState.ToLabel());
                node.SetProperty("Size", store.Size);
                node.SetProperty("CreatebyName", store.CreateByName);
                node.SetProperty("CreateDate", store.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                node.SetProperty("IsFile", true);
                node.SetProperty("FilePath", store.Path + "/" + store.ServerFileName);
                node.SetProperty("ServerFileName", store.ServerFileName);
                res.Add(node);
            });

            return res;
        }

        /// <summary>
        /// 根据文件设置前端的数据
        /// </summary>
        /// <param name="folders">文件夹</param>
        /// <param name="fileFolderDatas">文件内容</param>
        /// <param name="hasChild">是否有子文件夹</param>
        /// <returns>前端的数据</returns>
        /// <remarks>兼容文件夹内有文件夹或有文件，设置hasChild为true</remarks>
        private List<EntityJson> GetEntityJson(EntityList<Folder> folders, List<double> fileFolderDatas = null, bool hasChild = false)
        {
            List<EntityJson> res = new List<EntityJson>();
            folders.OrderBy(p => p.Id).ForEach(store =>
            {
                EntityJson node = new EntityJson();
                node.SetProperty("FId", store.Id);
                node.SetProperty("FileName", store.Name);
                node.SetProperty("IsFile", false);
                if (fileFolderDatas != null && fileFolderDatas.Contains(store.Id))
                {
                    node.SetProperty("HasChild", true);
                }
                else node.SetProperty("HasChild", hasChild);
                res.Add(node);
            });

            return res;
        }

        /// <summary>
        /// 获取导航条数据
        /// </summary>
        /// <param name="navData">json</param>
        /// <param name="folders">文件夹</param>
        /// <param name="preId">上一级Id</param>
        /// <param name="leafId"></param>
        private void GetNavJsons(List<EntityJson> navData, EntityList<Folder> folders, double? preId, double leafId)
        {
            var fo = folders.FirstOrDefault(p => p.PreFolderId == preId);
            EntityJson item = new EntityJson();
            item.SetProperty("FolderId", fo.Id);
            item.SetProperty("Name", fo.Name);
            navData.Add(item);
            if (fo.Id != leafId)
                GetNavJsons(navData, folders, fo.Id, leafId);
        }

        #endregion

        #region 文件权限管理
        /// <summary>
        /// 获取文件权限
        /// </summary>
        /// <param name="folderId">文件夹ID</param>
        /// <returns>文件权限</returns>
        public List<FilePermissionInfo> GetUserGroupPermissions(double? folderId)
        {
            List<FilePermissionInfo> res = new List<FilePermissionInfo>();
            var permissions = RT.Service.Resolve<FileManageController>().GetPermissionsByFolderId(folderId);
            var fileUserGroups = RF.GetAll<FileUserGroup>();
            foreach (var group in fileUserGroups)
            {
                var permission = permissions.FirstOrDefault(p => p.FileUserGroupId == group.Id);
                FilePermissionInfo data = new FilePermissionInfo();
                data.FileUserGroupId = group.Id;
                data.FileUserGroupName = group.Name;
                data.IsModified = false;
                if (permission != null)
                {
                    data.PermissionId = permission.Id;
                    data.Upload = (permission.Permissions & PermissionType.Upload) == PermissionType.Upload;
                    data.Modify = (permission.Permissions & PermissionType.Modify) == PermissionType.Modify;
                    data.Scrap = (permission.Permissions & PermissionType.Scrap) == PermissionType.Scrap;
                    data.Download = (permission.Permissions & PermissionType.Download) == PermissionType.Download;
                    data.Preview = (permission.Permissions & PermissionType.Preview) == PermissionType.Preview;
                    data.Publish = (permission.Permissions & PermissionType.Publish) == PermissionType.Publish;
                    data.Delete = (permission.Permissions & PermissionType.Delete) == PermissionType.Delete;
                }
                res.Add(data);
            }
            return res;
        }

        /// <summary>
        /// 文件权限提交保存
        /// </summary>
        /// <param name="curFolderId">当前文件夹Id</param>
        /// <param name="permissionInfos">文件权限信息</param>
        public void PermissionSubmit(double? curFolderId, List<FilePermissionInfo> permissionInfos)
        {
            RT.Service.Resolve<FileManageController>().SavePermission(curFolderId, permissionInfos);
        }

        /// <summary>
        /// 获取当前登录用户此文件夹的权限
        /// </summary>
        /// <param name="folderId">当前文件夹ID</param>
        /// <returns>文件权限</returns>
        public FilePermissionInfo GetCommandsPermission(double? folderId)
        {
            return RT.Service.Resolve<FileManageController>().GetCommandsPermission(folderId);
        }

        /// <summary>
        /// 获取管理员和审核权限
        /// </summary>
        public EntityJson GetAdminPermission()
        {
            EntityJson res = new EntityJson();
            var isAdmin = RT.Service.Resolve<FileManageController>().IsFileAdmin();
            var isAudit = RT.Service.Resolve<FileManageController>().IsHaveAuditPermission();
            res.SetProperty("IsAdmin", isAdmin);
            res.SetProperty("IsAudit", isAudit);
            return res;
        }
        #endregion
    }
}
