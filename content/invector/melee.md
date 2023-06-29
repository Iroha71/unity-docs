# Melee Template

## ローリング

- RollTransition: 0
- RollSpeed: 1.5
- RollRotationSpeed: 55
- ローリングキャンセル

``` cs[vThirdPersonInput.cs]
// RollInput()
if (rollInput.GetButtonDown() && cc.IsAnimatorTag("Attack"))
{
  if (rollInput.GetButtonDown() && RollConditions())
  {
    cc.Roll();
  }
}
```

## Ragdoll

- Invector > Basic Locomotion > vRagdoll

## Death By AnimationWithRagdollのタイミング調整

- vAIMotor.cs
  - `info.normalizedTime >= 0.8f`の値を大きくする

``` cs[vAIMotor.cs]
protected virtual void AnimatorDeath()
  {
      // death by animation & ragdoll after a time
      else if (deathBy == DeathBy.AnimationWithRagdoll)
      {
          int deadLayer = 0;
          var info = animatorStateInfos.GetStateInfoUsingTag("Dead");
          if (info != null)
          {
              if (!animator.IsInTransition(deadLayer) && info.normalizedTime >= 0.95f && GroundDistanceAnim <= 0.1f)
              {                      
                  onActiveRagdoll.Invoke(null);
                  RemoveComponents();
              }
          }
      }
  }
```

## テイクダウン

- AI側のDetection(MinDistance)を0にする
  - 接近時にプレイヤーを検知しないようにするため
- AIへTriggerGenericAction.csを設置
  - Animation >>> プレイヤー側のアニメーション
  - Event > OnPressedAction >>> AI.Animator.PlayFixedTime()
  - Event > OnPressedAction >>> AI.vEventWithDelay.DoEvent(0)
- vEventWithDelay.cs
  - AI.vHealthController.AddHealth(-100) / Rigidbody.isKinematic = false
- AnimatorControllerにはテイクダウンモーションを追加
  - vAnimatorTag.cs追加 >>> Death / CustomActionタグ追加
  - vSetInteger.cs追加 >>> ActionStateを-1に変更する
    - 通常時の死亡モーションを再生しないようにするため

## ボタン同時押し

``` cs[SameButton.cs]
// LB + Xボタンの例
if (exampleInput.GetButtonDown() && otherInput.GetButton())
{
  同時押しの処理
}
```

## 追加ダメージ設定

- `vMeleeAttackObject.cs` > `ApplyDamage()`の処理を変更するとダメージが変わる
- meleeWeapon.damageが武器
- meleeWeapon.damageModifierが追加ダメージ設定
- 武器ダメージのみにしたい場合は`MeleeManager.cs` > defaultDamageを0にする
- 属性や増加値はvItemAttributeを増やして対応
