# 長押し処理

## 強攻撃をチャージタイプにする

- vMeleeCombatInput.csにプロパティを追加する

  ``` csharp [vMeleeCombatInput.cs]
  // チャージ中か
  protected bool isChargingStrongAttack = false;
  // 強攻撃ボタン長押し中のコールバック
  public UnityAction OnPressingStrongAttack;
  // 強攻撃ボタンを離したコールバック
  public UnityAction OnReleasedStrongAttack;
  // 強攻撃がキャンセルされた際のコールバック
  public UnityAction OnCanceledStrongAttack;
  ```

- vMeleeCombatInput.cs > MeleeStrongAttackInput()を編集する

  ``` csharp [vMeleeCombatInput.cs]
  public virtual void MeleeStrongAttackInput()
  {
      if (animator == null)
      {
          return;
      }

      // 元のコードを削除
      //if (strongAttackInput.GetButtonDown() && (!meleeManager.CurrentActiveAttackWeapon || meleeManager.CurrentActiveAttackWeapon.useStrongAttack) && MeleeAttackStaminaConditions())
      //{
      //    TriggerStrongAttack();
      //}
      
      // 強攻撃可能な武器か判定
      if (meleeManager.CurrentActiveAttackWeapon == null || meleeManager.CurrentActiveAttackWeapon.useStrongAttack == false)
          return;
      // 強攻撃長押し
      if (strongAttackInput.GetButton()
          && MeleeAttackStaminaConditions())
      {
          isChargingStrongAttack = true;
          OnPressingStrongAttack?.Invoke();
      }
      // 強攻撃ボタンを離した際
      else if (isChargingStrongAttack)
      {
          if (meleeManager.CurrentActiveAttackWeapon.useStrongAttack)
          {
              OnReleasedStrongAttack?.Invoke();
          }
          else
          {
              OnCanceledStrongAttack?.Invoke();
          }
          isChargingStrongAttack = false;
      }
  }
  ```

- MeleeStrongAttackInput()の呼び出し元の編集
  - Shooter = vShooterMeleeInput > InputHandle()
  - Melee = vMeleeCombatInput > InputHandle()

  ``` csharp
  #region MeleeInput

  if (MeleeAttackConditions() && !IsAiming && !isReloading && !lockMeleeInput && !CurrentActiveWeapon)
  {
      // チャージ中は弱攻撃ができないように
      if ((shooterManager.canUseMeleeWeakAttack_H || shooterManager.CurrentWeapon == null)
          && isChargingStrongAttack == false)
      {
          MeleeWeakAttackInput();
      }

      if (shooterManager.canUseMeleeStrongAttack_H || shooterManager.CurrentWeapon == null)
      {
          MeleeStrongAttackInput();
      }
      // チャージ中はガードができないように
      if ((shooterManager.canUseMeleeBlock_H || shooterManager.CurrentWeapon == null)
          && isChargingStrongAttack == false)
      {
          BlockingInput();
      }
      else
      {
          isBlocking = false;
      }
  }

  #endregion
  ```

### 使用例

- [ここのスクリプトを参照](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/chargable_strong_attack/)

## 長押し・単押し出し分け

- 強攻撃出し分ける例

  ``` csharp [SameButton.cs]
  public virtual void MeleeStrongAttackInput()
  {
    if (animator == null)
    {
      return;
    }

    if (strongAttackInput.GetButtonTimer(0.7f) && 
      (!meleeManager.CurrentActiveAttackWeapon || 
      meleeManager.CurrentActiveAttackWeapon.useStrongAttack) 
      && MeleeAttackStaminaConditions())
    {
      isArtsReady = true;
      OnInputArtsAttack?.Invoke();
      return;
    }

    if (strongAttackInput.GetButtonDown() && isArtsReady)
      isArtsReady = false;

    if (strongAttackInput.GetButtonUp() && 
      isArtsReady == false && 
      (!meleeManager.CurrentActiveAttackWeapon ||
      meleeManager.CurrentActiveAttackWeapon.useStrongAttack)
      && MeleeAttackStaminaConditions())
    {
      TriggerStrongAttack();
    }
  }
  ```

## ボタン同時押し

``` csharp [SameButton.cs]
// LB + Xボタンの例
if (exampleInput.GetButtonDown() && otherInput.GetButton())
{
  同時押しの処理
}
```
