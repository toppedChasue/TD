using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRook : Tower
{
    public int rookAtk;
    [SerializeField]
    private float rookAtkRange;
    [SerializeField]
    private float rookAtkDelay;
    [SerializeField]
    private float rookCurrentTime;

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
        atk = rookAtk;
        attackRange = rookAtkRange;
        attackDelay = rookAtkDelay;
        currentTime = rookCurrentTime;
    }
}
