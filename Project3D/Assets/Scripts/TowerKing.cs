using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerKing : Tower
{
    public int kingAtk;
    [SerializeField]
    private float kingAtkRange;
    [SerializeField]
    private float kingAtkDelay;
    [SerializeField]
    private float kingCurrentTime;

    void Start()
    {
        Init();
    }
    void Update()
    {
        Attack();
    }

    protected override void Init()
    {
        base.Init();
        anim = GetComponent<Animator>();
        atk = kingAtk;
        attackRange = kingAtkRange;
        attackDelay = kingAtkDelay;
        currentTime = kingCurrentTime;
    }
}
