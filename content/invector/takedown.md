# テイクダウン

1. Animator設定（プレイヤー）
   1. **Base Layer** > テイクダウンアニメーション追加する
   2. テイクダウンアニメーションに**vAnimatorTag**を追加し、**CustomAction**を設定する
2. Animator設定（敵側）
   1. **Fullbody** > テイクダウンされた際のアニメーション追加
   2. アニメーションに**vAnimatorTag**を追加し、以下を追加
      1. CustomAction
   3. アニメーションに**vAnimatorSetInt**を追加し、以下を設定
      1. Deadアニメーションが発動しないようにするため
      2. 必要に応じてスタン用フラグを無効化するSetBoolも追加する

        |項目|値|
        |---|---|
        |Animator Parameter|ActionState|
        |Set On Enter|true|
        |Enter Value|-1|
        |Set On Exit|true|
        |Exit Value|0|

   4. Deadステートへの遷移条件に**ActionState Equals 0**を追加
3. 敵側に**vTriggerGenericAction**オブジェクト追加
4. **vTriggerGenericAction**オブジェクトに**vEventWithDelay**追加
5. 敵側のルートモーション位置の反映を行うコードを追加
    - 敵の任意コンポーネントにコードを追加

    ``` csharp
    private void OnAnimatorMove()
    {
        if (isTakedown == false) return;
        transform.position = anim.rootPosition;
    }
    ```

6. **vTriggerGenericAction**を以下のように設定

    |タブ名|設定項目|値|備考|
    |---|---|---|---|
    |Trigger|Destroy After|false|vEventDelayが呼び出しに失敗しないようにオブジェクトは生存させる|
    |Animation|Play Animation|テイクダウンアニメーション|-|
    |Events|On Pressed Action Input|Animator.PlayFixedTime(2で追加したアニメーション名)|敵側のテイクダウンされたアニメーション|
    |^  |^  |vEventWithDelay.DoEvent(0)|-|
    |^  |^  |(Triggerオブジェクトの)BoxCollider.enabled = false|Triggerの無効化|
    |^  |^  |Rigidbody.isKinematic = true|物理の無効化|
    |^  |^  |OnAnimatorMoveを呼び出すコンポーネント.isTakedown = true|ルートモーションの位置をキャラに反映する|
    |^  |^  |StunManager.UnlockStun|スタンモジュールがある場合はスタンを解除するメソッドを←のように呼び出す|

7. **vEventWithDelay**を以下のように設定

    |要素番号|ディレイ|値|備考|
    |---|---|---|---|
    |0|0.8|vControlAI.AddHealth(-maxHealth)|強制的にHPを0にする(ディレイはどのタイミングで死亡判定を起こすかで決める)|

8. テイクダウンアニメーション終了時の処理

    |タブ名|設定項目|値|備考|
    |---|---|---|---|
    |Events|On Finish Animationなど|(Triggerオブジェクトの)BoxCollider.enabled = true|Trigger再有効化|
    |^  |^  |OnAnimatorMoveを呼び出すコンポーネント.isTakedown = false|ルートモーション反映無効化|
    |^  |^  |Triggerオブジェクト.SetActive = false|Trigger自体の無効化|
    |^  |^  |Rigidbody.isKinematic = false|物理の有効化|

9. （任意）ステルスの場合
   1. AI側のDetection > MinDistanceを0にする
      1. 背後に接近時プレイヤーを検知しないようにするため
