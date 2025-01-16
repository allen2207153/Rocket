using UnityEngine;

public class VfxManager : MonoBehaviour
{

    public static VfxManager Instance { get; private set; }

    [SerializeField] private VfxSetting vfxSetting;
    [SerializeField] private Camera playerCamera;

    private GameObject activeVfx; // いま作用中のVFXを確認
    private bool isTriggered = false; // トリガー条件を確認

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
        }

        // Find camera if not assigned
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Player_Camera")?.GetComponent<Camera>();
            if (playerCamera == null)
            {
                Debug.LogError("Player_Camera 割り当てられていません");
            }
        }
    }

    private void Update()
    {
        // キー条件判定
        bool keyPressed = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.LeftControl);

        if (keyPressed && !isTriggered)
        {
            PlayScreenSpaceVfx("vfx_Concentration");
            isTriggered = true;
        }
        else if (!keyPressed && isTriggered)
        {
            StopAndDestroyVfx();
            isTriggered = false;
        }
    }

    // トリガー判定
    public void HandleCollision(Collision collision)
    {
        bool isWall = collision.gameObject.layer == LayerMask.NameToLayer("Wall");
        bool isSpeedDash = collision.gameObject.CompareTag("SpeedDash");

        if ((isWall || isSpeedDash) && !isTriggered)
        {
            PlayScreenSpaceVfx("vfx_Concentration");
            isTriggered = true;
        }
        else if (!isWall && !isSpeedDash && isTriggered)
        {
            StopAndDestroyVfx();
            isTriggered = false;
        }
    }

    private void PlayScreenSpaceVfx(string tag)
    {
        if (vfxSetting == null || playerCamera == null)
        {
            Debug.LogError("VfxSetting 又は PlayerCamera 割り当てられていません");
            return;
        }

        VFX vfxToPlay = vfxSetting.Get_VFXList.Find(vfx => vfx.tag == tag);

        if (vfxToPlay.VfxPrefab != null)
        {
            // カメラ位置と回転
            Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            Quaternion spawnRotation = playerCamera.transform.rotation;

            // カメラ回転するとVFX生成
            GameObject instantiatedVfx = Instantiate(vfxToPlay.VfxPrefab, spawnPosition, spawnRotation);
            instantiatedVfx.transform.parent = playerCamera.transform;

            // VFXがカメラの前に表示のを確保
            instantiatedVfx.transform.localPosition = new Vector3(0, 0, 2f);
            instantiatedVfx.transform.localRotation = Quaternion.identity;

            ParticleSystem ps = instantiatedVfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                // world spaceを利用
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                var renderer = ps.GetComponent<ParticleSystemRenderer>();
                renderer.sortingOrder = 999;
                ps.Play();
            }
        }
    
        else
        {
            Debug.LogWarning($"タグ '{tag}' の VFX が見つかりませんでした。");
        }
    }

    private void StopAndDestroyVfx()
    {
        if (activeVfx != null)
        {
            var ps = activeVfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(true); // 止まる
            }
            Destroy(activeVfx);
            activeVfx = null;
        }
    }

    private void OnDisable()
    {
        StopAndDestroyVfx();
    }



    //VFXを（一般的）表示する　
    public void PlayVfx(string tag, Vector3 position)
    {
        if (vfxSetting == null)
        {
            Debug.LogError("VfxSetting が割り当てられていません！");
            return;
        }

        VFX vfxToPlay = vfxSetting.Get_VFXList.Find(vfx => vfx.tag == tag);

        if (vfxToPlay.VfxPrefab != null)
        {

            GameObject instantiatedVfx = Instantiate(vfxToPlay.VfxPrefab, position, Quaternion.identity);
            ParticleSystem ps = instantiatedVfx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();

                // 自動的に消滅する
                float lifetime = ps.main.duration;
                Destroy(instantiatedVfx, lifetime);
            }
        }

        else
        {
            Debug.LogWarning($"タグ '{tag}' の VFX が見つかりませんでした。");
        }
    }

}


