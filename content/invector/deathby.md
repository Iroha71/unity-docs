# Ragdoll

- Invector > Basic Locomotion > vRagdoll

## Death By AnimationWithRagdollのタイミング調整

- vAIMotor.cs
  - `info.normalizedTime >= 0.8f`の値を大きくする

  ```csharp [vAIMotor.cs]
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