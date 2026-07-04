using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ListAtts
{
    /// <summary>
    /// 考勤原始数据对象属性
    /// </summary>
    [Serializable]
    public class ListAttInfo
    {
        /// <summary>
        /// 对应的考勤数据的原始ID
        /// </summary>
        public string DataId { get; set; }       
        /// <summary>
        /// 记录设备触发时间
        /// </summary>
        public DateTime? EventTime { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>      
        public string Pin { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Name { get; set; }       
        /// <summary>
        /// 人员姓名，英文下才有用
        /// </summary>
        public string LastName { get; set; }       
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }       
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }        
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }       
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DevSn { get; set; }       
        /// <summary>
        /// 验证方式名称
        /// </summary>
        public string VerifyModeName { get; set; }       
        /// <summary>
        /// 事件描述
        /// </summary>
        public string EventName { get; set; }       
        /// <summary>
        /// 事件出发点名称
        /// </summary>
        public string EventPointName { get; set; }       
        /// <summary>
        /// 读头名称
        /// </summary>
        public string ReaderName { get; set; }      
        /// <summary>
        /// 门禁区域名称
        /// </summary>
        public string AccZone { get; set; }       
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName { get; set; }       
        /// <summary>
        /// 事件索引值
        /// </summary>
        public string LogId { get; set; }       
        /// <summary>
        /// 位置
        /// </summary>
        public string AttPlace { get; set; }       
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }

    }

    
    /// <summary>
    /// 请求参数的Context部分
    /// </summary>
    public class ApiContext
    {
        /// <summary>
        /// InvOrgId
        /// </summary>
        public int InvOrgId { get; set; }
        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; }
    }

   
    /// <summary>
    /// 请求参数的Parameters子项
    /// </summary>
    public class ParameterItem
    {
        /// <summary>
        /// Value对应List<ListAttInfo>
        /// </summary>
        public List<ListAttInfo> Value { get; set; }
    }

    /// <summary>
    ///  完整的POST请求参数模型
    /// </summary>
    public class ApiRequestModel
    {
        /// <summary>
        /// ApiType
        /// </summary>
        public string ApiType { get; set; }
        /// <summary>
        /// Parameters
        /// </summary>
        public List<ParameterItem> Parameters { get; set; }
        /// <summary>
        /// Method
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Context
        /// </summary>
        public ApiContext Context { get; set; }
    }

    /// <summary>
    /// SaveListAtt的POST返回数据
    /// </summary>
    public class PostResData
    {
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int DataRows { get; set; }
        /// <summary>
        /// 添加成功条数
        /// </summary>
        public int AddRows { get; set; }
        /// <summary>
        /// 添加失败条数
        /// </summary>
        public int FailRows { get; set; }
        /// <summary>
        /// 忽略条数
        /// </summary>
        public int IgnoreRow { get; set; }
    }


    /// <summary>
    /// ApiServerInfo
    /// </summary>
    public class ApiServerInfo 
    {
        /// <summary>
        /// ApiUrl
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// ApiIpPort
        /// </summary>
        public string ApiIpPort { get; set; }
        /// <summary>
        /// ApiPostResData
        /// </summary>
        public PostResData ApiPostResData { get; set; }
    }


}
