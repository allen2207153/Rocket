using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    public Vector3 originalPosition; // �I�u�W�F�N�g�̌��̈ʒu
    public Quaternion originalRotation; // �I�u�W�F�N�g�̌��̉�]

    private void Start()
    {
        originalPosition = transform.position; // �����ʒu��ۑ�
        originalRotation = transform.rotation; // ������]��ۑ�
    }
}
