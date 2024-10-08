using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private TMP_Text _text;

    private int _index;
    
    public void Switch()
    {
        _gameObjects[_index].SetActive(false);

        _index++;

        if (_index >= _gameObjects.Count)
            _index = 0;
        
        _gameObjects[_index].SetActive(true);
        _text.text = _gameObjects[_index].name;
    }
}
