# Shooter Template

- [Shooter Template](#shooter-template)
  - [セットアップ](#セットアップ)
  - [キャラクターの設定](#キャラクターの設定)
    - [カバーアドオン](#カバーアドオン)
  - [武器の設定](#武器の設定)
    - [ID設定](#id設定)
    - [スナイパースコープの設定例](#スナイパースコープの設定例)
    - [IK設定](#ik設定)
    - [エイムカメラ](#エイムカメラ)
    - [武器の設定](#武器の設定-1)
    - [エフェクト](#エフェクト)
    - [弾丸の編集](#弾丸の編集)
    - [リロード](#リロード)
    - [ショットガンの実装](#ショットガンの実装)

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

### ID設定

**VShooterWeapon > WeaponID**を設定する
レティクルの変更は**ScopeView > Scope ID**を変更する
レティクルのIDは**AimCanvas**配下を参照する

- ハンドガン

  |ID|設定内容|
  |---|---|
  |Move Set ID|1|
  |Upper Body ID|1|
  |Shot ID|-1|
  |Reload ID|1|
  |Equip ID|5|
  |Scope ID|1|

- ライフル

  |ID|設定内容|
  |---|---|
  |Move Set ID|1|
  |Upper Body ID|2|
  |Shot ID|2|
  |Reload ID|2|
  |Equip ID|0|
  |Scope ID|2|

- ショットガン

  |ID|設定内容|
  |---|---|
  |Move Set ID|1|
  |Upper Body ID|3|
  |Shot ID|3|
  |Reload ID|3|
  |Equip ID|0|
  |Scope ID|4|

- スナイパー
  - スナイパースコープの設定も参照

  |ID|設定内容|
  |---|---|
  |Move Set ID|1|
  |Upper Body ID|2|
  |Shot ID|4|
  |Reload ID|2|
  |Equip ID|0|
  |Scope ID|0|
  |Use UI|true|

### スナイパースコープの設定例

- スコープ用カメラを設置する
  - 銃身が映らないように設置する
  - Projection > Clipping > Clipping Planesを**0.3/250**に変更する
  - Output > Output Textureに**aimCameraRenderer**を設定する
- V Shooter Weapon > Scope View
  - Use UI: true
  - Scope Zoom: 45
  - Custom Scope Camera State: ScopeAiming
  - Zoom Scope Camera: スコープ用カメラ
- スコープ用UIの編集は以下を参照する
  - Aim Canvas > AimID_0_Sniper
    - Scope Aim Groupe

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

### エフェクト

- マズルフラッシュ
  - renderer配下にParticlesオブジェクト作成
  - Particles配下にマズルフラッシュのパーティクル配置
    - Play on awakeをオフにする
  - V Shooter Weapon > Audio & VFX > Emitt Shuryken Particleにパーティクルをセットする
- 射撃音
  - V Shooter Weapon > Audio & VFX > Fire Clip / EmptyClipを設定する
  - 【参考】gamemaster audioの射撃音
    - gun_xxx_auto_shot_01 - 08…射撃音に使う（ランダム再生で利用）
    - gun_xxx_auto_shot_00…1発目の音に使う
  - ランダム化
    - vShooterWeaponBaseに変数`fireClips: AudioClip[]`を用意する
    - vShooterWeaponBase > ShotEffect()のif文を以下のように変更する

      ``` csharp
      if (source && fireClip)
      {

          //source.PlayOneShot(fireClip);
          int rand = Random.Range(0, fireClips.Length);
          source.PlayOneShot(fireClips[rand]);
      }
      ```

### 弾丸の編集

- vDefaultBullet
  - Trailを削除する
  - bulletオブジェクトを適当なサイズに変更する

### リロード

- 武器にvAnimatorEventReceiverを取り付ける
- AnimatorEventsの設定を行う
  - マガジンを落とす演出
  
  ![instantiate_rifleclip](/img/shooter/clip_instantiate.png)
  ![removable_clip](/img/shooter/removable_clip.png)

  - マガジン表示演出

  ![turnon_rifleclip](/img/shooter/turnon_rifleclip.png)

  - アニメーションイベント設定

  ![animation_event_rifleclip](/img/shooter/animation_event_rifleclip.png)

### ショットガンの実装

- V Shooter Weapon > Projectileの設定を変更する
  
  |項目|内容|備考|
  |---|---|---|
  |Projectile|vDefaultBullet|-|
  |Projectiles Per Shot|6|一度に何発放出するか|
  |Dispersion|4|拡散範囲|
  |DispersionSharp|Circle|拡散の形|