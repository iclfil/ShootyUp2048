using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

     public Image circleOutline;
	// public Text text, relativeText, scrambledText;
     public GameObject playButton;
     public GameObject restartButton;
     public GameObject UIPanel;
     public GameObject UIPanelRestart;
     ////////////////////////////////////////////// нужно подкорректировать//////////////////////////
     public GamePanel gamePanel;
     public GameObject GamePanel;
     public GameObject sumPoint;
     public GameObject recordSumPoint;
     public int intSumPoint;
     public int intRecordSumPoint; 
     

     private void Start() {
          UIPanelRestart.SetActive(false);
         
          Init();
     }
     public void UpdateSumPoint(){
          sumPoint.GetComponent<TextMeshPro>().text = intSumPoint.ToString();
     }

     private void Init(){
          playButton.transform.DOScale(1.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetId("TapToPlay");
     }

     public void UpdateRecordPoint(){
          if(intRecordSumPoint < intSumPoint){
            recordSumPoint.GetComponent<TextMeshPro>().text = intSumPoint.ToString();
          }
     }

     public void EndGame(){
         UpdateRecordPoint();
         SaveRecordPoints();
         DOTween.Kill("GameGrid");
         DOTween.Kill(gamePanel.gameGrid.transform);
         gamePanel.gameGrid.ClearGameGrid();
         GamePanel.SetActive(false);
         UIPanelRestart.SetActive(true);
         circleOutline.DOColor(RandomColor(), 1.5f).SetEase(Ease.Linear).SetId("RestartCircleColor");
		circleOutline.DOFillAmount(0, 1.5f).SetId("RestartCircleMove").SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo)
			.OnStepComplete(()=> {
				circleOutline.fillClockwise = !circleOutline.fillClockwise;
				circleOutline.DOColor(RandomColor(), 1.5f).SetEase(Ease.Linear).SetId("RestartCircleColor");;
			});
          restartButton.transform.DOScale(4f, 1f).SetLoops(-1, LoopType.Yoyo).SetId("RestartButton");
           DOTween.Kill("GameGrid"); //останавливаем движение GameGrid;
          
     }

     public void StartGame(){
          LoadRecordPoints();
          //обнуляем очки
           intSumPoint = 0;
          UpdateSumPoint();
          //
         

          DOTween.Kill("RestartCircleColor");
          DOTween.Kill("RestartCircleMove");
          DOTween.Kill("RestartButton");
         
          

          if(UIPanelRestart){
               
               DOTween.Kill("RestartCircleColor");
               DOTween.Kill("RestartCircleMove");
               circleOutline.GetComponent<Image>().fillAmount = 1f;
               DOTween.Kill("RestartButton");
               UIPanelRestart.SetActive(false);
          }

         

          Debug.Log("Включаем грид");

          // откючаем панель с UI.
          // UIPanel.SetActive(false);
          playButton.SetActive(false);
          // Включаем панель с игрой.
          gamePanel.gameObject.SetActive(true);
          gamePanel.StartGameGrid();
     }

     Color RandomColor()
	{
		return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
	}


     void SaveRecordPoints()
    {

        intRecordSumPoint = Convert.ToInt32(recordSumPoint.GetComponent<TextMeshPro>().text);

        Debug.Log("Должен сохранить " + intRecordSumPoint);
        PlayerPrefs.SetInt("RecordScore", intRecordSumPoint);
        Debug.Log("Должен сохранить " + intRecordSumPoint);
        PlayerPrefs.Save();
    }

     void LoadRecordPoints()
    {
         intRecordSumPoint = PlayerPrefs.GetInt("RecordScore");
         Debug.Log("Загрузка " + intRecordSumPoint);
         recordSumPoint.GetComponent<TextMeshPro>().text = intRecordSumPoint.ToString();
    }


}




