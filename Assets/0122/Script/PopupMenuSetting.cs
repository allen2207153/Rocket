using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupMenuSetting : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] private KeyCode menuKey = KeyCode.O;      // ���j���[���J���̃L�[
    [SerializeField] private GameObject menuPanel;                  // ���j���[Prefab

    [Header("Mouse Sensitivity")]    //�}�E�X���x����
    [SerializeField] private Player_Camera playerCamera;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private float minSensitivity = 10f;            // �}�E�X���x����
    [SerializeField] private float maxSensitivity = 200f;           // �}�E�X���x���
    [SerializeField] private float defaultSensitivity = 100f;       // �}�E�X���x�����l

    [Header("Sound Settings")]       //BGM���ʒ���
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    [SerializeField] private float defaultBGMVolume = 0.15f;          //BGM���ʏ����l0~1�i0%~100%�j
   
    [SerializeField] private Slider seVolumeSlider;  
    [SerializeField] private TextMeshProUGUI seVolumeText;
    [SerializeField] private float defaultSEVolume = 1.0f;           //SE���ʏ����l0~1�i0%~100%�j
    
    private bool isMenuOpen = false;

    #region Unity Lifecycle

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // �V�[���ǂݍ��݃C�x���g���X�i�[��ǉ�����
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ���j���[�͏�����\��
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializeSensitivitySettings();
        InitializeBGMSettings();

        LoadSettings();
    }

    private void Update()
    {
        HandleMenuInput();
    }

    #endregion

    #region Menu Control

    private void HandleMenuInput()
    {
        if (Input.GetKeyDown(menuKey))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        // �}�E�X�̕\���ƃ��b�N�𐧌䂷��
        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // ���j���[���J���Ȃ�Q�[�����ꎞ��~
        Time.timeScale = isMenuOpen ? 0 : 1;

        SoundManager.Instance?.PlaySE("popupOpen");
    }

    #endregion

    #region Sensitivity Settings           

    private void InitializeSensitivitySettings()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        }
    }

    public void UpdateSensitivity(float value)
    {
        if (playerCamera != null)
        {
            playerCamera.sensX = value;
            playerCamera.sensY = value;

            if (sensitivityText != null)
            {
                sensitivityText.text = $"{value:F0}";
            }

            PlayerPrefs.SetFloat("Sensitivity", value);
            PlayerPrefs.Save();
        }
    }

    public void ResetSensitivityToDefault()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = defaultSensitivity;
        }
    }
    #endregion

    #region Sound Settings

    private void InitializeBGMSettings()
    {
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.minValue = 0f;
            bgmVolumeSlider.maxValue = 1f;
            bgmVolumeSlider.onValueChanged.AddListener(UpdateBGMVolume);
        }

        if (seVolumeSlider != null)
        {
            seVolumeSlider.minValue = 0f;
            seVolumeSlider.maxValue = 1f;
            seVolumeSlider.onValueChanged.AddListener(UpdateSEVolume);
        }

    }

    public void UpdateBGMVolume(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(value);

            if (bgmVolumeText != null)
            {
                bgmVolumeText.text = $"{(value * 100):F0}%";
            }

            PlayerPrefs.SetFloat("BGMVolume", value);
            PlayerPrefs.Save();
        }
    }

    public void UpdateSEVolume(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSEVolume(value);  

            if (seVolumeText != null)
            {
                seVolumeText.text = $"{(value * 100):F0}%";
            }

            PlayerPrefs.SetFloat("SEVolume", value);
            PlayerPrefs.Save();
        }
    }

    public void ResetBGMVolumeToDefault()
    {
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = defaultBGMVolume;
            UpdateBGMVolume(defaultBGMVolume);
        }
    }

    public void ResetSEVolumeToDefault()
    {
        if (seVolumeSlider != null)
        {
            seVolumeSlider.value = defaultSEVolume;
            UpdateSEVolume(defaultSEVolume);
        }
    }


    #endregion


    private void LoadSettings()
    {
        // �}�E�X���x�̐ݒ�
        float savedSens = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSens;
        }
        UpdateSensitivity(savedSens);

        // BGM���ʂ̐ݒ�
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = savedBGMVolume;
        }
        UpdateBGMVolume(savedBGMVolume);

        float savedSEVolume = PlayerPrefs.GetFloat("SEVolume", defaultSEVolume);
        if (seVolumeSlider != null)
        {
            seVolumeSlider.value = savedSEVolume;
        }
        UpdateSEVolume(savedSEVolume);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player_Camera newPlayerCamera = FindObjectOfType<Player_Camera>();
        if (newPlayerCamera != null)
        {
            playerCamera = newPlayerCamera;
            float currentSensitivity = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
            UpdateSensitivity(currentSensitivity);
        }
    }

    #region Public Methods

    // ���̃X�N���v�g�����j���[�𐧌�ł���悤�Ƀp�u���b�N���\�b�h��ݒ肷��
    public void OpenMenu()
    {
        if (!isMenuOpen)
        {
            ToggleMenu();
        }
    }

    public void CloseMenu()
    {
        if (isMenuOpen)
        {
            ToggleMenu();
        }
    }

    // ���̑��̐ݒ�֘A�̃��\�b�h�͂����ɒǉ��ł��܂�

    #endregion
}