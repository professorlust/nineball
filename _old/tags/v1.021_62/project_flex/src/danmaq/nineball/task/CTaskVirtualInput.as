package danmaq.nineball.task{

	import danmaq.nineball.core.*;
	import danmaq.nineball.struct.*;
	
	import flash.events.*;
	import flash.geom.Point;

	/**
	 * 仮想ボタン入力を管理するタスクです。
	 * 
	 * @author Mc(danmaq)
	 */
	public final class CTaskVirtualInput implements ITask{

		////////// FIELDS //////////

		/**	レイヤ値が格納されます。 */
		private var m_uLayer:uint = 0;

		/**	仮想ボタン割り当て構造体が格納されます。 */
		private var m_aVIData:Vector.<CVirtualInput>;

		/**	仮想ボタンバッファが格納されます。 */
		private var m_aBuffer:Vector.<CVirtualInputBufferData> =
			new Vector.<CVirtualInputBufferData>();

		/**	イベントリスナに登録されたかどうかが格納されます。 */
		private var m_bAddedEventListener:Boolean = false;

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
		 * 一時停止に対応しているかどうかを取得します。
		 * 
		 * @return 無条件にfalse
		 */
		public function get isAvailablePause():Boolean{ return false; }

		/**
		 * 仮想ボタン入力状態の一覧を取得します。
		 * 
		 * @return 仮想ボタン入力状態の一覧
		 */
		public function get inputTable():Vector.<Boolean>{
			var result:Vector.<Boolean> = new Vector.<Boolean>();
			for each( var data:CVirtualInput in m_aVIData ){ result.push( data.hold ); }
			return result;
		}
		
		////////// METHODS //////////

		/**
		 * コンストラクタ。
		 * 
		 * @param uLayer レイヤ番号
		 */
		public function CTaskVirtualInput( uLayer:uint = 0 ){
			m_uLayer = uLayer;
			resetVI();
		}
		
		/**
		 * コンストラクタの後、タスクが管理クラスに登録された直後に、
		 * 1度だけ自動的に呼ばれます。
		 */
		public function initialize():void{
			if( CScreen.stage == null ){
				throw new Error( "画面オブジェクト管理クラス(danmaq.nineball.struct.CScreen)のルートクラスが画面オブジェクトとして登録されている必要があります。" );
			}
			CScreen.stage.addEventListener( KeyboardEvent.KEY_DOWN, onKeyDown );
			CScreen.stage.addEventListener( KeyboardEvent.KEY_UP, onKeyUp );
			CScreen.stage.addEventListener( MouseEvent.MOUSE_DOWN, onMouseDown );
			CScreen.stage.addEventListener( MouseEvent.MOUSE_UP, onMouseUp );
			m_bAddedEventListener = true;
		}
		
		/**
		 * 解放時に管理クラスから呼び出される処理です。
		 */
		public function dispose():void{
			if( m_bAddedEventListener ){
				CScreen.stage.removeEventListener( KeyboardEvent.KEY_DOWN, onKeyDown );
				CScreen.stage.removeEventListener( KeyboardEvent.KEY_UP, onKeyUp );
				CScreen.stage.removeEventListener( MouseEvent.MOUSE_DOWN, onMouseDown );
				CScreen.stage.removeEventListener( MouseEvent.MOUSE_UP, onMouseUp );
			}
		}
		
		/**
		 * タスクを1フレーム分動かします。
		 * 
		 * @return 無条件にtrue
		 */
		public function update():Boolean{
			var prev:Vector.<Boolean> = inputTable;
			while( m_aBuffer.length > 0 ){
				var data:CVirtualInputBufferData = m_aBuffer.pop();
				prev[ data.id ] = data.push;
			}
			var uLength:uint = m_aVIData.length;
			for( var i:uint = 0; i < uLength; i++ ){ m_aVIData[ i ].update( prev[ i ] ); }
			return true;
		}
		
		/**
		 * 仮想ボタンを初期状態に戻します。
		 */
		public function resetVI():void{ m_aVIData = new Vector.<CVirtualInput>(); }
		
		/**
		 * 仮想ボタンを追加します。
		 * 
		 * @param vkData 仮想ボタン情報構造体
		 * @return 仮想ボタンID
		 */
		public function addVI( viData:CVirtualInput ):uint{
			m_aVIData.push( viData );
			return m_aVIData.length;
		}

		/**
		 * 仮想ボタンを取得します。
		 * 
		 * @param uVIID 仮想ボタンID
		 * @return 仮想ボタン情報構造体
		 */
		public function getVI( uVIID:uint ):CVirtualInput{ return m_aVIData[ uVIID ]; }

		/**
		 * 強制的にキーを押下または解放させます。
		 * 
		 * @param uKeyCode キーコード
		 * @param bPush 押下かどうか
		 */
		public function forceKeyChange( uKeyCode:uint, bPush:Boolean ):void{
			var uLength:uint = m_aVIData.length;
			for( var i:uint = 0; i < uLength; i++ ){
				if( m_aVIData[ i ].findKey( uKeyCode ) ){ forceVIChange( i, bPush ); }
			}
		}

		/**
		 * 強制的に所定位置にマウスボタンを押下または解放させます。
		 * カーソル自体は移動しません。
		 * 
		 * @param pos 座標
		 * @param bPush 押下かどうか
		 */
		public function forceMouseChange( pos:Point, bPush:Boolean ):void{
			var uLength:uint = m_aVIData.length;
			for( var i:uint = 0; i < uLength; i++ ){
				if( m_aVIData[ i ].isHitArea( pos ) ){ forceVIChange( i, bPush ); }
			}
		}

		/**
		 * 強制的に仮想ボタンを押下または解放させます。
		 * 
		 * @param uVIID 仮想ボタンID
		 * @param bPush 押下かどうか
		 */
		public function forceVIChange( uVIID:uint, bPush:Boolean ):void{
			var data:CVirtualInputBufferData = new CVirtualInputBufferData();
			data.id = uVIID;
			data.push = bPush;
			m_aBuffer.push( data );
		}

		/**
		 * マウスボタンを押すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		private function onMouseDown( event:MouseEvent ):void{
			forceMouseChange( new Point( event.stageX, event.stageY ), true ); 
		}

		/**
		 * マウスボタンを離すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		private function onMouseUp( event:MouseEvent ):void{
			forceMouseChange( new Point( event.stageX, event.stageY ), false ); 
		}

		/**
		 * キーを押すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		private function onKeyDown( event:KeyboardEvent ):void{
			forceKeyChange( event.keyCode, true );
		}

		/**
		 * キーを離すイベントが発生した時にコールバックされます。
		 * 
		 * @param event キーボードイベント パラメータ
		 */
		private function onKeyUp( event:KeyboardEvent ):void{
			forceKeyChange( event.keyCode, false );
		}
	}
}

import flash.geom.Point;

/**
 * 入力バッファ構造体です。
 * 
 * @author Mc(danmaq)
 */
class CVirtualInputBufferData{

	/**	仮想ボタンIDが格納されます。 */
	public var id:uint = 0;

	/**	押下されたかどうかが格納されます。 */
	public var push:Boolean = false;
}
