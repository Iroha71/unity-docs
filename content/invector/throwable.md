# 投擲物の実装

## 投擲物の作成

- 既存のプレハブをコピーする
  - 場所: Inventor-3rdPersonController > Shooter > Scripts > ThrowSystem > Prefabs > Grenades
- vGrenadeモデルを差し替え
- vFragmentGrenade > Box Colliderのサイズを編集する

## 投擲物の編集

- 投擲物 > ThrowableObjectの編集
  - エフェクト設定
    - 投擲物配下にエフェクトを追加
    - OnExplode() > vExplosive.RemoveParentOfOtherにエフェクトを設定
    - OnExplode() > GameObject.SetActive等でエフェクトを有効化

## ThrowManagerの編集

- プレイヤー配下のThrowManagerを有効にする

  ![throw-manager](/img/throw-manager-hieralchy.png)

  |オブジェクト名|用途|
  |---|---|
  |~-Inventory_EquipArea|投擲物を装備欄に装備するタイプ|
  |~-Inventory_Item|Itemとして管理するが、装備の必要がない|
  |~-Standalone|ItemManagerとは独立して管理|

- vThrowManagerInventory > Throwable > Default Handlerを設定する
