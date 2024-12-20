using UnityEngine;

public class VfxManager : MonoBehaviour
{

    public static VfxManager Instance { get; private set; }

    [SerializeField] private VfxSetting vfxSetting;

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
    }

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
            }
        }
        else
        {
            Debug.LogWarning($"タグ '{tag}' の VFX が見つかりませんでした。");
        }
    }
}