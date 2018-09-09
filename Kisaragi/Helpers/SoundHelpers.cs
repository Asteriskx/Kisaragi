using System;
using System.IO;
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
	public class SoundHelpers : IDisposable
	{

		#region Properties

		/// <summary>
		/// 音声ファイルのファイルパスを管理します。
		/// </summary>
		public string FilePath { get; private set; }

		/// <summary>
		/// 音声ファイルのエイリアス名を管理します。
		/// </summary>
		public string AliasName { get; private set; } = "MediaFile";

		#endregion

		#region externs

		[DllImport("winmm.dll")]
		private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

		#endregion

		#region Constractor

		/// <summary>
		/// 
		/// </summary>
		public SoundHelpers() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		private SoundHelpers(string filePath) => this.FilePath = filePath;

		#endregion

		#region SoundHelpers Method's

		/// <summary>
		/// mciSendString の薄いラッパーメソッド
		/// </summary>
		/// <param name="command"></param>
		/// <param name="aliasName"></param>
		/// <returns></returns>
		private static (int action, string state) _MciCommand(string command, string aliasName)
		{
			var status = new StringBuilder(16);
			var (action, state) = (0, string.Empty);

			// 状態を監視したいものとそうでないものの 2 パターンに分類
			if (command == "open \"" || command == "play" || command == "status")
				(action, state) = (mciSendString($"{command} {aliasName}", status, status.Capacity, IntPtr.Zero), status.ToString());
			else if (command == "stop" || command == "close")
				(action, state) = (mciSendString($"{command} {aliasName}", null, 0, IntPtr.Zero), string.Empty);
			else
				(action, state) = (-1, "err");

			return (action, state);
		}

		/// <summary>
		/// 非同期でボイスファイルを再生します。
		/// </summary>
		public async Task<SoundHelpers> _PlayingVoiceAsync(string filePath)
		{
			SoundHelpers sound = null;
			if (filePath == null) return null;

			try
			{
				sound = Open(filePath, AliasName);
				sound.Play();

				// 再生状態の監視
				do
				{
					// 意図的な待ち時間
					await Task.Delay(1000);

				} while (sound.Status().state == "playing");

				await _StoppedVoiceAsync(sound);
				return sound;
			}
			catch (FileNotFoundException ex)
			{
				sound?.Close();
				MessageBox.Show($"再生エラーが発生しました。\r\n{ex.StackTrace}", "再生エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
		}

		/// <summary>
		/// 非同期でボイスファイルを停止します。
		/// </summary>
		private async Task _StoppedVoiceAsync(SoundHelpers sound)
		{
			if (sound == null) return;
			sound.Stop();
			sound.Close();
			await Task.Delay(100);
		}

		/// <summary>
		/// 音声ファイルをオープンします。
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static SoundHelpers Open(string filePath, string aliasName)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException($"ファイルが存在しません。: {filePath}");

			var (action, state) = _MciCommand("open \"", $"{filePath} \" type mpegvideo alias {aliasName}");
			if (action == -1 && state == "error")
				throw new ApplicationException($"ファイルオープンに失敗しました。\"{filePath}\"");

			return new SoundHelpers(filePath);
		}

		#endregion

		#region SoundHelpers Command's 

		/// <summary>
		/// 音声ファイルの再生状態を監視し、Tuple にて返却します。
		/// </summary>
		/// <returns></returns>
		public (int action, string state) Status()
		{
			return _MciCommand("status", $"{this.AliasName} mode");
		}

		/// <summary>
		/// 音声ファイルの再生を行います。
		/// </summary>
		public void Play() => _MciCommand("play", this.AliasName);

		/// <summary>
		/// 音声ファイルの停止を行います。
		/// </summary>
		public void Stop() => _MciCommand("stop", this.AliasName);

		/// <summary>
		/// 音声ファイルをクローズします。
		/// </summary>
		public void Close() => _MciCommand("close", this.AliasName);

		/// <summary>
		/// 後処理を行います。
		/// </summary>
		public void Dispose() => this.Close();

		#endregion

	}
}
