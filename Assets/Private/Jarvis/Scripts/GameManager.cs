using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    private string _currentScene;
    private string _previousScene;
    public string CurrentScene { get { return _currentScene; } }

    public  enum GameState { Running, Paused}
    public GameState State;

    public CharacterController playerOne;
    public CharacterController playerTwo;

    private List<AsyncOperation> loadOperations;
    // Start is called before the first frame update
    void Start()
    {

        _currentScene = "";
        LoadScene("Menu");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        if (_currentScene != sceneName)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (ao == null)
            {
                return;
            }

            _previousScene = _currentScene;
            _currentScene = sceneName;
            ao.completed += OnLoadSceneComplete;
        }
        
    }

    public void UnloadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if (ao == null)
        {
            return;
        }
    }

    public void OnLoadSceneComplete(AsyncOperation ao)
    {

        if (_previousScene != "")
        {            
            UnloadScene(_previousScene);
        }
        
        
    }

    


}
