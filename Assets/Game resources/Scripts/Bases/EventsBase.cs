using System;
using UnityEngine;

public class EventsBase : MonoBehaviour
{
    public static event Action<EntityComponentsData> EntityDeath;

    public static void OnEntityDeath(EntityComponentsData componentsData)
    {
        try
        {
            EntityDeath?.Invoke(componentsData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
