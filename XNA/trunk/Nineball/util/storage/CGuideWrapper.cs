﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace danmaq.nineball.util.storage
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>ガイド ユーザー インターフェイスのラッパー。</para>
	/// <para>
	/// ガイド使用の可不可を自動判断し、使用不可の場合はWindowsの機能で代用します。
	/// </para>
	/// </summary>
	/// <remarks>
	/// 当分の間はWindowsの機能で代用可能なダイアログ機能、ストレージ選択機能のみ
	/// サポートします。それ以外の機能は直接Guideを呼び出してください。
	/// </remarks>
	public sealed class CGuideWrapper
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メソッドの進行状況の追跡に使用されるクラス。</summary>
		/// <remarks>
		/// このクラスを使用する場合、処理は全て同期的に行われるため、
		/// 実質的にダミーのオブジェクトです。
		/// </remarks>
		private sealed class CNullAsyncResult
			: IAsyncResult
		{

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="state">
			/// この要求を一意に識別するユーザー作成オブジェクト。
			/// </param>
			public CNullAsyncResult(object state)
			{
				AsyncState = state;
			}

			//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* properties ──────────────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作についての情報を限定または格納するユーザー定義の
			/// オブジェクトを取得します。
			/// </summary>
			/// 
			/// <value>
			/// 非同期操作についての情報を限定または格納するユーザー定義のオブジェクト。
			/// </value>
			public object AsyncState
			{
				get;
				private set;
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作が完了するまで待機するために使用するオブジェクトを取得します。
			/// </summary>
			/// 
			/// <value>
			/// <c>null</c>。このオブジェクトが返される場合、非同期処理は行われません。
			/// </value>
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return null;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>
			/// 非同期操作が同期的に完了したかどうかを示す値を取得します。
			/// </summary>
			/// 
			/// <value>
			/// <c>true</c>。このオブジェクトが返される場合、非同期処理は行われません。
			/// </value>
			public bool CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			//* -----------------------------------------------------------------------*
			/// <summary>非同期操作が完了したかどうかを示す値を取得します。</summary>
			/// 
			/// <value>
			/// <c>true</c>。このオブジェクトが返された時には、既に処理を完了しています。
			/// </value>
			public bool IsCompleted
			{
				get
				{
					return true;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ゲーマー サービスが使用可能かどうか。</summary>
		public readonly bool isAvaliableUseGamerService = true;

		/// <summary>ボタン一覧。</summary>
		private readonly string[] buttons = { "OK" };

#if WINDOWS
		/// <summary>
		/// ガイド ユーザー インターフェイスに対応するWindowsアイコン一覧。
		/// </summary>
		private readonly System.Windows.Forms.MessageBoxIcon[] iconset =
			new System.Windows.Forms.MessageBoxIcon[4];
#endif

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		public CGuideWrapper(Game game)
		{
			if (instance != null)
			{
				throw new InvalidOperationException(
					string.Format(Resources.ERR_SINGLETON, typeof(CGuideWrapper).FullName));
			}
			instance = this;
			try
			{
				game.Components.Add(new GamerServicesComponent(game));
			}
			catch (Exception e)
			{
				CLogger.add(Resources.WARN_GAMER_SERVICE);
				CLogger.add(e);
				isAvaliableUseGamerService = false;
			}
#if WINDOWS
			iconset[(int)MessageBoxIcon.None] = System.Windows.Forms.MessageBoxIcon.None;
			iconset[(int)MessageBoxIcon.Error] = System.Windows.Forms.MessageBoxIcon.Exclamation;
			iconset[(int)MessageBoxIcon.Warning] = System.Windows.Forms.MessageBoxIcon.Information;
			iconset[(int)MessageBoxIcon.Alert] = System.Windows.Forms.MessageBoxIcon.Stop;
#endif
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>クラス オブジェクトを取得します。</summary>
		/// 
		/// <value>クラス オブジェクト。</value>
		public static CGuideWrapper instance
		{
			get;
			private set;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスの表示を開始します。</summary>
		/// 
		/// <param name="title">メッセージのタイトル。</param>
		/// <param name="text">メッセージ ボックスに表示されるテキスト。</param>
		/// <param name="icon">メッセージ ボックスに表示されるアイコンの種類。</param>
		/// <param name="callback">
		/// 非同期操作が終了すると呼び出されるメソッド。
		/// </param>
		/// <param name="state">
		/// この要求を一意に識別するユーザー作成オブジェクト。
		/// </param>
		/// <returns>メソッドの進行状況の追跡に使用されるオブジェクト。</returns>
		public IAsyncResult BeginShowMessageBox(string title, string text,
			MessageBoxIcon icon, AsyncCallback callback, object state)
		{
			IAsyncResult result = null;
			if (isAvaliableUseGamerService)
			{
				result = Guide.BeginShowMessageBox(
					title, text, buttons, 0, icon, callback, state);
			}
			else
			{
				result = new CNullAsyncResult(state);
#if WINDOWS
				System.Windows.Forms.MessageBox.Show(
					text, title, System.Windows.Forms.MessageBoxButtons.OK, iconset[(int)icon]);
#endif
				callback(result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックスの表示を終了します。</summary>
		/// 
		/// <param name="result">
		/// メソッドの進行状況の追跡に使用されるオブジェクト。
		/// </param>
		public int? EndShowMessageBox(IAsyncResult result)
		{
			int? res = null;
			if (isAvaliableUseGamerService)
			{
				res = Guide.EndShowMessageBox(result);
			}
			return res;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ストレージ セレクター ユーザー インターフェイスの表示を開始します。
		/// </summary>
		/// 
		/// <param name="callback">
		/// 非同期操作が終了すると呼び出されるメソッド。
		/// </param>
		/// <param name="state">
		/// この要求を一意に識別するユーザー作成オブジェクト。
		/// </param>
		/// <returns>メソッドの進行状況の追跡に使用されるオブジェクト。</returns>
		public IAsyncResult BeginShowStorageDeviceSelector(AsyncCallback callback, Object state)
		{
			IAsyncResult result = null;
			if (isAvaliableUseGamerService)
			{
				result = Guide.BeginShowStorageDeviceSelector(callback, state);
			}
			else
			{
				result = new CNullAsyncResult(state);
				callback(result);
			}
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// ストレージ セレクター ユーザー インターフェイスの表示を終了します。
		/// </summary>
		/// 
		/// <param name="result">
		/// メソッドの進行状況の追跡に使用されるオブジェクト。
		/// </param>
		public StorageDevice EndShowStorageDeviceSelector(IAsyncResult result)
		{
			StorageDevice res = null;
			if (isAvaliableUseGamerService)
			{
				res = Guide.EndShowStorageDeviceSelector(result);
			}
			return res;
		}
	}
}
