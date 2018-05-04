using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Kisaragi.Helper
{
	/// <summary>
	/// 音声設定ファイルの情報を管理するクラス(Jsonファイル R/W)
	/// </summary>
	public class NotifyVoiceSettingJsonObject
	{

		#region Constant Variable

		/// <summary>
		/// 音声ファイルの規定場所(各個人の環境で合わせること)
		/// </summary
		private const string _DefaultFilePath = "please your hope voice file.";

		#endregion

		#region Properties 

		/// <summary>
		/// 音声ファイルの格納場所を保存する List
		/// </summary>
		[JsonProperty("voiceData")]
		private List<string> _voiceData { get; set; } = new List<string>();

		/// <summary>
		/// 音声ファイルデータの格納場所を公開するプロパティ
		/// </summary>
		public List<string> Voice => _voiceData;

		/// <summary>
		/// 音声ファイルデータの格納場所を設定します。
		/// </summary>
		private string _VoicePath { get; set; }

		/// <summary>
		/// 音声ファイルデータ保存ファイル名を設定します。
		/// </summary>
		private string _FileName { get; set; }

		#endregion

		#region Constractor 

		public NotifyVoiceSettingJsonObject(string fileName, string voicePath = null)
		{
			this._FileName = fileName;
			this._VoicePath = voicePath;
		}

		#endregion

		#region .Json File I/O Methods.

		/// <summary>
		/// settings.json から設定を読み込みます
		/// <para> voiceSettings.json が存在しないときは新規に生成します</para>
		/// </summary>
		public async Task LoadFileAsync()
		{
			try
			{
				string jsonString = null;
				using (var reader = new StreamReader(this._FileName, Encoding.UTF8))
					jsonString = await reader.ReadToEndAsync();

				dynamic json = JsonConvert.DeserializeObject(jsonString);
				foreach (var j in json)
					_voiceData.Add(j.Value);
			}
			catch
			{
				await SaveFileAsync();
			}
		}

		/// <summary>
		/// voiceSettings.json へ値を保存します。
		/// </summary>
		public async Task SaveFileAsync()
		{
			// voiceData の規定パスを指定します。
			var directory = new DirectoryInfo(this._VoicePath);

			// voiceData の実ファイル名を列挙します。
			var files = directory.GetFiles("*.mp3", SearchOption.AllDirectories);

			// voice再生に必要なファイルパス(フルパス)を構築します。
			_voiceData.AddRange(from f in files select this._VoicePath + f);

			// シリアライズした結果を受け取ります。
			var result = JsonConvert.SerializeObject(_voiceData, new JsonSerializerSettings
			{
				StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
			});

			// Json ファイルに対して、シリアライズしたデータを書き込みます。
			using (var writer = new StreamWriter(this._FileName, false, Encoding.UTF8))
				await writer.WriteAsync(result);

		}

		#endregion

	}
}