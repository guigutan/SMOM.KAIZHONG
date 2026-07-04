using SIE;
using SIE.Domain;
using SIE.Mda.Metadatas.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常来源
	/// </summary>
	[RootEntity, Serializable]
	[Label("异常监控实体")]
	[DisplayMember(nameof(Name))]

    public partial class AbnormalEntityMetadata : EntityMetadata
	{

	}
}