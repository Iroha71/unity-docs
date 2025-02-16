# 吹き飛ばしの実装

- 爆発を実装するコンポーネントを投擲物へ取り付ける

``` csharp [SampleExplode.cs]
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