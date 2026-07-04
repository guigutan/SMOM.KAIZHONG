using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SIE.Api;
using SIE.Modules;
using SIE.Reflection;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志特性管理
    /// </summary>
    public static class ApiLogAttrManager
    {
        /// <summary>
        /// 控制器与方法分隔符
        /// </summary>
        private static string split = "-";
        /// <summary>
        /// 字典:控制器-方法,API日志特性
        /// </summary>
        private static Dictionary<string, ApiLogAttrInfo> ApiLogAttrDict { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            ApiLogAttrDict = new Dictionary<string, ApiLogAttrInfo>();
            foreach (ModuleAssembly allModule in AppRuntime.GetAllModules())
            {
                //加载程序集类型
                Type[] array = allModule.Assembly.WithTypesHandler(AppRuntime.PlatformErrorHandler);
                foreach (Type type in array)
                {
                    if (!type.IsSubclassOf(typeof(DomainController)))
                        continue;

                    var ctlApiLogAttr = CustomAttributeExtensions.GetCustomAttribute(type, typeof(ApiLogAttribute), false) as ApiLogAttribute;
                    //仅找本类公开实例方法
                    var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    foreach (MethodInfo method in methods)
                    {
                        var apiServiceAttr = CustomAttributeExtensions.GetCustomAttribute<ApiServiceAttribute>(method);
                        var methodApiLogAttr = CustomAttributeExtensions.GetCustomAttribute<ApiLogAttribute>(method);
                        var methodDisableApiLogAttr = CustomAttributeExtensions.GetCustomAttribute<DisableApiLogAttribute>(method);
                        //只有未禁用API日志，并且控制器或方法标识了启用API日志的，才登记至字典
                        if (!(methodDisableApiLogAttr == null && (methodApiLogAttr != null || ctlApiLogAttr != null)))
                            continue;
                        var logAttr = methodApiLogAttr != null ? methodApiLogAttr : ctlApiLogAttr;
                        var key = GetKey(method.DeclaringType.Name, method.Name);
                        if (!ApiLogAttrDict.ContainsKey(key))
                        {
                            ApiLogAttrDict.Add(key, new ApiLogAttrInfo(apiServiceAttr?.Description ?? method.Name, logAttr.FetchOverTime, logAttr.Logger));
                        }
                        var apiLogAttrInfo = ApiLogAttrDict[key];
                        //为方法增加关键字信息
                        MethodAddKeyInfo(method, apiLogAttrInfo);
                    }
                }
            }
        }

        #region 为方法增加关键字信息
        /// <summary>
        /// 为方法增加关键字信息（仅支持方法的参数是内置类型/自定义类型/列表）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="apiLogAttrInfo"></param>
        private static void MethodAddKeyInfo(MethodInfo method, ApiLogAttrInfo apiLogAttrInfo)
        {
            var methodParams = method.GetParameters();
            for (int paramIndex = 0; paramIndex < methodParams.Length; paramIndex++)
            {
                var baseJsonPath = $"$.Parameters[{paramIndex}].Value";
                var methodParam = methodParams[paramIndex];
                var apiLogParamAttr = CustomAttributeExtensions.GetCustomAttribute<ApiLogKeyParamAttribute>(methodParam);
                if (apiLogParamAttr == null)
                    continue;
                var name = apiLogParamAttr.KeyName;
                if (name.IsNotEmpty())
                    baseJsonPath += "◇" + name;
                //值类型
                if (methodParam.ParameterType.IsValueType || methodParam.ParameterType == typeof(String))
                {
                    apiLogAttrInfo.AddKeyInfo(apiLogParamAttr.Index, baseJsonPath);
                }
                //泛型枚举器
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(methodParam.ParameterType) && methodParam.ParameterType.IsGenericType)
                {
                    AddJsonPathForEnumerable(methodParam.ParameterType, apiLogAttrInfo, apiLogParamAttr.Index, baseJsonPath);
                }
                //普通对象
                else
                {
                    AddJsonPathForCustomType(methodParam.ParameterType, apiLogAttrInfo, apiLogParamAttr.Index, baseJsonPath);
                }
            }
        }
        #endregion

        #region 为列表添加JsonPath
        /// <summary>
        /// 为列表类型添加JsonPath
        /// </summary>
        /// <param name="enumerableType"></param>
        /// <param name="apiLogAttrInfo"></param>
        /// <param name="keyIndex"></param>
        /// <param name="preJsonPath"></param>
        /// <returns></returns>
        private static void AddJsonPathForEnumerable(Type enumerableType, ApiLogAttrInfo apiLogAttrInfo, int keyIndex, string preJsonPath)
        {
            if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(enumerableType))
                return;

            var baseJsonPath = $"{preJsonPath}[*]";
            foreach (var genericType in enumerableType.GetGenericArguments())
            {
                if (genericType.IsValueType || genericType == typeof(String))
                {
                    apiLogAttrInfo.AddKeyInfo(keyIndex, baseJsonPath);
                }
                else if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(genericType) && genericType.IsGenericType)
                {
                    foreach (var genericArgument in genericType.GetGenericArguments())
                    {
                        AddJsonPathForEnumerable(genericArgument, apiLogAttrInfo, keyIndex, baseJsonPath);
                    }
                }
                else
                {
                    AddJsonPathForCustomType(genericType, apiLogAttrInfo, keyIndex, baseJsonPath);
                }
            }
        }
        #endregion

        #region 为自定义类型添加JsonPath
        /// <summary>
        /// 为自定义类型添加JsonPath
        /// </summary>
        /// <param name="customType"></param>
        /// <param name="apiLogAttrInfo"></param>
        /// <param name="keyIndex"></param>
        /// <param name="preJsonPath"></param>
        /// <returns></returns>
        private static void AddJsonPathForCustomType(Type customType, ApiLogAttrInfo apiLogAttrInfo, int keyIndex, string preJsonPath)
        {
            if (customType.IsValueType || customType == typeof(String) || typeof(System.Collections.IEnumerable).IsAssignableFrom(customType))
                return;

            //处理实体属性上的关键字特性
            foreach (var typeProperty in customType.GetProperties())
            {
                var typePropAttr = CustomAttributeExtensions.GetCustomAttribute<ApiLogKeyPropertyAttribute>(typeProperty);
                if (typePropAttr == null)
                    continue;
                var name = typePropAttr.PropertyName;
                var baseJsonPath = $"{preJsonPath}.{typeProperty.Name}";
                if (name.IsNotEmpty())
                    baseJsonPath += "◇" + name;
                if (typeProperty.PropertyType.IsValueType || typeProperty.PropertyType == typeof(String))
                {
                    apiLogAttrInfo.AddKeyInfo(keyIndex, baseJsonPath);
                }
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(typeProperty.PropertyType) && typeProperty.PropertyType.IsGenericType)
                {
                    AddJsonPathForEnumerable(typeProperty.PropertyType, apiLogAttrInfo, keyIndex, baseJsonPath);
                }
                else
                {
                    apiLogAttrInfo.AddKeyInfo(keyIndex, $"{baseJsonPath}.{GetJsonPathForCustomType(typeProperty.PropertyType)}");
                }
            }
        }
        #endregion

        #region 为自定义类型获取JsonPath
        /// <summary>
        /// 为自定义类型获取JsonPath
        /// </summary>
        /// <param name="customType"></param>
        /// <returns></returns>
        private static string GetJsonPathForCustomType(Type customType)
        {
            var jsonPath = string.Empty;
            if (customType.IsValueType || customType == typeof(String) || typeof(System.Collections.IEnumerable).IsAssignableFrom(customType))
                return jsonPath;

            //处理实体属性上的关键字特性
            foreach (var typeProperty in customType.GetProperties())
            {
                var typePropAttr = CustomAttributeExtensions.GetCustomAttribute<ApiLogKeyPropertyAttribute>(typeProperty);
                if (typePropAttr == null)
                    continue;
                if (typeProperty.PropertyType.IsValueType || typeProperty.PropertyType == typeof(String))
                {
                    return typeProperty.Name;
                }
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(typeProperty.PropertyType) && typeProperty.PropertyType.IsGenericType)
                {
                    foreach (var propGenericType in typeProperty.PropertyType.GetGenericArguments())
                    {
                        if (propGenericType.IsValueType || propGenericType == typeof(String))
                        {
                            return $"{typeProperty.Name}[*]";
                        }
                        else
                        {
                            return $"{typeProperty.Name}[*].{GetJsonPathForCustomType(typeProperty.PropertyType)}";
                        }
                    }
                }
                else
                {
                    //不支持
                    return $"{typeProperty.Name}.{GetJsonPathForCustomType(typeProperty.PropertyType)}";
                }
            }
            return jsonPath;
        }
        #endregion

        #region 获取API日志特性
        /// <summary>
        /// 获取API日志特性
        /// </summary>
        /// <param name="type">API控制器类型</param>
        /// <param name="method">API方法</param>
        /// <returns></returns>
        public static ApiLogAttrInfo TryGetApiLogAttrInfo(string type, string method)
        {
            var key = GetKey(type, method);
            return ApiLogAttrDict.ContainsKey(key) ? ApiLogAttrDict[key] : null;
        }
        #endregion

        #region 获取API方法
        /// <summary>
        /// 获取API方法
        /// </summary>
        /// <returns></returns>
        public static List<string> GetApiNames()
        {
            return ApiLogAttrDict.Select(p => p.Value.MethodDescription).ToList();
        }
        #endregion

        #region 获取字典key
        /// <summary>
        /// 获取字典key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string GetKey(string type, string method)
        {
            return $"{type}{split}{method}";
        }
        #endregion
    }

    /// <summary>
    /// API日志特性
    /// </summary>
    public class ApiLogAttrInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="methodDescription">方法描述</param>
        /// <param name="fetchOverTime">超时毫秒</param>
        /// <param name="logger">日志记录器</param>
        public ApiLogAttrInfo(string methodDescription, int fetchOverTime, IApiLogLogger logger)
        {
            MethodDescription = methodDescription;
            FetchOverTime = fetchOverTime;
            Logger = logger;
            KeyIndexJsonPathDict = new Dictionary<int, List<string>>();
        }
        /// <summary>
        /// 方法描述
        /// </summary>
        public string MethodDescription { get; set; }
        /// <summary>
        /// 超时毫秒
        /// </summary>
        public int FetchOverTime { get; set; }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public IApiLogLogger Logger { get; set; }
        /// <summary>
        /// 字典:关键字索引,JsonPath
        /// </summary>
        public Dictionary<int, List<string>> KeyIndexJsonPathDict { get; private set; }
        /// <summary>
        /// 添加关键字信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="jsonPath"></param>
        public virtual void AddKeyInfo(int index, string jsonPath)
        {
            if (!KeyIndexJsonPathDict.ContainsKey(index))
            {
                KeyIndexJsonPathDict.Add(index, new List<string> { jsonPath });
            }
            else
            {
                KeyIndexJsonPathDict[index].Add(jsonPath);
            }
        }
    }
}
