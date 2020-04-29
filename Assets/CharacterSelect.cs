using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    [SerializeField] List<Display> player1Display;
    [SerializeField] List<Display> player2Display;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] int player1Index;
    [SerializeField] int player2Index;
    int previousPlayer1Index;
    int previousPlayer2Index;
    CustomInput[] playerInput;

    bool[] playerButtonPressed;
    [SerializeField] bool[] isSelected;
    GameObject[] CharacterSlected;
    [SerializeField] GameObject StartButton;





    // Start is called before the first frame update
    void Start()
    {
        
        playerInput = new CustomInput[2];
        playerInput[0] = new CustomInput(1);
        playerInput[1] = new CustomInput(2);

        for (int i = 0; i < player1Display.Count; i++)
        {
            player1Display[i].InstanitatePrefab(spawnPoints[0].position);
            player1Display[i].playerModelPefab.SetActive(false);

        }

        for (int i = 0; i < player2Display.Count; i++)
        {
            player2Display[i].InstanitatePrefab(spawnPoints[1].position);
            player2Display[i].playerModelPefab.SetActive(false);

        }

        player1Index = 0;
        player2Index = 0;

        player1Display[player1Index].playerModelPefab.SetActive(true);
        player2Display[player2Index].playerModelPefab.SetActive(true);
        playerButtonPressed = new bool[2];
        isSelected = new bool[2];
        //StartButton.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        
        
        if (isSelected[0] == false)
        {
            if (playerInput[0].GetAxisRaw("Horizontal") > -.2f && playerInput[0].GetAxisRaw("Horizontal") < .2f) playerButtonPressed[0] = false;
            if (playerInput[0].GetAxisRaw("Horizontal") < -.2f && playerButtonPressed[0] == false) { player1Index++; playerButtonPressed[0] = true; } else if (playerInput[0].GetAxisRaw("Horizontal") > .2f && playerButtonPressed[0] == false) { player1Index--; playerButtonPressed[0] = true; } else { };
            if (player1Index < 0) { player1Index = player1Display.Count - 1; } else if (player1Index >= player1Display.Count) { player1Index = 0; } else { };
            if (previousPlayer1Index != player1Index)
            {

                player1Display[previousPlayer1Index].playerModelPefab.SetActive(false);
                player1Display[player1Index].playerModelPefab.SetActive(true);
                previousPlayer1Index = player1Index;
            }

            player1Display[previousPlayer1Index].playerModelPefab.transform.Rotate(new Vector3(0, 1, 0), 10f * Time.deltaTime);

            if (playerInput[0].GetKeyDown(2)) isSelected[0] = true;
        }

        if (isSelected[1] == false)
        {
            if (playerInput[1].GetAxisRaw("Horizontal") > -.2f && playerInput[1].GetAxisRaw("Horizontal") < .2f) playerButtonPressed[1] = false;
            if (playerInput[1].GetAxisRaw("Horizontal") < -.2f && playerButtonPressed[1] == false) { player2Index++; playerButtonPressed[1] = true; } else if (playerInput[1].GetAxisRaw("Horizontal") > .2f && playerButtonPressed[1] == false) { player2Index--; playerButtonPressed[1] = true; } else { };
            if (player2Index < 0) { player2Index = player2Display.Count - 1; } else if (player2Index >= player2Display.Count) { player2Index = 0; } else { };
            if (previousPlayer2Index != player2Index)
            {

                player2Display[previousPlayer2Index].playerModelPefab.SetActive(false);
                player2Display[player2Index].playerModelPefab.SetActive(true);
                previousPlayer2Index = player2Index;
            }

            player2Display[previousPlayer2Index].playerModelPefab.transform.Rotate(new Vector3(0, 1, 0), 10f * Time.deltaTime);

            if (playerInput[1].GetKeyDown(2)) isSelected[1] = true;
        }

        if (isSelected[0] && isSelected[1])
        {
            StartButton.SetActive(true);
            if (playerInput[0].GetKeyDown(7) || playerInput[1].GetKeyDown(7))
            {
                CleanUp();
                //GameManager.Instance.SpawnSelectedCharacters(player1Display[player1Index].playerCharacter, player2Display[player2Index].playerCharacter);
                StartButton.GetComponent<Button>().onClick.Invoke();
            }
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isSelected[0] && isSelected[1])
        {
            CleanUp();
            GameManager.Instance.SpawnSelectedCharacters(player1Display[player1Index].playerCharacter, player2Display[player2Index].playerCharacter);
            GameManager.Instance.LoadScene(sceneName);
        }
        
    }

        public void CleanUp()
    {
        for (int i = 0; i < player1Display.Count; i++)
        {
            player1Display[i].RemoveModel();
        }

        for (int i = 0; i < player2Display.Count; i++)
        {
            player2Display[i].RemoveModel();
        }
    }
}
