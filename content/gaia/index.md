# GAIA

## Gaiaの注意点

### PostProcessセットアップ

- MainCamera
  - **PostProcess**にチェック
  - 環境 > **ボリュームマスク**を**PostProcess**に変更
- Global Post Processing
  - レイヤーを**PostProcess**へ変更
- UnderWater Post Processing
  - レイヤーを**PostProcess**へ変更

## レベルデザイン TIPS

### スタンプの境界線をなくす方法

- StamperにImage Maskを追加する
  - Influenceをグローバルに変更
  - Imageにスタンプと同じテクスチャを選択する

  ![stamper_config](img/stamper_config.png)

- 斜面の角度等を変更したい場合は Image Mask > Strength Transformのカーブを編集する

### 高地を作成する

- Stamper > Image Mask > Strength Transformのカーブを編集する

  ![strength_config](img/stamper_strength_config.png)

## 草原の表現

- global spawn density: 0.8
- ProtoType / Prefab
  - width min max: 1.5
  - height min max: 1.5
  - noise spread: 3
- Spawing
  - Detail Density: 40
  - Min Fitness %: 50
  - Fadeout Fitness %: 60

## スタンプ

- 初期の大きさ
  - x: 100 y: 5 z:100
