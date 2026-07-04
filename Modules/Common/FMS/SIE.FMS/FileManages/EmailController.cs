using MimeKit;
using Newtonsoft.Json;
using SIE.Common.Employees;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Senders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.FMS
{
    /// <summary>
    /// 关键事件跟催表控制器
    /// </summary>
    public class EmailController : DomainController
    {
        /// <summary>
        /// 获取员工消息
        /// </summary>
        /// <param name="empIds">员工ID集合</param>
        /// <returns>返回员工消息</returns>
        public virtual EntityList<Employee> GetEmployees(List<double> empIds)
        {
            return Query<Employee>().Where(q => empIds.Contains(q.Id) && q.Email != string.Empty).ToList();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="dataEmails">邮件数据</param>
        public virtual void SendEmail(FileDataEmail dataEmails)
        {
            List<double> userIds = dataEmails.EmployeeIds;
            EntityList<Employee> employeeList = GetEmployees(userIds);
            if (employeeList.Count == 0)
            {
                throw new ValidationException("当前需要发送的负责人都没有维护电子邮箱!".L10N());
            }
            EmailSendParam emailSendParam = new EmailSendParam();
            employeeList.ForEach(p =>
            {
                emailSendParam.SendTos.Add(new MailboxAddress(p.Email));
            });
            string codes = string.Join(",", dataEmails.FileDatas.Select(p => p.Code));
            string names = string.Join(",", dataEmails.FileDatas.Select(p => p.FileName));
            string filePath = dataEmails.FileDatas.FirstOrDefault().FilePath;
            const string style = "line-height:35px;height:35px; border-bottom:1px solid #9A9A9A;";
            string temp = GetBodyTemplate(dataEmails.EmailType);
            if (dataEmails.EmailType == 0)
            {
                emailSendParam.Subject = "文件发布审核流程".L10N();
                emailSendParam.Body = string.Format(temp, codes, names, filePath, dataEmails.AuditUrl, style);
            }
            else if (dataEmails.EmailType == 1)
            {
                emailSendParam.Subject = "文件发布审核流程被驳回".L10N();
                emailSendParam.Body = string.Format(temp, codes, names, filePath, dataEmails.ReturnReason, style);
            }
            else if (dataEmails.EmailType == 2)
            {
                emailSendParam.Subject = "文件发布审核已撤回".L10N();
                emailSendParam.Body = string.Format(temp, codes, names, filePath, dataEmails.ReturnReason, style);
            }
            else
            {
                emailSendParam.Subject = "文件已发布".L10N();
                emailSendParam.Body = string.Format(temp, codes, names, filePath, dataEmails.ReturnReason, style);
            }
            try
            {
                EmailSender senderObj = new EmailSender();
                senderObj.Config = GetEmailSenderConfig();
                senderObj.Send(emailSendParam);
            }
            catch (Exception ex)
            {
                throw new ValidationException("邮件推送失败，错误信息:[{0}]".L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 获取邮件配置信息
        /// </summary>
        /// <returns>邮件配置信息</returns>
        private EmailSenderConfig GetEmailSenderConfig()
        {
            var plug = GetPushPlug();
            if (plug == null)
                throw new ValidationException("推送管理模块没有类型为[{0}]的邮件推送方式！".L10nFormat(typeof(FileManageSender).ToString()));
            var config = JsonConvert.DeserializeObject<EmailSenderConfig>(plug.Config);
            return config;
        }

        /// <summary>
        /// 获取邮件配置信息
        /// </summary>
        /// <returns>邮件配置信息</returns>
        private PushPlug GetPushPlug()
        {
            return Query<PushPlug>().Where(p => p.PushClass == typeof(FileManageSender).ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 获取邮件模板
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>模板</returns>
        private string GetBodyTemplate(int type)
        {
            if (type == 0)
                return @"<html><head><title>文件发布审核流程</title></head>
                        <body><div style='{4}'>您好！</div>
                        <div style='{4}'>文件发布流程待您审核：</div>
                        <div style='{4}'>文件编码：{0}</div>
                        <div style='{4}'>文件名称：{1}</div>
                        <div style='{4}'>文件路径：<a style='color:blue;' href='{2}'>{2}</a></div>
                        <div style='{4}'>审核链接：<a style='color:blue;' href='{3}'>{3}</a></div>
                        </body></html>";
            if (type == 1)
                return @"<html><head><title>文件发布审核流程被驳回</title></head>
                        <body><div style='{4}'>您好！</div>
                        <div style='{4}'>文件发布审核流程被驳回：</div>
                        <div style='{4}'>驳回原因：{3}</div>
                        <div style='{4}'>文件编码：{0}</div>
                        <div style='{4}'>文件名称：{1}</div>
                        <div style='{4}'>文件路径：<a style='color:blue;' href='{2}'>{2}</a></div>
                        </body></html>";
            if (type == 2)
                return @"<html><head><title>文件发布审核流程已撤回</title></head>
                        <body><div style='{4}'>您好！</div>
                        <div style='{4}'>文件发布流程已撤回：</div>
                        <div style='{4}'>文件编码：{0}</div>
                        <div style='{4}'>文件名称：{1}</div>
                        <div style='{4}'>文件路径：<a style='color:blue;' href='{2}'>{2}</a></div>
                        </body></html>";
            if (type == 3)
                return @"<html><head><title>文件已发布</title></head>
                        <body><div style='{4}'>您好！</div>
                        <div style='{4}'>文件已发布：</div>
                        <div style='{4}'>文件编码：{0}</div>
                        <div style='{4}'>文件名称：{1}</div>
                        <div style='{4}'>文件路径：<a style='color:blue;' href='{2}'>{2}</a></div>
                        </body></html>";
            return string.Empty;
        }
    }
}