# Horse Animset Pro

## Invector対応スクリプト

- MalbersAnimationのInputシステムを削除しておく
- [コントローラ対応スクリプト](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/RiderInput.cs){:target="_blank"}を作成
- 同階層の`vChangeInputTypeTrigger`に以下のイベントを設定

    |イベント|値|
    |---|---|
    |OnChangeKeyboard|RiderInput.SwitchInputType(true)|
    |OnChangeToJoystick|RiderInput.SwitchInputType(false)|
