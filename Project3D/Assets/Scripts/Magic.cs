using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject bullet;
    public Transform target; //목표물
    public int pow; //공격력

    public float delayTime;

    public MagicBullet magicBullet;

    void Update()
    {
        transform.LookAt(target); //목표물방향으로 바라보게하고
        StartCoroutine(Move());
        if (target == null)//목표물이 제거되었다면
        {
            Destroy(gameObject,0.5f);
        }
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(delayTime);
        magicBullet.bulletPow = pow;
        bullet.transform.Translate(Vector3.forward * 10 * Time.deltaTime); //정면으로 이동
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //부딪히면 적에게 비숍타워의 공격력 만큼 데미지를 줌
            other.GetComponent<IEnemy>().CurrentHp -= pow;
            Destroy(gameObject, 0.5f);
        }
    }
}
