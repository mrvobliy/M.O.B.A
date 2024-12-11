using System;
using UnityEngine;

public class EventsBase : MonoBehaviour
{
    [Tooltip("In event “EntityComponentsData” of the “killer”, not of the deceased, called event")]
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
