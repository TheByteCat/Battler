using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float DistBetweenCharacters = 3.3f;
    public float DistBetweenTeams = 4.7f;
    public float Speed = 10;
    public Renderer Renderer;

    private float realDistance;
    private float distanceLeft;
    private Vector3 direction;
    private bool IsReady = false;
    // Use this for initialization
    void Start()
    {

    }

    public void Send(int Distance, Material material)
    {
        realDistance = DistBetweenTeams + Distance * DistBetweenCharacters;
        direction = Vector3.right;
        distanceLeft = 0;
        Renderer.material = material;
        IsReady = true;
    }

    public float ShotTime { get { return realDistance / Speed; } }

    // Update is called once per frame
    void Update()
    {
        if (IsReady)
        {
            if (distanceLeft < realDistance)
            {
                var deltaDist = direction * Time.deltaTime * Speed;
                gameObject.transform.Translate(deltaDist);
                distanceLeft += deltaDist.x;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
