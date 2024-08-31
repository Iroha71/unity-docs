# Invector-AI

- [Invector-AI](#invector-ai)
  - [新しいパラメータの追加（スタミナなど）](#新しいパラメータの追加スタミナなど)
  - [ステートの追加](#ステートの追加)
    - [Actionの追加（メッセンジャー利用）](#actionの追加メッセンジャー利用)
  - [ノイズ機能](#ノイズ機能)
  - [予備動作実装](#予備動作実装)

## 新しいパラメータの追加（スタミナなど）

- Interfaceの作成
  - `IStaminaAI.cs`等
  - `vIControlAI.cs`を実装する
    - vControlAIはAIのパラメータを実装する

``` csharp[IStaminaAI.cs]
public partial interface IStaminaAI : vIControlAI
{
    int Stamina { get; }
    int MaxStamina { get; }
    UnityAction OnOutOfStamina { get; set; }
    void AddStamina(int value);
}
```

- 作成したInterfaceを実装したスクリプトを実装
  - AIにパラメータを持たせたい場合は↓のように`partial class`にしておく
    - partialにした場合`Start() / Update()`は`vControlAI.cs`にある

``` csharp[vControlAICombatStamina.cs]
namespace Invector.vCharacterController.AI
{
    public partial class vControlAICombat : IStaminaAI
    {
        private int stamina;
        public int Stamina { get { return stamina; } }

        [vEditorToolbar("Stamina")]
        [SerializeField]
        private int maxStamina;
        public int MaxStamina { get { return maxStamina; } }

        public UnityAction OnOutOfStamina { get; set; }

        public void AddStamina(int value)
        {
            stamina = Mathf.Clamp(value + stamina, 0, maxStamina);
            if (stamina <= 0)
            {
                OnOutOfStamina?.Invoke();
            }
        }
        // vControlAI > Start()から呼び出す
        private void InitStamina()
        {
            stamina = maxStamina;
            onReceiveDamage.AddListener((damage) => AddStamina(-(int)damage.staminaDamage));
        }
    }
}
```

## ステートの追加

- DecisionとActionのスクリプトを作成
- Anyステートから伸びるステートの場合、既存のステートにも新しいDecisionを追加しておく
- アニメーションを実装する場合は`anim.CrossFade()`を利用する
  - Playだと正常に実行されない

### Actionの追加（メッセンジャー利用）

- **AIFSM**ウィンドウで**Action > Controller > Send Message**を選択

    |設定項目|値|
    |---|---|
    |Execution Type|OnStateEnter|
    |Listener Name|任意|
    |Message|引数を渡したい場合は追加|

- AIに`vMessageReceiver.cs`追加
  - **Message Listeners**に以下を追加

    |設定項目|値|
    |---|---|
    |Name|上記で追加したListener Name|
    |OnReceiveMessage|実行したいメソッド|

- スクリプトで任意のタイミングでメッセージを送りたい場合
  - `fsmBehaviour.messageReceiver.Send("Listener Name")`

## ノイズ機能

- 音を出すオブジェクト

1. vNoiseObjectを取り付け
2. 音を発するタイミングで**vNoiseObject.TriggerNoise()**を呼び出し
   1. プレイヤーの足音 → **footTrigger.OnStep**で呼び出し

- 音を聞き取るオブジェクト

1. vNoiseListenerを取り付け
2. AIのステート作成

  ![noise-state](/img/noise-state.png)

## 予備動作実装

``` csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Invector.vCharacterController.AI;

/// <summary>
/// 攻撃の予備動作を遅くする処理を実装する。（拡張版）
/// </summary>
public class AnticipationEmphasizerAdvanced : StateMachineBehaviour
{
    private float defaultAnimSpeed;
    [Tooltip("予備動作が始まる時間（ENTERなら不要）")]
    public float anticipationStartTime = 0f;
    [Tooltip("予備動作終了時間")]
    public float anticipatingEndTime = 0.5f;
    [Tooltip("予備動作中のAnimation速度")]
    public float anticipationSpeed = 0.4f;
    
    [Tooltip("予備動作に入ったか")]
    private bool isEntered = false;
    private vAIMotor ai;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
            ai = animator.GetComponent<vAIMotor>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 予備動作のタイミングに突入 & 予備動作前の状態
        if (stateInfo.normalizedTime % 1 >= anticipationStartTime && isEntered == false)
        {
            defaultAnimSpeed = ai.animatorSpeed;
            ChangeAnimSpeed(anticipationSpeed, animator);
            isEntered = true;
        }

        // 予備動作終了タイミングに突入 & 予備動作が実行済み
        if (stateInfo.normalizedTime % 1 >= anticipatingEndTime && isEntered)
        {
            ChangeAnimSpeed(defaultAnimSpeed, animator);
            isEntered = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.animatorSpeed = defaultAnimSpeed;
        animator.speed = defaultAnimSpeed;
        isEntered = false;
    }

    /// <summary>
    /// アニメーション速度を変更する
    /// </summary>
    /// <param name="animSpeed">変更後の速度</param>
    /// <param name="animator">対象Animator</param>
    private void ChangeAnimSpeed(float animSpeed, Animator animator)
    {
        ai.animatorSpeed = animSpeed;
        animator.speed = animSpeed;
    }

    /// <summary>
    /// 予備動作の為に一定時間待機する
    /// </summary>
    /// <param name="animator">アニメーター</param>
    /// <param name="anticipationTime">予備動作にかかる時間</param>
    private async void DelayAnimSpeedRealtime(Animator animator, float anticipationTime)
    {
        await UniTask.Delay((int)(anticipationTime * 1000), DelayType.Realtime);
        ai.animatorSpeed = defaultAnimSpeed;
        animator.speed = defaultAnimSpeed;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
```

## 参考にできるFSM

- ダメージを受けたらアクションを起こしたい
  - FSM_CivilianToShooterを参照

- 武器を取りに行く
  - FSM_CivilianToShooterを参照
  - vAISendMessage→vMessageReceiver→vAIMoveToPosition.Move()
  - vFSMChangeBehaviourでCivilianToShooter→ShooterSniperへ変更

  ![sniper](/uml/take_item.png)
