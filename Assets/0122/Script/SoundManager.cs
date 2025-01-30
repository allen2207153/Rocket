using UnityEngine;


//サウンド管理クラス
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioClip currentAudioClip;

    [SerializeField] private SoundSetting SoundSetting;     //サウンドデータ
    [SerializeField] private string initialBGM = "bgm1";    //初始BGM
    [SerializeField] private bool playBGMOnStart = true;    //起動時にBGMを再生するかどうか?

    private AudioSource bgmAudioSource;                     //BGM音を鳴らすためのコンポーネント
    private AudioSource seAudioSource;                      //SE音を鳴らすためのコンポーネント

    [SerializeField] private float bgmVolume = 0.15f;        //初始BGM音量
    [SerializeField] private float seVolume = 1.0f;         //初始SE音量

    private AudioClip currentBGMAudioClip;                  //最後に鳴らした音

    private void Awake()
    {
      if (Instance == null)
      {
           Instance = this;
           DontDestroyOnLoad(gameObject);

            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource = gameObject.AddComponent<AudioSource>();

            //初始音量を設定する
            bgmAudioSource.volume = bgmVolume;
            bgmAudioSource.playOnAwake = false;

            seAudioSource.volume = seVolume;
            seAudioSource.playOnAwake = false;
        }
       else
        {
            Destroy(gameObject);
           return;
        }
    }

    private void Start()
    {
        //Instance.PlayBGM("bgm2");
        if (playBGMOnStart && !string.IsNullOrEmpty(initialBGM))
        {
            PlayBGM(initialBGM);
        }
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
        ////AudioClipを取得する
        //currentAudioClip = GetAudioClip_SE(tag);

        ////サウンドデータがあるかを確認する
        //if (currentAudioClip == null) { Debug.Log("サウンドデータがありません"); return; }

        ////サウンドを再生する
        //audioSource.PlayOneShot(currentAudioClip);


        if (SoundSetting == null) return;                                               //サウンドデータがあるかを確認する

        Sound seData = SoundSetting.Get_SEList.Find(se => se.tag == tag);               //AudioClipを取得する
        if (seData.audioClip != null)
        {
            seAudioSource.PlayOneShot(seData.audioClip, seVolume * seData.volume);      //サウンドを再生する
        }
    }
    public void PlayBGM(string tag)
    {
        //// 保存當前音量設定
        //float currentVolume = audioSource.volume;

        ////AudioClipを取得する
        //currentAudioClip = GetAudioClip_BGM(tag);

        ////サウンドデータがあるかを確認する
        //if (currentAudioClip == null) { Debug.Log("サウンドデータがありません"); return; }
        
        ////現在のBGMを停止
        //audioSource.Stop();

        ////BGMの設定
        //audioSource.clip = currentAudioClip;

        ////Sound設定を取得
        //foreach (var Sound in SoundSetting.Get_BGMList)
        //{
        //    if (Sound.tag == tag)
        //    {
        //        //audioSource.volume = Sound.volume;
        //        audioSource.loop = Sound.isLoop;
        //        break;
        //    }
        //}

        ////BGMを再生
        //audioSource.volume = currentVolume;
        //audioSource.Play();


        if (SoundSetting == null) return;
        //AudioClipを取得する
        Sound bgmData = SoundSetting.Get_BGMList.Find(bgm => bgm.tag == tag);
        if (bgmData.audioClip != null)
        {
            if (currentBGMAudioClip != bgmData.audioClip)
            {
                bgmAudioSource.Stop();                          //現在のBGMを停止
                //BGMの設定
                currentBGMAudioClip = bgmData.audioClip;        
                bgmAudioSource.clip = currentBGMAudioClip;
                bgmAudioSource.loop = bgmData.isLoop;
                bgmAudioSource.volume = bgmVolume * bgmData.volume;

                bgmAudioSource.Play();
            }
        }
    }

    // BGMを停止
    public void StopBGM()
    {
        // audioSource.Stop();

        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
            currentBGMAudioClip = null;
        }
    }

    public void StopSE()
    {
        if (seAudioSource != null && seAudioSource.isPlaying)
        {
            seAudioSource.Stop();
        }
    }


    //// BGMを一時停止
    //public void PauseBGM()
    //{
    //    audioSource.Pause();
    //}

    //// BGMを再開
    //public void ResumeBGM()
    //{
    //    audioSource.UnPause();
    //}

    public void SetBGMVolume(float volume)
    {
        //if (audioSource != null)
        //{
        //    audioSource.volume = volume;
        //}

        bgmVolume = volume;
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }
    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        if (seAudioSource != null)
        {
            seAudioSource.volume = volume;
        }
    }


    //// 現在のBGMの音量を取得する
    //public float GetBGMVolume()
    //{
    //    return audioSource != null ? audioSource.volume : 0f;
    //}


    
}