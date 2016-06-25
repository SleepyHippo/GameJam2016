using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SleepyHippo.Util;

public class Board2048 : MonoBehaviour {

    public const int WIDTH = 4;
    public const int HEIGHT = 4;

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
        Messenger.AddListener(MessageConst.GAME_START, Init);

        Messenger.AddListener(MessageConst.INPUT_LEFT, DoLeft);
        Messenger.AddListener(MessageConst.INPUT_RIGHT, DoRight);
        Messenger.AddListener(MessageConst.INPUT_UP, DoUp);
        Messenger.AddListener(MessageConst.INPUT_DOWN, DoDown);
    }

    void Init()
    {
        Reset();
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
        PutItemAt(tower, tower.power, randomIndex);
    }

    Tower GenerateTower()
    {
        Tower tower = GameObjectPool.Instance.Spawn(TowerTemplate, 16, true).GetComponent<Tower>();
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
            Tower firstTower = null;
            Tower secondTower = null;
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
                    if(firstTower == null)
                    {
                        firstTower = tower;
                        firstTower.MoveLeft(lineGap);
                    }
                    else if(secondTower == null)//找到第二个塔
                    {
                        secondTower = tower;
                        if(firstTower.power == secondTower.power)
                        {
                            firstTower.Upgrade();
                            lineGap++;
                        }
                        secondTower.MoveLeft(lineGap);
                    }
                    else//之后的塔
                    {
                        tower.MoveLeft(lineGap);
                    }
                }
            }
        }
    }

    void DoRight()
    {
        Debug.Log("Right");
        for(int y = 0; y < HEIGHT; ++y)
        {
            int lineGap = 0;
            Tower firstTower = null;
            Tower secondTower = null;
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
                    if(firstTower == null)
                    {
                        firstTower = tower;
                        firstTower.MoveRight(lineGap);
                    }
                    else if(secondTower == null)//找到第二个塔
                    {
                        secondTower = tower;
                        if(firstTower.power == secondTower.power)
                        {
                            firstTower.Upgrade();
                            lineGap++;
                        }
                        secondTower.MoveRight(lineGap);
                    }
                    else//之后的塔
                    {
                        tower.MoveRight(lineGap);
                    }
                }
            }
        }
    }

    void DoUp()
    {
        Debug.Log("Up");
        for(int x = 0; x < WIDTH; ++x)
        {
            int columnGap = 0;
            Tower firstTower = null;
            Tower secondTower = null;
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
                    if(firstTower == null)
                    {
                        firstTower = tower;
                        firstTower.MoveUp(columnGap);
                    }
                    else if(secondTower == null)//找到第二个塔
                    {
                        secondTower = tower;
                        if(firstTower.power == secondTower.power)
                        {
                            firstTower.Upgrade();
                            columnGap++;
                        }
                        secondTower.MoveUp(columnGap);
                    }
                    else//之后的塔
                    {
                        tower.MoveUp(columnGap);
                    }
                }
            }
        }
    }

    void DoDown()
    {
        Debug.Log("Down");
        for(int x = 0; x < WIDTH; ++x)
        {
            int columnGap = 0;
            Tower firstTower = null;
            Tower secondTower = null;
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
                    if(firstTower == null)
                    {
                        firstTower = tower;
                        firstTower.MoveUp(columnGap);
                    }
                    else if(secondTower == null)//找到第二个塔
                    {
                        secondTower = tower;
                        if(firstTower.power == secondTower.power)
                        {
                            firstTower.Upgrade();
                            columnGap++;
                        }
                        secondTower.MoveUp(columnGap);
                    }
                    else//之后的塔
                    {
                        tower.MoveUp(columnGap);
                    }
                }
            }
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
