# 追加ダメージ設定

- `vMeleeAttackObject.cs` > `ApplyDamage()`の処理を変更するとダメージが変わる
- meleeWeapon.damageが武器
- meleeWeapon.damageModifierが追加ダメージ設定
- 武器ダメージのみにしたい場合は`MeleeManager.cs` > defaultDamageを0にする
- 属性や増加値はvItemAttributeを増やして対応（vMeleeEquipment.csでAttribute→武器ダメージ反映が実行されるため）

## vDamageObjectのTips

ダメージオブジェクトそのものにアタッチすると複数回ヒット判定が起こることがある。
→そのため子要素にvDamageObjectを設置する。

- 子要素に空のオブジェクトを作成し、vDamageObjectをアタッチする
- vDamageObject > OnHitで自身のオブジェクトのSetActive(false)等を呼び出す

## ダメージに新たな要素を追加する場合

- `vDamage.cs` にプロパティを追加
  - `public AttributeCompatibleDataList.Attribute attribute;`
- `vDamage.cs` > `public vDamage(vDamage damage)` のコンストラクタに新しいプロパティを追加

  ``` csharp [vDamage.cs]
  public vDamage(vDamage damage)
  {
      this.damageValue = damage.damageValue;
      ~~~
      // 追加
      this.attribute = damage.attribute;
  }
  ```

- Inspectorに表示するために`vDamageDrawer.cs`に以下を追加

  ```csharp [vDamageDrawer.cs]
  var attribute = property.FindPropertyRelative("attribute");

  // 66行目以降
  if (attribute != null)
  {
    position.y += 20;
    EditorGUI.PropertyField(position, attribute);
  }

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    // return !isOpen ? 25 : (valid ? 210 : 130 + helpBoxHeight);
    // ↓に変更（+40部分を20ずつ加算する）
    return !isOpen ? 25 : (valid ? 210 + 40 : 130 + helpBoxHeight);
  }
  ```

## ダメージに新たな要素を追加する場合（vMeleeControlに追加）

- **vMeleeAttackControl**にプロパティを追加
  - 例）public int isStrongAttack
- 攻撃側：**vMeleeManager**にプロパティを追加し、セッターを追加

  ``` csharp
  protected virtual bool IsStrongAttacking { get; set; }

  public virtual void SetStrongAttack(bool isStrongAttack)
  {
    this.IsStrongAttacking = isStrongAttack;
  }
  ```

- **vMeleeAttackControl** > ActiveDamageで呼び出し

  ``` csharp [vMeleeAttackControl.cs]
  void ActiveDamage(Animator animator, bool value)
  {
    var meleeManager = animator.GetComponent<vMeleeManager>();
    if (meleeManager)
    {
      meleeManager.SetActiveAttack(
        bodyParts,
        meleeAttackType, 
        value, 
        damageMultiplier, 
        recoilID, 
        reactionID, 
        ignoreDefense, 
        activeRagdoll, 
        senselessTime, 
        damageType);
      meleeManager.SetStrongAttack(isStrongAttack);
      meleeManager.SetHitStopState(disableHitStop);
    }   
  }
  ```

- 攻撃側：**vMeleeManager** > OnDamageHit()でdamageやhitinfoに代入する

  ``` csharp [vMeleeManager.cs]
  public virtual void OnDamageHit(ref vHitInfo hitInfo)
  {
    vDamage damage = new vDamage(hitInfo.attackObject.damage);
    ~~~
    damage.isStrongAttack = IsStrongAttacking;
    ~~~
    hitInfo.isStrongAttacking = damage.isStrongAttack;
    ~~~
  }
  ```

- damage/hitinfoにそれぞれプロパティを追加する
- 攻撃側vMeleeWeaponのonDamageHitイベント等で取り出す

## 無敵化

1. 以下のようなフラグを実装するインターフェースを作成する

  ```csharp [IInvicible.cs]
  public interface IInvincible
  {
      bool IsInvincible { get; set; }
  }
  ```

2. `vThirdPersonMotor.cs`へ実装する
3. `vThirdPersonMotor.cs` > `TakeDamage`の条件式にフラグを追加する

  ```csharp [vThirdPersonMortor.cs]
  if (currentHealth <= 0 || (IgnoreDamageRolling()) || IsInvincible)
  {
    if (damage.activeRagdoll && !IgnoreDamageActiveRagdollRolling())
    {
        onActiveRagdoll.Invoke(damage);
    }

    return;
  }
  ```

## ヘッドショット判定

- キャラクターにラグドール設定を行う
  - 各パーツにvDamageReceiverが取り付けられる
- 頭のvDamageReceiver > Damage Multiplerの値を変更する
  - 必要であればOverride Reaction IDを有効にする
- V Shooter Weapon > Layer & Tagに**Default, BodyPart**を設定する