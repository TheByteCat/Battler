using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInfo
{
    public int Attack;
    public float CooldownTime;
    public int Ditance;
    public float Accuracy;
    public int BulletCount;
    public float DamageReduce;
    public float AccuracyReduce;
    public float BulletSpeed;
}

[CreateAssetMenu(menuName = "Battler/Character")]
public class CharacterInfo : ScriptableObject
{

    public string Name;
    public int MaxHp;
    public float HideSpeed;
    public WeaponInfo WeaponInfo;

    public static CharacterInfo Generate()
    {
        CharacterInfo info = new CharacterInfo
        {
            MaxHp = Random.Range(70, 200),
            HideSpeed = Random.Range(0.3f, 2f),
            WeaponInfo = new WeaponInfo
            {
                Attack = Random.Range(8, 30),
                CooldownTime = Random.Range(0.5f, 3f),
                Ditance = Random.Range(1, 5),
                Accuracy = Random.Range(0.4f, 1f),
                BulletCount = Random.Range(3, 10),
                BulletSpeed = Random.Range(5, 20),
                DamageReduce = Random.Range(0.3f, 1f),
                AccuracyReduce = Random.Range(0.3f, 1f)
            }
        };
        return info;
    }
}


