using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBullet : MonoBehaviour
{
    public int bulletPow;
    public Collider bulletCol;

    bool isAttack = true;

    public enum Type { Lighting, Water }
    public Type type;

    [SerializeField]
    private float doteTime;

    private void Update()
    {
        switch(type)
        {
            case Type.Lighting:
                doteTime = 0.2f;
                break;
            case Type.Water:
                doteTime = 1;
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && bulletCol.enabled)
        {
            other.GetComponent<IEnemy>().CurrentHp -= bulletPow;
            StartCoroutine(DotDamage(isAttack, doteTime));
        }
    }

    private IEnumerator DotDamage(bool isAttack, float doteTime)
    {
        while (isAttack)
        {
            bulletCol.enabled = isAttack;
            isAttack = false;
            yield return new WaitForSeconds(doteTime);
            bulletCol.enabled = !isAttack;
            isAttack = true;
        }
    }
}
