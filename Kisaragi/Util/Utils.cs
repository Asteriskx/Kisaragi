using System;

namespace Kisaragi.Util
{
	/// <summary>
	/// 汎用的なイベントハンドラクラス。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Utils<T> : EventArgs
	{
		public Utils(T args) => Args = args;
		public T Args { get; private set; }
	}
}