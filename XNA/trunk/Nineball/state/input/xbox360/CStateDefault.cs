﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using danmaq.nineball.entity.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace danmaq.nineball.state.input.xbox360
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ゲーム コントローラ既定の入力状態。</summary>
	public sealed class CStateDefault : CState<CInputXBOX360, List<SInputState>>
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateDefault instance = new CStateDefault();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateDefault()
		{
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の更新処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="buttonsState">ボタン押下情報一覧。</param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(
			CInputXBOX360 entity, List<SInputState> buttonsState, GameTime gameTime
		)
		{
			GamePadState state = GamePad.GetState(entity.playerIndex);
			if(state.IsConnected)
			{
				IList<Buttons> assignList = entity.assignList;
				for(int i = assignList.Count - 1; i >= 0; i--)
				{
					buttonsState[i].refresh(state.IsButtonDown(assignList[i]));
				}
			}
			else
			{
				entity.Dispose();
			}
			base.update(entity, buttonsState, gameTime);
		}
	}
}