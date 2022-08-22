using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAttack : MonoBehaviour
{
    public enum Type { White, Black }
    public Type type;

    public Transform target; //목표물
    public int pow; //공격력
    public GameObject[] Rocks;

    public MagicBullet[] RocksBullet;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        RocksBullet = GetComponentsInChildren<MagicBullet>();
    }

    private void Update()
    {
        transform.LookAt(target);
        Attack();
        if (target == null)//목표물이 제거되었다면
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        switch(type)
        {
            case Type.White:
                rigid.velocity = transform.forward * 5f;
                RocksBullet[0].bulletPow = pow;
                break;
            case Type.Black:
                StartCoroutine(RockAttack());
                break;
        }
    }

    private IEnumerator RockAttack()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Rocks.Length; i++)
        {
            RocksBullet[i].bulletPow = pow;
            Rocks[i].transform.Translate(Vector3.forward * 15 * Time.deltaTime);
            yield return new WaitForSeconds(0.5f);
        }
        Destroy(gameObject, 0.5f);

    }

}
