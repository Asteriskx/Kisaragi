using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kisaragi.Helper
{
	/// <summary>
	/// Kisaragi 時報システムのヘルパクラス
	/// 音声ファイルのコマンドを取り扱います。
	/// </summary>
	public class Helpers
	{
		#region readonly Variable

		private static readonly string _AliasName = "MediaFile";

		#endregion

		#region externs

		[DllImport("winmm.dll")]
		private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

		#endregion

		#region Constractor

		public Helpers() { }

		#endregion

		/// <summary>
		/// 非同期でボイスファイルを再生します。
		/// </summary>
		public async Task _PlayingVoiceAsync(string fileName)
		{
			string cmd;
			string jdg;
			var status = new StringBuilder(16);

			try
			{
				// ファイルを開く
				cmd = "open \"" + fileName + "\" type mpegvideo alias " + _AliasName;

				if (mciSendString(cmd, status, status.Capacity, IntPtr.Zero) != 0)
					throw new ApplicationException();

				// 再生する
				cmd = "play " + _AliasName;
				mciSendString(cmd, status, status.Capacity, IntPtr.Zero);

				// 再生状態の監視
				do
				{
					Console.WriteLine($"playState = {status.ToString()}");

					// 再生状態の取得
					jdg = $"status { _AliasName} mode";
					mciSendString(jdg, status, status.Capacity, IntPtr.Zero);

					// 意図的な待ち時間
					await Task.Delay(1000);

				} while (status.ToString() == "playing");

				// 音声ファイル停止・そっ閉じ
				await _StoppedVoiceAsync();
			}
			catch (ApplicationException ex)
			{
				MessageBox.Show($"mp3 再生エラーが発生しました。\r\n{ex.StackTrace}",
								"mp3 再生エラー",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
			}
			finally
			{
				// NOP
			}
		}

		/// <summary>
		/// 非同期でボイスファイルを停止します。
		/// </summary>
		public async Task _StoppedVoiceAsync()
		{
			string cmd;

			// Voice を停止する
			cmd = "stop " + _AliasName;
			mciSendString(cmd, null, 0, IntPtr.Zero);

			// 閉じる
			cmd = "close " + _AliasName;
			mciSendString(cmd, null, 0, IntPtr.Zero);

			await Task.Delay(100);
		}
	}
}
