# セットアップ

- Invector > Cover Add-On > Create Cover Controllerでキャラクターを作成する
- UIの変更（任意）
  - キャラクター > Cover UI Prefabを編集する
- NavMeshを導入し、床にBakeする
- カバーラインの作成
  - Invector > Cover Add-On > Create New Cover Lineでカバーラインを作成する
  - 赤点をクリックするとカバーラインの長さを変更できる
  - カバーラインを直角に曲げる方法
    - カバーライン > New Cover Pointで新しいポイントを作成する
    - カメラを俯瞰に移動し、赤点をドラッグして直角に移動させる
    - コンポーネント内の設定を変更することで、厚さや幅を調整可能。
- 必要に応じてカバーオブジェクトにNavMeshObstaclesをアタッチする
  - オブジェクトが移動しても、Bakeしなおす必要がなくなる

## キャラクターの設定

### カバーアドオン

- V Cover Controller > Cover Settings

  |項目|内容|
  |---|---|
  |Enter/Exit Input|カバーを取るキー設定|
  |Auto Enter Cover|自動カバー|
  |Auto Exit Cover|壁を離れるような入力を受けるとカバーを外す|


## AIのセットアップ手順

### 各パラメータの設定

- Nav Mesh Agent

  |設定項目|値|
  |---|---|
  |角速度|200～1000|
  |加速度|50|
  |停止距離|0.1|

- Movement
  - Free Speed / Strafe Speed
    - すべて 1
- Agent

  |設定項目|値|
  |---|---|
  |Acceleration|50|
  |Stoping Distance|0.1|
  |Walking|0|
  |Running|0.05|
  |Sprinting|0.1|

- Detection

  |設定項目|値|
  |---|---|
  |Find Other Target|true|
  |Max Targets Detection|10|
  |Change Target Delay|2|
  |Field Of View|130|
  |Min Distance To Detect|3|
  |Max|20|
  |Lost Target Distance|6|
  |Time To Lost Without Sight|5|

- Combat Settings

  |設定項目|値|
  |---|---|
  |Min Attack Time|0.5|
  |Max|3|
  |Attack Distance|1.25 ~ 1.65|
  |Min Stay Blocking Time|1|
  |Max|3|
  |Min Distance Of The Target|1 ~ 1.4|
  |Combat Distance|2.5~3|

### Root Motionの修正

攻撃アニメーションにRoot付きのものを利用すると
AIが攻撃時に対象に吸い寄せられる現象が発生するため、
RootMotionの上書きを無効にする。
<br/>

- **vControlAI** > OnAnimatorMoveで呼び出している**base.OnAnimatorMove**をコメントアウトする
- または攻撃アニメーションにInplaceのものを利用する

### 武器の設定

- RightHandにSimpleHolderを作成し、武器を配置する

  ![simple_holder](img/ai/simple_holder.png)

- 任意の場所に納刀状態の武器を取り付ける

- キャラに**vSimpleHolder**を取り付ける
  - Events > On Enable Weapon: **作成したSimpleHolder.SetActive(false)** を設定
  - Events > On Disable Weapon: **作成したSimpleHolder.SetActive(true)** を設定
  - Weapon Object: 納刀状態の武器オブジェクト
- キャラに **vMessageReceiver** を取り付ける
  - Message Listenersに以下を設定

    |リスナー名|On Receive Messageの設定|
    |---|---|
    |EnableWeapon|vSimpleHolder.EquipWeapon|
    |DisableWeapon|vSimpleHolder.UnequipWeapon|

- FSM > Chaseステートなどのアクションに以下を設定

  |アクション名|スクリプト|パラメータ|
  |---|---|---|
  |EnableWeapon|vAISendMessage|EnableWeapon|
  |DisableWeapon|vAISendMessage|DisableWeapon|

### 攻撃モーションとリアクション

- 攻撃モーション中にリアクションを割り込ませる設定
  - 攻撃モーションのトランジション > 中断要因を「Next State」に変更する
    - Null→〇〇AttacksのトランジションとA→Bのようなトランジションへ設定する
  - Big hit reactionのAnyState→各ステートのトランジション > 中断要因を「Current State」へ変更する
  - Bit hit reactionのExitへ伸びるトランジション > 中断要因を「Next State」へ変更する
- 攻撃後に若干時間を空けてあげると、AIが連続的に攻撃することを防げる

## ThrowManagerの編集

- プレイヤー配下のThrowManagerを有効にする

  ![throw-manager](img/throw-manager-hieralchy.png)

  |オブジェクト名|用途|
  |---|---|
  |~-Inventory_EquipArea|投擲物を装備欄に装備するタイプ|
  |~-Inventory_Item|Itemとして管理するが、装備の必要がない|
  |~-Standalone|ItemManagerとは独立して管理|

- vThrowManagerInventory > Throwable > Default Handlerを設定する
  - 場所: RightHand配下に作成