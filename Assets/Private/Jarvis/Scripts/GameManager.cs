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

        _currentScene = "";
        LoadScene("Menu");
    }
    protected override void Start()
    {




    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public void LoadScene(string sceneName)
    {
        Debug.Log(sceneName);
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

    public void SpawnSelectedCharacters(GameObject playerOnePrefab, GameObject playerTwoPrefab)
    {

        // Player 1
        Debug.Log(playerOnePrefab.GetComponent<CharacterController>().playerID);
        playerOnePrefab.GetComponent<CharacterController>().playerID = 1;
        //Debug.Log("prefab1" + playerOnePrefab.GetComponent<CharacterController>().playerID.ToString());
        GameObject PlayerOne = Instantiate(playerOnePrefab, new Vector3(10, 0, 0), Quaternion.identity);
        PlayerOne.name = "Player1";
        //Debug.Log(PlayerOne.name + PlayerOne.GetComponent<CharacterController>().playerID.ToString());
        PlayerOne.GetComponent<CharacterController>().playerID = 1;
        playerOne = PlayerOne.GetComponent<CharacterController>();

        



        // Player 2
        Debug.Log(playerOnePrefab.GetComponent<CharacterController>().playerID);
        playerTwoPrefab.GetComponent<CharacterController>().playerID = 2;        
        GameObject PlayerTwo = Instantiate(playerTwoPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
        PlayerTwo.name = "Player2";        
        PlayerTwo.GetComponent<CharacterController>().playerID = 2;
        playerTwo = PlayerTwo.GetComponent<CharacterController>();



        PlayerOne.GetComponent<CharacterController>().opponentController = PlayerTwo.GetComponent<CharacterController>();
        PlayerOne.GetComponent<CharacterController>().enviornments = enviornment;
        PlayerTwo.GetComponent<CharacterController>().opponentController = PlayerOne.GetComponent<CharacterController>();
        PlayerTwo.GetComponent<CharacterController>().enviornments = enviornment;
        PlayerOne.SetActive(false);
        PlayerTwo.SetActive(false);
    }
}

    
