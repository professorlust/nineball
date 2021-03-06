﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.nineball.entity.fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.fonts
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>通常のフォント状態。</summary>
	public sealed class CStateDefault : CState<CFont, object>
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
		/// <summary>
		/// 登録されているスプライトフォントを基準に原点を算出します。
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <returns>原点座標</returns>
		public static Vector2 getOrigin(CFont entity)
		{
			return entity.getOrigin(entity.font.MeasureString(entity.text) * entity.scale);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1フレーム分の描画処理を実行します。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void draw(CFont entity, object privateMembers, GameTime gameTime)
		{
			if(entity.sprite != null && entity.font != null && entity.text.Length > 0)
			{
				Vector2 origin = getOrigin(entity);
				float fLayer;
				float fShadowLayer;
				entity.getShadowLayer(out fLayer, out fShadowLayer);
				if(entity.isDrawShadow)
				{
					entity.sprite.add(entity.font, entity.text,
						entity.pos - origin + entity.gapShadow,
						new Color(Color.Black, (byte)(entity.colorAlpha / 1.5f)),
						0.0f, Vector2.Zero, entity.scale, SpriteEffects.None, fShadowLayer);
				}
				entity.sprite.add(entity.font, entity.text, entity.pos - origin,
					new Color(
						(byte)entity.colorRed, (byte)entity.colorGreen,
						(byte)entity.colorBlue, (byte)entity.colorAlpha),
					0.0f, Vector2.Zero, entity.scale, SpriteEffects.None, fLayer);
			}
			base.draw(entity, privateMembers, gameTime);
		}
	}
}
