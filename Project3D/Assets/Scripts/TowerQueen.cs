using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerQueen : Tower
{
    public int queenAtk;
    [SerializeField]
    private float queenAtkRange;
    [SerializeField]
    private float queenAtkDelay;
    [SerializeField]
    private float queenCurrentTime;

    private float guageCurrentTime = 0;
    private float guageMaxTime = 30;

    void Start()
    {
        Init();
    }
    void Update()
    {
        Attack();
        StartCoroutine(PowerUp());
    }

    protected override void Init()
    {
        base.Init();
        anim = GetComponent<Animator>();
        atk = queenAtk;
        attackRange = queenAtkRange;
        attackDelay = queenAtkDelay;
        currentTime = queenCurrentTime;
    }

    private IEnumerator PowerUp()
    {
        guageCurrentTime += Time.deltaTime;

        if (guageCurrentTime == guageMaxTime)
        {
            currentTime = 0;
            atk = queenAtk * 2;
            attackDelay = queenAtkDelay / 2;
            guageCurrentTime = 0;
        }
            yield return new WaitForSeconds(5);
        atk = queenAtk;
        attackDelay = queenAtkDelay;
    }


}
