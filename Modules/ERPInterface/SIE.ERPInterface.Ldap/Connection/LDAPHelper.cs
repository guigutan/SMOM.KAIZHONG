using SIE.Domain.Validation;
using System;
using System.DirectoryServices;

namespace SIE.ERPInterface.Ldap.Connection
{
    /// <summary>
    /// 连接LDAP服务获取数据
    /// </summary>
    public static class LDAPHelper
    {
        /// <summary>
        /// 查询LDAP结果
        /// </summary>
        /// <param name="ldapAddress">LDAP服务地址</param>
        /// <param name="ldapAdUser">绑定用户名</param>
        /// <param name="ldapAdPwd">密码</param>
        /// <param name="propertiesToLoad">返回属性字段</param>
        /// <returns></returns>
        public static SearchResultCollection GetDatas(string ldapAddress, string ldapAdUser, string ldapAdPwd, string[] propertiesToLoad)
        {
            try
            {
                //创建目录对象
                var root = new DirectoryEntry(ldapAddress, ldapAdUser, ldapAdPwd, AuthenticationTypes.FastBind);
                //创建查询器
                DirectorySearcher mySearcher = new DirectorySearcher(root);
                mySearcher.SearchScope = SearchScope.Subtree;
                mySearcher.PropertiesToLoad.AddRange(propertiesToLoad);

                //查询所有数据(可根据需要设置分页参数)
                var searchResultCollection = mySearcher.FindAll();
                string strName = root.Name;//失败，会抛出异常

                root.Close();
                root = null;
                return searchResultCollection;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
    }
}
