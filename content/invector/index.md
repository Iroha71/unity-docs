# Invector Template

- [Melee](invector/melee.md)
- [Item Manager](invector/item-manager.md)
- [FSM AI](invector/ai.md)
- [魔術などの拡張](invector/arts.md)

## 基本フロー

### キャラクター自身に機能追加する場合

- interfaceを作成する
- vThirdPersonMotor or vThirdPersonControllerに実装を行う
  - Motor: スタミナ・ローリングなどの動作定義・TakeDamage()の実装が行われている
- vEditorToolBarでインスペクタ上でグリッド表示が行える
