﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.low
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲームパッド低位入力制御・管理クラスの既定の状態。</summary>
	public sealed class CStateGamePadInput
		: CStateXNAInput<GamePadState>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>XBOX360ゲームパッド用クラス オブジェクト。</summary>
		public static readonly CStateGamePadInput player1 =
			new CStateGamePadInput(PlayerIndex.One);

		/// <summary>XBOX360ゲームパッド用クラス オブジェクト。</summary>
		public static readonly CStateGamePadInput player2 =
			new CStateGamePadInput(PlayerIndex.Two);

		/// <summary>XBOX360ゲームパッド用クラス オブジェクト。</summary>
		public static readonly CStateGamePadInput player3 =
			new CStateGamePadInput(PlayerIndex.Three);

		/// <summary>XBOX360ゲームパッド用クラス オブジェクト。</summary>
		public static readonly CStateGamePadInput player4 =
			new CStateGamePadInput(PlayerIndex.Four);

		/// <summary>XBOX360ゲームパッド用クラス オブジェクトの一覧。</summary>
		public static ReadOnlyCollection<CStateGamePadInput> instanceList;

		/// <summary>割り当てられたプレイヤー番号。</summary>
		public readonly PlayerIndex playerIndex;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		static CStateGamePadInput()
		{
			CStateGamePadInput[] array = new CStateGamePadInput[4];
			array[(int)PlayerIndex.One] = player1;
			array[(int)PlayerIndex.Two] = player2;
			array[(int)PlayerIndex.Three] = player3;
			array[(int)PlayerIndex.Four] = player4;
			instanceList = new List<CStateGamePadInput>(array).AsReadOnly();
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="playerIndex">割り当てられたプレイヤー番号。</param>
		private CStateGamePadInput(PlayerIndex playerIndex)
			: base(() => GamePad.GetState(playerIndex, GamePadDeadZone.None))
		{
			this.playerIndex = playerIndex;
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>プレイヤー番号に該当する状態を取得します。</summary>
		/// 
		/// <param name="playerIndex">割り当てられたプレイヤー番号。</param>
		/// <returns>XBOX360ゲームパッド低位入力制御・管理クラスの状態。</returns>
		public static CStateGamePadInput getInstance(PlayerIndex playerIndex)
		{
			return instanceList[(int)playerIndex];
		}
	}
}
