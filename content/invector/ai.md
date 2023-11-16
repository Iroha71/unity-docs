# Invector-AI

- [Invector-AI](#invector-ai)
  - [新しいパラメータの追加（スタミナなど）](#新しいパラメータの追加スタミナなど)
  - [ステートの追加](#ステートの追加)
    - [Actionの追加（メッセンジャー利用）](#actionの追加メッセンジャー利用)
  - [ノイズ機能](#ノイズ機能)

## 新しいパラメータの追加（スタミナなど）

- Interfaceの作成
  - `IStaminaAI.cs`等
  - `vIControlAI.cs`を実装する
    - vControlAIはAIのパラメータを実装する

``` cs[IStaminaAI.cs]
public partial interface IStaminaAI : vIControlAI
{
    int Stamina { get; }
    int MaxStamina { get; }
    UnityAction OnOutOfStamina { get; set; }
    void AddStamina(int value);
}
```

- 作成したInterfaceを実装したスクリプトを実装
  - AIにパラメータを持たせたい場合は↓のように`partial class`にしておく
    - partialにした場合`Start() / Update()`は`vControlAI.cs`にある

``` cs[vControlAICombatStamina.cs]
namespace Invector.vCharacterController.AI
{
    public partial class vControlAICombat : IStaminaAI
    {
        private int stamina;
        public int Stamina { get { return stamina; } }

        [vEditorToolbar("Stamina")]
        [SerializeField]
        private int maxStamina;
        public int MaxStamina { get { return maxStamina; } }

        public UnityAction OnOutOfStamina { get; set; }

        public void AddStamina(int value)
        {
            stamina = Mathf.Clamp(value + stamina, 0, maxStamina);
            if (stamina <= 0)
            {
                OnOutOfStamina?.Invoke();
            }
        }
        // vControlAI > Start()から呼び出す
        private void InitStamina()
        {
            stamina = maxStamina;
            onReceiveDamage.AddListener((damage) => AddStamina(-(int)damage.staminaDamage));
        }
    }
}
```

## ステートの追加

- DecisionとActionのスクリプトを作成
- Anyステートから伸びるステートの場合、既存のステートにも新しいDecisionを追加しておく
- アニメーションを実装する場合は`anim.CrossFade()`を利用する
  - Playだと正常に実行されない

### Actionの追加（メッセンジャー利用）

- **AIFSM**ウィンドウで**Action > Controller > Send Message**を選択

    |設定項目|値|
    |---|---|
    |Execution Type|OnStateEnter|
    |Listener Name|任意|
    |Message|引数を渡したい場合は追加|

- AIに`vMessageReceiver.cs`追加
  - **Message Listeners**に以下を追加

    |設定項目|値|
    |---|---|
    |Name|上記で追加したListener Name|
    |OnReceiveMessage|実行したいメソッド|

- スクリプトで任意のタイミングでメッセージを送りたい場合
  - `fsmBehaviour.messageReceiver.Send("Listener Name")`

## ノイズ機能

- 音を出すオブジェクト

1. vNoiseObjectを取り付け
2. 音を発するタイミングで**vNoiseObject.TriggerNoise()**を呼び出し
   1. プレイヤーの足音 → **footTrigger.OnStep**で呼び出し

- 音を聞き取るオブジェクト

1. vNoiseListenerを取り付け
2. AIのステート作成

  ![noise-state](/img/noise-state.png)
