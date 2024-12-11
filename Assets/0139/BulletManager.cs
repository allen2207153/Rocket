//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BulletManager : MonoBehaviour
//{
//    public GameObject bulletPrefab; // �e�ۂ̃v���n�u
//    public Transform firePoint; // �e�۔��ˈʒu
//    public float bulletSpeed = 10f; // �e�ۂ̑��x

//    // �e�ۂ𔭎˂��鏈��
//    public void ShootBullet()
//    {
//        // �e�ۂ̐���
//        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//        // Bullet_Explosion�R���|�[�l���g���擾���āA������
//        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();

//            // �e�ۂ̕����Ƒ��x��ݒ�
//        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
//        bulletExplosion.Move();

//    }

//    // ���t���[���X�V����鏈��
//    private void Update()
//    {
//        // ���˃e�X�g�p�Ƀ}�E�X�N���b�N�Œe������
//        if (Input.GetButtonDown("Fire1"))
//        {
//            ShootBullet();
//            Debug.Log("Success");
//        }
//    }
//}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro; 

//public class BulletManager : MonoBehaviour
//{
//    public GameObject bulletPrefab; // �e�ۂ̃v���n�u\
//    public Transform firePoint; // �e�۔��ˈʒu
//    public float bulletSpeed = 10f; // �e�ۂ̑��x
//    public float fireRate = 0.5f; // �J�΂̊Ԋu�i�b�j
//    public int maxBullets = 6; // �ő�e�ې�

//    private int currentBullets; // ���݂̎c��e�ې�
//    private float nextFireTime = 0f; // ���ɔ��˂ł��鎞��

//    public TextMeshProUGUI bulletTextTMP; // �q�[�ɗʂ�\������ TextMeshPro UI ���f
//   //public Stick_Explosion gggl;

//    private void Start()
//    {
//        // ���������ɒe�ې����ő�l�ɐݒ�
//        currentBullets = maxBullets;
//        UpdateBulletUI(); // �����̎q�[����\��
//        //gggl = GetComponent<Stick_Explosion>();
//    }

//    // �e�ۂ𔭎˂��鏈��
//    public void ShootBullet()
//    {
//        // �e�ۂ��c���Ă��Ȃ��ꍇ�A���˂������b�Z�[�W��\������
//        if (currentBullets <= 0)
//        {
//            Debug.Log("�e�ۂ�����܂���I�����[�h���K�v�ł��B");
//            return;
//        }

//        // �e�ۂ̐���
//        var bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//        // Bullet_Explosion�R���|�[�l���g���擾���āA������
//        var bulletExplosion = bulletInstance.GetComponent<Bullet_Explosion>();

//        // �e�ۂ̕����Ƒ��x��ݒ�
//        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);
//        bulletExplosion.Move();

//        // ���ˌ�ɒe�ې�������
//        currentBullets--;
//        Debug.Log("�c��e�ې�: " + currentBullets);

//        // �e�ې��� UI �\�����X�V
//        UpdateBulletUI();
//    }

//    // �����[�h����
//    private void Reload()
//    {
//        // �e�ې����ő�l�Ƀ��Z�b�g
//        currentBullets = maxBullets;
//        Debug.Log("�����[�h�����B�e�ې������Z�b�g����܂���: " + currentBullets);

//        // �e�ې��� UI �\�����X�V
//        UpdateBulletUI();
//    }

//    // ���t���[���X�V����鏈��
//    private void Update()
//    {
//        // ���݂̎��Ԃ����ɔ��˂ł��鎞�Ԃ𒴂��Ă���ꍇ�ɔ��˂���
//        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
//        {
//            ShootBullet();
//            nextFireTime = Time.time + fireRate; // ���̔��ˉ\���Ԃ��X�V
//        }

//        // R�L�[�������ƃ����[�h���s��
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Reload();
//        }
//    }

//    // �e�ې��� UI �\�����X�V���鏈��
//    private void UpdateBulletUI()
//    {
//        // �e�ې��� UI �ɕ\��
//        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public GameObject bulletPrefab; // �e�ۂ̃v���n�u
    public GameObject stickyBulletPrefab; // �S���e�ۂ̃v���n�u
    public GameObject gravityBulletPrefab; // �d�͒e�ۂ̃v���n�u
    public Transform firePoint; // �e�۔��ˈʒu
    public float bulletSpeed = 10f; // �e�ۂ̑��x
    public float fireRate = 0.5f; // �J�΂̊Ԋu�i�b�j
    public int maxBullets = 6; // �ő�e�ې�

    private int currentBullets; // ���݂̎c��e�ې�
    private float nextFireTime = 0f; // ���ɔ��˂ł��鎞��
    private enum BulletType { Normal, Sticky, Gravity } // �e�ۂ̎��
    private BulletType currentBulletType = BulletType.Normal; // ���݂̒e�ۂ̎��

    public TextMeshProUGUI bulletTextTMP; // �q�[�ɗʂ�\������ TextMeshPro UI ���f

    private void Start()
    {
        // ���������ɒe�ې����ő�l�ɐݒ�
        currentBullets = maxBullets;
        UpdateBulletUI(); // �����̎q�[����\��
    }

    // �e�ۂ𔭎˂��鏈��
    public void ShootBullet()
    {
        // �e�ۂ��c���Ă��Ȃ��ꍇ�A���˂������b�Z�[�W��\������
        if (currentBullets <= 0)
        {
            Debug.Log("�e�ۂ�����܂���I�����[�h���K�v�ł��B");
            return;
        }

        // �g�p����e�ۂ̎�ނ�����
        GameObject bulletToShoot = null;
        switch (currentBulletType)
        {
            case BulletType.Normal:
                bulletToShoot = bulletPrefab;
                break;
            case BulletType.Sticky:
                bulletToShoot = stickyBulletPrefab;
                break;
            case BulletType.Gravity:
                bulletToShoot = gravityBulletPrefab;
                break;
        }

        // �e�ۂ̐���
        var bulletInstance = Instantiate(bulletToShoot, firePoint.position, firePoint.rotation);

        // Bullet_Explosion�܂���StickyBulletExplosion�܂���GravityBulletExplosion�R���|�[�l���g���擾���āA������
        var bulletExplosion = bulletInstance.GetComponent<IBullet>();
        bulletExplosion.Initialize(firePoint.forward, bulletSpeed);

        // ���ˌ�ɒe�ې�������
        currentBullets--;
        Debug.Log("�c��e�ې�: " + currentBullets);

        // �e�ې��� UI �\�����X�V
        UpdateBulletUI();
    }

    // �����[�h����
    private void Reload()
    {
        // �e�ې����ő�l�Ƀ��Z�b�g
        currentBullets = maxBullets;
        Debug.Log("�����[�h�����B�e�ې������Z�b�g����܂���: " + currentBullets);

        // �e�ې��� UI �\�����X�V
        UpdateBulletUI();
    }

    // ���t���[���X�V����鏈��
    private void Update()
    {
        // ���݂̎��Ԃ����ɔ��˂ł��鎞�Ԃ𒴂��Ă���ꍇ�ɔ��˂���
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootBullet();
            nextFireTime = Time.time + fireRate; // ���̔��ˉ\���Ԃ��X�V
        }

        // R�L�[�������ƃ����[�h���s��
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // Q�L�[�������ƒe�ۂ̎�ނ�؂�ւ���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchBulletType();
        }
    }

    // �e�ۂ̎�ނ�؂�ւ��鏈��
    private void SwitchBulletType()
    {
        // �e�ۂ̎�ނ����Ԃɐ؂�ւ���
        currentBulletType = (BulletType)(((int)currentBulletType + 1) % System.Enum.GetValues(typeof(BulletType)).Length);
        Debug.Log(currentBulletType + " �ɐ؂�ւ��܂����B");
    }

    // �e�ې��� UI �\�����X�V���鏈��
    private void UpdateBulletUI()
    {
        // �e�ې��� UI �ɕ\��
        bulletTextTMP.text = "Bullet : " + currentBullets + "/" + maxBullets;
    }
}

