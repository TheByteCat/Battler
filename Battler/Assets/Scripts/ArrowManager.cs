using UnityEngine;
using System.Collections;
using System.Linq;

[System.Serializable]
public class ArrowPack
{
    public GameObject[] Arrows = new GameObject[3];
}


public class ArrowManager : MonoBehaviour
{
    public ArrowPack[] arrowPacks = new ArrowPack[3];
    // Use this for initialization
    void Start()
    {
        DisableAll();
    }

    public void DisableAll()
    {
        foreach (var ap in arrowPacks)
            foreach (var arr in ap.Arrows)
                arr.SetActive(false);
    }

    public void DisableColumn(int target)
    {
        foreach (var arr in arrowPacks[target].Arrows)
            arr.SetActive(false);
    }

    public void DisableRaw(int raw)
    {
        foreach (var ap in arrowPacks)
            ap.Arrows[raw].SetActive(false);
    }

    public void SelectTarget(int source, int target)
    {
        DisableRaw(source);
        arrowPacks[target].Arrows[source].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
