using UnityEngine;

public class ChangeWeaponColor : MonoBehaviour
{
    public Color defaultColor = Color.white;  // 初期の色（白）
    public Color greenColor = Color.green;   // 緑色
    public Color purpleColor = new Color(0.5f, 0f, 0.5f); // 紫色（RGB: 128, 0, 128）

    private Renderer weaponRenderer;  // 武器のレンダラー
    private Material weaponMaterial;  // 武器のマテリアル
    private int currentColorIndex = 0; // 0: 白, 1: 緑, 2: 紫

    void Start()
    {
        // 武器の Renderer と Material を取得
        weaponRenderer = GetComponent<Renderer>();
        weaponMaterial = weaponRenderer.material;

        // 初期色を設定（BaseColor）
        SetBaseColor(defaultColor);
    }

    void Update()
    {
        // "E" キーを押すと色を切り替える
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentColorIndex = (currentColorIndex + 1) % 3; // 0, 1, 2 をループ
            ChangeWeaponColorByIndex(currentColorIndex);
        }
    }

    /// <summary>
    /// 指定されたインデックスに基づいて武器の色を変更する
    /// </summary>
    /// <param name="index">0: 白, 1: 緑, 2: 紫</param>
    void ChangeWeaponColorByIndex(int index)
    {
        Color newColor;
        switch (index)
        {
            case 1:
                newColor = greenColor;  // 緑色に変更
                break;
            case 2:
                newColor = purpleColor; // 紫色に変更
                break;
            default:
                newColor = defaultColor; // 白色にリセット
                break;
        }
        SetBaseColor(newColor);
    }

    /// <summary>
    /// BaseColor を設定する
    /// </summary>
    /// <param name="color">変更する色</param>
    void SetBaseColor(Color color)
    {
        weaponMaterial.SetColor("_BaseColor", color);
    }
}
