# 投擲物の作成

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
