using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    private string _currentScene;
    private string _previousScene;
    public string CurrentScene { get { return _currentScene; } }

    public enum GameState { Running, Paused }
    public GameState State;

    public GameObject[] playerPrefabs;
    public Collider[] enviornment;

    public CharacterController playerOne;
    public CharacterController playerTwo;

    private List<AsyncOperation> loadOperations;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        //_currentScene = "";
        //LoadScene("Menu");
    }
    protected override void Start()
    {




    }

    // Update is called once per frame
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaceCharacters();
        }
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

    public void PlaceCharacters()
    {
        GameObject PlayerOne;
        GameObject PlayerTwo;

        PlayerOne = Instantiate(playerPrefabs[0], Vector3.zero,Quaternion.identity);
        PlayerTwo = Instantiate(playerPrefabs[0], Vector3.zero, Quaternion.identity);

        PlayerOne.GetComponent<CharacterController>().playerID = 1;
        PlayerTwo.GetComponent<CharacterController>().playerID = 2;
        PlayerOne.GetComponent<CharacterController>().opponentController = PlayerOne.GetComponent<CharacterController>();
        PlayerTwo.GetComponent<CharacterController>().opponentController = PlayerOne.GetComponent<CharacterController>();
        PlayerOne.GetComponent<CharacterController>().enviornments = enviornment;
        PlayerTwo.GetComponent<CharacterController>().enviornments = enviornment;

    }
}

    
