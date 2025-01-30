using UnityEngine;


//�T�E���h�Ǘ��N���X
public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;   //����炷���߂̃R���|�[�l���g
    private AudioClip currentAudioClip;//�Ō�ɖ炵����

    [SerializeField] private SoundSetting SoundSetting;  //�T�E���h�f�[�^
    [SerializeField] private string initialBGM = "bgm1";  //���nBGM
    [SerializeField] private bool playBGMOnStart = true;  // �N������BGM���Đ����邩�ǂ���?

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

        audioSource = GetComponent<AudioSource>(); //�����Q�[���I�u�W�F�N�g����擾
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
        //AudioClip���擾����
        currentAudioClip = GetAudioClip_SE(tag);

        //�T�E���h�f�[�^�����邩���m�F����
        if (currentAudioClip == null) { Debug.Log("�T�E���h�f�[�^������܂���"); return; }

        //�T�E���h���Đ�����
        audioSource.PlayOneShot(currentAudioClip);
    }
    public void PlayBGM(string tag)
    {
        // �ۑ��c�O���ʐݒ�
        float currentVolume = audioSource.volume;

        //AudioClip���擾����
        currentAudioClip = GetAudioClip_BGM(tag);

        //�T�E���h�f�[�^�����邩���m�F����
        if (currentAudioClip == null) { Debug.Log("�T�E���h�f�[�^������܂���"); return; }
        
        //���݂�BGM���~
        audioSource.Stop();

        //BGM�̐ݒ�
        audioSource.clip = currentAudioClip;

        //Sound�ݒ���擾
        foreach (var Sound in SoundSetting.Get_BGMList)
        {
            if (Sound.tag == tag)
            {
                //audioSource.volume = Sound.volume;
                audioSource.loop = Sound.isLoop;
                break;
            }
        }

        //BGM���Đ�
        audioSource.volume = currentVolume;
        audioSource.Play();
    }

    // BGM���~
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // BGM���ꎞ��~
    public void PauseBGM()
    {
        audioSource.Pause();
    }

    // BGM���ĊJ
    public void ResumeBGM()
    {
        audioSource.UnPause();
    }

    public void SetBGMVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    // ���݂�BGM�̉��ʂ��擾����
    public float GetBGMVolume()
    {
        return audioSource != null ? audioSource.volume : 0f;
    }


    private void Start()
    {
        //Instance.PlayBGM("bgm2");
        if (playBGMOnStart && !string.IsNullOrEmpty(initialBGM))
        {
            PlayBGM(initialBGM);
        }
    }
}