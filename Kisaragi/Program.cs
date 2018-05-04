using System;
using System.Windows.Forms;

using Kisaragi.Views;

namespace Kisaragi
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// スプラッシュウィンドウの生成
			using (var splash = new SplashWindow(3000))
				splash.ShowDialog();

			Application.Run(new Views.Kisaragi());
		}
	}
}
