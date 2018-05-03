using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kisaragi.TwitterAPI.OAuth
{
	/// <summary>
	/// Kisaragi 時報システムを Twitterへリンクさせるための認証クラス
	/// </summary>
	public class AuthKisaragi
	{

		#region Constant Variable

		private const string REQUEST_TOKEN_URL = "https://api.twitter.com/oauth/request_token";
		private const string ACCESS_TOKEN_URL = "https://api.twitter.com/oauth/access_token";
		private const string AUTHORIZE_URL = "https://api.twitter.com/oauth/authorize";

		#endregion

		#region Properties

		/// <summary>
		///  HttpClient インスタンス の管理を行います。
		/// </summary>
		private HttpClient _Http { get; set; }

		/// <summary>
		/// トークン生成時の乱数 インスタンス
		/// </summary>
		private Random _Rand { get; set; } = new Random();

		#endregion

		#region Constractor 

		/// <summary>
		/// AuthKisaragi Constractor
		/// </summary>
		/// <param name="httpClient"></param>
		public AuthKisaragi(HttpClient httpClient)
		{
			ServicePointManager.Expect100Continue = false;
			_Http = httpClient;
		}

		#endregion

		#region OAuth Authorize Method's.

		/// <summary>
		/// リクエストトークンの取得を行います。
		/// </summary>
		/// <returns></returns>
		public async Task GetRequestTokenAsync(Credentials credentials)
		{
			Debug.WriteLine("------------ リクエストトークン 生成開始 -----------------");

			string response =
				await RequestAsync(credentials.ConsumerKey, credentials.ConsumerKeySecret, "", "", REQUEST_TOKEN_URL, HttpMethod.Get, null);

			var perseData = _ParseStrings(response);

			credentials.RequestToken = perseData["oauth_token"];
			credentials.RequestTokenSecret = perseData["oauth_token_secret"];

			Debug.WriteLine("------------ リクエストトークン 生成完了 -----------------");
		}

		/// <summary>
		/// 認証ページの URL を取得します。
		/// </summary>
		/// <returns></returns>
		public Uri GetAuthorizeUrl(Credentials credentials)
		{
			Debug.WriteLine("------------ 認証用URL 取得シーケンス実施 -----------------");

			if (credentials.RequestToken != null)
				return new Uri($"{AUTHORIZE_URL}?oauth_token={credentials.RequestToken}");
			else
				throw new ApplicationException("リクエストトークンが未設定です。GetRequestTokenAsync() をコールしてください。");
		}

		/// <summary>
		/// Access Token の取得を行います。
		/// </summary>
		/// <param name="PIN"></param>
		/// <returns></returns>
		public async Task<(string accessToken, string accessTokenSecret)> GetAccessTokenAsync(Credentials credentials, string PIN)
		{
			Debug.WriteLine("------------ アクセストークン 生成開始 ----------------- >> " + PIN);

			var parameters = new Dictionary<string, string> { { "oauth_verifier", PIN } };

			var response = await RequestAsync(credentials.ConsumerKey, credentials.ConsumerKeySecret,
				credentials.RequestToken, credentials.RequestTokenSecret, ACCESS_TOKEN_URL, HttpMethod.Post, parameters);

			var perseData = _ParseStrings(response);

			credentials.AccessToken = perseData["oauth_token"];
			credentials.AccessTokenSecret = perseData["oauth_token_secret"];
			credentials.UserId = perseData["user_id"];
			credentials.ScreenName = perseData["screen_name"];

			Debug.WriteLine("------------ アクセストークン 生成完了 -----------------");

			return (credentials.AccessToken, credentials.AccessTokenSecret);
		}

		/// <summary>
		/// 認証パラメータを生成します。
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		private string _OAuthParameters(IDictionary<string, string> parameters, string spl = "&", string braket = "")
		{
			return string.Join(spl,
				from p in parameters select $"{UrlEncode(p.Key)}={braket}{UrlEncode(p.Value)}{braket}");
		}

		/// <summary>
		/// Twitter に対して GETリクエスト/POSTリクエスト を行います。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		/// <param name="oauthParameters"></param>
		/// <returns></returns>
		public async Task<string> RequestAsync(string consumerKey, string consumerKeySecret,
			string token, string tokenSecret, string url, HttpMethod type, IDictionary<string, string> parameters = null)
		{
			Debug.WriteLine("------------ リクエスト開始 ----------------- >> " + type.ToString() + " " + url);

			var oauthParameters = _GenerateParameters(token, consumerKey);

			if (type == HttpMethod.Get && parameters != null)
				url += $"?{_OAuthParameters(parameters)}";

			var request = new HttpRequestMessage(type, url);
			HttpResponseMessage response = null;

			if (oauthParameters != null)
			{
				if (parameters != null)
				{
					foreach (var p in parameters)
						oauthParameters.Add(p.Key, p.Value);
				}

				string signature = _GenerateSignature(consumerKeySecret, tokenSecret, type.ToString(), url, oauthParameters);
				oauthParameters.Add("oauth_signature", signature);

				// 各種認証系の Query を Header に格納(認証ヘッダとなる部分)
				request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", _OAuthParameters(oauthParameters, ",", "\""));
			}

			Exception tmp = null;
			await Task.Run(async () =>
			{
				try
				{
					if (type == HttpMethod.Get)
					{
						Debug.WriteLine($"---------------- リクエストヘッダ情報 {type.ToString()} --------------------");
						Debug.WriteLine($" Header : {request.Headers.ToString()}");
						Debug.WriteLine("---------------- リクエストヘッダ情報 ここまで -------------------------------");
					}
					if (type == HttpMethod.Post)
					{
						// ユーザが指定するパラメータを body に格納 e.g.) [status]
						if (parameters != null)
							request.Content = new FormUrlEncodedContent(parameters);

						Debug.WriteLine($"---------------- リクエストヘッダ情報 {type.ToString()} --------------------");
						Debug.WriteLine($" Body   : {await request.Content.ReadAsStringAsync()}");
						Debug.WriteLine($" Header : {request.Headers.ToString()}");
						Debug.WriteLine("---------------- リクエストヘッダ情報 ここまで -------------------------------");
					}

					request.Headers.ExpectContinue = false;
					response = await _Http.SendAsync(request);

					if (response.StatusCode != HttpStatusCode.OK)
						throw new HttpRequestException($"HTTP 通信エラーが発生しました。" +
							$"ステータス : {response.StatusCode} {await response.Content.ReadAsStringAsync()} 対象：HttpMultiRequestAsync()");
				}
				catch (HttpRequestException e) { tmp = e; }
			});

			if (tmp != null)
				Console.WriteLine($"{tmp.StackTrace} : {tmp.Message}");

			var result = await response.Content.ReadAsStringAsync();
			return result;
		}

		/// <summary>
		/// Signature 生成を行います。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="httpMethod"></param>
		/// <param name="query"></param>
		/// <param name="consumer_secret"></param>
		/// <param name="ats"></param>
		/// <param name="conectedQuery"></param>
		/// <returns></returns>
		private string _GenerateSignature(string consumerKeySecret, string tokenSecret, string httpMethod, string url, IDictionary<string, string> parameters)
		{
			Debug.WriteLine("------------ シグネチャ生成開始 -----------------");

			var sort = new SortedDictionary<string, string>(parameters);
			var uri = new Uri(url);
			var unQueryStringUrl = $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}";
			var signatureBase = $"{httpMethod}&{UrlEncode(unQueryStringUrl)}&{UrlEncode(_OAuthParameters(sort))}";

			HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(UrlEncode(consumerKeySecret) + '&' + UrlEncode(tokenSecret ?? "")));
			var result = Convert.ToBase64String(hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(signatureBase)));

			Debug.WriteLine("------------ シグネチャ生成完了 -----------------");

			return result;
		}

		/// <summary>
		/// 認証にて使用するパラメータを構築します。
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private SortedDictionary<string, string> _GenerateParameters(string token, string consumerKey)
		{
			Debug.WriteLine("------------ パラメータ生成開始 -----------------");

			var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			var timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();

			var result = new SortedDictionary<string, string> {
				{ "oauth_consumer_key", consumerKey },
				{ "oauth_nonce", _GenerateNonce(32) },
				{ "oauth_signature_method", "HMAC-SHA1" },
				{ "oauth_timestamp", timeStamp },
				{ "oauth_version", "1.0" }
			};

			if (!string.IsNullOrEmpty(token))
				result.Add("oauth_token", token);

			Debug.WriteLine($"  oauth_consumer_key = {consumerKey}");
			Debug.WriteLine($"  oauth_signature_method = HMAC-SHA1");
			Debug.WriteLine($"  oauth_timestamp = {timeStamp}");
			Debug.WriteLine($"  oauth_nonce = {_GenerateNonce(32)}");
			Debug.WriteLine($"  oauth_version = 1.0");

			Debug.WriteLine("------------ パラメータ生成完了 -----------------");

			return result;
		}

		private Dictionary<string, string> _ParseStrings(string query)
		{
			var persedStr = new Dictionary<string, string>();

			foreach (var items in query.Split('&'))
			{
				var sepalator = items.Split('=');
				persedStr.Add(sepalator[0], sepalator[1]);
			}

			return persedStr;
		}

		/// <summary>
		/// ワンタイムトークンを生成します。
		/// </summary>
		private string _GenerateNonce(int len)
		{
			string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return str.Aggregate(new StringBuilder(len), (sb, s) => sb.Append(str[_Rand.Next(str.Length)])).ToString();
		}

		/// <summary>
		/// 与えられた URL をエンコードします。
		/// </summary>
		public string UrlEncode(string value)
		{
			string unreserved = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
			var result = new StringBuilder();

			foreach (var b in Encoding.UTF8.GetBytes(value))
			{
				var isReserved = (b < 0x80 && unreserved.IndexOf((char)b) != -1);
				result.Append(isReserved ? ((char)b).ToString() : $"%{(int)b:X2}");
			}

			return result.ToString();
		}

		#endregion

	}
}
