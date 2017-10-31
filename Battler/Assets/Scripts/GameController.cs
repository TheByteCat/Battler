using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public Slot[] selfTeam = new Slot[3];
    public Slot[] enemyTeam = new Slot[3];
    public SelectTargetManager targetManager;

    // Use this for initialization
    void Start()
    {
        targetManager.SetUp(selfTeam, enemyTeam);
        StartCoroutine(StartBattle());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HideAll()
    {
        foreach (var c in selfTeam)
            c.character.SetPosition();
        foreach (var c in enemyTeam)
            c.character.SetPosition();
    }

    private IEnumerator StartBattle()
    {
        HideAll();
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(selfTeam[i].character.Battle(enemyTeam, i));
            StartCoroutine(enemyTeam[i].character.Battle(selfTeam, i));
        }
    }

}
