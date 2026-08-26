using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private static List<IActivateable> all = new List<IActivateable>();

    public static void Register(IActivateable obj)
    {
        if (!all.Contains(obj))
            all.Add(obj);
    }

    public static void Unregister(IActivateable obj)
    {
        all.Remove(obj);
    }

    public static void SetActiveAll(bool state)
    {
        foreach (var obj in all)
        {
            obj.SetActive(state);
        }
    }
}