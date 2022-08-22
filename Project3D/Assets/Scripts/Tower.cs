using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Type { Pwan, Bishop, Rook, Knight, Queen, King }
    public Type type;

    public GameObject node;
    public GameObject bullet; //총알 오브젝트
    public Transform bulletPos;

    // * 공격
    protected int atk; //공격력
    protected float attackRange; //공격범위
    protected float attackDelay; //공격딜레이
    protected float currentTime; //공격 후 시간

    protected Transform towerTarget; //공격 목표물
    protected Vector3 direction; //공격 목표로의 방향
    protected Animator anim; //애니메이터

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
        //Physics.OverlapSphere : Sphere모양으로 범위 안에 콜리더를 감지
        //transform.position : 중심점, attackRange : 사정거리
        //콜리더를 감지 후 감지한 모든 콜리더를 반환

        List<Collider> enemys = new List<Collider>();
      
        for (int i = 0; i < others.Length; i++)
        {//감지만 모든 콜라이더 갯수만큼 반복
            if (others[i].CompareTag("Enemy"))
            {//콜라이더 중에 Enemy태그를 가진 콜라이더만
                enemys.Add(others[i]); //리스트에 넣어줌 
            }
        }
        currentTime += Time.deltaTime;//쿨타임을 위한 시간 누적
        if (enemys != null) //리스트에 값이 있다면
        {
            float shortDis = attackRange; //공격 범위를 넣어줌
            towerTarget = null;
            for (int i = 0; i < enemys.Count; i++)//에너미 갯수만큼 반복
            {
                float dis = Vector3.Distance(transform.position, enemys[i].transform.position);
                //타워와 에너미의 거리를 dis변수에 넣어줌
                if (dis < shortDis)
                {
                    shortDis = dis;
                    towerTarget = enemys[i].transform;
                }//가장 가까운 거리에 있는 적을 타겟으로 넣어줌
            }
            if (towerTarget != null && currentTime > attackDelay)
            { //사정거리 안에 적이 있고, 공격 쿨타임이 돌면
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
                //타겟으로의 방향을 구함
                transform.rotation = Quaternion.LookRotation(direction.normalized);
                //타겟 방향으로 회전
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                //Y축으로만 회전하도록 변경
            }
        }

    }
    private void OnDrawGizmos()
    {//그려주는 생명주기 함수
        Gizmos.color = new Color32(50, 50, 50, 50);
        Gizmos.DrawSphere(transform.position, attackRange);
        //Sphere를 그려줌
    }
    private void PwanAttack()
    { // 폰의 공격 방식
        Bullet temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<Bullet>();
        //총알 생성 및 총알에 있는 Bullet컴포넌트를 temp에 넣어줌
        temp.target = towerTarget; //총알에 목표물 전달
        temp.pow = atk; //공격력 전달
        currentTime = 0; //딜레이 초기화
    }

    private void BishopAttack()
    {//비숍의 공격방식
        Magic temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<Magic>();
        //총알 생성 및 총알에 있는 Bullet컴포넌트를 temp에 넣어줌
        temp.target = towerTarget; //총알에 목표물 전달
        temp.pow = atk;//공격력 전달
        currentTime = 0; //딜레이 초기화
    }

    private void RookAttack()
    {
        StoneAttack temp = Instantiate(bullet, bulletPos.position, Quaternion.identity).GetComponent<StoneAttack>();
        //공격 수단 생성
        temp.target = towerTarget;
        temp.pow = atk;
        currentTime = 0;
    }

    private void KnightAttack()
    {//isAttack = true
        if (!isAttack) //1 .처음 상태가 true라 실행이 안됨
        {//3. if문 실행
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
        if (!isAttack) //1 .처음 상태가 true라 실행이 안됨
        {//3. if문 실행
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
        if (!isAttack) //1 .처음 상태가 true라 실행이 안됨
        {//3. if문 실행
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
