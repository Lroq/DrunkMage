using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float speed = 5f;
    public Transform player;

    void Update()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, speed * Time.deltaTime);
    }
}
