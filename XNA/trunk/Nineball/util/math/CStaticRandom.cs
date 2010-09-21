﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;

namespace danmaq.nineball.util.math
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>静的な擬似乱数ジェネレータ。</summary>
	public static class CStaticRandom
	{

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>擬似乱数系列の開始値を計算するために使用する数値。</summary>
		private static int m_nSeed;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>静的なコンストラクタ。</summary>
		/// <remarks>
		/// 既定のシード値を生成するために、乱数オブジェクトを1個余分に生成します。
		/// </remarks>
		static CStaticRandom()
		{
			reset();
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>擬似乱数ジェネレータを取得します。</summary>
		/// 
		/// <returns>擬似乱数ジェネレータ。</returns>
		public static Random random
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// 擬似乱数系列の開始値を計算するために使用する数値を取得します。
		/// </summary>
		/// 
		/// <returns>擬似乱数系列の開始値を計算するために使用する数値。</returns>
		public static int seed
		{
			get
			{
				return m_nSeed;
			}
			set
			{
				reset(value);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>既定のシード値を使用し、乱数ジェネレータをリセットします。</summary>
		/// 
		/// <returns>擬似乱数系列の開始値を計算するために使用する数値。</returns>
		public static int reset()
		{
			reset((random ?? new Random()).Next());
			return seed;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>指定のシード値を使用し、乱数ジェネレータをリセットします。</summary>
		/// 
		/// <param name="nSeed">
		/// <para>擬似乱数系列の開始値を計算するために使用する数値。</para>
		/// <para>負数を指定した場合、その数値の絶対値が使用されます。</para>
		/// </param>
		/// <exception cref="System.OverflowException">
		/// <para><c>Seed</c>>が<c>Int32.MinValue</c>です。</para>
		/// <para>これは、絶対値が計算されるときにオーバーフローの原因となります。</para>
		/// </exception>
		public static void reset(int nSeed)
		{
			m_nSeed = nSeed;
			random = new Random(nSeed);
		}
	}
}