using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState")]

public class EnemyState : ScriptableObject
{
    public Sprite EnemyPicture;

    public int Hp;

    public int Armor;

    public int Damage;

    public int Round;
}

