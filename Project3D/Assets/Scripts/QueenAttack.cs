using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAttack : MonoBehaviour
{
    public enum Type { White, Black }
    public Type type;

    public Transform target; //목표물
    public int pow; //공격력

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
