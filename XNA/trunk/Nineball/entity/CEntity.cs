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

namespace danmaq.nineball.entity {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>状態を持つオブジェクトの基底クラス。</para>
	/// <para>
	/// これを継承するか、または<c>IEntity</c>を実装することで、
	/// 状態を持つオブジェクトを作ることができます。
	/// </para>
	/// <para>また、このクラスに直接状態を持たせて使用することもできます。</para>
	/// </summary>
	public class CEntity : IEntity {

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>ダミーのオブジェクトと状態クラスのみがアクセス可能なフィールド。</summary>
		private readonly object dummyPrivateMembers = new object();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* events ────────────────────────────────*

		/// <summary>状態が遷移された時に呼び出されるイベント。</summary>
		public event EventHandler<CEventChangedState> changedState;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>既定の状態で初期化します。</para>
		/// </summary>
		public CEntity() {
			previousState = CState.empty;
			currentState = CState.empty;
			name = ToString();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>コンストラクタ。</para>
		/// <para>指定の状態で初期化します。</para>
		/// </summary>
		/// 
		/// <param name="state">状態。</param>
		public CEntity( IState state ) : this() { nextState = state; }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>最後に変化する前の状態を取得します。</summary>
		/// 
		/// <value>最後に変化する前の状態。初期値は<c>CState.empty</c>。</value>
		public IState previousState { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>現在の状態を取得します。</summary>
		/// 
		/// <value>現在の状態。初期値は<c>CState.empty</c>。</value>
		public IState currentState { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの名前を取得します。</summary>
		/// 
		/// <value>このオブジェクトの名前。</value>
		public virtual string name { get; private set; }

		//* -----------------------------------------------------------------------*
		/// <summary>次に変化する状態を設定します。</summary>
		/// 
		/// <value>次に変化する状態。</value>
		/// <exception cref="System.ArgumentNullException">
		/// <para>状態として、nullを設定しようとした場合。</para>
		/// <para>
		/// 何もしない状態を設定したい場合、<c>CState.empty</c>を使用します。
		/// </para>
		/// </exception>
		public IState nextState {
			set {
				if( value == null ) { throw new ArgumentNullException( "value" ); }
				currentState.teardown( this, privateMembers, value );
				IState oldPrevious = previousState;
				IState oldCurrent = currentState;
				previousState = currentState;
				currentState = value;
				currentState.setup( this, privateMembers );
				if( changedState != null ) {
					changedState( this,
						new CEventChangedState( oldPrevious, oldCurrent, value ) );
				}
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// オブジェクトと状態クラスのみがアクセス可能なフィールドを取得します。
		/// </summary>
		/// 
		/// <value>オブジェクトと状態クラスのみがアクセス可能なフィールド。</value>
		protected virtual object privateMembers {
			get { return dummyPrivateMembers; }
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの状態を含めた文字列情報を取得します。</summary>
		/// 
		/// <value>このオブジェクトを示す文字列情報。</value>
		public override string ToString() {
			return string.Format( "{0} STATE[CUR:{1}, PREV:{2}] {3} MEMBERS:{4}",
				name, currentState, previousState, Environment.NewLine, privateMembers );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>初期化処理を実行します。</summary>
		public virtual void initialize() { }

		//* -----------------------------------------------------------------------*
		/// <summary>このオブジェクトの終了処理を行います。</summary>
		public virtual void Dispose() {
			nextState = CState.empty;
			previousState = CState.empty;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void update( GameTime gameTime ) {
			currentState.update( this, privateMembers, gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public virtual void draw( GameTime gameTime ) {
			currentState.draw( this, privateMembers, gameTime );
		}
	}
}