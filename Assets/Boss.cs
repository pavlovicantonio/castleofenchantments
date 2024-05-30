using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject player;
    public float speed = 3f;
    public float attackDistance = 2f;
    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);


        if (distance > attackDistance)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }


    private void Attack()
    {

        Debug.Log("Boss is attacking the player!");
    }
}
