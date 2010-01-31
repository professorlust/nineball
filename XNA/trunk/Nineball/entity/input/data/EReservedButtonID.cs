﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if WINDOWS

using System;
using Microsoft.DirectX.DirectInput;
using Microsoft.Xna.Framework;
using danmaq.nineball.misc;

namespace danmaq.nineball.entity.input.data
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ用の予約されたボタンID。</summary>
	public enum EReservedButtonAxisID : short
	{

		/// <summary>POV(上)。</summary>
		povUp = -1,

		/// <summary>POV(下)。</summary>
		povDown = -2,

		/// <summary>POV(左)。</summary>
		povLeft = -3,

		/// <summary>POV(右)。</summary>
		povRight = -4,

		/// <summary>アナログ(上)。</summary>
		analogUp = -5,

		/// <summary>アナログ(下)。</summary>
		analogDown = -6,

		/// <summary>アナログ(左)。</summary>
		analogLeft = -7,

		/// <summary>アナログ(右)。</summary>
		analogRight = -8,

	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>レガシ ゲーム コントローラ状態の拡張機能。</summary>
	public static class JoystickStateExtension
	{

		//* -----------------------------------------------------------------------*
		/// <summary>入力状態をアナログ値で取得します。</summary>
		/// <remarks>
		/// デジタル値でのみ取得できるボタンを指定した場合、戻り値は0.0か1.0となります。
		/// </remarks>
		/// 
		/// <param name="state">入力状態の格納された構造体。</param>
		/// <param name="sButtonID">ボタンID。</param>
		/// <param name="nRange">アナログ値の取りうる最大値。</param>
		/// <returns>入力されたアナログ値(0.0～1.0)。</returns>
		public static float getInputState(this JoystickState state, short sButtonID, int nRange)
		{
			float fResult = 0f;
			switch(sButtonID)
			{
				case (short)EReservedButtonAxisID.povUp:
				case (short)EReservedButtonAxisID.povDown:
				case (short)EReservedButtonAxisID.povLeft:
				case (short)EReservedButtonAxisID.povRight:
					{
						int pov = state.GetPointOfView()[0];
						if(pov != -1)
						{
							float fRadian = MathHelper.ToRadians(pov * 0.01f) - MathHelper.PiOver2;
							Vector2 vector =
								new Vector2((float)Math.Cos(fRadian), (float)Math.Sin(fRadian));
							switch(sButtonID)
							{
								case (short)EReservedButtonAxisID.povUp:
									fResult = -MathHelper.Min(vector.Y, 0);
									break;
								case (short)EReservedButtonAxisID.povDown:
									fResult = MathHelper.Max(vector.Y, 0);
									break;
								case (short)EReservedButtonAxisID.povLeft:
									fResult = -MathHelper.Min(vector.X, 0);
									break;
								case (short)EReservedButtonAxisID.povRight:
									fResult = MathHelper.Max(vector.X, 0);
									break;
							}
						}
					}
					break;
				case (short)EReservedButtonAxisID.analogUp:
				case (short)EReservedButtonAxisID.analogDown:
				case (short)EReservedButtonAxisID.analogLeft:
				case (short)EReservedButtonAxisID.analogRight:
					{
						int[] sliders = state.GetSlider();
						switch(sButtonID)
						{
							case (short)EReservedButtonAxisID.analogUp:
								fResult = -MathHelper.Min(sliders[1], 0);
								break;
							case (short)EReservedButtonAxisID.analogDown:
								fResult = MathHelper.Max(sliders[1], 0);
								break;
							case (short)EReservedButtonAxisID.analogLeft:
								fResult = -MathHelper.Min(sliders[0], 0);
								break;
							case (short)EReservedButtonAxisID.analogRight:
								fResult = MathHelper.Max(sliders[0], 0);
								break;
						}
						fResult /= (float)nRange;
					}
					break;
				default:
					fResult = (state.GetButtons()[sButtonID] == 0) ? 0f : 1f;
					break;
			}
			return fResult;
		}

	}
}

#endif