# ノイズ機能

- 音を出すオブジェクト

1. vNoiseObjectを取り付け
2. 音を発するタイミングで**vNoiseObject.TriggerNoise()**を呼び出し
   1. プレイヤーの足音 → **footTrigger.OnStep**で呼び出し

- 音を聞き取るオブジェクト

1. vNoiseListenerを取り付け
2. AIのステート作成

  ![noise-state](/img/noise-state.png)