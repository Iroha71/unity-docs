# ローリングキャンセルの実装

## ローリング設定値

|設定項目|値|
|---|---|
|Roll Transition|0|
|Roll Speed|1.5|
|Roll RotationSpeed|55|

## ローリング後の硬直解消

1. Rollステート選択
2. インスペクタ > Transitions > 各遷移を選択
3. 遷移間隔を調整する

## ローリングキャンセル

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