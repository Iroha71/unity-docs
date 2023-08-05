# 自作スクリプト

## 戦闘補助

- 前提
  - DOTWEENが導入されている / UniTaskが導入されている
- [BattleSenceStrenthen](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/BattleSenceStrengthen.cs){:target="_blank"}
  - Animatorに`MoveTarget`イベントを追加する

## Horse Animset ProのInvector対応

- [RiderInput](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/RiderInput.cs){:target="_blank"}
- 同階層に`VChangeInputTypeTrigger`を設置
  - `OnChangeKeyboard` / `OnChangeJoyStick`へ`RiderInput.SwitchInputType`を設定する
