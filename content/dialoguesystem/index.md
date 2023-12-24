# Dialogue System

## セットアップ

1. Invector Supportパッケージ2つをインポートする
2. DialogueManagerを設置する
   - InputDeviceManager > ControlCursorStateのチェックを外す
3. プレイヤーにDialogueSystemInvectorBridgeをアタッチする
4. Dialogue Basic UI > NPC > Continue Buttonを画面全体に広げる

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

## QuestMachine連携

1. プレイヤー > Dialogue System Bridgeのすべての項目がオンになっていることを確認する
2. Pixel Crushers > Quest Machine > Third Party > Dialogue System > Quest DB To Dialogue DBを実行する
   - Dialogue側にクエスト情報が流し込まれ、Dialogue System上で操作できる
