## 新しいパラメータの追加（スタミナなど）

- Interfaceの作成
  - `IStaminaAI.cs`等
  - `vIControlAI.cs`を実装する
    - vControlAIはAIのパラメータを実装する

``` csharp[IStaminaAI.cs]
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

``` csharp[vControlAICombatStamina.cs]
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
