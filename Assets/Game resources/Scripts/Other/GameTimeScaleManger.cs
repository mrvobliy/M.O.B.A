using UnityEngine;

public class GameTimeScaleManger : MonoBehaviour
{
    public void SetScale(int value) => Time.timeScale = value;
}