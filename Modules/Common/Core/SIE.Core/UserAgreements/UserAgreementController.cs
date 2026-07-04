using NPOI.HPSF;
using SIE.Common.Attachments;
using SIE.Common.UserAgreement;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.IO;
using System;
using System.IO;
using System.Linq;

namespace SIE.Core.UserAgreements
{
    /// <summary>
    /// 用户协议控制器
    /// </summary>
    public partial class UserAgreementController : DomainController, IUserAgreement
    {
        #region 查询
        /// <summary>
        /// 查询所有用户协议 
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<UserAgreement> GetUserAgreements()
        {
            return Query<UserAgreement>().ToList();
        }

        /// <summary>
        /// 查询用户协议文件
        /// </summary>
        /// <returns></returns>
        public virtual UserAgreementAttachment GetUserAgreementAttach(double agreementId)
        {
            return Query<UserAgreementAttachment>().Where(p => p.OwnerId == agreementId).FirstOrDefault();
        }

        #endregion

        #region 上传协议
        /// <summary>
        /// 上传协议 
        /// </summary>
        /// <param name="attach"></param>
        public virtual void UploadAgreement(UserAgreementAttachment attach)
        {
            var maxVersion = GetMaxVersion(attach.AgreementType);
            using (var tran = DB.TransactionScope(CoreEntityDataProvider.ConnectionStringName))
            {
                //协议
                var userAgreement = new UserAgreement()
                {
                    AgreementType = attach.AgreementType,
                    FileName = attach.FileName,
                    IsUse = false,
                    Version = maxVersion == null ? 1 : maxVersion.Value + 1 //版本号自增
                };
                //首个协议默认启用
                if (maxVersion == null)
                    userAgreement.IsUse = true;

                RF.Save(userAgreement);

                //协议文件
                attach.OwnerId = userAgreement.Id;
                RF.Save(attach);
                tran.Complete();
            }
        }

        /// <summary>
        /// 查询最大版本
        /// </summary>
        /// <param name="agreementType"></param>
        /// <returns></returns>
        private int? GetMaxVersion(AgreementType agreementType)
        {
            var maxVersion = DB.Query<UserAgreement>().Where(p => p.AgreementType == agreementType).Select(p => new { Max = p.Version.MAX() }).ToList<int>().FirstOrDefault();
            return maxVersion;
        }


        #endregion

        #region 启用协议
        /// <summary>
        /// 启用协议
        /// </summary>
        /// <param name="id"></param>
        public virtual void EnableAgreement(double id)
        {
            using (var tran = DB.TransactionScope(CoreEntityDataProvider.ConnectionStringName))
            {
                var curAgree = RF.GetById<UserAgreement>(id);
                if (curAgree == null)
                    throw new EntityNotFoundException(typeof(UserAgreement), id);

                //先禁用已启用的协议
#pragma warning disable S1125 // Boolean literals should not be redundant
                DB.Update<UserAgreement>().Set(p => p.IsUse, false).Where(p => p.IsUse == true && p.AgreementType == curAgree.AgreementType).Execute();
#pragma warning restore S1125 // Boolean literals should not be redundant

                //启用当前协议
                DB.Update<UserAgreement>().Set(p => p.IsUse, true).Where(p => p.Id == id).Execute();

                tran.Complete();
            }
        }
        #endregion

        #region 用户协议接口实现
        /// <summary>
        /// 获取可用的用户协议
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual object GetEnableUserAgreement()
        {
            var agreement = Query<UserAgreement>().Where(p => p.IsUse && p.AgreementType == AgreementType.Serve).OrderByDescending(p => p.Version).FirstOrDefault();
            if (agreement == null)
                return null;

            var attach = GetUserAgreementAttach(agreement.Id);
            var fileBytes = RT.Service.Resolve<AttachmentController>().ViewFilePdf(attach.FilePath);

            var result = new AttachmentInfo()
            {
                FileName = attach.FileName,
                FliePath = attach.FilePath,
                FlieExtension = attach.FileExtesion,
                ContentBase64 = fileBytes,
            };
            return result;
        }
        #endregion

        /// <summary>
        /// 下载用户协议
        /// </summary>
        public virtual string DownLoadUserAgreement()
        {
            var agreement = Query<UserAgreement>().Where(p => p.IsUse && p.AgreementType == AgreementType.Serve).OrderByDescending(p => p.Version).FirstOrDefault();
            if (agreement == null)
            {
                return "";
            }

            var attach = GetUserAgreementAttach(agreement.Id);
            if (attach == null)
            {
                return "";
            }

            return attach.FileName + "#" + attach.FilePath;
        }
    }
}
