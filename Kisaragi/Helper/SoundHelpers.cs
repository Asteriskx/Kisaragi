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

		public string FilePath { get; private set; }
		public string AliasName { get; private set; }
		private static Random _Random { get; set; } = new Random();

		#endregion

		#region externs

		[DllImport("winmm.dll")]
		private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

		#endregion

		#region Constractor

		private SoundHelpers(string filePath, string aliasName)
		{
			this.FilePath = filePath;
			this.AliasName = aliasName;
		}

		#endregion

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

			if (command == "play" || command == "status")
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
				// ファイルを開く
				sound = Open(filePath);

				// 再生する
				sound.Play();

				// 再生状態の監視
				do
				{
					// 意図的な待ち時間
					await Task.Delay(1000);

				} while (sound.Status().state == "playing");

				// 音声ファイル停止・そっ閉じ
				await _StoppedVoiceAsync(sound);
				return sound;
			}
			catch (FileNotFoundException ex)
			{
				sound?.Close();
				MessageBox.Show($"mp3 再生エラーが発生しました。\r\n{ex.StackTrace}",
								"mp3 再生エラー",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
				return null;
			}
			finally
			{
				// NOP
			}
		}

		/// <summary>
		/// 非同期でボイスファイルを停止します。
		/// </summary>
		public async Task _StoppedVoiceAsync(SoundHelpers sound)
		{
			if (sound == null) return;
			sound.Stop();
			sound.Close();
			await Task.Delay(100);
		}

		public static SoundHelpers Open(string filePath)
		{
			var aliasName = $"{_Random.Next(0, 1000)}";

			if (!File.Exists(filePath))
				throw new FileNotFoundException($"file not found: {filePath}");

			var mci = _MciCommand("open", $"{filePath} type mpegvideo alias {aliasName}");
			if (mci.action != -1 && mci.state != "error")
				throw new ApplicationException($"failed to open sound file \"{filePath}\"");

			return new SoundHelpers(filePath, aliasName);
		}

		public (int action, string state) Status()
		{
			return _MciCommand("status", $"{this.AliasName} mode");
		}

		public void Play() => _MciCommand("play", this.AliasName);
		public void Stop() => _MciCommand("stop", this.AliasName);
		public void Close() => _MciCommand("close", this.AliasName);
		public void Dispose() => this.Close();
	}
}
