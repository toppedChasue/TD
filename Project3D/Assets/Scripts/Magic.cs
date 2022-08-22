using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject bullet;
    public Transform target; //��ǥ��
    public int pow; //���ݷ�

    public float delayTime;

    public MagicBullet magicBullet;

    void Update()
    {
        transform.LookAt(target); //��ǥ���������� �ٶ󺸰��ϰ�
        StartCoroutine(Move());
        if (target == null)//��ǥ���� ���ŵǾ��ٸ�
        {
            Destroy(gameObject,0.5f);
        }
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(delayTime);
        magicBullet.bulletPow = pow;
        bullet.transform.Translate(Vector3.forward * 10 * Time.deltaTime); //�������� �̵�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //�ε����� ������ ���Ÿ���� ���ݷ� ��ŭ �������� ��
            other.GetComponent<IEnemy>().CurrentHp -= pow;
            Destroy(gameObject, 0.5f);
        }
    }
}
