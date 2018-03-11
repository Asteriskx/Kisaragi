using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Kisaragi.Helper
{
	/// <summary>
	/// 音声ファイル再生に関しての情報を管理するクラス(Jsonファイル R/W)
	/// </summary>
	public class SettingJson
	{
		#region Readonly variable

		/// <summary>
		/// 音声ファイルの規定場所(各個人の環境で合わせること)
		/// </summary>
		private static readonly string _DefaultFilePath = @"C:\Users\Asterisk\source\repos\Kisaragi\Kisaragi\Voice\";

		#endregion

		#region Properties 

		/// <summary>
		/// 音声ファイルの格納場所を保存する List
		/// </summary>
		private List<string> _voiceData { get; set; } = new List<string>();

		/// <summary>
		/// 音声ファイルデータの格納場所を公開するプロパティ
		/// </summary>
		public List<string> Settings => _voiceData;

		#endregion

		#region .Json File I/O Methods.

		/// <summary>
		/// voiceSettings.json を生成します
		/// </summary>
		public async Task CreateVoiceSettingFileAsync()
		{
			// voiceData の規定パスを指定します。
			var directory = new DirectoryInfo(_DefaultFilePath);

			// voiceData の実ファイル名を列挙します。
			var files = directory.GetFiles("*.mp3", SearchOption.AllDirectories);

			// voice再生に必要なファイルパス(フルパス)を構築します。
			_voiceData.AddRange(from f in files select _DefaultFilePath + f);

			// シリアライズした結果を受け取ります。
			var result = JsonConvert.SerializeObject(_voiceData, new JsonSerializerSettings
			{
				StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
			});

			// Json ファイルに対して、シリアライズしたデータを書き込みます。
			using (var writer = new StreamWriter("voiceSettings.json", false, Encoding.UTF8))
				await writer.WriteAsync(result);
		}

		/// <summary>
		/// settings.json から設定を読み込みます
		/// <para> voiceSettings.json が存在しないときは新規に生成します</para>
		/// </summary>
		public async Task LoadSettingFileAsync()
		{
			try
			{
				string jsonString = null;
				using (var reader = new StreamReader("voiceSettings.json", Encoding.UTF8))
					jsonString = await reader.ReadToEndAsync();

				dynamic json = JsonConvert.DeserializeObject(jsonString);
				foreach (var j in json)
					_voiceData.Add(j.Value);
			}
			catch
			{
				await CreateVoiceSettingFileAsync();
			}
		}

		#endregion

	}
}