using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageShow : MonoBehaviour
{
    public bool Animating = true;
    public Text DamageSize;


    public void SetUp(float damage, Material material)
    {
        DamageSize.text = damage.ToString();
        DamageSize.color = material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Animating)
        {
            Destroy(gameObject);
        }
    }
}
