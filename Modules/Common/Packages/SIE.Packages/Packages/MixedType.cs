using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Packages.Packages
{
	/// <summary>
	/// 混放类型
	/// </summary>
    public enum MixedType
    {
        /// <summary>
		/// 允许
		/// </summary>
		[Label("允许")]
        Allow,

		/// <summary>
		/// 不允许
		/// </summary>
		[Label("不允许")]
		NoAllow,
	}
}
