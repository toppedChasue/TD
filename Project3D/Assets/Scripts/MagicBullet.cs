using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public int bulletPow;

    public enum Type { Front, Right }
    public Type type;

    private void Update()
    {
        switch(type)
        {
            case Type.Front:
                transform.Rotate(Vector3.left * -speed);
                break;
            case Type.Right:
                transform.Rotate(Vector3.forward * speed);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().CurrentHp -= bulletPow;
            gameObject.SetActive(false);
        }
    }

}
