# Qeust Machine

## セットアップ

1. QuestMachineプレハブを設置
2. QuestMachine > Quest Databaseをアタッチ
   - クエスト設定方法
3. プレイヤーへ`QuestJournal`をアタッチ

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
  - Sender: Quest Giver or Questerer, Target: Questerer, parameter: クエスト名など

## Journal UIを任意のUIに組み込む

1. QuestMachineプレハブ > QuestMachineConfigration > Hide Journal UI On Startのチェックを解除
2. Quest Journal UIを任意のCanvasへ移動させる
   - Animatorは削除
   - UIPanelのShow Animation Trigger / Hide Animation Triggerの中身を削除
   - Start StateをOpenへ設定
   - Deactivate On Hiddenを解除
3. QuestJournal > Journal UI へ↑のUIを割り当て

- Journalの内容を更新・開く場合はQuestJournal.ShowJournalUI()を呼び出す
