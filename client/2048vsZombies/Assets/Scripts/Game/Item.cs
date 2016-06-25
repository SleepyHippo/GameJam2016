using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {

    public int x;
    public int y;
    public virtual bool canMove
    {
        get;
        set;
    }

    public abstract void MoveLeft(int distance, bool destroy = false);
    public abstract void MoveRight(int distance, bool destroy = false);
    public abstract void MoveUp(int distance, bool destroy = false);
    public abstract void MoveDown(int distance, bool destroy = false);
    public abstract void OnTick();
}
