using System;

namespace Kisaragi.Util
{
	/// <summary>
	/// 汎用的なイベントハンドラクラス。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Utils<T> : EventArgs
	{
		public T Args { get; private set; }
		public Utils(T args) => Args = args;
	}
}