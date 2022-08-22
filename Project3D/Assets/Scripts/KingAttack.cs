using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAttack : MonoBehaviour
{
    public enum Type { White, Black }
    public Type type;

    public Transform target; //목표물
    public int pow; //공격력

    public GameObject[] swords;
    public GameObject sword;

    public SwordsBullet[] swordsBullets;
    public BlackKingBullet swordBullet;

    private void Awake()
    {
        swordsBullets = GetComponentsInChildren<SwordsBullet>();
        swordBullet = GetComponentInChildren<BlackKingBullet>();
        transform.LookAt(target);
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
        if (target != null)
        {
            switch (type)
            {
                case Type.White:
                    StartCoroutine(WhiteKingAttack());
                    break;
                case Type.Black:
                    transform.position = new Vector3(target.position.x, target.position.y + 6f, target.position.z);
                    StartCoroutine(BlackKingtAttack());
                    break;
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator WhiteKingAttack()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < swords.Length; i++)
        {
            swordsBullets[i].bulletPow = pow;
            swords[i].transform.LookAt(target);
            swords[i].transform.Translate(Vector3.forward * 15 * Time.deltaTime);
            yield return new WaitForSeconds(0.5f);
        }
        Destroy(gameObject, 0.5f);
    }

    private IEnumerator BlackKingtAttack()
    {
        if (target == null)
        {
            Destroy(sword, 0.1f);
        }

        swordBullet.bulletPow = pow;
        
        if (sword.transform.position.y > 1)
            sword.transform.Translate(Vector3.up * 10 * Time.deltaTime);
        else if (sword.transform.position.y == 1)
            sword.transform.Translate(Vector3.up * 0 * Time.deltaTime);
        
        yield return null;

    }
}

