using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    public enum Type { White, Black }
    public Type type;

    public Transform target; //��ǥ��
    public int pow; //���ݷ�
    
    public GameObject[] swords;
    public SwordsBullet[] swordsBullets;


    private void Awake()
    {
        swordsBullets = GetComponentsInChildren<SwordsBullet>();
    }

    private void Update()
    {
        Attack();
        if (target == null)//��ǥ���� ���ŵǾ��ٸ�
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if(target !=null)
        {
            switch (type)
            {
                case Type.White:
                    transform.position = new Vector3(target.position.x, target.position.y + 4f, target.position.z);
                    StartCoroutine(WhiteKnightAttack());
                    break;
                case Type.Black:
                    transform.position = new Vector3(target.position.x, target.position.y + 4f, target.position.z);
                    StartCoroutine(BlackKnightAttack());
                    break;
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private IEnumerator WhiteKnightAttack()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < swords.Length; i++)
        {
            if (target == null)
            {
                Destroy(swords[i],0.1f);
            }
            swordsBullets[i].bulletPow = pow;
            swords[i].transform.LookAt(target);
            swords[i].transform.Translate(Vector3.forward * 15 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject, 0.5f);


    }
    private IEnumerator BlackKnightAttack()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < swords.Length; i++)
        {//�ݺ�
            if (target == null)
            {
                Destroy(swords[i], 0.1f);
            }
            swordsBullets[i].bulletPow = pow;
            swords[i].transform.LookAt(target);
            swords[i].transform.Translate(Vector3.forward * 15 * Time.deltaTime);
            yield return new WaitForSeconds(0.5f);
        }
        Destroy(gameObject, 0.2f);
    }

}
