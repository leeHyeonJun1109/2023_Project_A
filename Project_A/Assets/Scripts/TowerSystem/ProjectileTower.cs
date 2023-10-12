using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    private Tower thisTower;

    public GameObject projectile;
    public Transform firePoint;
    public float timeBetweenShots = 1f;
    private float shotCounter;

    private Transform target;
    public Transform launcherModel;

    // Start is called before the first frame update
    void Start()
    {
        thisTower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            launcherModel.rotation = Quaternion.Slerp(launcherModel.rotation, Quaternion.LookRotation(target.position - target.position), 5f * Time.deltaTime);

            launcherModel.rotation = Quaternion.Euler(0f, launcherModel.rotation.eulerAngles.y, 0f);
        }

        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0 && target != null)
        {
            shotCounter = thisTower.firerate;

            firePoint.LookAt(target);

            Instantiate(projectile, firePoint.position, firePoint.rotation);
        }

        if(thisTower.enemiesUpdate)
        {
            if(thisTower.enemiesInRange.Count > 0)
            {
                float minDistance = thisTower.range + 1;

                foreach (EnemyController enemy in  thisTower.enemiesInRange)
                {
                    if(enemy != null)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);

                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            target = transform;
                        }


                    }
                }
            }
            else
            {
                target = null;
            }
        }

    }
}
