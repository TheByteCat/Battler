using UnityEngine;
using System.Collections;
using System.Linq;

public class SelectTargetManager:MonoBehaviour
{
    public ArrowManager arrows;

    private bool[] selfSelected = new bool[3];
    private Character[] selfTeam;
    private Character[] enemyTeam;

    public void SetUp(Slot[] self, Slot[] enemy)
    {
        selfTeam = self.Select(x => x.character).ToArray();
        enemyTeam = enemy.Select(x => x.character).ToArray();
    }

    public void SelectSelf(int pos)
    {
        selfSelected[pos] = !selfSelected[pos];
        selfTeam[pos].SetSelectState(selfSelected[pos]);
    }

    public void SelectEnemy(int pos)
    {
        for (int i = 0; i < 3; i++) {
            if (selfSelected[i]) {
                arrows.SelectTarget(i, pos);
                selfTeam[i].Target = enemyTeam[pos];
            }
            selfSelected[i] = false;
            selfTeam[i].SetSelectState(false);
        }
    }

    public void Select(int source, int target)
    {
        arrows.SelectTarget(source, target);
    }

    public void UnSelect(int source)
    {
        arrows.DisableRaw(source);
    }
}
