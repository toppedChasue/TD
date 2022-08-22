using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //인트로
    public GameObject startPanel;

    //일시정지
    public GameObject pausePanel;

    //아웃트로
    public GameObject overPanel;

    //클리어
    public GameObject clearPanel;
    //UI
    [SerializeField]
    private Text playerGoldTxt; //현재 골드표시 텍스트

    [SerializeField]
    private Image currentTimer;

    [SerializeField]
    private RectTransform hpGroup;
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private Image playerImage;

    public int stage;
    [SerializeField]
    private Text stageTxt;


    //UI-옵션창

    //플레이어 Hp관리
    [SerializeField]
    private float maxHP = 20; //최대체력
    public float currentHP; //현재체력

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    //필요컴포넌트
    [SerializeField]
    private PlayerGold playerGold;
    public SpwanManager spwanManager;
    public Timer timer;
    public BuildManager buildManager;
    public MixButton mixButton;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void GameStart()
    {
        startPanel.SetActive(false);
        timer.isStart = true;
        Time.timeScale = 1;
        stage = 0;
    }

    public void GamePause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PauseOut()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        Time.timeScale = 0;
        timer.isStart = false;
        spwanManager.WaveStop();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //stage = 0;
        //currentHP = maxHP;
        
        //overPanel.SetActive(false);
        //clearPanel.SetActive(false);
        //startPanel.SetActive(true);

        ////저장했던 것들을 초기화 해야함
        //spwanManager.Restart(); //적, 
        //buildManager.Retart();
        //mixButton.Restart();
        //timer.Restart();
        //playerGold.CurrentGold = 100;
    }

    public void TakeDamage(float damage)
    {
        //현재 체력을 damage만큼 감소
        currentHP -= damage;
        StartCoroutine(playerHit());

        if(currentHP <= 0)
        {
            GameOver();
        }
    }

    private void LateUpdate()
    {
        //골드 텍스트
        playerGoldTxt.text = string.Format("{0:n0}", playerGold.CurrentGold);
        //플레이어 체력
        hpBar.fillAmount = currentHP / maxHP;
        //타이머
        currentTimer.fillAmount = spwanManager.currentTime / spwanManager.restartSpwanTime;
        //스테이지
        stageTxt.text = "WAVE : " + stage;
    }

    private IEnumerator playerHit()
    {
        playerImage.color =  new Color32(255,0,0,255);
        yield return new WaitForSeconds(0.2f);
        playerImage.color = new Color32(255, 255, 255, 255);
        yield return null;
    }

    private void GameClear()
    {
        if(stage >= 20)
        {
            clearPanel.SetActive(true);
            Time.timeScale = 0;
            timer.isStart = false;
            spwanManager.WaveStop();
        }
    }

    
}
