using UnityEngine;
using UnityEngine.UI;

public class SkeletonSword : MonoBehaviour
{


    public GameObject player;
    public float speed;

    private float distance;

    void Start()
    {

    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }
}