////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.constant
{

	/**
	 * 文章定数定義クラスです。
	 * 
	 * <p>
	 * danmaq Nineball-Library内部で自動的に使用します。
	 * 通常ユーザが直接使用する必要はありません。
	 * </p>
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CSentence
	{

		////////// CONSTANTS //////////
		
		/**	文章：引数は1文字でなければなりません。 */
		public static const ARGS_NOT_CHAR:String = "引数は1文字でなければなりません。";
		
		/**	文章：引数{0}にnullを設定出来ません。 */
		public static const NOT_NULL:String = "引数{0}にnullを設定出来ません。";
		
		/**	文章：指定の文字は割り当てられていません： */
		public static const ILLEGAL_CHARACTER:String = "指定の文字は割り当てられていません：";
		
		/**	文章：画面オブジェクト管理クラス(danmaq.nineball.struct.CScreen)のルートクラスが画面オブジェクトとして登録されている必要があります。 */
		public static const ILLEGAL_SCREEN:String = "画面オブジェクト管理クラス(danmaq.nineball.struct.CScreen)のルートクラスが画面オブジェクトとして登録されている必要があります。";
		
		/**	文章：画面管理クラスはDisplayObjectContainerのサブクラス、またはCScreenクラスのインスタンスで無ければなりません。 */
		public static const ARGS_NOT_SCREEN:String = "画面管理クラスはDisplayObjectContainerのサブクラス、またはCScreenクラスのインスタンスで無ければなりません。";
		
		/**	文章：表示すべき画像が準備されていません。 */
		public static const ARGS_NOT_IMAGE:String = "表示すべき画像が準備されていません。";
	}
}
