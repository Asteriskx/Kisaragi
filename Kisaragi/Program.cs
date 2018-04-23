using System;
using System.Threading;
using System.Windows.Forms;

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
			using (var splash = new SplashWindow())
				splash.Showing();

			Application.Run(new Kisaragi());
		}
	}
}
