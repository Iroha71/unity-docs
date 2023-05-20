# Invector

## Melee

### ローリング

- RollTransition: 0
- RollSpeed: 1.5
- RollRotationSpeed: 55

### ローリングキャンセル

``` vThirdPersonInput.cs
// RollInput()
if (rollInput.GetButtonDown() && cc.IsAnimatorTag("Attack"))
{
  if (rollInput.GetButtonDown() && RollConditions())
  {
    cc.Roll();
  }
}
```

### Ragdoll

- vRagdollをアタッチ
- vThirdPersonController > Health > Death by ragdoll

### テイクダウン

- AI側のDetection(MinDistance)を0にする
  - 接近時にプレイヤーを検知しないようにするため
- AIへTriggerGenericAction.csを設置
  - Animation >>> プレイヤー側のアニメーション
  - Event > OnPressedAction >>> AI.Animator.PlayFixedTime()
  - Event > OnPressedAction >>> AI.vEventWithDelay.DoEvent(0)
- vEventWithDelay.cs
  - AI.vHealthController.AddHealth(-100)など
- AnimatorControllerにはテイクダウンモーションを追加
  - vAnimatorTag.cs追加 >>> Death / CustomActionタグ追加
  - vSetInteger.cs追加 >>> ActionStateを-1に変更する
    - 通常時の死亡モーションを再生しないようにするため

#### カメラ

``` vThirdPersonController.cs
public bool IsExecuting {
 get { return this.isExecuting; }
 set { this.isExecuting = value }
}  
*vThirdPersonInput 425行目 
　else if (cc.isExecuting) {
  tpCamera.ChangeState("Execution", true)
}
```

### AIのAttack DistanceとCombat Distanceの設定

- Attack Distance > Combat Distanceの関係になるように設定する

### ボタン同時押し

``` SameButton.cs
// LB + Xボタンの例
if (exampleInput.GetButtonDown() && isBlocking)
{
  同時押しの処理
}
```

- ジャンプと競合しないようにガード中はジャンプしないように変更

``` vMeleeInput.cs
// vMeleeInput.cs > JumpCondition()
return !isAttacking && base.JumpConditions() && !isBlocking;
```

### 追加ダメージ設定等

- `vMeleeAttackObject.cs` > ApplyDamage()の処理を変更するとダメージが変わる
- meleeWeapon.damageが武器
- meleeWeapon.damageModifierが追加ダメージ設定
- 武器ダメージのみにしたい場合は`MeleeManager.cs` > defaultDamageを0にする
- 属性や増加値はvItemAttributeを増やして対応
