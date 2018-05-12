using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Kisaragi.APIs.OAuth;

namespace Kisaragi.APIs.Twitter
{
	/// <summary>
	/// Twitter へ アクセスするための ラッパークラス
	/// </summary>
	public class Twitter : Credentials
	{

		#region Properties

		private HttpClient _Client { get; set; }
		public AuthKisaragi Auth { get; set; }
		public Credentials Credentials { get; set; }

		#endregion

		#region Constractor 

		/// <summary>
		/// Twitter コンストラクタ : CK / CKS / HttpClient 版
		/// </summary>
		/// <param name="consumerKey"></param>
		/// <param name="consumerKeySecret"></param>
		/// <param name="client"></param>
		public Twitter(string consumerKey, string consumerKeySecret, HttpClient client) : base(consumerKey, consumerKeySecret)
		{
			this._Client = client;
			this.Auth = new AuthKisaragi(client);
			this.Credentials = new Credentials(consumerKey, consumerKeySecret);
		}

		/// <summary>
		/// Twitter コンストラクタ : CK / CKS / AT / ATS / HttpClient 版
		/// </summary>
		/// <param name="consumerKey"></param>
		/// <param name="consumerKeySecret"></param>
		/// <param name="accessToken"></param>
		/// <param name="accessTokenSecret"></param>
		/// <param name="client"></param>
		public Twitter(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret, HttpClient client)
			: base(consumerKey, consumerKeySecret, accessToken, accessTokenSecret)
		{
			this._Client = client;
			this.Auth = new AuthKisaragi(client);
			this.Credentials = new Credentials(consumerKey, consumerKeySecret, accessToken, accessTokenSecret);
		}

		/// <summary>
		/// Twitter コンストラクタ : CK / CKS / AT / ATS / HttpClient 版
		/// </summary>
		/// <param name="consumerKey"></param>
		/// <param name="consumerKeySecret"></param>
		/// <param name="accessToken"></param>
		/// <param name="accessTokenSecret"></param>
		/// <param name="client"></param>
		public Twitter(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret, string userId, string screenName, HttpClient client) 
			: base(consumerKey, consumerKeySecret, accessToken, accessTokenSecret, userId, screenName)
		{
			this._Client = client;
			this.Auth = new AuthKisaragi(client);
			this.Credentials = new Credentials(consumerKey, consumerKeySecret, accessToken, accessTokenSecret, userId, screenName);
		}

		#endregion

		#region Wrapper for AuthKisaragi Method's

		/// <summary>
		/// TODO : Twitter アカウント情報の生成
		/// </summary>
		/// <returns></returns>
		/*
		public async Task<Twitter> Create()
		{
			var account = this.Credentials;
			return new Twitter(account.ConsumerKey ?? string.Empty,
				account.ConsumerKeySecret ?? string.Empty, account.AccessToken ?? string.Empty, account.AccessTokenSecret ?? string.Empty, this._Client);
		}
		*/

		/// <summary>
		/// 認証を実施します。
		/// </summary>
		/// <returns></returns>
		public async Task AuthorizeAsync()
		{
			Debug.WriteLine("------------ 認証シーケンス開始 -----------------");

			await this.Auth.GetRequestTokenAsync(Credentials);

			Uri url = this.Auth.GetAuthorizeUrl(Credentials);
			Process.Start(url.ToString());

			Debug.WriteLine("------------ 認証シーケンス完了 ----------------- >> " + url.ToString());
		}

		/// <summary>
		/// Twitter へ リクエストを投げるための薄いラッパーメソッド
		/// </summary>
		/// <param name="url"></param>
		/// <param name="type"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public Task<string> Request(string url, HttpMethod type, IDictionary<string, string> query)
		{
			return this.Auth.RequestAsync(Credentials.ConsumerKey, Credentials.ConsumerKeySecret,
				Credentials.AccessToken, Credentials.AccessTokenSecret, url, type, query);
		}

		/// <summary>
		/// Access Token の取得を行うためのラッパーメソッド。
		/// </summary>
		/// <param name="PIN"></param>
		/// <returns></returns>
		public async Task GetAccessTokenAsync(string pin) =>
			(this.AccessToken, this.AccessTokenSecret, this.UserId, this.ScreenName) = await this.Auth.GetAccessTokenAsync(Credentials, pin);

		#endregion

	}
}