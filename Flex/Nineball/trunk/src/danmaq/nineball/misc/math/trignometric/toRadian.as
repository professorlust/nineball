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
	 * 角度をラジアンに変換します。
	 *
	 * @param fDegree 角度
 	 * @return 角度に対応したラジアン値
	 */
	public function toRadian(fDegree:Number):Number
	{
		return fDegree / 180 * Math.PI;
	}
}