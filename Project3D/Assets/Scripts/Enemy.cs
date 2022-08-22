using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    //���� ����
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Gold { get; set; }
}

public enum EnemyDestroyType { Arrive=0, Kill }

public class Enemy : MonoBehaviour, IEnemy
{
    //�̵� ����
    [SerializeField]
    protected float moveSpeed;//�ӵ�
    [SerializeField]
    private Vector3 moveDirection;//����

    //��������Ʈ ����
    protected Transform wayPoints; //�̵� ��� ����
    private int currentIndex = 0; //���� ��ǥ���� �ε���

    //���
    [SerializeField]
    private int gold = 10;

    //�ʿ�������Ʈ
    public Animator anim;
    public SpwanManager spwanManager;
    public GameManager gameManager;

    public bool isDead = false;

    //������������
    public int MaxHp { get; set; }
    public int Gold { get; set; }
    public int currentHp;

    public int CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
            if(currentHp <= 0)
            {
                isDead = true;
                gameObject.tag = "Die";
                OnDie(EnemyDestroyType.Kill);
            }
        }
    }

    private void Start()
    {
        wayPoints = SpwanManager.wayPoints[1];
        Init();
    }

    private void Update()
    {
        Move();
    }

    //�ʱ⼳�� �Լ�
    private void Init() 
    {
        anim = GetComponent<Animator>();
        spwanManager = GameObject.Find("@SpawnManager").GetComponent<SpwanManager>();
        gameManager = GameObject.Find("@GameManager").GetComponent<GameManager>();
        //���̾��Ű â���� @SpawnManager ������Ʈ�� ã�Ƽ� �� �ȿ� �ִ� ������Ʈ SpwanManager�� ������ �־���
        MaxHp = (gameManager.stage) * 5;
        CurrentHp = MaxHp;
        moveSpeed = 0.5f + (gameManager.stage / 5 );
    }

    //�� �̵�
    private void Move()
    {
        if(!isDead)
        {
            //��������
            moveDirection = wayPoints.position - transform.position;
            //������
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, wayPoints.position) <= 0.2f)
            {
                GetNextWayPoint();
            }
        }

    }
    //�� ���� �̵���� ȹ�� �� ������������ ����
    private void GetNextWayPoint()
    {
        if (currentIndex >= SpwanManager.wayPoints.Length -1)
        {//�ι��� ���� ���ֹ�����
            isDead = true;
            OnDie(EnemyDestroyType.Arrive);
            currentIndex = 0;
            gold = 0;
        }
        currentIndex++;
        wayPoints = SpwanManager.wayPoints[currentIndex];
        transform.LookAt(wayPoints);
    }

    public void OnDie(EnemyDestroyType type)
    {
        if (isDead)
        {
            anim.SetTrigger("Die");
            spwanManager.DestroyEnemy(type, this, gold);
        }
    }
}
