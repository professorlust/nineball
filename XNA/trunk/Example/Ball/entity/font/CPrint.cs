﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.core;
using danmaq.ball.Properties;
using danmaq.ball.state.font;
using danmaq.nineball.data;
using danmaq.nineball.entity.fonts;
using danmaq.nineball.state.manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.ball.entity.font
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>よゆ風固定ピッチフォント クラス。</summary>
	public sealed class CPrint : CFont
	{

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strText">テキスト。</param>
		/// <param name="pos">描画開始カーソル位置。</param>
		public CPrint(string strText, Vector2 pos)
			: this(strText, pos, EAlign.LeftTop)
		{
		}

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="strText">テキスト。</param>
		/// <param name="pos">基準カーソル位置。</param>
		/// <param name="hAlign">水平位置揃え情報。</param>
		public CPrint(string strText, Vector2 pos, EAlign hAlign)
			: base(strText)
		{
			nextState = CState98.instance;
			font = CGame.instance.Content.Load<SpriteFont>(Resources.FONT_98);
			sprite = CStateMainLoopDefault.instance.sprite;
			this.pos = pos;
			alignHorizontal = hAlign;
		}
	}
}
