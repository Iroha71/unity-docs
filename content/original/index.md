# 自作スクリプト

- [自作スクリプト](#自作スクリプト)
  - [スローモーション演出秒数](#スローモーション演出秒数)
  - [戦闘補助](#戦闘補助)
  - [Horse Animset ProのInvector対応](#horse-animset-proのinvector対応)
  - [マップ機能実装](#マップ機能実装)
    - [TIPS](#tips)
  - [シーンロード完了時の工夫](#シーンロード完了時の工夫)

## スローモーション演出秒数

- TimeScale: 0.1s
- 継続時間: 現実時間で0.5s

## 戦闘補助

- 前提
  - DOTWEENが導入されている / UniTaskが導入されている
- [BattleSenceStrenthen](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/BattleSenceStrengthen.cs){:target="_blank"}
  - Animatorに`MoveTarget`イベントを追加する

## Horse Animset ProのInvector対応

- [RiderInput](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/RiderInput.cs){:target="_blank"}
- 同階層に`VChangeInputTypeTrigger`を設置
  - `OnChangeKeyboard` / `OnChangeJoyStick`へ`RiderInput.SwitchInputType`を設定する

## マップ機能実装

### TIPS

- 3d座標→2d座標に変換

  ```cs[worldpos.cs]
  Vector3 screenPosition = mapCamera.WorldToScreenPoint(worldPosition);
  RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImage, screenPosition, null, out Vector2 localpoint);
  transform.localPosition = localpoint;
  ```

## シーンロード完了時の工夫

- シーン開始時に暗転状態から始める
  - Start時に呼び出し

  ```cs[fade.cs]
  public async void HideScene()
  {
      canvas.enabled = GetComponent<Canvas>();
      canvasGroup.alpha = 1f;
      AudioListener.volume = 0f;
      await UniTask.Delay(500);
      await canvasGroup.DOFade(0f, 0.5f);
      AudioListener.volume = 1f;
  }
  ```
