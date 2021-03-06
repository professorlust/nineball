////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

package danmaq.nineball.misc.math.interpolate
{

	/**
	 * 補間ポイントを減速変化で算出します。
	 * 
	 * @param target 現在時間
	 * @param fLimit fEndに到達する時間
	 * @return 0からfLimitまでのtargetに相当する0.0から1.0までの値
	 */
	public function amountSlowdown(target:Number, fLimit:Number):Number
	{
		return 1 - Math.pow(1 - target / fLimit, 2);
	}
}
