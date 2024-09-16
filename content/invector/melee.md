# Melee Template

## 目次

- [Melee Template](#melee-template)
  - [目次](#目次)
  - [ガードブレイク](#ガードブレイク)
  - [パリィ](#パリィ)
    - [仕様](#仕様)
  - [ローリング](#ローリング)
  - [Ragdoll](#ragdoll)
  - [Death By AnimationWithRagdollのタイミング調整](#death-by-animationwithragdollのタイミング調整)
  - [スタン状態の実装](#スタン状態の実装)
  - [テイクダウン](#テイクダウン)
  - [長押し・単押し出し分け](#長押し単押し出し分け)
  - [ボタン同時押し](#ボタン同時押し)
  - [追加ダメージ設定](#追加ダメージ設定)
    - [vDamageObjectのTips](#vdamageobjectのtips)
    - [ダメージに新たな要素を追加する場合](#ダメージに新たな要素を追加する場合)
    - [ダメージに新たな要素を追加する場合（vMeleeControlに追加）](#ダメージに新たな要素を追加する場合vmeleecontrolに追加)
    - [状態異常等の実装](#状態異常等の実装)
  - [無敵化](#無敵化)
  - [戦闘状態移行](#戦闘状態移行)
  - [左手にも武器を装備する](#左手にも武器を装備する)

## ガードブレイク

- ダメージ設定の**IgnoreDefence**をオンにする

- **vControlAICombat** > **TryBlockAttack()**を以下のように変更

    ``` csharp
    protected virtual void TryBlockAttack(vDamage damage)
    {
        var canBlock = !ignoreDefenseDamageTypes.Contains(damage.damageType) && !damage.ignoreDefense;
        if (string.IsNullOrEmpty(damage.damageType) && canBlock)
        {
            ImmediateBlocking();              
        }

        // 新しく追加
        if (damage.ignoreDefense)
        {
            isBlocking = false;
        }
        damage.hitReaction = !isBlocking || !canBlock;
    }
    ```

## パリィ

1. Player > Animator > Fullbody > Hit Recoilに以下のステートを追加する

    |項目|値|
    |---|---|
    |アニメーション|パリィモーション|
    |遷移条件|ActionState = -1, TriggerRecoil|

    1. AIにパリィを実装する場合はPlayer側に以下を追加

        |項目|値|
        |---|---|
        |アニメーション|パリィされたときのモーション|
        |遷移条件|RecoilId = -2, TriggerRecoil|

2. 敵側 > Animator > Fullbody > Hit Recoilに以下のステートを追加する

    |項目|値|
    |---|---|
    |アニメーション|パリィされたときのモーション|
    |遷移条件|RecoilId = -1, TriggerRecoil|

    1. AIにパリィを実装する場合はAI側に以下を追加

        |項目|値|
        |---|---|
        |アニメーション|パリィモーション|
        |遷移条件|ActionState = -2, TriggerRecoil|

3. 以下のスクリプトを作成

    ``` csharp
    private vIMeleeFighter fighter;
    [SerializeField] private vMeleeWeapon weapon;
    [SerializeField] private float receiveTime = 0.5f;
    [SerializeField] private ParticleSystem defenceEffect;
    [SerializeField] private ParticleSystem parryEffect;
    private CancellationToken token;
    private float elapsedTime = 0f;
    private int originalRecoilID;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        fighter = GetComponentInParent<vIMeleeFighter>();
        weapon.onDefense.AddListener(CheckParriable);
        token = this.destroyCancellationToken;
        anim = fighter.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fighter.isBlocking)
        {
            // ガード開始時に受付開始
            if (elapsedTime <= 0)
                EnableParry();
            elapsedTime += Time.deltaTime;
            // 受付時間を越えたら無効化
            if (elapsedTime >= receiveTime)
                DisableParry();
        }

        // パリィ受付中且つガード解除時
        if (elapsedTime > 0 && !fighter.isBlocking)
        {
            elapsedTime = 0f;
            DisableParry();
        }
    }

    // パリィ受付開始
    private void EnableParry()
    {
        weapon.breakAttack = true;
        originalRecoilID = weapon.recoilID;
        weapon.recoilID = -1;
        anim.SetInteger("ActionState", -1);
    }

    // パリィ解除
    private void DisableParry()
    {
        weapon.breakAttack = false;
        weapon.recoilID = originalRecoilID;
        anim.SetInteger("ActionState", 0);
    }

    // 防御時にパリィ可能か判断し、実行する
    private async void CheckParriable()
    {
        // パリィ時
        if (elapsedTime <= receiveTime)
        {
            // パリィエフェクト
            ParticleSystem _effect = Instantiate(parryEffect);
            _effect.transform.localPosition = (transform.forward * 0.5f) + transform.position;
            try
            {
                // スロー演出
                Time.timeScale = 0.3f;
                await UniTask.Delay(500, DelayType.Realtime, cancellationToken: token);
                Time.timeScale = 1f;
            }
            catch (OperationCanceledException e)
            {
                Time.timeScale = 1f;
            }
        }
        // 通常ガード時
        else if (elapsedTime > receiveTime)
        {
            ParticleSystem _effect = Instantiate(defenceEffect);
            _effect.transform.localPosition = (transform.forward * 0.5f) + transform.position;
        }

        // パリィ受付時間を加算し、1度のみパリィ受付を行うようにする
        elapsedTime += receiveTime;
    }
    ```

### 仕様

- パリィ受付開始時

  ![parry-enable](/img/parry-enable.png)

- パリィ発生時

  ![parry](/img/parry.png)

- パリィ受付終了

  ![parry-disable](/img/parry-disable.png)

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
  - RollサブステートをFullbodyレイヤーに移動する
  - vThirdPersonController > Roll()のアニメーションレイヤーをfullbodyに変更

   ``` csharp
   public virtual void Roll()
    {
        OnRoll.Invoke();
        isRolling = true;
        // fullbodyLayerに変更
        animator.CrossFadeInFixedTime("Roll", rollTransition, fullbodyLayer);
        ReduceStamina(rollStamina, false);
        currentStaminaRecoveryDelay = 2f;
    }
   ```

  - vMeleeCombatInput > RollConditions()の&& !isAttackingを削除
    - IsAnimatorHasTagを使ってタイミングを制御してもいい

    ``` csharp
    public override bool RollConditions()
    {
        return base.RollConditions() && !animator.IsInTransition(cc.upperBodyLayer) && !animator.IsInTransition(cc.fullbodyLayer);
    }
    ```

## Ragdoll

- Invector > Basic Locomotion > vRagdoll

## Death By AnimationWithRagdollのタイミング調整

- vAIMotor.cs
  - `info.normalizedTime >= 0.8f`の値を大きくする

    ``` csharp[vAIMotor.cs]
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

    ``` csharp[AddStun.cs]
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

    ``` csharp[Stun.cs]
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

    ``` csharp[Update.cs]
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

    ``` csharp[Stun.cs]
    private void ChangeStunState(StunState state)
    {
        // AI挙動を再開
        vAIControl.EnableAIController();
        // スタン用アニメーション解除
        Animator.SetBool("IsStun", false);
    }
    ```

## テイクダウン

1. Animator設定（プレイヤー）
   1. **Base Layer** > テイクダウンアニメーション追加する
   2. テイクダウンアニメーションに**vAnimatorTag**を追加し、**CustomAction**を設定する
2. Animator設定（敵側）
   1. **Fullbody** > テイクダウンされた際のアニメーション追加
   2. アニメーションに**vAnimatorTag**を追加し、以下を追加
      1. CustomAction
   3. アニメーションに**vAnimatorSetInt**を追加し、以下を設定
      1. Deadアニメーションが発動しないようにするため
      2. 必要に応じてスタン用フラグを無効化するSetBoolも追加する

        |項目|値|
        |---|---|
        |Animator Parameter|ActionState|
        |Set On Enter|true|
        |Enter Value|-1|
        |Set On Exit|true|
        |Exit Value|0|

   4. Deadステートへの遷移条件に**ActionState Equals 0**を追加
3. 敵側に**vTriggerGenericAction**オブジェクト追加
4. **vTriggerGenericAction**オブジェクトに**vEventWithDelay**追加
5. 敵側のルートモーション位置の反映を行うコードを追加
    - 敵の任意コンポーネントにコードを追加

    ``` csharp
    private void OnAnimatorMove()
    {
        if (isTakedown == false) return;
        transform.position = anim.rootPosition;
    }
    ```

6. **vTriggerGenericAction**を以下のように設定

    |タブ名|設定項目|値|備考|
    |---|---|---|---|
    |Trigger|Destroy After|false|vEventDelayが呼び出しに失敗しないようにオブジェクトは生存させる|
    |Animation|Play Animation|テイクダウンアニメーション|-|
    |Events|On Pressed Action Input|Animator.PlayFixedTime(2で追加したアニメーション名)|敵側のテイクダウンされたアニメーション|
    |^  |^  |vEventWithDelay.DoEvent(0)|-|
    |^  |^  |(Triggerオブジェクトの)BoxCollider.enabled = false|Triggerの無効化|
    |^  |^  |Rigidbody.isKinematic = true|物理の無効化|
    |^  |^  |OnAnimatorMoveを呼び出すコンポーネント.isTakedown = true|ルートモーションの位置をキャラに反映する|
    |^  |^  |StunManager.UnlockStun|スタンモジュールがある場合はスタンを解除するメソッドを←のように呼び出す|

7. **vEventWithDelay**を以下のように設定

    |要素番号|ディレイ|値|備考|
    |---|---|---|---|
    |0|0.8|vControlAI.AddHealth(-maxHealth)|強制的にHPを0にする(ディレイはどのタイミングで死亡判定を起こすかで決める)|

8. テイクダウンアニメーション終了時の処理

    |タブ名|設定項目|値|備考|
    |---|---|---|---|
    |Events|On Finish Animationなど|(Triggerオブジェクトの)BoxCollider.enabled = true|Trigger再有効化|
    |^  |^  |OnAnimatorMoveを呼び出すコンポーネント.isTakedown = false|ルートモーション反映無効化|
    |^  |^  |Triggerオブジェクト.SetActive = false|Trigger自体の無効化|
    |^  |^  |Rigidbody.isKinematic = false|物理の有効化|

9. （任意）ステルスの場合
   1. AI側のDetection > MinDistanceを0にする
      1. 背後に接近時プレイヤーを検知しないようにするため

## 長押し・単押し出し分け

- 強攻撃出し分ける例

    ``` csharp[SameButton.cs]
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

``` csharp[SameButton.cs]
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

### vDamageObjectのTips

ダメージオブジェクトそのものにアタッチすると複数回ヒット判定が起こることがある。
→そのため子要素にvDamageObjectを設置する。

- 子要素に空のオブジェクトを作成し、vDamageObjectをアタッチする
- vDamageObject > OnHitで自身のオブジェクトのSetActive(false)等を呼び出す

### ダメージに新たな要素を追加する場合

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

    ``` csharp [vDamageDrawer.cs]
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

### ダメージに新たな要素を追加する場合（vMeleeControlに追加）

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

  ``` csharp
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

  ``` csharp
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

### 状態異常等の実装

![status_effect_class](/uml/status_effect_class.png)

- 攻撃側の処理

    ![status_effect_attacker](/uml/status_effect_attacker.png)

- 被弾側の処理

    ![status_effect](/uml/status_effect_sequence.png)

## 無敵化

1. 以下のようなフラグを実装するインターフェースを作成する

    ``` csharp[IInvicible.cs]
    public interface IInvincible
    {
        bool IsInvincible { get; set; }
    }
    ```

2. `vThirdPersonMotor.cs`へ実装する
3. `vThirdPersonMotor.cs` > `TakeDamage`の条件式にフラグを追加する

    ``` csharp[vThirdPersonMortor.cs]
    if (currentHealth <= 0 || (IgnoreDamageRolling()) || IsInvincible)
            {
                if (damage.activeRagdoll && !IgnoreDamageActiveRagdollRolling())
                {
                    onActiveRagdoll.Invoke(damage);
                }

                return;
            }
    ```

## 戦闘状態移行

![phase-control](/img/phase-control.png)

## 左手にも武器を装備する

左武器での攻撃用ボタンには使用していないボタンか
ボタンの組み合わせが必要。（LBやLT+RTなど）
<br/>
左攻撃の入力関数を作成し、
Interfaceを通して目的のコンポーネントの関数を実行する。

以下はLT+RTの例
<br/>

- **vMeleeCombatInput**に左武器攻撃用の関数を作成する

    ``` csharp
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

    ``` csharp
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