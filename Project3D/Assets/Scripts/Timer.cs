using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timer;
    public Text timeTxt;

    public bool isStart = true;

    [SerializeField]
    private Text cleatTimeTxt;

    // Update is called once per frame
    void Update()
    {
        if(isStart)
        timer += Time.deltaTime;
        
    }

    private void LateUpdate()
    {
        int hour = (int)(timer / 3600);
        int min = (int)((timer - hour * 3600) / 60);
        int second = (int)(timer % 60);

        timeTxt.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
        cleatTimeTxt.text = "±â·Ï : " + timeTxt;
    }

    public void Restart()
    {
        timer = 0f;
    }
}
