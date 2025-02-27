using System;
using UnityEngine;

[Serializable]
public class HeroInfo
{
    [SerializeField] private Sprite _avatar;
    [SerializeField] private string _nickname;

    public Sprite Avatar => _avatar;
    public string Nickname => _nickname;
}
