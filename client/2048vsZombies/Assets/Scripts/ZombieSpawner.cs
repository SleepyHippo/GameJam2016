using UnityEngine;
using System.Collections;
using SleepyHippo.Util;

public class ZombieSpawner : MonoBehaviour {

    public Zombie zombieTemplate;

    public Zombie Spawn()
    {
        GameObject zombieObject = GameObjectPool.Instance.Spawn(zombieTemplate.gameObject, 5);
        zombieObject.transform.position = transform.position;
        Zombie zombie = zombieObject.GetComponent<Zombie>();
        zombie.canMove = true;
        return zombie;
    }
}
