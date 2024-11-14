using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionForce = 500f;  // �����̗́i�����̋����j
    public float explosionRadius = 5f;   // �����͈̔�
    public float upwardsModifier = 1f;   // ���������ւ̗͂̋���

    // �����̏���
    public void Explode(Vector3 explosionPosition)
    {
        // �����͈͓��ɂ���S�ẴR���C�_�[���擾
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // �e�R���C�_�[�ɑ΂��Ĕ����͂�������
        foreach (Collider col in colliders)
        {
            // Rigidbody������Δ����͂�������
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)//
            {
                // �����ʒu���畨�̂ւ̕������v�Z
                Vector3 direction = col.transform.position - explosionPosition;

                // ���̂Ɣ������Ƃ̋������v�Z
                float distance = direction.magnitude;

                // �����Ɋ�Â��Ĕ����͂𒲐�
                float force = Mathf.Lerp(explosionForce, 0, distance / explosionRadius);

                // �����͂𕨑̂ɉ�����
                rb.AddExplosionForce(force, explosionPosition, explosionRadius, upwardsModifier);
            }
        }
    }

    // �V�[���r���[�Ŕ����͈͂�`��
    private void OnDrawGizmos()
    {
        // Gizmos�̐F�𔼓����̐Ԃɐݒ�
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        // ���̂̈ʒu�ɔ����͈͂������~��`��
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
