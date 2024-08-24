# 投擲物の実装

## 投擲物の作成

- 投擲物にするオブジェクトにコンポーネントをアタッチする
  - **VThrowableObject**
  - **Rigidbody**
    - isKinematic = true
    - まっすぐ飛ばす場合はConstraints > Position > Y を固定する
  - **Collider**
    - TriggerをONでも可能
  - **VSimpleTrigger**
    - Tags to Detect / Layers to Detectを設定する
- レイヤーをTriggersへ変更する
- VThrowableObject > OnThrowにイベントを設定する
  - Collider.enabled = true
  - vSimpleTrigger.enabled = true
- VSimpleTrigger > OnTriggerEnterにイベントを設定する
  - (爆発エフェクト).gameobject.SetActive(true)
    - エフェクトはRemoveParentで離脱するようにしておく

- 各グレネードの参考
  - 場所: Inventor-3rdPersonController > Shooter > Scripts > ThrowSystem > Prefabs > Grenades

## ThrowManagerの編集

- プレイヤー配下のThrowManagerを有効にする

  ![throw-manager](/img/throw-manager-hieralchy.png)

  |オブジェクト名|用途|
  |---|---|
  |~-Inventory_EquipArea|投擲物を装備欄に装備するタイプ|
  |~-Inventory_Item|Itemとして管理するが、装備の必要がない|
  |~-Standalone|ItemManagerとは独立して管理|

- vThrowManagerInventory > Throwable > Default Handlerを設定する
  - 場所: RightHand配下に作成
