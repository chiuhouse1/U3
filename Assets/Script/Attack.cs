using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    /*
    public int Sword { get; set; }
    public int Ax { get; set; }
    public int Spear { get; set; }
    public int Bow { get; set; }
    */
    public int Sword;
    public int Ax;
    public int Spear;
    public int Bow;
    public float MergeBonus { get; set; }
    public int AttackDamage { get; set; }

    public int DamageCalculation(int sword, int ax, int spear, int bow, float bonus)
    {
        return (int)Mathf.Floor((Sword * sword) + (Ax * ax) + (Spear * spear) + (Bow * bow) * bonus);
    }
}
