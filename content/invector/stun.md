# スタン状態の実装

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
