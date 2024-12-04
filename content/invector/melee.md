# Melee Template

## 目次

- [Melee Template](#melee-template)
  - [目次](#目次)
  - [Ragdoll](#ragdoll)
  - [Death By AnimationWithRagdollのタイミング調整](#death-by-animationwithragdollのタイミング調整)
  - [戦闘状態移行](#戦闘状態移行)

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







## 戦闘状態移行

![phase-control](/img/phase-control.png)

