using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public Slot[] selfTeam = new Slot[3];
    public Slot[] enemyTeam = new Slot[3];

    float timeBetweenAttack = .1f;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("AttackPhase", 1, timeBetweenAttack);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AttackPhase()
    {
        for (int i = 0; i < 3; i++)
        {
            TryAttack(enemyTeam, selfTeam[i].character, i);
            TryAttack(selfTeam, enemyTeam[i].character, i);
        }
    }

    private void TryAttack(Slot[] targetTeam, Character character, int position)
    {

        if (character.Alive && !character.IsHide && !character.Reloading && character.HaveBullets) {
            for (int i = 0; i < 3; i++)
            {
                if (((position + i) < character.characterInfo.WeaponInfo.Ditance) && !targetTeam[i].character.IsHide && targetTeam[i].character.Alive)
                {
                    character.Attack(targetTeam[i].character);
                    break;
                }
            }
        }
    }


}
