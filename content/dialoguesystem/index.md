# Dialogue System

## セットアップ

1. Invector Supportパッケージ2つをインポートする
2. DialogueManagerを設置する
   - InputDeviceManager > ControlCursorStateのチェックを外す
3. プレイヤーにDialogueSystemInvectorBridgeをアタッチする
4. Dialogue Basic UI > NPC > Continue Buttonを画面全体に広げる

## コントローラとマウス操作のだし分け

### Input Device Managerの設定

|項目名|設定内容|
|---|---|
|Joystick Key Codes To Check|中身を削除|
|Key Input Switches Mode To|Mouse|
|Detect Mouse Control|false|
|Control Cursor State|false|
|送信ボタン|Submit|

### Dialogue Managerプレハブへの追加設定

- 以下を含むスクリプトをDialogue Managerプレハブに追加

  ``` csharp
  public void ChangeKey(bool isKey)
  {
      InputDevice device = isKey ? InputDevice.Mouse : InputDevice.Joystick;
      inputDeviceManager.SetInputDevice(device);
  }
  ```

- vChangeInputTypeTrigger.csを追加

  |イベント|設定内容|
  |---|---|
  |On Change To Keyboard|ChangeKey(true)|
  |On Change To Joystick|ChangeKey(false)|

## 会話イベント作成

1. NPCにGenericTrigger・DialogueSystemTriggerをアタッチする
   - OnPressActionInput > DialogueSystemTrigger.OnUse()を設定
   - **Dialogue System Trigger** > **Conversation Actor** と **Conversation Conversant**を設定する
     - Actor…Playerなど
   - DialogueSystemTrigger > Show Cursor During Conversionsをオフにする

## Databaseを分割する

1. シーン内に空オブジェクト作成
2. 1.のオブジェクトにExtraDatabaseをアタッチする

## 文字送り音・クリック音

- NPC > sub title > TypeWriterEffect > 効果音とAudioSourceを設定する
  - Text Per Second = 25辺りに設定しておく
- StandardUIContinueButtonFastForward > OnFastForwardにPlayOneShot追加
- StandardUIResponseButton > OnClickにPlayOneShot追加
