using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpwanManager : MonoBehaviour
{
    //���̺갡 �����ؾ��ϴ� ������
    public static Transform[] wayPoints;
    //����������ġ
    [SerializeField]
    private Transform spwanPos;
    //�� ������
    [SerializeField]
    private GameObject enemyPrefab;
    //�� ��ȯ ���ýð�
    private float spwanStandByTime = 10; //ó�� ��ȯ���� ���ð�
    public float restartSpwanTime; //��� ��ȯ
    public float currentTime; //0�� �Ǹ� ��ȯ����

    public float waveCount; // �� ���� ��ȯ�� ���ΰ�
    private float countdown; //ī��Ʈ�ٿ��� ������ ��ȯ ��
    private float betweenSpwanTime = 1.5f;//�� ���̺� ������ ���� ���� �� �ð�

    //�ʿ� ������Ʈ
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

    //�ڷ�ƾ ������ ���� ����
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
    { //�� ��ȯ �ڷ�ƾ waveConunt ��ŭ ���� ��ȯ
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
        currentTime -= Time.deltaTime; //�ð� ���� �پ��

        if (currentTime <= 0)
        {//Ÿ�̸Ӱ� ������
            currentTime = restartSpwanTime;
            //�غ�ð� �־���
            coroutine = StartCoroutine(SpwanWave());
            //�ڷ�ƾ ����
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
