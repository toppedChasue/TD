using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
    //구현 강제
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Gold { get; set; }
}

public enum EnemyDestroyType { Arrive=0, Kill }

public class Enemy : MonoBehaviour, IEnemy
{
    //이동 정보
    [SerializeField]
    protected float moveSpeed;//속도
    [SerializeField]
    private Vector3 moveDirection;//방향

    //웨이포인트 정보
    protected Transform wayPoints; //이동 경로 정보
    private int currentIndex = 0; //현재 목표지점 인덱스

    //골드
    [SerializeField]
    private int gold = 10;

    //필요컴포넌트
    public Animator anim;
    public SpwanManager spwanManager;
    public GameManager gameManager;

    public bool isDead = false;

    //강제구현정보
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

    //초기설정 함수
    private void Init() 
    {
        anim = GetComponent<Animator>();
        spwanManager = GameObject.Find("@SpawnManager").GetComponent<SpwanManager>();
        gameManager = GameObject.Find("@GameManager").GetComponent<GameManager>();
        //하이어라키 창에서 @SpawnManager 오브젝트를 찾아서 그 안에 있는 컴포넌트 SpwanManager를 변수에 넣어줌
        MaxHp = (gameManager.stage) * 5;
        CurrentHp = MaxHp;
        moveSpeed = 0.5f + (gameManager.stage / 5 );
    }

    //적 이동
    private void Move()
    {
        if(!isDead)
        {
            //방향지정
            moveDirection = wayPoints.position - transform.position;
            //움직임
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, wayPoints.position) <= 0.2f)
            {
                GetNextWayPoint();
            }
        }

    }
    //적 다음 이동장소 획득 및 도착지점에서 삭제
    private void GetNextWayPoint()
    {
        if (currentIndex >= SpwanManager.wayPoints.Length -1)
        {//두바퀴 돌고 없애버리기
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
