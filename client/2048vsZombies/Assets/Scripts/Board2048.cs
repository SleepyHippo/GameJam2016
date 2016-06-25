using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SleepyHippo.Util;

public class Board2048 : MonoBehaviour {

    public const int WIDTH = 4;
    public const int HEIGHT = 4;

    public int turn = 1;

    public GameObject TowerTemplate;

    /// <summary>
    /// -1：不能走
    /// 0：空
    /// 2,4,8,16……：power
    /// 原点在左下角：
    /// 12 13 14 15
    ///  8  9 10 11
    ///  4  5  6  7
    ///  0  1  2  3
    /// </summary>
    public int[] typeMap = new int[16];

    /// <summary>
    /// 显示组件
    /// </summary>
    public Dictionary<int, Item> itemMap = new Dictionary<int, Item>();

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
        Messenger<int>.Broadcast(MessageConst.TURN_START, turn, MessengerMode.DONT_REQUIRE_LISTENER);
        RandomPutTower();
    }

    public void Reset()
    {
        for(int i = 0; i < typeMap.Length; ++i)
        {
            typeMap[i] = 0;
        }
        var iter = itemMap.GetEnumerator();
        while(iter.MoveNext())
        {
            GameObjectPool.Instance.Recycle(iter.Current.Value.gameObject);
        }
        itemMap.Clear();
    }

    public void RandomPutTower()
    {
        Tower tower = GenerateTower();
        int randomIndex = GetRandomAvailableIndex();
        if(randomIndex == -1)
        {
            Messenger.Broadcast(MessageConst.GAME_OVER_START);
            return;
        }
        PrintBoard();
        Debug.Log("put item");
        PutItemAt(tower, tower.power, randomIndex);
        PrintBoard();
    }

    Tower GenerateTower()
    {
        Tower tower = GameObjectPool.Instance.Spawn(TowerTemplate, 16, true).GetComponent<Tower>();
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
        int targetIndex = Random.Range(0, availableIndexList.Count-1);
        return availableIndexList[targetIndex];
    }

    void PutItemAt(Item item, int type, int index)
    {
        if(itemMap.ContainsKey(index))
        {
            Debug.LogError("Index: " + index + " has item: " + itemMap[index]);
            return;
        }
        itemMap[index] = item;
        typeMap[index] = type;
        item.x = CommonUtil.GetX(index, WIDTH);
        item.y = CommonUtil.GetY(index, HEIGHT);
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
        RandomPutTower();
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
        RandomPutTower();
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
        RandomPutTower();
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
        RandomPutTower();
    }

    void UpdateMapping()
    {
        PrintBoard();
        for(int i = 0; i < typeMap.Length; ++i)
        {
            if(typeMap[i] > 0)
            {
                typeMap[i] = 0;
            }
        }
        List<Item> tempItemList = new List<Item>();
        var iter = itemMap.GetEnumerator();
        while(iter.MoveNext())
        {
            Item item = iter.Current.Value;
            tempItemList.Add(item);
            if(item.canMove)
            {
                Tower tower = item as Tower;
                typeMap[CommonUtil.GetIndex(item, WIDTH)] = tower.power;
            }
        }

        itemMap.Clear();
        for(int i = 0; i < tempItemList.Count; ++i)
        {
            Item item = tempItemList[i];
            itemMap.Add(CommonUtil.GetIndex(item, WIDTH), item);
        }
        PrintBoard();
        Messenger<int>.Broadcast(MessageConst.TURN_END, turn++, MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<int>.Broadcast(MessageConst.TURN_START, turn, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    void PrintBoard()
    {
        Debug.Log("------------------");
        for(int y = HEIGHT - 1; y >= 0; --y)
        {
            string line = "";
            for(int x = 0; x < WIDTH; ++x)
            {
                line += typeMap[CommonUtil.GetIndex(x, y, WIDTH)] + "  ";
            }
            Debug.Log(line);
        }
    }
//    Tower ScanRight(int x, int y, out int gapCount)
//    {
//        Tower canMergeTower = null;
//        Tower firstTower = null;
//        gapCount = 0;
//        for(int i = x + 1; i < WIDTH; ++i)
//        {
//            int nextIndex = CommonUtil.GetIndex(i, y, WIDTH);
//            if(typeMap[nextIndex] == 0)
//            {
//                gapCount++;
//            }
//            if(typeMap[nextIndex] > 0)
//            {
//                canMergeTower = itemMap[nextIndex] as Tower;
//                if(firstTower == null)
//                {
//                    firstTower = canMergeTower;
//                }
//            }
//        }
//        if(firstTower == canMergeTower)
//            return canMergeTower;
//        else
//            return null;
//    }
//
//    Tower ScanLeft(int x, int y, out int gapCount)
//    {
//        Tower canMergeTower = null;
//        Tower firstTower = null;
//        gapCount = 0;
//        for(int i = x - 1; i >= 0; --i)
//        {
//            int nextIndex = CommonUtil.GetIndex(i, y, WIDTH);
//            if(typeMap[nextIndex] == 0)
//            {
//                gapCount++;
//            }
//            if(typeMap[nextIndex] > 0)
//            {
//                canMergeTower = itemMap[nextIndex] as Tower;
//                if(firstTower == null)
//                {
//                    firstTower = canMergeTower;
//                }
//            }
//        }
//        if(firstTower == canMergeTower)
//            return canMergeTower;
//        else
//            return null;
//    }
//
//    Tower ScanUp(int x, int y, out int gapCount)
//    {
//        Tower canMergeTower = null;
//        Tower firstTower = null;
//        gapCount = 0;
//        for(int i = y + 1; i < HEIGHT; --i)
//        {
//            int nextIndex = CommonUtil.GetIndex(x, i, HEIGHT);
//            if(typeMap[nextIndex] == 0)
//            {
//                gapCount++;
//            }
//            if(typeMap[nextIndex] > 0)
//            {
//                canMergeTower = itemMap[nextIndex] as Tower;
//                if(firstTower == null)
//                {
//                    firstTower = canMergeTower;
//                }
//            }
//        }
//        if(firstTower == canMergeTower)
//            return canMergeTower;
//        else
//            return null;
//    }
//    Tower ScanUp(int x, int y, out int gapCount)
//    {
//        Tower canMergeTower = null;
//        Tower firstTower = null;
//        gapCount = 0;
//        for(int i = y - 1; i >= 0; --i)
//        {
//            int nextIndex = CommonUtil.GetIndex(x, i, HEIGHT);
//            if(typeMap[nextIndex] == 0)
//            {
//                gapCount++;
//            }
//            if(typeMap[nextIndex] > 0)
//            {
//                canMergeTower = itemMap[nextIndex] as Tower;
//                if(firstTower == null)
//                {
//                    firstTower = canMergeTower;
//                }
//            }
//        }
//        if(firstTower == canMergeTower)
//            return canMergeTower;
//        else
//            return null;
//    }
}
