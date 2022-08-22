using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPwan : Tower
{
    public int pwanAtk;
    [SerializeField]
    private float pwanAtkRange;
    [SerializeField]
    private float pwanAtkDelay;
    [SerializeField]
    private float pawnCurrentTime;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    protected override void Init()
    {
        base.Init();
        anim = GetComponent<Animator>();
        atk = pwanAtk;
        attackRange = pwanAtkRange;
        attackDelay = pwanAtkDelay;
        currentTime = pawnCurrentTime;
    }
}
