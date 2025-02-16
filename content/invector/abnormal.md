# 状態異常等の実装

## クラス構成

- BuffData
  - 蓄積量や効果・時間を保持する
- BuffFactory
  - バフの生成者（データと生成指示を受け取り、生成する）
- StatusEffectReceiver
  - 状態異常ノードを受け取り、バフなどの生成指示を出す
  - バフの保持を行う
- StatusEffectGiver
  - 状態異常ノードをReceiverへ渡す
  - BuffNode
    - BuffDataやFactoryをまとめて設定・渡すクラス

![BuffData・Factoryの関係性](uml/buffdata_factory_relation.png)

![StatusEffectClass](uml/status_effect_class.png)

## BuffData / BuffFactory / Buff

```cs [BuffData.cs]
public class BuffData
{
  public float Duration;
  public float EffectValue;
  public int Stack;
  [HideInInspector]
  public int MaxStack;
}

public abstract class Buff
{
  public BuffData data;
  public Transform sender;
  public StatusEffectReceiver target;
  public UnityAction<Buff> OnRemoved;
  public abstract void Apply();
  public abstract void Remove();
}

public abstract class BuffFactory : ScriptableObject
{
  public abstract Buff GetBuff(Transform sender, StatusEffectReceiver target, BuffData data);
}
```

## 継承例

```cs [Poison.cs]
[CreateAssetMenu(menuName = ("Iroha/Status Effect/Poison"))]
public class PoisonBuffFactory : BuffFactory
{
  public override Buff GetBuff(Transform sender, StatusEffectReceiver target, BuffData data)
  {
    return new PoisonBuff()
    {
      sender = sender,
      target = target,
      data = data
    };
  }
}

public class PoisonBuff : Buff
{
  protected int currentStack;
  protected bool isEffecting = false;
  protected vHealthController targethealth;
  public override void Apply()
  {
    target.TryGetComponent(out targethealth);
    Debug.Log("apply");
    AddStack(data.Stack, target.PoisonRegist);
  }

  public override void Remove()
  {
    currentStack = 0;
    OnRemoved?.Invoke(this);
  }

  public void AddStack(int value, int limit)
  {
    if (currentStack >= limit)
      return;

    value = Mathf.Clamp(value + currentStack, 0, limit);
    if (value >= limit)
    {
      targethealth.StartCoroutine(PoisonCoroutine());
    }
  }

  protected IEnumerator PoisonCoroutine()
  {
    float startTime = Time.time;
    while (Time.time - startTime <= data.Duration && data.MaxStack <= currentStack)
    {
        yield return new WaitForSeconds(1f);
        targethealth.AddHealth((int)data.EffectValue);
        Debug.Log(targethealth.currentHealth);
    }
    Remove();
  }
}
```

## Receiver / Sender

```cs [StatusEffectGiver.cs]
using Invector.vMelee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectGiver : MonoBehaviour
{
    [SerializeField]
    private List<BuffNode> nodes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Send(Transform target)
    {
        target.TryGetComponent(out StatusEffectReceiver receiver);
        foreach (BuffNode node in nodes)
        {
            receiver.ApplyEffect(transform, node.factory, node.data);
        }
    }

    [System.Serializable]
    class BuffNode
    {
        public BuffData data;
        public BuffFactory factory;
    }
}
```

```cs [StatusEffectReceiver.cs]
using Invector.vItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

public class StatusEffectReceiver : MonoBehaviour
{
    public int PoisonRegist { get; set; } = 100;
    public List<Buff> activeBuff = new List<Buff>();
    private vItemManager itemManager;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out itemManager);
        if (itemManager != null)
        {
            itemManager.onUseItem.AddListener(UseItem);
        }
    }

    public void ApplyEffect(Transform sender, BuffFactory factory, BuffData data)
    {
        Buff buff = factory.GetBuff(sender, this, data);
        buff.OnRemoved += (_buff) => activeBuff.Remove(buff);
        buff.Apply();
    }

    private void UseItem(vItem item)
    {
        vItemAttribute statusEffectID = item.GetItemAttribute(vItemAttributes.StatusEffectID);
        if (statusEffectID == null)
            return;
        GameObject origin = Instantiate(item.originalObject);
        StatusEffectGiver giver = origin.GetComponent<StatusEffectGiver>();
        vItemAttribute targetmethod = item.GetItemAttribute(vItemAttributes.TargetMethod);
        // todo; targetmethodで対象を取得する
        giver.Send(null);
    }
}
```