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
  - [長押し・単押し出し分け](#長押し単押し出し分け)
  - [ボタン同時押し](#ボタン同時押し)
  - [追加ダメージ設定](#追加ダメージ設定)
    - [ダメージに新たな要素を追加する場合](#ダメージに新たな要素を追加する場合)
    - [状態異常等の実装](#状態異常等の実装)
  - [無敵化](#無敵化)
  - [戦闘状態移行](#戦闘状態移行)

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

    ``` cs
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

1. Animator設定（プレイヤー）
   1. **Base Layer** > テイクダウンアニメーション追加
   2. テイクダウンアニメーションに**vAnimatorTag**を追加し、**CustomAction**追加
2. Animator設定（敵側）
   1. **Fullbody** > テイクダウンされた際のアニメーション追加
   2. アニメーションに**vAnimatorTag**を追加し、以下を追加
      1. Dead / LockRotation / LockMovement （CustomActionだとRootがロックされる）
   3. アニメーションに**vAnimatorSetInt**を追加し、以下を設定

        |項目|値|
        |---|---|
        |Animator Parameter|ActionState|
        |Set On Enter|true|
        |Enter Value|-1|

   4. Deadステートへの遷移条件に**ActionState Equals 0**を追加
3. 敵側に**vTriggerGenericAction**オブジェクト追加
4. **vTriggerGenericAction**オブジェクトに**vEventWithDelay**追加
5. **vTriggerGenericAction**を以下のように設定

    |タブ名|設定項目|値|備考|
    |---|---|---|---|
    |Animation|Play Animation|1で追加アニメーション名|-|
    |Events|On Pressed Action Input|Animator.PlayFixedTime(2で追加したアニメーション名)|敵テイクダウン時のアニメーション再生|
    |^  |^  |vEventWithDelay.DoEvent(0)|-|
    |^  |^  |(Triggerオブジェクトの)BoxCollider.enabled = false|Triggerの無効化|
    |^  |^  |Rigidbody.isKinematic = true|重力の無効化|
    |^  |^  |StunManager.UnlockStun|スタンモジュールがある場合はスタンを解除するメソッドを←のように呼び出す|

6. **vEventWithDelay**を以下のように設定

    |要素番号|ディレイ|値|備考|
    |---|---|---|---|
    |0|0.8|vControlAI.AddHealth(-maxHealth)|強制的にHPを0にする(ディレイはどのタイミングで死亡判定を起こすかで決める)|

7. （任意）ステルスの場合
   1. AI側のDetection > MinDistanceを0にする
      1. 背後に接近時プレイヤーを検知しないようにするため

## 長押し・単押し出し分け

- 強攻撃出し分ける例

    ``` cs[SameButton.cs]
    public virtual void MeleeStrongAttackInput()
    {
        if (animator == null)
        {
            return;
        }

        if (strongAttackInput.GetButtonTimer(0.7f) && (!meleeManager.CurrentActiveAttackWeapon || meleeManager.CurrentActiveAttackWeapon.useStrongAttack) && MeleeAttackStaminaConditions())
        {
            isArtsReady = true;
            OnInputArtsAttack?.Invoke();
            return;
        }

        if (strongAttackInput.GetButtonDown() && isArtsReady)
            isArtsReady = false;

        if (strongAttackInput.GetButtonUp() && isArtsReady == false && (!meleeManager.CurrentActiveAttackWeapon || meleeManager.CurrentActiveAttackWeapon.useStrongAttack) && MeleeAttackStaminaConditions())
        {
            TriggerStrongAttack();
        }
    }
    ```

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

### 状態異常等の実装

![abnormal-status](/img/abnormal-status.png)

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

## 戦闘状態移行

![phase-control](/img/phase-control.png)
