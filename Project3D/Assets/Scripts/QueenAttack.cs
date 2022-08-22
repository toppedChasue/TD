using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAttack : MonoBehaviour
{
    public enum Type { White, Black }
    public Type type;

    public Transform target; //��ǥ��
    public int pow; //���ݷ�

    //public GameObject Bullet;
    public QueenBullet queenBullet;
    // Start is called before the first frame update
    private void Awake()
    {
        queenBullet = GetComponentInChildren<QueenBullet>();
    }

    private void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        queenBullet.bulletPow = pow;
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
        if (target != null)
        {
            switch (type)
            {
                case Type.White:
                    Destroy(gameObject, 1);
                    break;
                case Type.Black:
                    Destroy(gameObject, 1);
                    break;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
