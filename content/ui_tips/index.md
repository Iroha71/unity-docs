# UI TIPS

## TIPS

- EventSystem.current.SetCurrentSelectGameObject()をする前に1フレーム待機する
  - 正常に選択されない場合がある

## MVPパターンを適用したスタック可能UI

- [コード例](https://github.com/Iroha71/unity-docs/tree/develop/assets/origin-scripts/UI)

### 画面遷移のUML（戦技設定画面例）

![ui_class](/uml/ui_class.png)
![ui_framework](/uml/ui_framework_stack.png)
![ui_framework_weapon_select](/uml/ui_framework_weapon_select.png)