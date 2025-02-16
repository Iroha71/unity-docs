# 自作スクリプト

- [自作スクリプト](#自作スクリプト)
  - [各機能](#各機能)
  - [スローモーション](#スローモーション)
  - [戦闘補助](#戦闘補助)
    - [前提](#前提)
    - [ターゲットに接近する機能](#ターゲットに接近する機能)
    - [ノックバック](#ノックバック)
    - [ヒットストップ](#ヒットストップ)
  - [Horse Animset ProのInvector対応](#horse-animset-proのinvector対応)
  - [マップ機能実装](#マップ機能実装)
    - [TIPS](#tips)
  - [シーンロード完了時の工夫](#シーンロード完了時の工夫)

## 各機能

- [マップ](original/map.md)

## スローモーション

- TimeScale: 0.1s
- 継続時間: 現実時間で0.5s
- AnimatorのUpdateModeをNormalにする

## 戦闘補助

[BattleSenceStrenthen](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/BattleSenceStrengthen.cs){:target="_blank"}

### 前提

- DOTWEENが導入されている
- UniTaskが導入されている

### ターゲットに接近する機能

```csharp [BattleSenceStrenthen.cs]
/// <summary>
/// 攻撃時にロックオン中の敵へ移動させる
/// </summary>
/// <param name="animEvent"></param>
public void MoveToTarget(string animEvent)
{
    Transform target = lockon.currentTarget;
    if (!target)
        return;
    Transform player = transform.parent;
    player.LookAt(new Vector3(target.position.x, player.position.y, target.position.z));
    float distanceFromTarget = Vector3.Distance(player.position, target.position);
    if (distanceFromTarget <= attackRange || distanceFromTarget > MAX_MATCH_DISTANCE)
        return;
    
    distanceFromTarget -= attackRange;
    Vector3 goal = player.forward * distanceFromTarget + player.position;

    player.DOMove(goal, 0.1f);
}
```

- Animatorに`MoveTarget`イベントを追加する

### ノックバック

```csharp [BattleSenceStrenthen.cs]
void Start()
{
    mm.onDamageHit.AddListener(AddKnockBack);
}

public void AddKnockBack(vHitInfo hitinfo)
{
    if (!tpInput.cc.IsAnimatorTag("KnockBack"))
        return;
    Vector3 direction = hitinfo.targetCollider.transform.position - transform.root.position;
    hitinfo.targetCollider.TryGetComponent(out vControlAIMelee ai);
    if (ai == null)
        return;
    Vector3 knockBackDirection = ai.transform.position - transform.root.position;
    Vector3 targetPosition = ai.transform.position + (knockBackDirection.normalized * knockBackDistance);
    ai.isKnockbacking = true;
    ai.DisableAIController();
    if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, knockBackDistance, NavMesh.AllAreas))
    {
        targetPosition = hit.position;
    }
    ai._rigidbody.DOMove(targetPosition, knockBackTime).SetEase(Ease.OutCubic).OnComplete(() =>
    {
        ai.EnableAIController();
        ai.isKnockbacking = false;
    });
}
```

### ヒットストップ

ヒットストップ目安：0.15～0.23

```csharp [BattleSenceStrenthen.cs]
void Start()
{
    mm.onDamageHit.AddListener(IgnitHitStop);
}

/// <summary>
/// 攻撃ヒット時にヒットストップを発生させる
/// </summary>
/// <param name="hitinfo">ヒット対象情報</param>
private async void IgnitHitStop(vHitInfo hitinfo)
{
    if (anim.speed == 0f || !activateHitStop)
        return;

    float defaultSpeed = anim.speed;
    anim.speed = 0f;
    hitinfo.targetCollider.TryGetComponent(out IDamageReactable enemy);
    //enemy.CallHitstop(hitstopTime);
    await UniTask.Delay((int)(hitstopTime * 1000f), cancellationToken: token);
    anim.speed = defaultSpeed;
}
```

- 被弾側のヒットストップ実装

```csharp
public interface IDamageReactable
{
    public void CallHitstop(float stopTime);
}
```

```csharp [vControlAIMelee.cs]
private float defaultAnimSpeed;
public async void CallHitstop(float hitstopTime)
{
    if (animator.speed != 0)
        defaultAnimSpeed = animator.speed;
    animatorSpeed = 0f;
    animator.speed = 0f;
    await UniTask.Delay((int)(hitstopTime * 1000f));
    animator.speed = defaultAnimSpeed;
    animatorSpeed = defaultAnimSpeed;
}
```

- 被弾側のAnimationEventに登録しておき、リアクションモーションの1～4フレームの地点にイベントを設定しておく

## Horse Animset ProのInvector対応

- [RiderInput](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/RiderInput.cs){:target="_blank"}
- 同階層に`VChangeInputTypeTrigger`を設置
  - `OnChangeKeyboard` / `OnChangeJoyStick`へ`RiderInput.SwitchInputType`を設定する

## マップ機能実装

### TIPS

- 3d座標→2d座標に変換

  ``` csharp[worldpos.cs]
  Vector3 screenPosition = mapCamera.WorldToScreenPoint(worldPosition);
  RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImage, screenPosition, null, out Vector2 localpoint);
  transform.localPosition = localpoint;
  ```

## シーンロード完了時の工夫

- シーン開始時に暗転状態から始める
  - Start時に呼び出し

  ``` csharp[fade.cs]
  public async void HideScene()
  {
      canvas.enabled = GetComponent<Canvas>();
      canvasGroup.alpha = 1f;
      AudioListener.volume = 0f;
      await UniTask.Delay(500);
      await canvasGroup.DOFade(0f, 0.5f);
      AudioListener.volume = 1f;
  }
  ```
