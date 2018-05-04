namespace Kisaragi.APIs.OAuth
{
	/// <summary>
	/// Kisaragi 時報システム 認証基底クラス
	/// </summary>
	public class Credentials
	{

		#region Constractor 

		// インスタンス生成用
		public Credentials(string consumerKey, string consumerKeySecret)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerKeySecret = consumerKeySecret;
		}

		// Setting File用
		public Credentials(string consumerKey, string consumerKeySecret, string accessToken, string accessTokenSecret)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerKeySecret = consumerKeySecret;
			this.AccessToken = accessToken;
			this.AccessTokenSecret = accessTokenSecret;
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Consumer Key の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string ConsumerKey { get; set; }

		/// <summary>
		/// Consumer Key Secret の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string ConsumerKeySecret { get; set; }

		/// <summary>
		/// Request Token の管理を行います。
		/// </summary>
		public string RequestToken { get; set; }

		/// <summary>
		/// Request Token Secret の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string RequestTokenSecret { get; set; }

		/// <summary>
		/// Access Token の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string AccessToken { get; set; }

		/// <summary>S
		/// Access Token Secret の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string AccessTokenSecret { get; set; }

		/// <summary>
		/// User Id の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Screen Name の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string ScreenName { get; set; }

		/// <summary>
		/// Pin Code の管理を行います。
		///※ 認証キーの保持・参照タイミング的に Mutable Property にする必要あり。
		/// </summary>
		public string PinCode { get; set; }

		#endregion

	}
}
