using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerKnight : Tower
{
    public int knightAtk;
    [SerializeField]
    private float knightAtkRange;
    [SerializeField]
    private float knightAtkDelay;
    [SerializeField]
    private float knightCurrentTime;

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
        atk = knightAtk;
        attackRange = knightAtkRange;
        attackDelay = knightAtkDelay;
        currentTime = knightCurrentTime;
    }
}
