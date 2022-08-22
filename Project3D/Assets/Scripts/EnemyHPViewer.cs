using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private Enemy enemyHp;
    private Slider hpSlider;

    public void SetUp(Enemy enemyHp)
    {
        this.enemyHp = enemyHp;
        hpSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = enemyHp.currentHp / enemyHp.MaxHp;
    }
}
