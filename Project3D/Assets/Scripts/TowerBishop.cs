using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBishop : Tower
{
    public int bishopAtk;
    [SerializeField]
    private float bishopAtkRange;
    [SerializeField]
    private float bishopAtkDelay;
    [SerializeField]
    private float bishopCurrentTime;

    

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
        atk = bishopAtk;
        attackRange = bishopAtkRange;
        attackDelay = bishopAtkDelay;
        currentTime = bishopCurrentTime;
    }
}
