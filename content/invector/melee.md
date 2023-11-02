# Melee Template

## 目次

- [Melee Template](#melee-template)
  - [目次](#目次)
  - [パリィ](#パリィ)
    - [仕様](#仕様)
  - [ローリング](#ローリング)
  - [Ragdoll](#ragdoll)
  - [Death By AnimationWithRagdollのタイミング調整](#death-by-animationwithragdollのタイミング調整)
  - [スタン状態の実装](#スタン状態の実装)
  - [テイクダウン](#テイクダウン)
  - [ボタン同時押し](#ボタン同時押し)
  - [追加ダメージ設定](#追加ダメージ設定)
    - [ダメージに新たな要素を追加する場合](#ダメージに新たな要素を追加する場合)
  - [無敵化](#無敵化)

## パリィ

1. Player > Animator > Fullbody > Hit Recoilに以下のステートを追加する

    |項目|値|
    |---|---|
    |アニメーション|パリィモーション|
    |遷移条件|ActionState = -1, TriggerRecoil|

2. 以下のスクリプトを作成
3. vMeleeWeapon.cs > OnDefenceへParry()を設定する

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

|設定項目|値|
|---|---|
|Roll Transition|0|
|Roll Speed|1.5|
|Roll RotationSpeed|55|

- ローリング後の硬直解消
  1. Rollステート選択
  2. インスペクタ > Transitions > 各遷移を選択
  3. 遷移間隔を調整する

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

## スタン状態の実装

- Animator
  - パラメータ「**IsStun**」追加
  - FullBody > スタン用State追加
    - `vAnimatorTag` > **CustomAction**
    - AnyState > StateへTransition追加 + Conditionに**IsStun == true** / **isDead == false**追加

- スタン値を加算する

    ``` cs[AddStun.cs]
    enum StunState { STUNNED, NOT_STUN };

    public void AddStun(int value)
    {
        stun += Mathf.Clamp(stun+value, 0, maxStun);
        if (stun >= maxStun)
        {
            ChangeStunState(StunState.STUNNED);
        }
        // スタン中にスタン値が減少した場合、スタン状態解除
        else if (stunState == StunState.STUNNED && stun < maxStun)
        {
            ChangeStunState(StunState.NOT_STUN);
        }
    }
    ```

- スタン状態に変更

    ``` cs[Stun.cs]
    private void ChangeStunState(StunState state)
    {
        // AI挙動を停止
        vAIControl.DisableAIController();
        // スタン用アニメーション実行
        Animator.SetBool("IsStun", true);
    }
    ```

- スタン監視
  - Unitaskはキャンセルが面倒のため、Updateを利用

    ``` cs[Update.cs]
    void Update()
    {
        if (stunState == StunState.STUNNED)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stunTime)
            {
                // スタン時間を超えた場合は最大値を減算し、スタン値を0にする
                AddStun(-maxStun);
            }
        }
    }
    ```

- スタン状態に遷移するスクリプト

    ``` cs[Stun.cs]
    private void ChangeStunState(StunState state)
    {
        // AI挙動を再開
        vAIControl.EnableAIController();
        // スタン用アニメーション解除
        Animator.SetBool("IsStun", false);
    }
    ```

## テイクダウン

- AnimatorControllerにはテイクダウンモーションを追加
  - `vAnimatorTag.cs`追加

    |設定値|値|
    |---|---|
    |1|Dead|
    |2|IgnoreIK|

  - `vAnimatorSetInt.cs`追加
    - 通常時の死亡モーションを再生しないようにするため

    |設定値|値|
    |---|---|
    |Parameter|ActionState|
    |Set On Enter|true|
    |Enter Value|-1|

- AI側のDetection(MinDistance)を0にする
  - 接近時にプレイヤーを検知しないようにするため
- AIへ`vEventWithDelay.cs`追加

    |設定値|値|
    |---|---|
    |Delay|テイクダウン後、死亡判定を発生させる時間|
    |Event1つ目|vHealthController.AddHealth(-100)|
    |Event2つ目|Rigidbody.isKinematic = false|

- AIへTriggerGenericAction.csを設置

    |設定値|値|
    |---|---|
    |Animation|プレイヤーで再生したいアニメーションステート名|
    |Event > OnPressedAction|AI.Animator.PlayFixedTime("AI側のアニメーションステート名")|
    |Event > OnPressedAction|AI.vEventWithDelay.DoEvent(0)|

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
- 属性や増加値はvItemAttributeを増やして対応（vMeleeEquipment.csでAttribute→武器ダメージ反映が実行されるため）

### ダメージに新たな要素を追加する場合

- `vDamage.cs` にプロパティを追加
  - `public AttributeCompatibleDataList.Attribute attribute;`
- Inspectorに表示するために`vDamageDrawer.cs`に以下を追加

    ```cs[vDamageDrawer.cs]
    var attribute = property.FindPropertyRelative("attribute");

    // 66行目以降
    if (attribute != null)
    {
        position.y += 20;
        EditorGUI.PropertyField(position, attribute);
    }
    ```

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
