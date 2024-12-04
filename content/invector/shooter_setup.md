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