using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text oldText, newText;

    private Color _color, _nonvisColor;
    private Vector3 _oldPos, _newPos;

    private List<string> _queueText;

    private void Start()
    {
        _oldPos = oldText.transform.localPosition;
        _newPos = newText.transform.localPosition;
        _color = oldText.color;
        _nonvisColor = new Color(_color.r, _color.g, _color.b, 0);
        newText.color = _nonvisColor;
        _queueText = new List<string>();
        StartCoroutine(Queue());
    }

    public void ChangeText(string text)
    {
        _queueText.Add(text);
    }

    IEnumerator Queue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (_queueText.Count > 0)
            {
                ChangeTextAfterQueue(_queueText[0]);
                _queueText.Remove(_queueText[0]);
            }
        }
    }
    

    void ChangeTextAfterQueue(string text)
    {
        //Debug.LogError(text);
        
        
        oldText.DOKill();
        newText.DOKill();
        oldText.transform.localPosition = _oldPos;
        newText.transform.localPosition = _newPos;
        newText.color = _nonvisColor;
        oldText.color = _color;
        
        
        
        newText.text = text;
        var oldP = oldText.transform.localPosition;
        
        oldText.transform.DOLocalMoveY(-112.5f, 0.5f).OnComplete(() =>
        {
            oldText.transform.localPosition = _oldPos;
            newText.transform.localPosition = _newPos;

            oldText.text = newText.text;
            
            newText.color = _nonvisColor;
            oldText.color = _color;
        });
        oldText.DOColor(_nonvisColor, 0.5f);
        
        var newP = newText.transform.localPosition;
        
        newText.DOColor(_color, 0.5f);
        newText.transform.DOLocalMove(_oldPos, 0.5f).OnComplete(() =>
        {
            oldText.transform.localPosition = _oldPos;
            newText.transform.localPosition = _newPos;

            oldText.text = newText.text;
            
            newText.color = _nonvisColor;
            oldText.color = _color;
        });
    }
}
