﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2012 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

namespace Danmaq.Nineball.Core.State
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// Stateパターンにおけるコンテクストを表現するためのインターフェイス。
	/// </summary>
	public interface IContext
	{

		//* instance properties ─────────────────────────-*

		/// <summary>現在の状態を取得します。</summary>
		IState CurrentState
		{
			get;
		}

		/// <summary>現在の状態の以前に設定されていた状態を取得します。</summary>
		IState PreviousState
		{
			get;
		}

		/// <summary>次に遷移すべき状態を取得、及び設定します。</summary>
		IState NextState
		{
			get;
			set;
		}

		//* instance methods ───────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>状態を実行します。</summary>
		void Execute();
	}
}
