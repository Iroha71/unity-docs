# Qeust Machine

## 目次

- [Qeust Machine](#qeust-machine)
  - [目次](#目次)
  - [セットアップ](#セットアップ)
  - [クエスト設定方法](#クエスト設定方法)
  - [クエスト発行者の設定](#クエスト発行者の設定)
  - [クエスト状態変更時に任意のイベントを実行する](#クエスト状態変更時に任意のイベントを実行する)
  - [Journal UIを任意のUIに組み込む](#journal-uiを任意のuiに組み込む)
  - [LUAの拡張](#luaの拡張)

## セットアップ

1. QuestMachineプレハブを設置
2. QuestMachine > Quest Databaseをアタッチ
   - クエスト設定方法
3. プレイヤーへ`QuestJournal`をアタッチ
4. Quest Journal UI へ MessageEventsをアタッチ
5. MessageEvents > Snederへ**PausePlayer / UnPausePlayer**イベントを設定
6. UIPanel > OnOpen: QuestJournalUI.MessageEvents.SendToMessageSystem(0) / OnClose: QuestJournalUI.MessageEvents.SendToMessageSystem(1)

## クエスト設定方法

1. ID / Titleを入力
2. State毎にアクションを設定
   - Dialogue Text: QuestMachineの依頼ダイアログで表示される
     - Dialogue Systemと合わせるなら不要
   - Journal Text: クエストジャーナル（メニュー）で表示される
     - クエスト詳細が表示されるため、要設定
   - HUD Text: 画面上に表示されるクエスト名等を表示する
   - Conditions
     - 次ノードの遷移条件を設定する
     - SendMessage: TalkToNPC）←どこかでTalkToNPCがSendMessageされれば移行する

## クエスト発行者の設定

- 発行者オブジェクト > `QuestGiver`をアタッチ
  - Questsに発行予定クエストを設定

## クエスト状態変更時に任意のイベントを実行する

``` cs[HUD.cs]
[SerializeField]
private StringField questActivate;
[SerializeField]
private StringField questFinished;

// Start is called before the first frame update
void Start()
{
    // MessageSystemに特定のメッセージを登録する
    MessageSystem.AddListener(this, questActivate.value, string.Empty);
    MessageSystem.AddListener(this, questFinished.value, string.Empty);
}

// メッセージが発行された際に呼び出される
public void OnMessage(MessageArgs args)
  {
      if (args.message.Equals(questActivate.value))
      {
          DirectActivateQuest(questId: args.parameter, args.message);
      }
      else if (args.message.Equals(nextQuest.value))
      {
          // クエスト更新時の処理
      }
      else if (args.message.Equals(questFinished.value))
      {
          DirectFinishedQuest(questId: args.parameter, args.message);
      }
  }
```

- クエストオブジェクト > States > 各State > Actions > Messageで↑で指定したメッセージを入力

    |項目|値|
    |---|---|
    |Sender|Quest Giver or Questerer|
    |Target|Questerer|
    |parameter|クエスト名など|

## Journal UIを任意のUIに組み込む

1. QuestMachineプレハブ > QuestMachineConfigration > Hide Journal UI On Startのチェックを解除
2. Quest Journal UIを任意のCanvasへ移動させる
   - Animatorは削除
   - UIPanelのShow Animation Trigger / Hide Animation Triggerの中身を削除
   - Start StateをOpenへ設定
   - Deactivate On Hiddenを解除
3. QuestJournal > Journal UI へ↑のUIを割り当て

- Journalの内容を更新・開く場合はQuestJournal.ShowJournalUI()を呼び出す

## LUAの拡張

1. Pixcel Crusher > Dialogue System > Custom Function Lua Infoで作成
2. Script Functionsへ1行追加し、以下を設定
    |項目|値|
    |---|---|
    |Function Name|呼び出したいメソッド名|
    |Parameters|引数の型|
    |Return Value|戻り値|
3. 以下のようなスクリプトを作成する

    ``` cs[Sample.cs]
      using UnityEngine;
      using PixelCrushers.QuestMachine;
      using PixelCrushers.DialogueSystem;

      public class QuestFromDialogueContainer : MonoBehaviour
      {
          // Start is called before the first frame update
          void Start()
          {
              eventer.conversationEvents.onConversationEnd.AddListener(IgniteQuest);
              // Custom Lua Functionへメソッドを登録
              Lua.RegisterFunction("AddThisQuest", this, SymbolExtensions.GetMethodInfo(() => AddThisQuest(string.Empty)));
          }

          public async void IgniteQuest(Transform npc)
          {
              if (tempQuestId.Count <= 0)
                  return;

              await UniTask.Delay(1000);
              QuestMachine.GiveQuestToQuester(tempQuestId[0], "player");
          }

          public void AddThisQuest(string id)
          {
              tempQuestId.Add(id);
              Debug.Log(tempQuestId[0]);
          }
      }
    ```
