﻿////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2009 danmaq all rights reserved.
//
//		──音量構造体
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using danmaq.Nineball.misc;
using Microsoft.Xna.Framework;

namespace danmaq.Nineball.core.data {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>音量構造体。</summary>
	/// <remarks>
	/// 音量を設定すると自動的に0～2の範囲内に調整され、
	/// またSingle型とほぼ同じ感覚に扱えます。
	/// </remarks>
	[Serializable]
	public struct SVolume {

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>音量。</summary>
		private float m_fVolume;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// <remarks>初期音量が設定できます。</remarks>
		public SVolume( float fVolume ) : this() { volume = fVolume; }

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		/// <summary>音量。</summary>
		/// <remarks>設定すると自動的に0～2の範囲内に調整されます。</remarks>
		public float volume {
			get { return m_fVolume; }
			set { m_fVolume = CMisc.clampLoop( value, 0.0f, 2.0f ); }
		}

		/// <summary>音量(dB単位)。</summary>
		/// <remarks>減速線形補完を使用した非常に雑な変換です。</remarks>
		private float dB {
			// ! TODO : もうちょっとまともな変換方法検討する
			get {
				return ( volume > 1 ?
					CInterpolate.smooth( 0, 6, volume - 1, 1 ) :
					CInterpolate.slowdown( -96, 0, volume, 1 ) );
			}
			set {
				volume = ( value > 0 ?
					CInterpolate.smooth( 1, 2, value, 6 ) :
					CInterpolate.slowdown( 0, 1, value + 96, 96 ) );
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を取得します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量値</returns>
		public static implicit operator float( SVolume v ) { return v.volume; }

		//* -----------------------------------------------------------------------*
		/// <summary>音量値から音量クラスを生成します。</summary>
		/// 
		/// <param name="f">音量値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static implicit operator SVolume( float f ) { return new SVolume( f ); }

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを0.1加算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator ++( SVolume v ) { return v + 0.1f; }

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを0.1減算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator --( SVolume v ) { return v - 0.1f; }

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを指定値加算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <param name="f">加算値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator +( SVolume v, float f ) {
			return new SVolume( ( float )v + f );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量クラス オブジェクトを指定値減算します。</summary>
		/// 
		/// <param name="v">音量クラス オブジェクト</param>
		/// <param name="f">減算値</param>
		/// <returns>音量クラス オブジェクト</returns>
		public static SVolume operator -( SVolume v, float f ) {
			return new SVolume( ( float )v - f );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を文字列化します。</summary>
		/// 
		/// <param name="bSlider">スライダーを挿入するかどうか</param>
		/// <returns>文字列化した音量値</returns>
		public string ToString( bool bSlider ) {
			string strResult = "";
			string strDB = String.Format( "{0:+0.0;-0.0;0}dB", dB );
			if( bSlider ) {
				char[] szVolume = new string( '・', 10 ).ToCharArray();
				szVolume[ ( int )MathHelper.Min( CInterpolate.smooth( 0, 10, volume, 2 ), 9 ) ] = '◆';
				strResult += new string( szVolume ) + Environment.NewLine;
				strDB = string.Format( "({0})", strDB );
			}
			return strResult + strDB;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>音量値を文字列化します。</summary>
		/// <remarks><c>ToString(true)と同等です。</c></remarks>
		/// 
		/// <returns>文字列化した音量値</returns>
		public override string ToString() { return ToString( true ); }
	}
}
