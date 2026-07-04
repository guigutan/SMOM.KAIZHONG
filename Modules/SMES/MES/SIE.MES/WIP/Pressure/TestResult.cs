using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.Pressure
{
	/// <summary>
	/// 测试结果
	/// </summary>
	public enum TestResult
	{
        /// <summary>
        /// PASS
        /// </summary>
        [Label("PASS")]
		PASS = 1,
        /// <summary>
        /// Fail
        /// </summary>
        [Label("FAIL")]
		FAIL = 2,
	}
}
