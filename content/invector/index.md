# Invector Template

- [Melee](invector/melee.md)
- [Item Manager](invector/item-manager.md)
- [FSM AI](invector/ai.md)
- [魔術などの拡張](invector/arts.md)
- [投擲物の実装](invector/throwable.md)
- [弓の実装](invector/bow.md)

## Invectorに依存するコンポーネントについて

Invectorに依存するコンポーネント（Start時にGetComponentする）ものは

**DefaultExecutionOrder(5)** などを利用し、Invectorコンポーネントより

後にStartが実行されるようにする

## UIカーソルのパッド対応

- カーソル用のImageを配置
  - 最前面に配置する
  - RaycastTargetをオフにする
- background等のImageにvJoystickMouseInputをアタッチ
  - cursorに配置したImageを設定する

## カスタムタグがあるアニメーションか確認する

- プレイヤー
  - vThirdPersonController.IsAnimatorTag

- AI
  - vAIMotor.IsAnimatorTag

## 強制歩行

``` csharp
vThirdPersonController cc = GetComponent<vThirdPersonController>();
cc.alwaysWalkByDefault = true;
```

## ロックオンの強制解除

- **vLockOn**に以下を追記

    ``` csharp
    public void UnLockForce()
    {
        isLockingOn = false;
        LockOn(false);
    }
    ```

## キャラクター自身に機能追加する場合

- interfaceを作成する
- vThirdPersonMotor or vThirdPersonControllerに実装を行う
  - Motor: スタミナ・ローリングなどの動作定義・TakeDamage()の実装が行われている
- vEditorToolBarでインスペクタ上でグリッド表示が行える

## AIの調整項目

- vControlAI > Movement > StrafeSpeed
  - WalkSpeed
  - RunningSpeed
- vControlAI > Agent
  - Aceleration: 50
  - Stoping Distance: 1
- vControlAI > Combat Settings
  - Attack Distance: 1.25
  - Min Distance Of The Target: 1
  - Combat Distance: 2.5

## 演出強化

### ヒットストップ

- 前提
  - DOTWEENが導入されている / UniTaskが導入されている
- playerの子要素に以下のスクリプトを追加
  - [BattleSenceStrenthen](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/BattleSenceStrengthen.cs){:target="_blank"}
- Animatorに`MoveTarget`イベントを追加する

### 攻撃エフェクト

- 画像のように武器種毎のエフェクト管理リストを作成

    ![attackeffect-list](/img/attackeffect-datalist.png)

- エフェクトの発生を行うスクリプトを作成する

    ``` csharp
    public class EffectEmitter : MonoBehaviour
    {
        /// <summary>
        /// エフェクトを対象オブジェクトに出現させる
        /// </summary>
        /// <param name="effect">エフェクト</param>
        /// <param name="position">ローカル座標</param>
        /// <param name="rotation">ローカル角度</param>
        public void CreateAttackEffet(vDestroyGameObject effect, Vector3 position, Vector3 rotation)
        {
            vDestroyGameObject _effect = Instantiate(effect, transform);
            _effect.transform.localPosition = position;
            _effect.transform.localEulerAngles = rotation;
        }
    }
    ```

- 装備に**vAnimatorEventReceiver**を追加
  
    |項番|イベント名|
    |---|---|
    |1|Start_Weak|
    |2|Start_Strong|
    |3|Start_Other|

- 装備からエフェクトを発生させるスクリプトを作成し、装備に追加

    ``` csharp
    [RequireComponent(typeof(vAnimatorEventReceiver))]
    public class EquipmentEffectEmitter : MonoBehaviour
    {
        [SerializeField] private AttackEffectDataList dataList;
        private EffectEmitter emitter;
        private Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            emitter = GetComponentInParent<EffectEmitter>();
            anim = GetComponentInParent<Animator>();
        }

        /// <summary>
        /// 攻撃エフェクトを実行する
        /// </summary>
        /// <param name="animEvent"></param>
        public void EmitEffect(string animEvent)
        {
            AttackEffectData data = GetAttackEffect(animEvent);
            if (data == null)
                return;

            emitter.CreateAttackEffet(data.effect, data.position, data.rotation);
        }

        /// <summary>
        /// エフェクト一覧からアニメーションにあったデータを取得
        /// </summary>
        /// <param name="animEvent"></param>
        /// <returns>エフェクト（またはnull）</returns>
        private AttackEffectData GetAttackEffect(string animEvent)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(7);
            AttackEffectData.Type type = AttackEffectData.Type.OTHER;
            if (animEvent.Equals("Start_Weak"))
                type = AttackEffectData.Type.WEAK;
            else if (animEvent.Equals("Start_Strong"))
                type = AttackEffectData.Type.STRONG;

            return dataList.effects.Find(effect => effect.type == type && state.IsName(effect.stateName));
        }
    }
    ```
