using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : myEventSystem.SingletonAsComponent<GameController>
{
    public Slot[] selfTeam = new Slot[3];
    public Slot[] enemyTeam = new Slot[3];
    public SelectTargetManager TargetManager;
    public bool IsPause;

    // Use this for initialization
    void Start()
    {
        TargetManager.SetUp(selfTeam, enemyTeam);
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
        //HideAll();
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            selfTeam[i].character.StartBattle(enemyTeam, i);
            enemyTeam[i].character.StartBattle(selfTeam, i);
        }
    }

    public void PauseGame()
    {
        IsPause = true;
    }

    public void ResumeGame()
    {
        IsPause = false;
    }
}
