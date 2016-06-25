// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//

public class StaticItem : Item
{
    #region implemented abstract members of Item
    public override void MoveLeft(int distance, bool destroy = false)
    {
        return;
    }
    public override void MoveRight(int distance, bool destroy = false)
    {
        return;
    }
    public override void MoveUp(int distance, bool destroy = false)
    {
        return;
    }
    public override void MoveDown(int distance, bool destroy = false)
    {
        return;
    }
    public override void OnTick()
    {

    }

    public override bool canMove
    {
        get
        {
            return false;
        }
        set
        {
        }
    }
    #endregion
}