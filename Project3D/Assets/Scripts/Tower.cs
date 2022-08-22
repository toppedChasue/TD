using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Type { Pwan, Bishop, Rook, Knight, Queen, King }
    public Type type;

    public GameObject node;
    public GameObject bullet; //�Ѿ� ������Ʈ
    public Transform bulletPos;

    // * ����
    protected int atk; //���ݷ�
    protected float attackRange; //���ݹ���
    protected float attackDelay; //���ݵ�����
    protected float currentTime; //���� �� �ð�

    protected Transform towerTarget; //���� ��ǥ��
    protected Vector3 direction; //���� ��ǥ���� ����
    protected Animator anim; //�ִϸ�����

    bool isAttack = false;

    protected virtual void Init()
    {
        anim = GetComponent<Animator>();
        atk = 0;
        attackRange = 0;
        attackDelay = 0;
        currentTime = 0;
    }

    protected void Attack()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, attackRange);
        //Physics.OverlapSphere : Sphere������� ���� �ȿ� �ݸ����� ����
        //transform.position : �߽���, attackRange : �����Ÿ�
        //�ݸ����� ���� �� ������ ��� �ݸ����� ��ȯ

        List<Collider> enemys = new List<Collider>();
      
        for (int i = 0; i < others.Length; i++)
        {//������ ��� �ݶ��̴� ������ŭ �ݺ�
            if (others[i].CompareTag("Enemy"))
            {//�ݶ��̴� �߿� Enemy�±׸� ���� �ݶ��̴���
                enemys.Add(others[i]); //����Ʈ�� �־��� 
            }
        }
        currentTime += Time.deltaTime;//��Ÿ���� ���� �ð� ����
        if (enemys != null) //����Ʈ�� ���� �ִٸ�
        {
            float shortDis = attackRange; //���� ������ �־���
            towerTarget = null;
            for (int i = 0; i < enemys.Count; i++)//���ʹ� ������ŭ �ݺ�
            {
                float dis = Vector3.Distance(transform.position, enemys[i].transform.position);
                //Ÿ���� ���ʹ��� �Ÿ��� dis������ �־���
                if (dis < shortDis)
                {
                    shortDis = dis;
                    towerTarget = enemys[i].transform;
                }//���� ����� �Ÿ��� �ִ� ���� Ÿ������ �־���
            }
            if (towerTarget != null && currentTime > attackDelay)
            { //�����Ÿ� �ȿ� ���� �ְ�, ���� ��Ÿ���� ����
                switch (type)
                {
                    case Type.Pwan:
                        PwanAttack();
                        break;

                    case Type.Bishop:
                        BishopAttack();
                        break;

                    case Type.Rook:
                        RookAttack();
                        break;

                    case Type.Knight:
                        KnightAttack();
                        break;

                    case Type.King:
                        KingAttack();
                        break;

                    case Type.Queen:
                        QueenAttack();
                        break;

                }

            }
            if (towerTarget != null)
            {
                direction = towerTarget.transform.position - transform.position;
                //Ÿ�������� ������ ����
                transform.rotation = Quaternion.LookRotation(direction.normalized);
                //Ÿ�� �������� ȸ��
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                //Y�����θ� ȸ���ϵ��� ����
            }
        }

    }
    private void OnDrawGizmos()
    {//�׷��ִ� �����ֱ� �Լ�
        Gizmos.color = new Color32(50, 50, 50, 50);
        Gizmos.DrawSphere(transform.position, attackRange);
        //Sphere�� �׷���
    }
    private void PwanAttack()
    { // ���� ���� ���
        Bullet temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<Bullet>();
        //�Ѿ� ���� �� �Ѿ˿� �ִ� Bullet������Ʈ�� temp�� �־���
        temp.target = towerTarget; //�Ѿ˿� ��ǥ�� ����
        temp.pow = atk; //���ݷ� ����
        currentTime = 0; //������ �ʱ�ȭ
    }

    private void BishopAttack()
    {//����� ���ݹ��
        Magic temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<Magic>();
        //�Ѿ� ���� �� �Ѿ˿� �ִ� Bullet������Ʈ�� temp�� �־���
        temp.target = towerTarget; //�Ѿ˿� ��ǥ�� ����
        temp.pow = atk;//���ݷ� ����
        currentTime = 0; //������ �ʱ�ȭ
    }

    private void RookAttack()
    {
        StoneAttack temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<StoneAttack>();
        //���� ���� ����
        temp.target = towerTarget;
        temp.pow = atk;
        currentTime = 0;
    }

    private void KnightAttack()
    {//isAttack = true
        if (!isAttack) //1 .ó�� ���°� true�� ������ �ȵ�
        {//3. if�� ����
            isAttack = true;
            StartCoroutine(TowerMove());
            currentTime = 0;
        }
    }

    private IEnumerator TowerMove()
    {
        while (transform.position.y < 1.5f)
        {
            transform.Translate(new Vector3(0, 2, 0) * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y > 0.5f)
        {
            transform.Translate(new Vector3(0, -3, 0) * Time.deltaTime);
            yield return null;
        }

        KnightAttack temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<KnightAttack>();
        temp.target = towerTarget;
        temp.pow = atk;

        yield return new WaitForSeconds(1);
        isAttack = false;
    }
    private void QueenAttack()
    {//isAttack = true
        if (!isAttack) //1 .ó�� ���°� true�� ������ �ȵ�
        {//3. if�� ����
            isAttack = true;
            StartCoroutine(TowerMove2());
            currentTime = 0;
        }
    }
    private IEnumerator TowerMove2()
    {
        while (transform.position.y < 1.5f)
        {
            transform.Translate(new Vector3(0, 2, 0) * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y > 0.5f)
        {
            transform.Translate(new Vector3(0, -3, 0) * Time.deltaTime);
            yield return null;
        }

        QueenAttack temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<QueenAttack>();
        temp.target = towerTarget;
        temp.pow = atk;
        yield return new WaitForSeconds(1);
        isAttack = false;
    }

    private void KingAttack()
    {
        if (!isAttack) //1 .ó�� ���°� true�� ������ �ȵ�
        {//3. if�� ����
            isAttack = true;
            StartCoroutine(TowerMove3());
            currentTime = 0;
        }
    }

    private IEnumerator TowerMove3()
    {
        while (transform.position.y < 1.5f)
        {
            transform.Translate(new Vector3(0, 2, 0) * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y > 0.5f)
        {
            transform.Translate(new Vector3(0, -3, 0) * Time.deltaTime);
            yield return null;
        }
        KingAttack temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<KingAttack>();
        temp.target = towerTarget;
        temp.pow = atk;
        yield return new WaitForSeconds(1);
        isAttack = false;
    }

}
