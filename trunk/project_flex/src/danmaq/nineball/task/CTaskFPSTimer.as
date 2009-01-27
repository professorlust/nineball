package danmaq.nineball.task{

	import danmaq.nineball.core.*;
	
	import flash.display.Stage;
	import flash.utils.Timer;

	/**
	 * フレームレート制御タスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskFPSTimer implements ITask{

		////////// CONSTANTS //////////

		/**	フェーズ管理クラスが格納されます。 */
		private const phaseManager:CPhaseManager = new CPhaseManager();

		////////// FIELDS //////////

		/**	レイヤ番号が格納されます。 */
		private var m_uLayer:uint;

		/**	FPS更新フレーム間隔が格納されます。 */
		private var m_uRefleshInterval:uint;
		
		/**	タイマーに使用するFPSが格納されます。 */
		private var m_uTimerValue:uint;
		
		/**	実測内部FPSの最低許容値が格納されます。 */
		private var m_uSlowdownLimit:uint;
		
		/**	実測内部FPSの最低許容値を下回る許容回数が格納されます。 */
		private var m_uSlowdownCountLimit:uint;

		/**	内部FPS理論値が格納されます。 */
		private var m_uTheoretical:uint;

		/**	前回計測時の時間(秒)が格納されます。 */
		private var m_nPrevSeconds:int = 0;

		/**	前回計測時の時間(秒)が格納されます。 */
		private var m_uReal:uint = 0;
		
		/**	低FPSが記録された回数が格納されます。 */
		private var m_uSlowdownCount:uint = 0;

		/**	低FPSにより描画FPSペナルティを加えた回数が格納されます。 */
		private var m_uPenaltyCount:uint = 0;
		
		/**	メイン描画領域オブジェクトが格納されます。 */
		private var m_stage:Stage = null;

		////////// PROPERTIES //////////

		/**
		 * レイヤ値を取得します。
		 * 
		 * @return レイヤ値
		 */
		public function get layer():uint{ return m_uLayer; }
		
		/**
		 * タスク管理クラスを設定します。
		 * このクラスでは特に必要ないので何も設定しません。
		 * 
		 * @param value タスク管理クラス
		 */
		public function set manager( value:CTaskManager ):void{}

		/**
		 * FPS更新フレーム間隔を取得します。
		 * 
		 * @return FPS更新フレーム間隔
		 */
		public function get refleshInterval():uint{ return m_uRefleshInterval; }

		/**
		 * ループ用タイマを取得します。
		 * 
		 * @return タイマ
		 */
		public function get timer():Timer{
			return new Timer( 1000 / m_uTimerValue, refleshInterval );
		}
		
		/**
		 * 実測FPSを取得します。
		 * 
		 * @return 実測FPS
		 */
		public function get realFPS():uint{ return m_uReal; }

		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param stage メイン描画領域オブジェクト
		 * @param uLayer レイヤ番号
		 * @param uReflesh FPS可変更新フレーム間隔
		 * @param uTheoretical FPS理論値
		 * @param uLowLimit 実測FPS最低理論値
		 * @param uLowCount 実測FPSの最低許容値を下回る許容回数
		 */
		public function CTaskFPSTimer(
			stage:Stage, uLayer:uint = 0, uReflesh:uint = 0, uTheoretical:uint = 60,
			uSlowdownLimit:uint = 0, uSlowdownCount:uint = 0
		){
			m_stage = stage;
			m_uLayer = uLayer;
			m_uRefleshInterval = uReflesh;
			m_uTheoretical = uTheoretical;
			m_uTimerValue = uTheoretical;
			m_uReal = uTheoretical;
			m_uSlowdownLimit = uSlowdownLimit;
			m_uSlowdownCountLimit = uSlowdownCount;
		}

		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{}
		
		/**
		 * デストラクタ。
		 */
		public function dispose():void{}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件でtrue
		 */
		public function update():Boolean{
			var bFirst:Boolean = ( phaseManager.count == 0 ); 
			var nSeconds:int = new Date().seconds;
			if( m_nPrevSeconds != nSeconds ){
				m_nPrevSeconds = nSeconds;
				m_uReal = phaseManager.phaseCount;
				if( m_uReal < m_uSlowdownLimit ){
					m_uSlowdownCount++;	// あんまり重いようだと描画FPSを半分にする
					if( m_uSlowdownCount == m_uSlowdownCountLimit ){
						m_uPenaltyCount++;
						if( CMainLoop.instance.screenParent.screen.stage != null ){
							CMainLoop.instance.screenParent.screen.stage.frameRate =
								Math.min( 60, m_uTheoretical ) / ( m_uPenaltyCount + 1 );
						}
					}
				}
				if( bFirst ){
					if( CMainLoop.instance.screenParent.screen.stage != null ){
						CMainLoop.instance.screenParent.screen.stage.frameRate =
							Math.min( 60, m_uTheoretical );
					}
				}
				else{
					m_uTimerValue = Math.max( 1,
						m_uTimerValue + int( ( m_uTheoretical - int( m_uReal ) ) * 0.8 ) );
				}
				phaseManager.isReserveNextPhase = true;
			}
			phaseManager.count++;
			return true;
		}
		
		/**
		 * FPS補正をリセットします。
		 * 急激な負荷の変化が予想される時に実行してください。
		 * 注意：あまり頻繁に呼び出すと補正の効果が薄れます。
		 */
		public function resetCalibration():void{ m_uTimerValue = m_uTheoretical; }
	}
}
