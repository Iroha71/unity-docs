# 左手にも武器を装備する

左武器での攻撃用ボタンには使用していないボタンか
ボタンの組み合わせが必要。（LBやLT+RTなど）
<br/>
左攻撃の入力関数を作成し、
Interfaceを通して目的のコンポーネントの関数を実行する。

以下はLT+RTの例
<br/>

- **vMeleeCombatInput**に左武器攻撃用の関数を作成する

  ``` csharp [vMeleeConbatInput.cs]
  public virtual void MeleeExtendAttackInput()
  {
    if (!isBlocking) return;

    if (weakAttackInput.GetButtonDown() && MeleeAttackStaminaConditions())
    {
      // interfaceにアクセス
      extendAttackable.TriggerExtendAttack();
    }
  }
  ```

- **vShooterMeleeInput** > InputHandleから関数を呼び出す

  ``` csharp [vShooterMeleeInput.cs]
  public override void InputHandle()
  {
    /// ～～省略～～
    #region MeleeInput

    if (MeleeAttackConditions() && !IsAiming && !isReloading && !lockMeleeInput && !CurrentActiveWeapon)
    {
      if (shooterManager.canUseMeleeWeakAttack_H || shooterManager.CurrentWeapon == null)
      {
        MeleeWeakAttackInput();
      }

      if (shooterManager.canUseMeleeStrongAttack_H || shooterManager.CurrentWeapon == null)
      {
        MeleeStrongAttackInput();
      }

      // 追加
      if (shooterManager.canUseMeleeWeakAttack_H || shooterManager.CurrentWeapon == null)
      {
        MeleeExtendAttackInput();
      }
    }
  ```

- 攻撃用関数の例

  ``` csharp
  public void TriggerExtendAttack()
  {
    if (cc.meleeManager.leftWeapon == null)
        return;
    cc.animator.CrossFade("Null", 0f, 7);
    cc.animator.SetBool("IsFlipHand", true);
    cc.animator.SetInteger("AttackID", cc.meleeManager.leftWeapon.attackID);
    cc.animator.SetTrigger("WeakAttack");
  }
  ```