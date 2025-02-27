using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TeamsScoreTextAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _oldText;
    [SerializeField] private TMP_Text _newText;
    
    private Color _color, _nonVisColor;
    private Vector3 _oldPos, _newPos;
    private List<string> _queueText;

    private void Start()
    {
        _oldPos = _oldText.transform.localPosition;
        _newPos = _newText.transform.localPosition;
        _color = _oldText.color;
        _nonVisColor = new Color(_color.r, _color.g, _color.b, 0);
        _newText.color = _nonVisColor;
        _queueText = new List<string>();
        
        StartCoroutine(OnQueue());
    }

    public void ChangeText(string text) => _queueText.Add(text);

    private IEnumerator OnQueue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (_queueText.Count <= 0) continue;
            
            ChangeTextAfterQueue(_queueText[0]);
            _queueText.Remove(_queueText[0]);
        }
    }
    
    private void ChangeTextAfterQueue(string text)
    {
        _oldText.DOKill();
        _newText.DOKill();
        _oldText.transform.localPosition = _oldPos;
        _newText.transform.localPosition = _newPos;
        _newText.color = _nonVisColor;
        _oldText.color = _color;
        _newText.text = text;
        
        _oldText.transform.DOLocalMoveY(-112.5f, 0.5f).OnComplete(() =>
        {
            _oldText.transform.localPosition = _oldPos;
            _newText.transform.localPosition = _newPos;

            _oldText.text = _newText.text;
            
            _newText.color = _nonVisColor;
            _oldText.color = _color;
        });
        
        _oldText.DOColor(_nonVisColor, 0.5f);
        _newText.DOColor(_color, 0.5f);
        _newText.transform.DOLocalMove(_oldPos, 0.5f).OnComplete(() =>
        {
            _oldText.transform.localPosition = _oldPos;
            _newText.transform.localPosition = _newPos;

            _oldText.text = _newText.text;
            
            _newText.color = _nonVisColor;
            _oldText.color = _color;
        });
    }
}