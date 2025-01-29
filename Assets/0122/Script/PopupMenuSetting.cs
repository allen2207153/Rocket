using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupMenuSetting : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] private KeyCode menuKey = KeyCode.O;      // メニューを開くのキー
    [SerializeField] private GameObject menuPanel;                  // メニューPrefab

    [Header("Mouse Sensitivity")]    //マウス感度調整
    [SerializeField] private Player_Camera playerCamera;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private float minSensitivity = 10f;            // マウス感度下限
    [SerializeField] private float maxSensitivity = 200f;           // マウス感度上限
    [SerializeField] private float defaultSensitivity = 100f;       // マウス感度初期値

    [Header("Sound Settings")]       //BGM音量調整
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    [SerializeField] private float defaultBGMVolume = 0.5f;           //BGM音量初期値0~1（0%~100%）
    private bool isMenuOpen = false;

    #region Unity Lifecycle

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // シーン読み込みイベントリスナーを追加する
        SceneManager.sceneLoaded += OnSceneLoaded;

        // メニューは初期非表示
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

        // マウスの表示とロックを制御する
        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // メニューを開きならゲームを一時停止
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

    public void ResetBGMVolumeToDefault()
    {
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = defaultBGMVolume;
        }
    }

    #endregion


    private void LoadSettings()
    {
        // マウス感度の設定
        float savedSens = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSens;
        }
        UpdateSensitivity(savedSens);

        // BGM音量の設定
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", defaultBGMVolume);
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = savedBGMVolume;
        }
        UpdateBGMVolume(savedBGMVolume);
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

    // 他のスクリプトがメニューを制御できるようにパブリックメソッドを設定する
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

    // その他の設定関連のメソッドはここに追加できます

    #endregion
}