using UnityEngine;

public class ServerOnlinePlayers : MonoBehaviour
{
    private static int onlinePlayers = 0; 

    public static int getOnlinePlayers()  
    {
        return onlinePlayers;
    }

    public static void addOnlinePlayer() 
    {
        onlinePlayers++;
    }

    public static void removeOnlinePlayer()  
    {
        onlinePlayers--;
    }
}
