using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private float timeAlive = 0.0f;
    void Start()
    {

    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > 1.0f)
        {
            Destroy(gameObject);
        }
    }
}