using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private float _delay;
    [SerializeField] private ShowAnimEvents _showAnimEvents;
    [SerializeField] private HideAnimEvents _hideAnimEvents;
    [SerializeField] private GameObject _uiRoot;
    [SerializeField] private GameObject _environmentRoot;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(Load);
        _showAnimEvents.AnimationShowEvent += StartLoadScene;
        _hideAnimEvents.AnimationHideEvent += DestroyCurrentScene;
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(Load);
        _showAnimEvents.AnimationShowEvent -= StartLoadScene;
        _hideAnimEvents.AnimationHideEvent -= DestroyCurrentScene;
    }

    private void Load()
    {
        _animator.transform.parent.gameObject.SetActive(true);
    }
    
    private IEnumerator LoadSceneAndWait()
    {
        yield return new WaitForSeconds(_delay);
        
        var asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _animator.SetTrigger("Hide");
    }

    private void StartLoadScene()
    {
        _uiRoot.SetActive(false);
        _environmentRoot.SetActive(false);
        StartCoroutine(LoadSceneAndWait());
    }

    private void DestroyCurrentScene()
    {
        SceneManager.UnloadSceneAsync(0);
    }
}
