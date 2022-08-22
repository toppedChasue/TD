using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackKingBullet : MonoBehaviour
{
    [SerializeField]
    public int bulletPow;

    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private new BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BackGround"))
        {
            transform.position = transform.position;
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().CurrentHp -= bulletPow;
            StartCoroutine(Effect());
        }
    }

    private IEnumerator Effect()
    {
        collider.enabled = true;
        effect.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}
