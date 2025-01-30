using UnityEngine;


//�T�E���h�Ǘ��N���X
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioClip currentAudioClip;

    [SerializeField] private SoundSetting SoundSetting;     //�T�E���h�f�[�^
    [SerializeField] private string initialBGM = "bgm1";    //���nBGM
    [SerializeField] private bool playBGMOnStart = true;    //�N������BGM���Đ����邩�ǂ���?

    private AudioSource bgmAudioSource;                     //BGM����炷���߂̃R���|�[�l���g
    private AudioSource seAudioSource;                      //SE����炷���߂̃R���|�[�l���g

    [SerializeField] private float bgmVolume = 0.15f;        //���nBGM����
    [SerializeField] private float seVolume = 1.0f;         //���nSE����

    private AudioClip currentBGMAudioClip;                  //�Ō�ɖ炵����

    private void Awake()
    {
      if (Instance == null)
      {
           Instance = this;
           DontDestroyOnLoad(gameObject);

            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource = gameObject.AddComponent<AudioSource>();

            //���n���ʂ�ݒ肷��
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


    //SoundData����AudioClip���擾����
    private AudioClip GetAudioClip_SE(string tag)
    {
        if (SoundSetting == null) return null;

        //�f�[�^���P�����ׂ�
        foreach (var Sound in SoundSetting.Get_SEList)
        {

            //tag����v���Ă����猟�����I�����ăf�[�^��Ԃ�
            if (Sound.tag == tag)
            {
                return Sound.audioClip;
            }
        }

        //tag����v���Ȃ����Null��Ԃ�
        return null;
    }

    private AudioClip GetAudioClip_BGM(string tag)
    {
        if (SoundSetting == null) return null;

        //�f�[�^���P�����ׂ�
        foreach (var Sound in SoundSetting.Get_BGMList)
        {

            //tag����v���Ă����猟�����I�����ăf�[�^��Ԃ�
            if (Sound.tag == tag)
            {
                return Sound.audioClip;
            }
        }

        //tag����v���Ȃ����Null��Ԃ�
        return null;
    }

    //����炷
    public void PlaySE(string tag)
    {
        ////AudioClip���擾����
        //currentAudioClip = GetAudioClip_SE(tag);

        ////�T�E���h�f�[�^�����邩���m�F����
        //if (currentAudioClip == null) { Debug.Log("�T�E���h�f�[�^������܂���"); return; }

        ////�T�E���h���Đ�����
        //audioSource.PlayOneShot(currentAudioClip);


        if (SoundSetting == null) return;                                               //�T�E���h�f�[�^�����邩���m�F����

        Sound seData = SoundSetting.Get_SEList.Find(se => se.tag == tag);               //AudioClip���擾����
        if (seData.audioClip != null)
        {
            seAudioSource.PlayOneShot(seData.audioClip, seVolume * seData.volume);      //�T�E���h���Đ�����
        }
    }
    public void PlayBGM(string tag)
    {
        //// �ۑ��c�O���ʐݒ�
        //float currentVolume = audioSource.volume;

        ////AudioClip���擾����
        //currentAudioClip = GetAudioClip_BGM(tag);

        ////�T�E���h�f�[�^�����邩���m�F����
        //if (currentAudioClip == null) { Debug.Log("�T�E���h�f�[�^������܂���"); return; }
        
        ////���݂�BGM���~
        //audioSource.Stop();

        ////BGM�̐ݒ�
        //audioSource.clip = currentAudioClip;

        ////Sound�ݒ���擾
        //foreach (var Sound in SoundSetting.Get_BGMList)
        //{
        //    if (Sound.tag == tag)
        //    {
        //        //audioSource.volume = Sound.volume;
        //        audioSource.loop = Sound.isLoop;
        //        break;
        //    }
        //}

        ////BGM���Đ�
        //audioSource.volume = currentVolume;
        //audioSource.Play();


        if (SoundSetting == null) return;
        //AudioClip���擾����
        Sound bgmData = SoundSetting.Get_BGMList.Find(bgm => bgm.tag == tag);
        if (bgmData.audioClip != null)
        {
            if (currentBGMAudioClip != bgmData.audioClip)
            {
                bgmAudioSource.Stop();                          //���݂�BGM���~
                //BGM�̐ݒ�
                currentBGMAudioClip = bgmData.audioClip;        
                bgmAudioSource.clip = currentBGMAudioClip;
                bgmAudioSource.loop = bgmData.isLoop;
                bgmAudioSource.volume = bgmVolume * bgmData.volume;

                bgmAudioSource.Play();
            }
        }
    }

    // BGM���~
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


    //// BGM���ꎞ��~
    //public void PauseBGM()
    //{
    //    audioSource.Pause();
    //}

    //// BGM���ĊJ
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


    //// ���݂�BGM�̉��ʂ��擾����
    //public float GetBGMVolume()
    //{
    //    return audioSource != null ? audioSource.volume : 0f;
    //}


    
}