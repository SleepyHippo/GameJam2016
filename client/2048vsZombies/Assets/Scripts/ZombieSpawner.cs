using UnityEngine;
using System.Collections;
using SleepyHippo.Util;

public class ZombieSpawner : MonoBehaviour {

    public Zombie zombieTemplate;
    public Zombie zombieTemplate2;

    float _ratio = 0.8f;

    public Zombie Spawn()
    {
        float random = Random.Range(0f, 1f);
        if(random < 0.1f)//10%不出怪
            return null;
        else if(random < 0.3f)//20%大怪
        {
            GameObject zombieObject = GameObjectPool.Instance.Spawn(zombieTemplate2.gameObject, 5);
            zombieObject.transform.position = transform.position;
            Zombie zombie = zombieObject.GetComponent<Zombie>();
            zombie.canMove = true;
            zombie.hp = (int)(4 + TurnManager.Turn * 3 * _ratio);
            zombie.moveInterval = 1;
            zombie.moveDistance = 1;
            return zombie;
        }
        else//70%小怪
        {
            GameObject zombieObject = GameObjectPool.Instance.Spawn(zombieTemplate.gameObject, 5);
            zombieObject.transform.position = transform.position;
            Zombie zombie = zombieObject.GetComponent<Zombie>();
            zombie.canMove = true;
            zombie.hp = (int)(4 + TurnManager.Turn * 3 * _ratio) * 4;
            zombie.moveInterval = 3;
            zombie.moveDistance = 1;
            return zombie;
        }
    }
}
