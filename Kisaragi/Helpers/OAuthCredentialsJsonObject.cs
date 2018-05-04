using System.IO;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Kisaragi.Helper
{
	/// <summary>
	/// Twitter 認証情報の Json ファイルを管理します。
	/// </summary>
	public class OAuthCredentialsJsonObject
	{

		#region Readonly variable

		/// <summary>
		/// Default 値
		/// </summary>
		private static readonly string _DefaultValue = "please set your tokens";

		#endregion

		#region Properties

		/// <summary>
		/// Consumer Key を管理します。
		/// </summary>
		public string ConsumerKey { get; set; }

		/// <summary>
		/// Consumer Key Secret を管理します。
		/// </summary>
		public string ConsumerKeySecret { get; set; }

		/// <summary>
		/// Access Token を管理します。
		/// </summary>
		public string AccessToken { get; set; }

		/// <summary>
		/// Access Token Secret を管理します。
		/// </summary>
		public string AccessTokenSecret { get; set; }

		#endregion

		#region Constractor 

		private OAuthCredentialsJsonObject() { }

		#endregion

		#region .Json File I/O Methods.

		/// <summary>
		/// tokens.json からアカウント情報を読み込みます
		/// </summary>
		public static async Task<OAuthCredentialsJsonObject> LoadAsync()
		{
			try
			{
				string jsonString = null;
				using (var reader = new StreamReader("tokens.json", Encoding.UTF8))
					jsonString = await reader.ReadToEndAsync();

				return JsonConvert.DeserializeObject<OAuthCredentialsJsonObject>(jsonString);
			}
			catch
			{
				// JSONの構造が間違っている、もしくは存在しなかった場合は新規に生成
				var oauth = new OAuthCredentialsJsonObject();
				await oauth.SaveAsync();

				return oauth;
			}
		}

		/// <summary>
		/// tokens.json を生成します
		/// </summary>
		public async Task SaveAsync()
		{
			var jsonString = JsonConvert.SerializeObject(this, new JsonSerializerSettings
			{
				StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
			});

			// 存在する場合は上書き
			using (var writer = new StreamWriter("tokens.json", false, Encoding.UTF8))
				await writer.WriteAsync(jsonString);

		}

		#endregion

	}
}
