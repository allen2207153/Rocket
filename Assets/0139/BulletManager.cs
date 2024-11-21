using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // �e�ۂ̃v���n�u
    public Transform firePoint; // �e�۔��ˈʒu
    public float bulletSpeed = 10f; // �e�ۂ̑��x

    // �e�ۂ𔭎˂��鏈��
    public void ShootBullet()
    {
        // �e�ۂ̐���
        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Bullet_Explosion�R���|�[�l���g���擾���āA������
        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();
       
            // �e�ۂ̕����Ƒ��x��ݒ�
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
        bulletExplosion.Move();
        
    }

    // ���t���[���X�V����鏈��
    private void Update()
    {
        // ���˃e�X�g�p�Ƀ}�E�X�N���b�N�Œe������
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
            Debug.Log("Success");
        }
    }
}