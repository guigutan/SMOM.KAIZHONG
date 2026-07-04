using SIE.Warehouses.Stations;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// LED屏幕基础数据视图配置
    /// </summary>
    internal class LEDViewConfig : WebViewConfig<LED>
	{		
		///<summary>
		/// 配置列表视图
		/// </summary>
		protected override void ConfigListView()
		{ 	  
			View.UseDefaultCommands().UseImportCommands(); 
			View.Property(p => p.Code);
            View.Property(p => p.ShowStype);
			View.Property(p => p.CardNo);
			View.Property(p => p.CardType);
			View.Property(p => p.CommunicationMode);
			View.Property(p => p.ScreemWidth);
			View.Property(p => p.ScreemHeight);
			View.Property(p => p.SerialBaud);
			View.Property(p => p.SerialNum);
			View.Property(p => p.IpAddress);
			View.Property(p => p.NetPort);
			View.Property(p => p.ColorStyle);
			View.Property(p => p.Title);
			View.Property(p => p.Timeout);
			View.Property(p => p.DefaultText);
			View.Property(p => p.Password);

			View.Property(p => p.Note);
		}

		/// <summary>
		/// 查询视图
		/// </summary>
        protected override void ConfigQueryView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.ShowStype);
		}

		/// <summary>
		/// 选择视图
		/// </summary>
        protected override void ConfigSelectionView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.ShowStype);
			View.Property(p => p.CardNo);
			View.Property(p => p.CardType);
			View.Property(p => p.CommunicationMode);
			View.Property(p => p.ScreemWidth);
			View.Property(p => p.ScreemHeight);
			View.Property(p => p.SerialBaud);
			View.Property(p => p.SerialNum);
			View.Property(p => p.IpAddress);
			View.Property(p => p.NetPort);
			View.Property(p => p.ColorStyle);
			View.Property(p => p.Title);
			View.Property(p => p.Timeout);
			View.Property(p => p.DefaultText);
			View.Property(p => p.Password);
			View.Property(p => p.Note);
		}

		/// <summary>
		/// 导入视图
		/// </summary>
        protected override void ConfigImportView()
        {
			View.Property(p => p.Code);
			View.Property(p => p.CardNo);
			View.Property(p => p.CardType);
			View.PropertyRef(p => p.ShowStype.Code).HasLabel("LED屏显样式");
			View.Property(p => p.CommunicationMode);
			View.Property(p => p.ScreemWidth);
			View.Property(p => p.ScreemHeight);
			View.Property(p => p.SerialBaud);
			View.Property(p => p.SerialNum);
			View.Property(p => p.IpAddress);
			View.Property(p => p.NetPort);
			View.Property(p => p.ColorStyle);
			View.Property(p => p.Title);
			View.Property(p => p.Timeout);
			View.Property(p => p.DefaultText);
			View.Property(p => p.Password);
			View.Property(p => p.Note);
		}
    }
}