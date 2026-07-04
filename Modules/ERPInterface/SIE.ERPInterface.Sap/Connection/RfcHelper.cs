using Newtonsoft.Json;
using SapNwRfc;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Datas;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SIE.ERPInterface.Sap.Connection
{
    /// <summary>
    /// SAP RFC帮助类
    /// </summary>
    public static class RfcHelper
    {
        /// <summary>
        /// SAP连接配置
        /// </summary>
        public static string ConnectionString = AppRuntime.Config.Get("SapConnection");

        /// <summary>
        /// 执行SAP查询
        /// </summary>
        /// <typeparam name="T">返回结构类型</typeparam>
        /// <typeparam name="M">结果数据集类型</typeparam>
        /// <typeparam name="E">查询参数类型</typeparam>
        /// <param name="functionName">参数数据类型</param>
        /// <param name="sapParameters">查询参数</param>
        /// <param name="func">结果数据集转换委托</param>
        /// <returns></returns>
        public static SapResult Sap<T, E>(string functionName, E sapParameters, Func<T, IList> func = null)
        {
            SapResult interfaceResult = new SapResult();
            var connection = new SapConnection(ConnectionString);
            try
            {
                //记录请求LOG
                string jsonData = JsonConvert.SerializeObject(sapParameters);
                interfaceResult.RequestStr = jsonData;
                interfaceResult.RequestDate = DateTime.Now;

                //连接SAP
                connection.Connect();
                var someFunction = connection.CreateFunction(functionName);
                var result = someFunction.Invoke<T>(sapParameters);

                //记录接收LOG
                interfaceResult.ResponseStr = JsonConvert.SerializeObject(result);
                interfaceResult.ResponseDate = DateTime.Now;
                interfaceResult.IsSuccess = true;
              
                //if (func != null)
                //    interfaceResult.SapResultData = func.Invoke(result);

                return interfaceResult;
            }
            catch (Exception ex)
            {
                interfaceResult.IsSuccess = false;
                interfaceResult.SapResultData.MSGTX = ex.Message;
                return interfaceResult;
            }
            finally
            {
                connection.Disconnect();
                connection.Dispose();
            }

        }


        /// <summary>
        /// SAP无参数调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static T Sap<T>(string functionName)
        {
            var connection = new SapConnection(ConnectionString);
            try
            {
                connection.Connect();
                var someFunction = connection.CreateFunction(functionName);
                var result = someFunction.Invoke<T>();
                return result;
            }
            catch (Exception ex)
            {
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
            finally
            {
                connection.Disconnect();
                connection.Dispose();
            }
        }

        /// <summary>
        /// 无返回结果
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="sapParameters"></param>
        public static void Sap(string functionName, Object sapParameters)
        {
            var connection = new SapConnection(ConnectionString);
            try
            {
                connection.Connect();
                var someFunction = connection.CreateFunction(functionName);
                someFunction.Invoke(sapParameters);
            }
            catch (Exception ex)
            {
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
            finally
            {
                connection.Disconnect();
                connection.Dispose();
            }

        }

        /// <summary>
        /// 在同一次连接中使用循环调用。（部分接口缺少批量传参的情况可以使用）
        /// </summary>
        /// <param name="functionName">方法名</param>
        /// <param name="list">入参数组</param>
        public static List<R> Sap<T, R>(string functionName, List<T> list)
        {
            var connection = new SapConnection(ConnectionString);
            List<R> results = new List<R>();
            try
            {

                foreach (var sapParameters in list)
                {
                    connection.Connect();
                    var someFunction = connection.CreateFunction(functionName);
                    var result = someFunction.Invoke<R>(sapParameters);
                    results.Add(result);
                    connection.Disconnect();
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
            finally
            {
                connection.Disconnect();
                connection.Dispose();
            }
        }

        /// <summary>
        /// 批量传入SAP且回传数据与现有数据进行一对一关联
        /// </summary>
        /// <param name="functionName">方法名</param>
        /// <param name="list">入参数组</param>
        public static List<SapParameters<T, R>> Sap<T, R>(string functionName, List<SapParameters<T, R>> list)
        {
            var connection = new SapConnection(ConnectionString);
            try
            {
                foreach (var sapParameter in list)
                {
                    connection.Connect();
                    var someFunction = connection.CreateFunction(functionName);
                    var result = someFunction.Invoke<R>(sapParameter.InputParams);
                    sapParameter.OutputParams = result;
                    connection.Disconnect();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex.InnerException != null ? ex.InnerException : ex;
            }
            finally
            {
                connection.Disconnect();
                connection.Dispose();
            }
        }
    }
}
