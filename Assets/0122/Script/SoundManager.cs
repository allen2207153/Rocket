using UnityEngine;


//サウンド管理クラス
public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;   //音を鳴らすためのコンポーネント
    private AudioClip currentAudioClip;//最後に鳴らした音

    [SerializeField] private SoundSetting SoundSetting;  //サウンドデータ

    private void Awake()
    {
      if (Instance == null)
      {
           Instance = this;
           DontDestroyOnLoad(gameObject);
        }
       else
        {
            Destroy(gameObject);
           return;
        }

        audioSource = GetComponent<AudioSource>(); //同じゲームオブジェクトから取得
    }

    //SoundDataからAudioClipを取得する
    private AudioClip GetAudioClip_SE(string tag)
    {
        if (SoundSetting == null) return null;

        //データを１つずつ調べる
        foreach (var Sound in SoundSetting.Get_SEList)
        {

            //tagが一致していたら検索を終了してデータを返す
            if (Sound.tag == tag)
            {
                return Sound.audioClip;
            }
        }

        //tagが一致しなければNullを返す
        return null;
    }

    private AudioClip GetAudioClip_BGM(string tag)
    {
        if (SoundSetting == null) return null;

        //データを１つずつ調べる
        foreach (var Sound in SoundSetting.Get_BGMList)
        {

            //tagが一致していたら検索を終了してデータを返す
            if (Sound.tag == tag)
            {
                return Sound.audioClip;
            }
        }

        //tagが一致しなければNullを返す
        return null;
    }

    //音を鳴らす
    public void PlaySE(string tag)
    {
        //AudioClipを取得する
        currentAudioClip = GetAudioClip_SE(tag);

        //サウンドデータがあるかを確認する
        if (currentAudioClip == null) { Debug.Log("サウンドデータがありません"); return; }

        //サウンドを再生する
        audioSource.PlayOneShot(currentAudioClip);
    }
    public void PlayBGM(string tag)
    {
        //AudioClipを取得する
        currentAudioClip = GetAudioClip_BGM(tag);

        //サウンドデータがあるかを確認する
        if (currentAudioClip == null) { Debug.Log("サウンドデータがありません"); return; }

        //サウンドを再生する
        //  audioSource.PlayOneShot(currentAudioClip);
        
        //現在のBGMを停止
        audioSource.Stop();

        //BGMの設定
        audioSource.clip = currentAudioClip;

        //Sound設定を取得
        foreach (var Sound in SoundSetting.Get_BGMList)
        {
            if (Sound.tag == tag)
            {
                audioSource.volume = Sound.volume;
                audioSource.loop = Sound.isLoop;
                break;
            }
        }

        //BGMを再生
        audioSource.Play();
    }

    // BGMを停止
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // BGMを一時停止
    public void PauseBGM()
    {
        audioSource.Pause();
    }

    // BGMを再開
    public void ResumeBGM()
    {
        audioSource.UnPause();
    }

    private void Start()
    {
        Instance.PlayBGM("bgm3");
    }
}