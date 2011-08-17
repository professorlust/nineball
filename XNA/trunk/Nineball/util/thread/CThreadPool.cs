﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

namespace danmaq.nineball.util.thread
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>スレッド プール クラスです。</summary>
	/// <remarks>
	/// 基本的にはThreadPoolクラスの車輪の再発明(縮小版)です。
	/// XBOX360で使うとどうもスレッドが毎回落ちるようなので、独自に作りました。
	/// </remarks>
	public static class CThreadPool
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>スレッド情報。</summary>
		private class CThreadInfo
			: IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>排他制御のために使用するオブジェクト。</summary>
			private readonly object syncLock = new object();

			/// <summary>スレッド。</summary>
			public readonly Thread thread;

			//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* fields ────────────────────────────────*

			/// <summary>スレッドの停止を予約するかどうか。</summary>
			private bool m_terminate = false;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			public CThreadInfo()
			{
				ThreadStart method = () =>
				{
					while (true)
					{
						lock (syncLock)
						{
							if (m_terminate)
							{
								break;
							}
						}
						KeyValuePair<WaitCallback, object> info = CThreadPool.pop();
						if (info.Key != null)
						{
							info.Key(info.Value);
						}
						Thread.Sleep(10);
					}
				};
				thread = new Thread(method);
				thread.Name = "danmaq.nineball.util.thread.CThreadPool";
				thread.Start();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>スレッドをキリのよいところで停止させます。</summary>
			public void Dispose()
			{
				lock (syncLock)
				{
					m_terminate = true;
				}
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>排他制御のために使用するオブジェクト。</summary>
		private static readonly object syncLock = new object();

		/// <summary>実行キュー。</summary>
		private static readonly List<KeyValuePair<WaitCallback, object>> queue =
			new List<KeyValuePair<WaitCallback, object>>();

		/// <summary>スレッド一覧。</summary>
		private static readonly List<CThreadInfo> threads = new List<CThreadInfo>();

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>スレッドの数を取得/設定します。</summary>
		/// <remarks>設定された数に応じて、スレッドの本数が増減します。</remarks>
		/// 
		/// <value>スレッドの数。</value>
		public static int count
		{
			get
			{
				int result;
				lock (syncLock)
				{
					result = threads.Count;
				}
				return result;
			}
			set
			{
				lock (syncLock)
				{
					int gap = value - threads.Count;
					while (gap > 0)
					{
						threads.Add(new CThreadInfo());
						gap--;	// gap-- > 0とかやるとまずいのでここで
					}
					while (gap < 0)
					{
						// TODO : Deadlockする。↑のsyncLockとterm後の最後のpopで死ぬ
						threads[0].Dispose();
						threads.RemoveAt(0);
						gap++;	// gap++ < 0とかやるとまずいのでここで
					}
				}
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを追加します。</summary>
		/// 
		/// <param name="callback">タスク。</param>
		/// <param name="state">タスクに渡す値。</param>
		/// <returns><c>true</c>。</returns>
		public static bool pushTask(WaitCallback callback, object state)
		{
			lock (syncLock)
			{
				queue.Add(new KeyValuePair<WaitCallback, object>(callback, state));
			}
			return true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスクを取得します。</summary>
		/// 
		/// <returns>タスク。</returns>
		private static KeyValuePair<WaitCallback, object> pop()
		{
			KeyValuePair<WaitCallback, object> result =
				new KeyValuePair<WaitCallback,object>(null, null);
			lock (syncLock)
			{
				if (queue.Count > 0)
				{
					result = queue[0];
					queue.RemoveAt(0);
				}
			}
			return result;
		}
	}
}
