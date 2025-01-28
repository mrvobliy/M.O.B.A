using System;
using System.Collections.Generic;
using UnityEngine;

public class EventsBase : MonoBehaviour
{
    public static event Action<EntityComponentsData, List<Attackers>> EntityDeath;

    public static void OnEntityDeath(EntityComponentsData componentsData, List<Attackers> attackers)
    {
        try
        {
            EntityDeath?.Invoke(componentsData, attackers);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}