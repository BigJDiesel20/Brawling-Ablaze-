using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    [SerializeField] HealthBar healthbar1;
    [SerializeField] HealthBar healthbar2;
    [SerializeField] Transform player1Pos;
    [SerializeField] Transform player2Pos;
    [SerializeField] bool positionSet;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (positionSet == false && GameManager.Instance.playerOne != null && GameManager.Instance.playerTwo != null)
        {
            GameManager.Instance.playerOne.gameObject.transform.position = player1Pos.position;
            GameManager.Instance.playerTwo.gameObject.transform.position = player2Pos.position;
            healthbar1.player = GameManager.Instance.playerOne;
            healthbar2.player = GameManager.Instance.playerTwo;

            GameManager.Instance.playerOne.gameObject.SetActive(true);
            GameManager.Instance.playerTwo.gameObject.SetActive(true);
            positionSet = true;
        }
    }
}
