////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc.math.trignometric
{

	/**
	 * ラジアンを角度に変換します。
	 *
	 * @param nRadian ラジアン値
	 * @return ラジアン値に対応した角度
	 */
	public function toDegree(fRadian:Number):Number
	{
		return fRadian * 180 / Math.PI;
	}
}