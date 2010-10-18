﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using danmaq.nineball.state;
using danmaq.nineball.state.manager.taskmgr;

namespace danmaq.nineball.entity.manager
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>タスク管理クラス。</para>
	/// <para>
	/// タスクの追加削除が少なく、総数も少ない場合は<c>CGameComponentManager</c>が
	/// 便利ですが、追加削除のオーバーヘッドが大きいため、
	/// それを回避したい場合はこのクラスを使うとよいでしょう。
	/// </para>
	/// </summary>
	public sealed class CTaskManager
		: CEntity, ICollection<ITask>
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		public sealed class CPrivateMembers : IDisposable
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>登録されているタスク一覧。</summary>
			public readonly List<ITask> tasks = new List<ITask>();

			/// <summary>追加予約されているタスク一覧。</summary>
			public readonly List<ITask> add = new List<ITask>();

			/// <summary>削除予約されているタスク一覧。</summary>
			public readonly List<ITask> remove = new List<ITask>();

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>フィールドのオブジェクトを解放します。</summary>
			public void Dispose()
			{
				tasks.Clear();
				add.Clear();
				remove.Clear();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>タスク管理に使用したメモリを切り詰めます。</summary>
			public void TrimExcess()
			{
				tasks.TrimExcess();
				add.TrimExcess();
				remove.TrimExcess();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>登録されているタスクを全削除します。</summary>
			/// 
			/// <param name="callback">削除時に呼ばれるコールバック。</param>
			public void Clear(Action<ITask> callback)
			{
				// TODO : ちょっとお行儀悪いかな？
				tasks.ForEach(callback);
				tasks.ForEach(task => task.Dispose());
				tasks.Clear();
				remove.Clear();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>タスク追加・削除の予約を確定します。</summary>
			public void commit()
			{
				remove.ForEach(commitRemove);
				tasks.AddRange(add);
				remove.Clear();
				add.Clear();
			}

			//* -----------------------------------------------------------------------*
			/// <summary>タスクを即時削除します。</summary>
			/// 
			/// <param name="task">削除対象のタスク</param>
			private void commitRemove(ITask task)
			{
				tasks.Remove(task);
				task.Dispose();
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>オブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly CPrivateMembers _private;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>全削除フラグ。</summary>
		private bool m_allClear = false;

		/// <summary>全削除時に呼ばれるコールバック。</summary>
		private Action<ITask> m_callBackOnClear = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CTaskManager()
			: this(CStateDefault.instance)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="firstState">初期の状態。</param>
		public CTaskManager(IState firstState)
			: base(firstState, new CPrivateMembers())
		{
			_private = (CPrivateMembers)privateMembers;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>登録されているタスクの総数を取得します。</summary>
		/// 
		/// <value>登録されているタスクの総数。</value>
		public int Count
		{
			get
			{
				return _private.tasks.Count;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>この管理クラスが読み取り専用かどうかを取得します。</summary>
		/// 
		/// <value>この管理クラスが読み取り専用である場合、<c>true</c>。</value>
		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<ITask>)_private.tasks).IsReadOnly;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public override void Dispose()
		{
			_private.Dispose();
			base.Dispose();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク管理に使用したメモリを切り詰めます。</summary>
		public void TrimExcess()
		{
			_private.TrimExcess();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク追加・削除の予約を確定します。</summary>
		public void commit()
		{
			if (m_allClear)
			{
				m_allClear = false;
				_private.Clear(m_callBackOnClear);
			}
			else
			{
				_private.commit();
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク追加の予約をします。</summary>
		/// 
		/// <param name="item">タスク。</param>
		public void Add(ITask item)
		{
			_private.add.Add(item);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク削除の予約をします。</summary>
		/// 
		/// <param name="task">タスク。</param>
		/// <returns><c>true</c>。</returns>
		public bool Remove(ITask task)
		{
			_private.remove.Add(task);
			return true;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>タスク削除の予約をします。</summary>
		/// 
		/// <param name="match">削除条件。</param>
		/// <returns>検出されたタスク一覧。</returns>
		public List<ITask> Remove(Predicate<ITask> match)
		{
			List<ITask> result = _private.tasks.FindAll(match);
			_private.remove.AddRange(result);
			return result;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理しているタスクを全て解放するための予約を入れます。</summary>
		public void Clear()
		{
			Clear(null);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>管理しているタスクを全て解放するための予約を入れます。</summary>
		/// 
		/// <param name="callback">全削除時に実行されるコールバック</param>
		public void Clear(Action<ITask> callback)
		{
			m_allClear = true;
			m_callBackOnClear = callback;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>特定の値が格納されているかどうかを判断します。</summary>
		/// 
		/// <param name="item">検索するオブジェクト。</param>
		/// <returns>存在する場合、<c>true</c>。</returns>
		public bool Contains(ITask item)
		{
			return _private.tasks.Contains(item);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>要素を配列にコピーします。</summary>
		/// 
		/// <param name="array">
		/// 要素がコピーされる1次元かつ0から始まるインデックス番号の配列。
		/// </param>
		/// <param name="arrayIndex">
		/// コピーの開始位置となる、配列の0から始まるインデックス番号。
		/// </param>
		public void CopyTo(ITask[] array, int arrayIndex)
		{
			_private.tasks.CopyTo(array, arrayIndex);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 指定した型のコレクションに対する単純な反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		public IEnumerator<ITask> GetEnumerator()
		{
			return _private.tasks.GetEnumerator();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 非ジェネリック コレクションに対する反復処理をサポートする列挙子を公開します。
		/// </summary>
		/// 
		/// <returns>列挙するオブジェクトの型。</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_private.tasks).GetEnumerator();
		}
	}
}