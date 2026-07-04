using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using SIE.MES.WIP;
using System;
using Xunit;

namespace SIE.UT.MES
{
    public class WipScriptTest : IClassFixture<AppInit>
    {
        [Fact]
        public void WipScriptExecuteTest()
        {
            CollectData collectData = new CollectData();
            //数值类型
            string script1 = @"def GetNextProcess(collectData):
                                   return collectData.NgQty==1";
            collectData.NgQty = 0;
            Assert.False(ExecuteScript<bool>(script1, collectData));
            collectData.NgQty = 1;
            Assert.True(ExecuteScript<bool>(script1, collectData));
            //字符串类型
            string script2 = @"def GetNextProcess(collectData):
                                   return collectData.OutputBatch.BatchNo=='BatchNo'";
            collectData.OutputBatch = new SIE.MES.BatchWIP.OutputBatch();
            collectData.OutputBatch.BatchNo = "Test";
            Assert.False(ExecuteScript<bool>(script2, collectData));
            collectData.OutputBatch.BatchNo = "BatchNo";
            Assert.True(ExecuteScript<bool>(script2, collectData));
            ////枚举类型
            //string script3 = @"def GetNextProcess(collectData):
            //                       return collectData.Result";
            //collectData.Result = Common.ResultType.Fail;
            //var res = ExecuteScript<ResultType>(script3, collectData);

            //collectData.Result = Common.ResultType.Pass;
            //Assert.False(ExecuteScript<bool>(script3, collectData));
            //collectData.Result = Common.ResultType.Fail;
            //Assert.True(ExecuteScript<bool>(script3, collectData));

            //for x in [1, 2, 3]: print x,

            string script4 = @"def GetNextProcess(collectData):
                                   return 1,2";
            ExecuteScript<PythonTuple>(script4, collectData);

        }

        private T ExecuteScript<T>(string script, CollectData collectData)
        {
            if (script.IsNullOrEmpty())
                return default(T);
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.CreateScriptSourceFromString(script).Compile().Execute(scope);
            var func = scope.GetVariable<Func<CollectData, T>>("GetNextProcess");
            return func(collectData);
        }
    }
}