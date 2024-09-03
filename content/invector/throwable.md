# 投擲物の実装

## 吹き飛ばしの実装

- 爆発を実装するコンポーネントを投擲物へ取り付ける

``` csharp
public async void Explode()
{
    // 爆発範囲内にあるRigidbodyを取得
    Collider[] targets = Physics.OverlapSphere(transform.position, colliderS.radius, layer);
    for (int i = 0; i < targets.Length; i++) 
    {
        vDamage _damage = new vDamage(damageConfig);
        targets[i].gameObject.ApplyDamage(_damage);
        targets[i].TryGetComponent(out Rigidbody rb);
        // ラグドールが適用された後に吹き飛ばすため、1フレーム待機
        await UniTask.DelayFrame(1);
        rb.AddExplosionForce(forceValue, transform.position, colliderS.radius, upward, ForceMode.Impulse);
    }
}
```

- 検出対象のlayerにはBodyPartsを指定する

- damageConfig
  - ignoreDefense: true
  - activeRagdoll: true

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

## 投擲軌道の修正

- 軌道を水平に近づける場合
  - ThrowSettings@Default > Min Max Timeの最大値を小さくする
- 軌道を大きくする場合
  - ThrowSettings@Default > Min Max Timeの最大値を大きくする

![throw_settings](/img/throwable_settings.png)

MinMaxTimeは放物線計算時の時間である。

この時間を大きくすると滞空時間が増加＝滞空時間が長くなるように放物線が高くなる。

小さくすると滞空時間は減少＝滞空時間が短くなるように放物線が低くなる。
