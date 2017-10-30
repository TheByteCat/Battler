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
}

[CreateAssetMenu(menuName = "Battler/Character")]
public class CharacterInfo : ScriptableObject {

    public string Name;
    public int MaxHp;
    public WeaponInfo WeaponInfo;
	
}
