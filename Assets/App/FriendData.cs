using System.Collections;
using System.Collections.Generic;
using UnityIoC;

public class FriendData : IDataBinding<FriendDataView>
{
    public string Name;
    public string ProfilePictureUrl;

    public FriendData(string name)
    {
        Name = name;
    }
}