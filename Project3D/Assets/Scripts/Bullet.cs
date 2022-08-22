using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect; //이펙트
    public Transform target; //목표물
    public int pow; //공격력
    public TrailRenderer trailRenderer;

    void Update()
    {
        transform.LookAt(target); //목표물방향으로 바라보게하고
        transform.Translate(Vector3.forward * 10 * Time.deltaTime); //정면으로 이동
        if (target == null)//목표물이 제거되었다면
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
