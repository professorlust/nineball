////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc
{

	import flash.geom.Point;

	/**
	 * ベクトルを作成します。
	 * 
	 * @param fDeg 角度(度)
	 * @param fSpeed 速度
	 * @return ベクトル
	 */
	public function createVector(fDeg:Number, fSpeed:Number):Point
	{
		// TODO : そのうち行列に移行する。
		var fRad:Number = Math.PI * fDeg / 180;
		return new Point(
			Math.sin(fRad) * fSpeed, -Math.cos(fRad) * fSpeed);
	}
}
