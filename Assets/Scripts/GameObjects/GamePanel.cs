using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour {

	public GameGrid gameGrid;
     public GameObject controlPanel;
         

     public void StartGameGrid(){
          controlPanel.gameObject.SetActive(true);
          gameGrid.StartGame();
     }
}
