using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpwanManager : MonoBehaviour
{
    //웨이브가 도착해야하는 지점들
    public static Transform[] wayPoints;
    //스폰시작위치
    [SerializeField]
    private Transform spwanPos;
    //적 프리펩
    [SerializeField]
    private GameObject enemyPrefab;
    //적 소환 관련시간
    private float spwanStandByTime = 10; //처음 소환까지 대기시간
    public float restartSpwanTime; //계속 소환
    public float currentTime; //0이 되면 소환시작

    public float waveCount; // 몇 마리 소환할 것인가
    private float countdown; //카운트다운이 끝나면 소환 끝
    private float betweenSpwanTime = 1.5f;//한 웨이브 사이의 몹과 몹의 젠 시간

    //필요 컴포넌트
    public GameManager gameManager;
    public GameObject spwanParticle;
    [SerializeField]
    private PlayerGold playerGold;

    [SerializeField]
    private GameObject enemyHPSliderPrefab;
    [SerializeField]
    private Transform canvasTransform;
    List<Enemy> enemyList = new List<Enemy>();
    List<GameObject> enemyObj = new List<GameObject>();
    public Timer timer;
    public Button waveStartButton;

    //코루틴 정지를 위한 변수
    Coroutine coroutine;
    void Awake()
    {
        SetUp();
        currentTime = spwanStandByTime;
    }
    private void Update()
    {
        if (timer.isStart)
            Spwan();
        ButtonInterectable();
    }
    public void Restart()
    {
        EnemyClear();
        WaypointsClear();
        WaveStop();
        currentTime = spwanStandByTime;
    }

    private void EnemyClear()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList != null)
            {
                Destroy(enemyList[i]);
                Destroy(enemyObj[i]);
            }
        }
        enemyList.Clear();
        enemyObj.Clear();
    }
    private void WaypointsClear()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (wayPoints != null)
            {
                Destroy(wayPoints[i]);
            }
        }
    }

    private void SetUp()
    {
        currentTime -= Time.deltaTime;
        wayPoints = new Transform[transform.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = transform.GetChild(i);
        }
    }

    private IEnumerator SpwanWave()
    { //적 소환 코루틴 waveConunt 만큼 적을 소환
        countdown = waveCount;

        for (int i = 0; i < countdown; i++)
        {
            if (countdown > 0)
            {
                GameObject obj = Instantiate(enemyPrefab, spwanPos.position, spwanPos.rotation);
                enemyObj.Add(obj);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemyList.Add(enemy);
                spwanParticle.SetActive(true);
                yield return new WaitForSeconds(betweenSpwanTime);
                countdown -= Time.deltaTime;
                spwanParticle.SetActive(false);
            }
        }
    }

    private void Spwan()
    {
        currentTime -= Time.deltaTime; //시간 점점 줄어듬

        if (currentTime <= 0)
        {//타이머가 끝나고
            currentTime = restartSpwanTime;
            //준비시간 주어짐
            coroutine = StartCoroutine(SpwanWave());
            //코루틴 실행
            gameManager.stage++;
        }
    }

    public void WaveStart()
    {
        if (currentTime < 30)
        {
            currentTime = 5;
            Spwan();
        }
        else
            return;
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if (type == EnemyDestroyType.Arrive)
        {
            gameManager.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += (gold * gameManager.stage) * 2;
        }
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject, 1);
    }

    public void WaveStop()
    {
        spwanParticle.SetActive(false);
        StopCoroutine(coroutine);
    }

    private void ButtonInterectable()
    {
        if (currentTime <= 30)
            waveStartButton.interactable = true;
        else if (currentTime > 30)
            waveStartButton.interactable = false;
    }

}
