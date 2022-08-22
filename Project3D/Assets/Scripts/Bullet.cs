using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect; //����Ʈ
    public Transform target; //��ǥ��
    public int pow; //���ݷ�
    public TrailRenderer trailRenderer;

    void Update()
    {
        transform.LookAt(target); //��ǥ���������� �ٶ󺸰��ϰ�
        transform.Translate(Vector3.forward * 10 * Time.deltaTime); //�������� �̵�
        if (target == null)//��ǥ���� ���ŵǾ��ٸ�
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().CurrentHp -= pow;
            Instantiate(effect, transform.position + new Vector3(0, 0.5f, 0),
                Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
