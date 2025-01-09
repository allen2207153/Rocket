using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    public Vector3 originalPosition; // オブジェクトの元の位置
    public Quaternion originalRotation; // オブジェクトの元の回転

    private void Start()
    {
        originalPosition = transform.position; // 初期位置を保存
        originalRotation = transform.rotation; // 初期回転を保存
    }
}
