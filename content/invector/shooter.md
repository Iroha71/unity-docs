# Shooter Template

## セットアップ

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

## 武器の設定

### IK設定

- Invector > Shooter > IK Adjust Windowを開く
- Unityを再生し、プレイヤーに武器を装備させる
- 「Hand IK Target」を選択する
  - 右手・左手の球体を選択し、適切な位置になるように変更する
  - ここで設定した内容はしゃがみやエイム時などの状態に引き継がれる
- Adjustの設定は以下の4つを設定する
  - 非エイム時・立ち/しゃがみ
  - エイム時・立ち/しゃがみ
- IK Adjustの設定は武器カテゴリごとに設定する
  - カテゴリの設定は武器 > Weapon Settings > Weapon Category
- 武器ごとにLeftHandIKの位置を調整する

### エイムカメラ

- vThirdPersonCameraのAiming設定を変更する

### 武器の設定

- モデルはrenderer配下に格納し、座標を0にする
- ScopeTarget

  ![scope_target](/img/shooter/scope_target.png)

- AimReference

  ![aim_reference](/img/shooter/aim_reference.png)

- Lefthand IK

  ![lefthand_ik](/img/shooter/lefthand_ik.png)