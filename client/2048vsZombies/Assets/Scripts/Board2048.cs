using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SleepyHippo.Util;

public class Board2048 : MonoBehaviour {

    public const int WIDTH = 4;
    public const int HEIGHT = 10;

//    public GameObject zombieTemplate;
    public GameObject towerTemplate;
    public ZombieSpawner zombieSpawner;

    public int zombieSpawnInterval = 5;
    private int nowZombieSpawnInterval;

    /// <summary>
    /// -99：被污染的土地
    /// -2：僵尸技能
    /// -1：僵尸
    /// 0：空
    /// 2,4,8,16……：power
    /// 原点在左下角：
    /// 12 13 14 15
    ///  8  9 10 11
    ///  4  5  6  7
    ///  0  1  2  3
    /// </summary>
    private int[] typeMap = new int[WIDTH * HEIGHT];

    /// <summary>
    /// 显示组件
    /// </summary>
    public Dictionary<int, Item> itemMap = new Dictionary<int, Item>();

    public Land[] lands;

    void Awake()
    {
        Init();
        Messenger.AddListener(MessageConst.GAME_START, Init);

        Messenger.AddListener(MessageConst.INPUT_LEFT, DoLeft);
        Messenger.AddListener(MessageConst.INPUT_RIGHT, DoRight);
        Messenger.AddListener(MessageConst.INPUT_UP, DoUp);
        Messenger.AddListener(MessageConst.INPUT_DOWN, DoDown);
    }

    void Init()
    {
        Reset();
        SpawnZombie();
        StartTurn();
    }

    void Reset()
    {
        nowZombieSpawnInterval = zombieSpawnInterval;
        for(int i = 0; i < typeMap.Length; ++i)
        {
            if(i < 16)
            {
                typeMap[i] = 0;
                lands[i].SetType(true);
            }
            else
            {
                typeMap[i] = -99;
                lands[i].SetType(false);
            }
        }
        var iter = itemMap.GetEnumerator();
        while(iter.MoveNext())
        {
            GameObjectPool.Instance.Recycle(iter.Current.Value.gameObject);
        }
        itemMap.Clear();
    }

    void SpawnZombie()
    {
        int randomIndex = RandomPickZombieRoad();
        if(randomIndex == -1)
        {
            //TODO：处理满了的情况
            return;
        }
        Zombie zombie = zombieSpawner.Spawn();
        int x = CommonUtil.GetX(randomIndex, WIDTH);
        int y = CommonUtil.GetY(randomIndex, WIDTH);
        typeMap[randomIndex] = -1;
        itemMap[randomIndex] = zombie;
        zombie.MoveTo(new Vector3(x, 0, y));
    }

    int RandomPickZombieRoad()
    {
        List<int> availableIndexList = new List<int>();
        for(int i = typeMap.Length - 4; i < typeMap.Length; ++i)
        {
            if(typeMap[i] != -1)//这个格子不是僵尸，就可以用
            {
                availableIndexList.Add(i);
            }
        }
        if(availableIndexList.Count == 0)
        {
            return -1;
        }
        int targetIndex = Random.Range(0, availableIndexList.Count);
        return availableIndexList[targetIndex];
    }

    void MoveZombie()
    {
        for(int x = 0; x < WIDTH; ++x)
        {
            for(int y = 0; y < HEIGHT; ++y)
            {
                int index = CommonUtil.GetIndex(x, y, WIDTH);
                int type = typeMap[index];
                if(type == -1)
                {
                    Zombie zombie = itemMap[index] as Zombie;
                    if(zombie == null)
                        Debug.LogError("Zombie is null");
                    if(zombie.canMove && zombie.readyToMove)
                    {
                        bool hasCollider = false;
                        for(int i = 1; i <= zombie.moveDistance; ++i)
                        {
                            int targetX = x;
                            int targetY = y - i;
                            if(targetY >= 0)
                            {
                                int targetIndex = CommonUtil.GetIndex(targetX, targetY, WIDTH);
                                if(typeMap[targetIndex] == -1 || typeMap[targetIndex] == -2)//前面有僵尸或僵尸技能，不能走
                                {
                                    hasCollider = true;
                                }
                            }
                        }
                        if(!hasCollider)
                        {
                            zombie.MoveDown(1);
                            if(zombie.y <= 0)
                            {
                                zombie.y = 0;
                                Messenger.Broadcast(MessageConst.GAME_OVER_START, MessengerMode.DONT_REQUIRE_LISTENER);
                                return;
                            }
                            int zombieNewIndex = CommonUtil.GetIndex(zombie, WIDTH);
                            lands[zombieNewIndex].SetType(false);
                            if(itemMap.ContainsKey(zombieNewIndex))
                            {
                                Item item = itemMap[zombieNewIndex];
                                if(item is Tower)
                                {
                                    itemMap.Remove(zombieNewIndex);
                                    GameObjectPool.Instance.Recycle(item.gameObject);
                                }
                            }
                            if(typeMap[zombieNewIndex] != -1)
                            {
                                typeMap[zombieNewIndex] = -99;
                            }
                        }
                    }
                }
            }
        }
        UpdateMapping();
    }

    void DrawBadLand()
    {
        for(int i = 0; i < typeMap.Length; ++i)
        {
            int type = typeMap[i];
            if(type == -1)
            {
                int x = CommonUtil.GetX(i, WIDTH);
                int y = CommonUtil.GetY(i, WIDTH);
                for(int j = y * WIDTH; j < (y+1) * WIDTH; ++j)//把僵尸这一行的土地都设置成badland
                {
                    int index = j;
                    lands[index].SetType(false);
                    if(itemMap.ContainsKey(index))
                    {
                        Item item = itemMap[index];
                        if(item is Tower)
                        {
                            itemMap.Remove(index);
                            GameObjectPool.Instance.Recycle(item.gameObject);
                        }
                    }
                    if(typeMap[index] != -1)
                    {
                        typeMap[index] = -99;
                    }
                }
            }
        }
    }

    void RandomPutTower()
    {
        int randomIndex = GetRandomAvailableIndex();
        if(randomIndex == -1)
        {
            Messenger.Broadcast(MessageConst.GAME_OVER_START, MessengerMode.DONT_REQUIRE_LISTENER);
            return;
        }
        Tower tower = GenerateTower();
        PrintBoard();
        Debug.Log("put item");
        PutItemAt(tower, tower.power, randomIndex);
        PrintBoard();
    }

    Tower GenerateTower()
    {
        Tower tower = GameObjectPool.Instance.Spawn(towerTemplate, 16, true).GetComponent<Tower>();
        tower.canMove = true;
        int choice = Random.Range(1, 3);
        switch(choice)
        {
            case 1:
                tower.power = 2;
                break;
            case 2:
                tower.power = 4;
                break;
            case 3:
                tower.power = 8;
                break;
        }
        return tower;
    }

    int GetRandomAvailableIndex()
    {
        List<int> availableIndexList = new List<int>();
        for(int i = 0; i < typeMap.Length; ++i)
        {
            if(typeMap[i] == 0)
            {
                availableIndexList.Add(i);
            }
        }
        if(availableIndexList.Count == 0)
        {
            return -1;
        }
        int targetIndex = Random.Range(0, availableIndexList.Count);
        return availableIndexList[targetIndex];
    }

    void PutItemAt(Item item, int type, int index)
    {
        if(itemMap.ContainsKey(index))
        {
            Debug.LogError("Index: " + index + " has item: " + itemMap[index]);
            return;
        }
        if(typeMap[index] != 0)
        {
            Debug.LogError("Index: " + index + " 'type is not 0: " + typeMap[index]);
            return;
        }
        itemMap[index] = item;
        typeMap[index] = type;
        item.x = CommonUtil.GetX(index, WIDTH);
        item.y = CommonUtil.GetY(index, WIDTH);
        item.gameObject.transform.position = new Vector3(item.x, 0, item.y);
    }

    void DoLeft()
    {
        Debug.Log("Left");
        for(int y = 0; y < HEIGHT; ++y)
        {
            int lineGap = 0;
            bool hasMerge = false;
            Tower lastTower = null;
            for(int x = 0; x < WIDTH; ++x)
            {
                int index = CommonUtil.GetIndex(x, y, WIDTH);
                int type = typeMap[index];
                if(type == 0)
                {
                    lineGap++;
                    continue;
                }
                else if(type < 0)
                {
                    lineGap = 0;
                    hasMerge = false;
                    lastTower = null;
                    continue;
                }
                else if(type > 0)
                {
                    Tower tower = itemMap[index] as Tower;
                    if(tower == null)
                        Debug.LogError("Tower is null");
                    if(lastTower == null)//第一个塔
                    {
                        tower.MoveLeft(lineGap);
                    }
                    else//已经不是第一个塔
                    {
                        if(hasMerge)//前面已经合并过了
                        {
                            tower.MoveLeft(lineGap+1);
                        }
                        else
                        {
                            if(lastTower.power == tower.power)//可以和上一个塔合并
                            {
                                lastTower.Upgrade();
                                hasMerge = true;
                                itemMap.Remove(index);
                                tower.MoveLeft(lineGap + 1, true);
                            }
                            else
                            {
                                tower.MoveLeft(lineGap);
                            }
                        }
                    }
                    lastTower = tower;
                }
            }
        }
        UpdateMapping();
        EndTurn();
        StartTurn();
    }

    void DoRight()
    {
        Debug.Log("Right");
        for(int y = 0; y < HEIGHT; ++y)
        {
            int lineGap = 0;
            bool hasMerge = false;
            Tower lastTower = null;
            for(int x = WIDTH - 1; x >= 0; --x)
            {
                int index = CommonUtil.GetIndex(x, y, WIDTH);
                int type = typeMap[index];
                if(type == 0)
                {
                    lineGap++;
                    continue;
                }
                else if(type < 0)
                {
                    lineGap = 0;
                    hasMerge = false;
                    lastTower = null;
                    continue;
                }
                else if(type > 0)
                {
                    Tower tower = itemMap[index] as Tower;
                    if(tower == null)
                        Debug.LogError("Tower is null");
                    if(lastTower == null)//第一个塔
                    {
                        tower.MoveRight(lineGap);
                    }
                    else//已经不是第一个塔
                    {
                        if(hasMerge)//前面已经合并过了
                        {
                            tower.MoveRight(lineGap+1);
                        }
                        else
                        {
                            if(lastTower.power == tower.power)//可以和上一个塔合并
                            {
                                lastTower.Upgrade();
                                hasMerge = true;
                                itemMap.Remove(index);
                                tower.MoveRight(lineGap + 1, true);
                            }
                            else
                            {
                                tower.MoveRight(lineGap);
                            }
                        }
                    }
                    lastTower = tower;
                }
            }
        }
        UpdateMapping();
        EndTurn();
        StartTurn();
    }

    void DoUp()
    {
        Debug.Log("Up");
        for(int x = 0; x < WIDTH; ++x)
        {
            int columnGap = 0;
            bool hasMerge = false;
            Tower lastTower = null;
            for(int y = HEIGHT - 1; y >= 0; --y)
            {
                int index = CommonUtil.GetIndex(x, y, WIDTH);
                int type = typeMap[index];
                if(type == 0)
                {
                    columnGap++;
                    continue;
                }
                else if(type < 0)
                {
                    columnGap = 0;
                    hasMerge = false;
                    lastTower = null;
                    continue;
                }
                else if(type > 0)
                {
                    Tower tower = itemMap[index] as Tower;
                    if(tower == null)
                        Debug.LogError("Tower is null");
                    if(lastTower == null)//第一个塔
                    {
                        tower.MoveUp(columnGap);
                    }
                    else//已经不是第一个塔
                    {
                        if(hasMerge)//前面已经合并过了
                        {
                            tower.MoveUp(columnGap+1);
                        }
                        else
                        {
                            if(lastTower.power == tower.power)//可以和上一个塔合并
                            {
                                lastTower.Upgrade();
                                hasMerge = true;
                                itemMap.Remove(index);
                                tower.MoveUp(columnGap + 1, true);
                            }
                            else
                            {
                                tower.MoveUp(columnGap);
                            }
                        }
                    }
                    lastTower = tower;
                }
            }
        }
        UpdateMapping();
        EndTurn();
        StartTurn();
    }

    void DoDown()
    {
        Debug.Log("Down");
        for(int x = 0; x < WIDTH; ++x)
        {
            int columnGap = 0;
            bool hasMerge = false;
            Tower lastTower = null;
            for(int y = 0; y < HEIGHT; ++y)
            {
                int index = CommonUtil.GetIndex(x, y, WIDTH);
                int type = typeMap[index];
                if(type == 0)
                {
                    columnGap++;
                    continue;
                }
                else if(type < 0)
                {
                    columnGap = 0;
                    hasMerge = false;
                    lastTower = null;
                    continue;
                }
                else if(type > 0)
                {
                    Tower tower = itemMap[index] as Tower;
                    if(tower == null)
                        Debug.LogError("Tower is null");
                    if(lastTower == null)//第一个塔
                    {
                        tower.MoveDown(columnGap);
                    }
                    else//已经不是第一个塔
                    {
                        if(hasMerge)//前面已经合并过了
                        {
                            tower.MoveDown(columnGap+1);
                        }
                        else
                        {
                            if(lastTower.power == tower.power)//可以和上一个塔合并
                            {
                                lastTower.Upgrade();
                                hasMerge = true;
                                itemMap.Remove(index);
                                tower.MoveDown(columnGap + 1, true);
                            }
                            else
                            {
                                tower.MoveDown(columnGap);
                            }
                        }
                    }
                    lastTower = tower;
                }
            }
        }
        UpdateMapping();
        EndTurn();
        StartTurn();
    }

    void UpdateMapping()
    {
        PrintBoard();
        for(int i = 0; i < typeMap.Length; ++i)
        {
            if(typeMap[i] > 0)//把炮台和僵尸都先置空
            {
                typeMap[i] = 0;
            }
            if(typeMap[i] == -1)
            {
                typeMap[i] = -99;
            }
        }
        List<Item> tempItemList = new List<Item>();
        var iter = itemMap.GetEnumerator();
        while(iter.MoveNext())
        {
            Item item = iter.Current.Value;
            tempItemList.Add(item);
            if(item is Tower)
            {
                Tower tower = item as Tower;
                typeMap[CommonUtil.GetIndex(item, WIDTH)] = tower.power;
            }
            if(item is Zombie)
            {
                Zombie zombie = item as Zombie;
                typeMap[CommonUtil.GetIndex(item, WIDTH)] = -1;
            }
        }

        itemMap.Clear();
        for(int i = 0; i < tempItemList.Count; ++i)
        {
            Item item = tempItemList[i];
            itemMap.Add(CommonUtil.GetIndex(item, WIDTH), item);
        }
        PrintBoard();
    }

    void StartTurn()
    {
        TurnManager.StartTurn();
        if(nowZombieSpawnInterval <= 0)
        {
            SpawnZombie();
            nowZombieSpawnInterval = zombieSpawnInterval;
        }
        MoveZombie();
        DrawBadLand();
        RandomPutTower();
    }

    void EndTurn()
    {
        var iter = itemMap.GetEnumerator();
        while(iter.MoveNext())
        {
            iter.Current.Value.OnTick();
        }
        nowZombieSpawnInterval--;
        TurnManager.EndTurn();
    }

    void PrintBoard()
    {
//        ClearLog();
//        Debug.Log("------------------");
//        for(int y = HEIGHT - 1; y >= 0; --y)
//        {
//            string line = "";
//            for(int x = 0; x < WIDTH; ++x)
//            {
//                line += typeMap[CommonUtil.GetIndex(x, y, WIDTH)] + "  ";
//            }
//            Debug.Log(line);
//        }
    }

//    public static void ClearLog()
//    {
//        var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
//        var type = assembly.GetType("UnityEditorInternal.LogEntries");
//        var method = type.GetMethod("Clear");
//        method.Invoke(new object(), null);
//    }
}
