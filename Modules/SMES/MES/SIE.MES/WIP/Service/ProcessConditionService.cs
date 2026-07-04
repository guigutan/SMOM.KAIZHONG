using SIE.Core.Logs;
using SIE.Domain;
using SIE.EventMessages.MES.WIP;
using SIE.Tech.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIE.MES.WIP.Service
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ProcessConditionService”的 XML 注释
    public class ProcessConditionService : DomainController, IProcessConditionService
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ProcessConditionService”的 XML 注释
    {
        static IList<KeyValuePair<string, string>> ProcessConditionItems = new List<KeyValuePair<string, string>>();

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ProcessConditionService.GetProcessConditionItems()”的 XML 注释
        public virtual IList<KeyValuePair<string, string>> GetProcessConditionItems()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ProcessConditionService.GetProcessConditionItems()”的 XML 注释
        {
            SaveGetProcessConditionItemsLog();
            if (ProcessConditionItems.Any())
            {
                return ProcessConditionItems;
            }
            var props = typeof(CollectData).GetProperties().Where(p => p.GetCustomAttributes(typeof(ConditionItemAttribute)).Any());
            foreach (var prop in props)
            {
                Console.WriteLine();
                var attr = prop.CustomAttributes.First(m => m.AttributeType == typeof(ConditionItemAttribute));
                ProcessConditionItems.Add(new KeyValuePair<string, string>((string)attr.ConstructorArguments[0].Value, (string)attr.ConstructorArguments[1].Value));
                Console.WriteLine();
            }
            return ProcessConditionItems;
        }

        /// <summary>
        /// 保存接口日志
        /// </summary>
        private void SaveGetProcessConditionItemsLog()
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var log = new InterfaceLog()
                {
                    Name = "IProcessConditionService",
                    Method = "GetProcessConditionItems",
                    ControllerName = "ProcessConditionService",
                    InputValue = "",
                };

                RF.Save(log);
                tran.Complete();
            }
        }
    }
}
