using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsBullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public int bulletPow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().CurrentHp -= bulletPow;

            gameObject.SetActive(false);
        }
    }
}
