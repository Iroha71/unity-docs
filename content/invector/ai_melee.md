# 予備動作実装

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



## 弾き

- vControlAI > Combat Settingsを編集
  - On Damage Blocking Chanceを任意の値に変更
  - Min Stay Blocking Time: 0
  - Max Stay Blocking Time: 0.1

- vConrolAICombat > ImmediateBlocking()

  ``` csharp
  protected virtual void ImmediateBlocking()
  {
      if (CheckChanceToBlock(_onDamageBlockingChance))
      {                
          _blockingTime = Random.Range(_minStayBlockingTime, _maxStayBlockingTime) + Time.time;
          isBlocking = true;
          // 弾き後の攻撃↓
          Attack(forceCanAttack: true);
      }          
  }
  ```