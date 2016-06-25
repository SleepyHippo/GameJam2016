// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2016/6/25
//
// -----------------------------------------------------------------------------
//
using System;

public class TurnManager
{
    private static int _turn = 1;
    public static int Turn
    {
        get
        {
            return _turn;
        }
    }

    public static void Reset()
    {
        _turn = 1;
    }

    public static void StartTurn()
    {
        Messenger<int>.Broadcast(MessageConst.TURN_START, _turn, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public static void EndTurn()
    {
        Messenger<int>.Broadcast(MessageConst.TURN_END, _turn++, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}

