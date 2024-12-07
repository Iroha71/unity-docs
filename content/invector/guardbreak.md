# ガード関係

## ガードブレイク

- ダメージ設定の**IgnoreDefence**をオンにする

- **vControlAICombat** > **TryBlockAttack()**を以下のように変更

    ``` csharp
    protected virtual void TryBlockAttack(vDamage damage)
    {
        var canBlock = !ignoreDefenseDamageTypes.Contains(damage.damageType) && !damage.ignoreDefense;
        if (string.IsNullOrEmpty(damage.damageType) && canBlock)
        {
            ImmediateBlocking();              
        }

        // 新しく追加
        if (damage.ignoreDefense)
        {
            isBlocking = false;
        }
        damage.hitReaction = !isBlocking || !canBlock;
    }
    ```

## パリィ

### アニメーション設定

- パリィされた際のRecoilIDを決めておく
- プレイヤー
  - アニメーターにパラメータを追加する
    - TriggerParry: Trigger
  - Fullbody > HitRecoilに弾きモーションを追加する
    - 遷移条件にTriggerParryを設定する
- 敵
  - Fullbody > HitRecoilにパリィされた際のアニメーションを設定する
    - 遷移条件にTriggerRecoil・RecoilID＝あらかじめ決めたIDを設定する


### パリィ用スクリプト作成

- 以下のスクリプトを作成し、プレイヤーに取り付ける

    ``` csharp
    public class BreakManager : MonoBehaviour
    {
        [SerializeField]
        private int parryReceiveMs = 200;
        public bool IsParring { get; set; } = false;
        private Animator anim;
        private vShooterMeleeInput tpInput;
        private bool isPressingBlock = false;
        private CancellationToken _token;
        private CancellationTokenSource cts;

        // Start is called before the first frame update
        void Start()
        {
            TryGetComponent(out anim);
            TryGetComponent(out tpInput);
        }

        // Update is called once per frame
        void Update()
        {
            if (tpInput.isBlocking && !isPressingBlock)
            {
                isPressingBlock = true;
                ActivateParryMode();
            }

            if (!tpInput.isBlocking && isPressingBlock)
            {
                isPressingBlock = false;
                cts.Cancel();
            }
        }

        /// <summary>
        /// パリィの受付開始
        /// </summary>
        public async void ActivateParryMode()
        {
            cts = new CancellationTokenSource();
            _token = cts.Token;
            IsParring = true;
            // UniTaskのためキャンセル例外をキャッチする
            try
            {
                await UniTask.Delay(parryReceiveMs, cancellationToken: _token);
                IsParring = false;
            }
            catch (OperationCanceledException)
            {
                IsParring = false;
            }
        }

        /// <summary>
        /// パリィ実行
        /// </summary>
        /// <param name="attacker">攻撃してきたオブジェクト</param>
        public async void Parry(vIMeleeFighter attacker)
        {
            anim.SetTrigger("TriggerParry");
            // あらかじめ決めたパリィされた際のRecoilIDを指定する
            attacker.BreakAttack(1);
            await UniTask.DelayFrame(20, cancellationToken: destroyCancellationToken);
            Time.timeScale = 0.3f;
            
            await UniTask.Delay(200, cancellationToken: destroyCancellationToken, ignoreTimeScale: true);
            Time.timeScale = 1f;
        }
    }
    ```

### 仕様

![just_guard](/uml/just_guard.png)
