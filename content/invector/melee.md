# Melee Template

## 目次

- [パリィ](#パリィ)
- [ローリング](#ローリング)
- [Ragdoll](#ragdoll)
- [Death By AnimationWithRagdollのタイミング調整](#death-by-animationwithragdollのタイミング調整)
- [テイクダウン](#テイクダウン)
- [ボタン同時押し](#ボタン同時押し)
- [追加ダメージ設定](#追加ダメージ設定)
- [無敵化](#無敵化)

## パリィ

### 仕様

- パリィ受付開始時
        ![parry-enable](/img/parry-enable.png)

- パリィ発生時
        ![parry](/img/parry.png)

- パリィ受付終了
        ![parry-disable](/img/parry-disable.png)

- 毎フレームの動作

    ``` cs[Shield.cs]
    void Update()
    {
        if (tpInput.isBlocking)
        {
            CountParryTime();
        }

        if (!tpInput.isBlocking && isReceivingParry)
        {
            DisableParry();
        }
    }
    ```

- パリィの有効化

    ``` cs[Shield.cs]
    private void EnableParry()
    {
        isReceivingParry = true;
        weapon.breakAttack = true;
        // FullBody > Hit RecoilにParryアニメーションを追加（トリガー: ActionState -1を追加）
        anim.SetInteger("ActionState", -1);
    }
    ```

- パリィの無効化

    ``` cs[Shield.cs]
    private void DisableParry()
    {
        isReceivingParry = false;
        weapon.breakAttack = false;
        anim.SetInteger("ActionState", 0);
        elapsedTime = 0f;
    }
    ```

- パリィ受付時間の計測

    ``` cs[Shield.cs]
    private void CountParryTime()
    {
        if (elapsedTime == 0f)
            EnableParry();

        elapsedTime += Time.deltaTime;
        if (elapsedTime > parriableTime)
        {
            DisableParry();
        }
    }
    ```

- パリィ演出

    ``` cs[Shield.cs]
    private async void Parry()
    {
        if (isReceivingParry == false)
        {
            ParticleSystem _defenceEffect = Instantiate(defenceEffect, this.transform);
            _defenceEffect.transform.localPosition = Vector3.zero;

            return;
        }

        ParticleSystem _parryEffect = Instantiate(parryEffect, this.transform);
        _parryEffect.transform.localPosition = Vector3.zero;
        cc.IsInvincible = true;
        await UniTask.Delay(300);
        Time.timeScale = 0.3f;
        await UniTask.Delay(500, DelayType.Realtime);
        cc.IsInvincible = false;
        Time.timeScale = 1f;
    }    
    ```

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

## 無敵化

1. 以下のようなフラグを実装するインターフェースを作成する

    ``` cs[IInvicible.cs]
    public interface IInvincible
    {
        bool IsInvincible { get; set; }
    }
    ```

2. `vThirdPersonMotor.cs`へ実装する
3. `vThirdPersonMotor.cs` > `TakeDamage`の条件式にフラグを追加する

    ``` cs[vThirdPersonMortor.cs]
    if (currentHealth <= 0 || (IgnoreDamageRolling()) || IsInvincible)
            {
                if (damage.activeRagdoll && !IgnoreDamageActiveRagdollRolling())
                {
                    onActiveRagdoll.Invoke(damage);
                }

                return;
            }
    ```