using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    void Initialize(Vector3 direction, float speed);
    void Move();
    void OnHit(GameObject target);
}