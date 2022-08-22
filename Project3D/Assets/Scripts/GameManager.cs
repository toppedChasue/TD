using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //��Ʈ��
    public GameObject startPanel;

    //�Ͻ�����
    public GameObject pausePanel;

    //�ƿ�Ʈ��
    public GameObject overPanel;

    //Ŭ����
    public GameObject clearPanel;
    //UI
    [SerializeField]
    private Text playerGoldTxt; //���� ���ǥ�� �ؽ�Ʈ

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


    //UI-�ɼ�â

    //�÷��̾� Hp����
    [SerializeField]
    private float maxHP = 20; //�ִ�ü��
    public float currentHP; //����ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    //�ʿ�������Ʈ
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

        ////�����ߴ� �͵��� �ʱ�ȭ �ؾ���
        //spwanManager.Restart(); //��, 
        //buildManager.Retart();
        //mixButton.Restart();
        //timer.Restart();
        //playerGold.CurrentGold = 100;
    }

    public void TakeDamage(float damage)
    {
        //���� ü���� damage��ŭ ����
        currentHP -= damage;
        StartCoroutine(playerHit());

        if(currentHP <= 0)
        {
            GameOver();
        }
    }

    private void LateUpdate()
    {
        //��� �ؽ�Ʈ
        playerGoldTxt.text = string.Format("{0:n0}", playerGold.CurrentGold);
        //�÷��̾� ü��
        hpBar.fillAmount = currentHP / maxHP;
        //Ÿ�̸�
        currentTimer.fillAmount = spwanManager.currentTime / spwanManager.restartSpwanTime;
        //��������
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
