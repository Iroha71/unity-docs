# Horse Animset Pro

## Invector対応スクリプト

- MalbersAnimationのInputシステムを削除しておく
- [コントローラ対応スクリプト](https://github.com/Iroha71/unity-docs/blob/develop/assets/origin-scripts/RiderInput.cs){:target="_blank"}を作成
- 同階層の`vChangeInputTypeTrigger`に以下のイベントを設定

    |イベント|値|
    |---|---|
    |OnChangeKeyboard|RiderInput.SwitchInputType(true)|
    |OnChangeToJoystick|RiderInput.SwitchInputType(false)|

## 足音調整

- AudioCLipListVar.cs

    ```cs
    // 追加
    private int index = 0;

    public void Play(AudioSource source)
    {
        //var clip = Item_GetRandom();
        source.clip = GetFootSound(); // Item_GetRandom()から変更
        source.pitch = pitch.RandomValue;
        source.volume = volume.RandomValue;
        source.spatialBlend = SpatialBlend;
        source.Play();
    }


    public void Play()
    {
        var NewGO = new GameObject() { name = "Audio [" + this.name +"]"};
        var source = NewGO.AddComponent<AudioSource>();
        source.spatialBlend = 1f;
        var clip = GetFootSound(); // Item_GetRandomから変更
        source.clip = clip;
        source.pitch = pitch.RandomValue;
        source.volume = volume.RandomValue;
        source.spatialBlend = SpatialBlend;
        source.Play();
    }

    // 追加
    private AudioClip GetFootSound()
    {
        AudioClip clip = Items[index];
        index++;
        if (index >= Items.Count)
            index = 0;

        return clip;
    }
    ```
