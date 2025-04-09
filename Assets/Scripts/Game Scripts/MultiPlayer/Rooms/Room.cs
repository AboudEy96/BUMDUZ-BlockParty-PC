using UnityEngine;

public class Room : IRoom
{
    private int _roomCode;
    private string _roomName;
    private string _roomOwner;

    public Room(string roomName, int roomCode, string roomOwner)
    {
        this._roomCode = roomCode;
        this._roomOwner = roomOwner;
        this._roomName = roomName;
    }

    public string GetRoomName()
    {
        return _roomName;
    }

    public string GetRoomOwner()
    {
        return _roomOwner;
    }

    public int GetRoomCode()
    {
        return _roomCode;
    }

    public void ChangeName(string name)
    {
        _roomName = name;
    }

    public void ChangeCode(int code)
    {
        _roomCode = code;
    }
}