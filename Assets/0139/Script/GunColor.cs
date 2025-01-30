using UnityEngine;

public class ChangeWeaponColor : MonoBehaviour
{
    public Color defaultColor = Color.white;  // �����̐F�i���j
    public Color greenColor = Color.green;   // �ΐF
    public Color purpleColor = new Color(0.5f, 0f, 0.5f); // ���F�iRGB: 128, 0, 128�j

    private Renderer weaponRenderer;  // ����̃����_���[
    private Material weaponMaterial;  // ����̃}�e���A��
    private int currentColorIndex = 0; // 0: ��, 1: ��, 2: ��

    void Start()
    {
        // ����� Renderer �� Material ���擾
        weaponRenderer = GetComponent<Renderer>();
        weaponMaterial = weaponRenderer.material;

        // �����F��ݒ�iBaseColor�j
        SetBaseColor(defaultColor);
    }

    void Update()
    {
        // "E" �L�[�������ƐF��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentColorIndex = (currentColorIndex + 1) % 3; // 0, 1, 2 �����[�v
            ChangeWeaponColorByIndex(currentColorIndex);
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�C���f�b�N�X�Ɋ�Â��ĕ���̐F��ύX����
    /// </summary>
    /// <param name="index">0: ��, 1: ��, 2: ��</param>
    void ChangeWeaponColorByIndex(int index)
    {
        Color newColor;
        switch (index)
        {
            case 1:
                newColor = greenColor;  // �ΐF�ɕύX
                break;
            case 2:
                newColor = purpleColor; // ���F�ɕύX
                break;
            default:
                newColor = defaultColor; // ���F�Ƀ��Z�b�g
                break;
        }
        SetBaseColor(newColor);
    }

    /// <summary>
    /// BaseColor ��ݒ肷��
    /// </summary>
    /// <param name="color">�ύX����F</param>
    void SetBaseColor(Color color)
    {
        weaponMaterial.SetColor("_BaseColor", color);
    }
}
