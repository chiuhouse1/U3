using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class Enemy : MonoBehaviour
{
    public EnemyState state { get; private set; }
    public int hp { get; private set; }
    public int armor { get; private set; }
    public int damage { get; private set; }
    public int round { get; private set; }

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetState(EnemyState state, Slider hpbar, Slider hpanibar, Slider armorbar)     //³]¸mª¬ºA
    {
        this.state = state;

        hp = state.Hp;
        armor = state.Armor;
        damage = state.Damage;
        round = state.Round;
        spriteRenderer.sprite = state.EnemyPicture;
        hpbar.maxValue = hp;
        hpbar.value = hpbar.maxValue;
        hpanibar.maxValue = hp;
        hpanibar.value = hpanibar.maxValue;
        if (state.Armor != 0)
        {
            armorbar.maxValue = armor;
            armorbar.value = armor;
        }
        else
        {
            armorbar.value = 0;
        }
    }
}
