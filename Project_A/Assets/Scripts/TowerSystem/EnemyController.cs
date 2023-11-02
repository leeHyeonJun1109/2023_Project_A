using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float speedMod = 1.0f;
    public float timeSinceStart = 0.0f;

    private MonsterPath thePath;
    private int currentPoint;
    private bool reachedEnd;

    private bool modEnd = true;
    // Start is called before the first frame update
    void Start()
    {
        if (thePath == null)
        {
            thePath = FindObjectOfType<MonsterPath>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(modEnd == false)
        {
            timeSinceStart -= Time.deltaTime;

            if(timeSinceStart <= 0.0f)
            {
                speedMod = 1.0f;
                modEnd = true;
            }
        }

        if(reachedEnd == false)
        {
            transform.LookAt(thePath.points[currentPoint]);

            transform.position = Vector3.MoveTowards(transform.position, thePath.points[currentPoint].position, moveSpeed * Time.deltaTime * speedMod);

            if(Vector3.Distance(transform.position, thePath.points[currentPoint].position) <  0.01f)
            {
                currentPoint += 1;

                if(currentPoint >= thePath.points.Length)
                {
                    reachedEnd = true;
                }
            }
        }

    }
    public void SetMode(float value)
    {
        modEnd = false;
        speedMod = value;
        timeSinceStart = 2.0f;
    }
}
