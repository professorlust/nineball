﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.nineball.data;
using danmaq.nineball.state;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>状態を持つオブジェクトの基底クラス。</para>
	/// <para>
	/// これを継承するか、または<c>IEntity</c>を実装することで、
	/// 状態を持つオブジェクトを作ることができます。
	/// </para>
	/// <para>また、このクラスに直接状態を持たせて使用することもできます。</para>
	/// </summary>
	public class CEntity : IEntity
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>状態が遷移された時に呼び出されるイベント。</summary>
		public event EventHandler<CEventChangedState> changedState;

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>このオブジェクトを所有する親オブジェクト。</summary>
		private IEntity m_owner = null;

		/// <summary>
		/// このオブジェクトを所有する親オブジェクトが
		/// <c>CEntity</c>または<c>CEntity</c>を継承したクラスであるかどうか。
		/// </summary>
		private bool m_bOwnerIsCEntity = false;

		/// <summary>型名のキャッシュ。</summary>
		private string m_strTypeName = null;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CEntity()
		{
			previousState = CState.empty;
			currentState = CState.empty;
			lastStateChangeTime = DateTime.Now;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="entity">親オブジェクト。</param>
		public CEntity(IEntity entity) : this()
		{
			owner = entity;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="state">状態。</param>
		public CEntity(IState state) : this()
		{
			nextState = state;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="entity">親オブジェクト。</param>
		/// <param name="state">状態。</param>
		public CEntity(IEntity entity, IState state)
			: this()
		{
			owner = entity;
			nextState = state;
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最後に変化する前の状態を取得します。</summary>
		/// 
		/// <value>最後に変化する前の状態。初期値は<c>CState.empty</c>。</value>
		public IState previousState
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>現在の状態を取得します。</summary>
		/// 
		/// <value>現在の状態。初期値は<c>CState.empty</c>。</value>
		public IState currentState
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>汎用フレーム カウンタを取得します。</summary>
		/// 
		/// <value>汎用フレーム カウンタ。</value>
		public int counter
		{
			get;
			protected set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>最後に状態が変化した時間を取得します。</summary>
		/// 
		/// <value>最後に状態が変化した時間。</value>
		public DateTime lastStateChangeTime
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトを所有する親オブジェクトを取得します。</summary>
		/// 
		/// <value>親オブジェクト。</value>
		public IEntity owner
		{
			get
			{
				return m_owner;
			}
			protected set
			{
				m_owner = value;
				Type typeExpr = value.GetType();
				Type typeExpect = typeof(CEntity);
				m_bOwnerIsCEntity = value != null &&
					(typeExpr == typeExpect || typeExpr.IsSubclassOf(typeExpect));
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を予約します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		public IState nextState
		{
			private get;
			set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected virtual object privateMembers
		{
			get
			{
				return m_bOwnerIsCEntity ? ((CEntity)owner).privateMembers : null;
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの状態を含めた文字列情報を取得します。</summary>
		/// 
		/// <value>このオブジェクトを示す文字列情報。</value>
		public override string ToString()
		{
			if(m_strTypeName == null)
			{
				m_strTypeName = GetType().ToString();
			}
			return string.Format("{0} STATE[CUR:{1}, PREV:{2}] {3} MEMBERS:{4}",
				m_strTypeName, currentState, previousState,
				Environment.NewLine, privateMembers);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public virtual void initialize()
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトに空の状態を設定します。</summary>
		public virtual void setEmptyState()
		{
			nextState = CState.empty;
		}
	
		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public virtual void Dispose()
		{
			nextState = CState.empty;
			previousState = CState.empty;
			changedState = null;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void update(GameTime gameTime)
		{
			if (nextState != null)
			{
				commitNextState();
			}
			currentState.update(this, privateMembers, gameTime);
			counter++;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void draw(GameTime gameTime)
		{
			currentState.draw(this, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>予約していた次の状態を確定します。</summary>
		private void commitNextState()
		{
			if (!(nextState == null || currentState == nextState))
			{
				IState _nextState = this.nextState;
				nextState = null;
				currentState.teardown(this, privateMembers, _nextState);
				IState oldPrevious = previousState;
				IState oldCurrent = currentState;
				previousState = currentState;
				currentState = _nextState;
				lastStateChangeTime = DateTime.Now;
				currentState.setup(this, privateMembers);
				if (changedState != null)
				{
					changedState(this, new CEventChangedState(oldPrevious, oldCurrent, _nextState));
				}
				if (nextState != null)
				{
					commitNextState();
				}
			}
			nextState = null;
		}
	}
}
