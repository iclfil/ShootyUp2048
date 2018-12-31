using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class GameGrid : MonoBehaviour {
     public GameObject blackBlock;
     public GameObject scriptObject;


     #region PublicFields
     [Header("Prefabs")]
     public GameObject dropPoints;
     public Tween dropDownGameGridTween;
     public GameObject blockPrefab;
     public GameObject gameGrid;
     public GameObject controlPanel;

     [Header("Settings")]
     public UIManager ui;
     public GameObject gamePanel;
     public float speedPlayerBlock;
     public float speedFoldBackGameGrid;
     public float speedDropDownGameGrid;
     public float offsetYFoldBack = 1f;
     public float targetPositionYGameGrid = -10f;
     public float topBorder = 2.5f;
     public GameObject positionPlayerBlock;
     public GameObject positionNextPlayerBlock;
     public int debugNumber = 0;
     public int WIDTH = 5;
     public int HEIGHT = 6;
     public bool isMatchesFound = false;
     public enum StateGame {
          None,
          Waiting,
          FindMatches,
          Fly
     }
     public StateGame stateGame = StateGame.None;
     #endregion


     #region PrivateField
     private Block[,] blocks;
     private GameObject[,] positionsBlocks;
     private Block currentPlayerBlock;
     private Block nextPlayerBlock;

     //private int[] numbers = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

     #endregion

     // private void Start() {
     // }
     public void CreateNoGameBlockOnGrid(){
            for (int x = 0; x < WIDTH; x++) {
               for (int y = HEIGHT - 1; y >= HEIGHT/2+2; y--) {
                    InstantiateNoGameBlock(x,y);
           }
     }
     }
     public void StartGame() {
          InitGameGrid();
     }

     private void Update() {
          CheckBorder();
     }
     public void ClearGameGrid(){
            if (blocks != null) {
               Array.Clear(blocks, 0, blocks.Length);
          }

          if (gameGrid.transform.childCount > 0) {
               foreach (Transform o in gameGrid.transform) {
                    Destroy(o.gameObject);
               }
     }
     }
     
     public void InitGameGrid() {
       

         
          MoveDownGameGrid();
          // У тебя есть метод, который делает то же самое - используй его.
          //dropDownGameGridTween = gameGrid.transform.DOMove(new Vector3(0, -10, 0), 40).SetId("GameGrid");
          //dropDownGameGridTween.Pause();
          stateGame = StateGame.Waiting;
          blocks = new Block[WIDTH, HEIGHT]; 
           CreateNoGameBlockOnGrid();
          Debug.Log("HEIGHT =" +  HEIGHT);  // создаем поле из блоков (высота в 2 раза больше)
          HEIGHT = HEIGHT/2;//приводим к первоначальному виду
          currentPlayerBlock = CreatePlayerBlock();
          nextPlayerBlock = CreateNextPlayerBlock();
     }

     public Block CreatePlayerBlock() {          // создается первый блок (самый первый)
          Block block = InstantiateBlock(0, 0);
          block.transform.position = new Vector3(positionPlayerBlock.transform.position.x, positionPlayerBlock.transform.position.y, 0);
          block.transform.SetParent(controlPanel.transform);
          return block;
     }

     public Block CreateNextPlayerBlock() { //???????????????
          Block block = InstantiateBlock(0, 0);
          block.transform.localScale = new Vector3(0, 0, 0);
          block.transform.position = new Vector3(positionNextPlayerBlock.transform.position.x, positionNextPlayerBlock.transform.position.y, 0);
          block.transform.DOScale(1.2f, 0.25f);
          block.transform.SetParent(controlPanel.transform);
          return block;

     }

     /// <summary>
     /// Выпускает блок игрока.
     /// </summary>
     /// <param name="x">Столбец в котором блок</param>
     public void FirePlayerBlock(int x) { // вызывается при нажатии мышки на блок снизу

          if (stateGame != StateGame.Waiting)
               return;

          stateGame = StateGame.Fly;

          currentPlayerBlock.transform.localPosition = new Vector3(Helper.TransformBlockX(x), currentPlayerBlock.transform.localPosition.y, currentPlayerBlock.transform.localPosition.z); //перемещает кубик в точку выстрела вверх

          Debug.Log("Игрок выстреливает блоком.");

          int y = GetYEmptyCell(x); // получает y пустой ячейку по вертикале по номеру нижней
          currentPlayerBlock.transform.SetParent(gameGrid.transform);
          currentPlayerBlock.transform.position = new Vector3(currentPlayerBlock.transform.position.x,currentPlayerBlock.transform.position.y,15f);
          if (y != -1) {
               StartCoroutine(UpdateNormalMove(x, y));
          } else {
               StartCoroutine(UpdateLoseMove());
          }

          dropDownGameGridTween.Play();
     }

     public IEnumerator FindMatches(Block playerBlock) {
          isMatchesFound = false;
          // Потом смотрим матчи с блоками, которые упали(сюда так же попададает блок игрока).
          foreach (var block in blocks) {
               if (block != null) {
                    if (block.drop == true) {
                         int curNumber = GetSumNumbersBlocks(block.x, block.y);

                         if (curNumber > block.GetCurrentNumber()) {
                              isMatchesFound = true;
                              block.SetNextCurrentNumber(curNumber);
                         }
                    }
               }
          }

          // Потом смотрим матчи с остальнымим блоками, которые остались.
          for (int x = 0; x < WIDTH; x++) {
               for (int y = HEIGHT - 1; y >= 0; y--) {
                    if (blocks[x, y] != null) {
                         if (blocks[x, y].GetTargetBlock() == null) {
                              int curNumber = GetSumNumbersBlocks(x, y);

                              if (curNumber > blocks[x, y].GetCurrentNumber()) {
                                   isMatchesFound = true;
                                   blocks[x, y].SetNextCurrentNumber(curNumber);
                              }
                         }
                    }
               }
          }

          yield break;



          //isMatchesFound = false;
          //// Сначала рассматриваем матчи с блоком, который бросил игрок.
          //if (playerBlock != null)
          //     playerBlock.SetCurrentNumber(GetNumberAndUpdateBlocks(playerBlock.x, playerBlock.y));
          //yield return new WaitForSeconds(0.01f);
          //Debug.Log("Блок долетел, смотри матчи");
          //for (int x = 0; x < WIDTH; x++) {
          //     for (int y = HEIGHT - 1; y >=0; y--) {
          //          if (blocks[x, y] != null) {
          //               int curNumber = GetNumberAndUpdateBlocks(x, y);
          //               blocks[x, y].SetCurrentNumber(curNumber);
          //               yield return new WaitForSeconds(0.01f);
          //          }
          //     }
          //}
          //yield break;
     }
     // Очстить поле. Тестовый режим игры.
     public IEnumerator FindMatchesTwo(Block playerBlock) {
          isMatchesFound = false;

          for (int x = 0; x < WIDTH; x++) {
               for (int y = 0; y < HEIGHT; y++) {
                    if (blocks[x, y] != null) {

                         if (y < HEIGHT - 1) {
                              int curY = y + 1;

                              if (blocks[x, curY] != null) {
                                   if (blocks[x, curY].GetCurrentNumber() > blocks[x, y].GetCurrentNumber()) {
                                        isMatchesFound = true;
                                        int curN = blocks[x, curY].GetCurrentNumber();
                                        blocks[x, curY].SetCurrentNumber(curN - blocks[x, y].GetCurrentNumber());
                                        GameObject block = blocks[x, y].gameObject;
                                        blocks[x, y].transform.DOMove(blocks[x, curY].transform.position, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                                        yield return new WaitForSeconds(0.25f);
                                        continue;
                                   }

                                   if (blocks[x, curY].GetCurrentNumber() == blocks[x, y].GetCurrentNumber()) {
                                        isMatchesFound = true;
                                        int curN = blocks[x, curY].GetCurrentNumber();
                                        blocks[x, curY].SetCurrentNumber(curN + blocks[x, y].GetCurrentNumber());
                                        GameObject block = blocks[x, y].gameObject;
                                        blocks[x, y].transform.DOMove(blocks[x, curY].transform.position, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                                        yield return new WaitForSeconds(0.25f);
                                        continue;
                                   }
                              }
                         }


                         //if (y > 0) {
                         //     int curY = y - 1;
                         //     if (blocks[x, curY] != null) {

                         //          if (blocks[x, curY].Equals(playerBlock)){
                         //               if (blocks[x, curY].GetCurrentNumber() == 1) {
                         //                    GameObject block = blocks[x, curY].gameObject;
                         //                    int curN = 1;
                         //                    isMatchesFound = true;
                         //                    blocks[x, y].SetCurrentNumber(blocks[x, y].GetCurrentNumber() + curN);
                         //                    blocks[x, curY].transform.DOLocalMove(blocks[x, y].transform.localPosition, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                         //                    yield return new WaitForSeconds(0.25f);
                         //                    continue;
                         //               }
                         //          }

                         //          //if (blocks[x, curY].GetCurrentNumber() == 2) {
                         //          //     GameObject block = blocks[x, curY].gameObject;
                         //          //     int curN = 2;
                         //          //     isMatchesFound = true;
                         //          //     blocks[x, y].SetCurrentNumber(blocks[x, y].GetCurrentNumber() + curN);
                         //          //     blocks[x, curY].transform.DOLocalMove(blocks[x, y].transform.localPosition, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                         //          //     yield return new WaitForSeconds(0.15f);
                         //          //     continue;
                         //          //}

                         //          if (blocks[x, curY].GetCurrentNumber() < blocks[x, y].GetCurrentNumber()) {
                         //               GameObject block = blocks[x, curY].gameObject;
                         //               int curN = blocks[x, y].GetCurrentNumber();
                         //               isMatchesFound = true;
                         //               blocks[x, y].SetCurrentNumber(Mathf.Abs(blocks[x, curY].GetCurrentNumber() - curN));
                         //               blocks[x, curY].transform.DOLocalMove(blocks[x, y].transform.localPosition, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                         //               yield return new WaitForSeconds(0.25f);
                         //          }

                         //          if (blocks[x, curY].GetCurrentNumber() == blocks[x, y].GetCurrentNumber()) {
                         //               GameObject block = blocks[x, curY].gameObject;
                         //               int curN = blocks[x, y].GetCurrentNumber();
                         //               isMatchesFound = true;
                         //               blocks[x, y].SetCurrentNumber(blocks[x, curY].GetCurrentNumber() + curN);
                         //               blocks[x, curY].transform.DOLocalMove(blocks[x, y].transform.localPosition, 5).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(block));
                         //               yield return new WaitForSeconds(0.25f);
                         //          }
                         //     }
                         //}
                    }
               }

          }


          // Проверяем на двойки.

          for (int x = 0; x < WIDTH; x++) {
               for (int y = 0; y < HEIGHT; y++) {
                    if (blocks[x, y] != null) {
                         if (blocks[x, y].GetCurrentNumber() == 1) {
                              GameObject b = blocks[x, y].gameObject;
                              blocks[x, y].transform.DOScale(0, 0.25f).OnComplete(() => Destroy(b));
                              isMatchesFound = true;


                         }
                    }
               }
               yield return new WaitForSeconds(0.05f);
          }

          yield break;
     }

     public int GetNumberAndUpdateBlocks(int x, int y) {
          int minX = Mathf.Max(0, x - 1);
          int maxX = Mathf.Min(WIDTH, x + 2);

          int minY = Mathf.Max(0, y - 1);
          int maxY = Mathf.Min(HEIGHT, y + 2);

          //Debug.LogFormat("BLOCK:X{0}Y{1}",x,y);
          // Debug.LogFormat("minX{0} - maxX{1} - minY{2} - maxY{3}",minX,maxX,minY,maxY);

          int curNumber = blocks[x, y].GetCurrentNumber();
          for (int i = minX; i < maxX; i++) {
               for (int j = minY; j < maxY; j++) {
                    if (blocks[i, j] != null) {

                         // Пропускаем блок, с которым сравниваем.
                         if (blocks[i, j].Equals(blocks[x, y]))
                              continue;

                         //Пропускаем углы.
                         if (i == x - 1 && j == y - 1)
                              continue;

                         if (i == x + 1 && j == y + 1)
                              continue;

                         if (i == x - 1 && j == y + 1)
                              continue;

                         if (i == x + 1 && j == y - 1)
                              continue;

                         if (blocks[i, j].GetCurrentNumber() == blocks[x, y].GetCurrentNumber()) {
                              isMatchesFound = true;
                              curNumber += blocks[i, j].GetCurrentNumber();
                              GameObject block = blocks[i, j].gameObject;
                              blocks[i, j].transform.DOLocalMove(blocks[x, y].transform.localPosition, 15).OnComplete(() => Destroy(block)).SetSpeedBased().SetEase(Ease.Linear);
                         }
                    }
               }
          }

          return curNumber;
     }

     /// <summary>
     /// Получает сумму чисел блоков вокруг и указывает к какому блоку двигаться при схлопывании.
     /// </summary>
     /// <param name="x"></param>
     /// <param name="y"></param>


          public int GetSumNumbersBlocks(int x, int y) {

          int minX = Mathf.Max(0, x - 1);
          int maxX = Mathf.Min(WIDTH, x + 2);

          int minY = Mathf.Max(0, y - 1);
          int maxY = Mathf.Min(HEIGHT, y + 2);

          int sumNumbers = blocks[x, y].GetCurrentNumber();

          for (int i = minX; i < maxX; i++) {
               for (int j = minY; j < maxY; j++) {
                    if (blocks[i, j] != null) {

                         // Пропускаем блок, с которым сравниваем.
                         if (blocks[i, j].Equals(blocks[x, y]))
                              continue;

                         //Пропускаем углы.
                         if (i == x - 1 && j == y - 1)
                              continue;

                         if (i == x + 1 && j == y + 1)
                              continue;

                         if (i == x - 1 && j == y + 1)
                              continue;

                         if (i == x + 1 && j == y - 1)
                              continue;

                         if (blocks[i, j].GetCurrentNumber() == blocks[x, y].GetCurrentNumber()) {

                              sumNumbers += blocks[i, j].GetCurrentNumber();
                              blocks[i, j].SetTargetBlock(blocks[x, y]);
                              AnimationNumeralDown(i, j);  
                         }
                    }
               }
          }

          return sumNumbers;
     }

     public void AnimationNumeralDown(int i, int j){ //анимация выпадания очков из блоков
          GameObject points = Instantiate(dropPoints, new Vector3(blocks[i, j].gameObject.transform.position.x, blocks[i, j].gameObject.transform.position.y, 0), Quaternion.identity);
          points.GetComponent<TextMeshPro>().text = "+" + blocks[i,j].GetCurrentNumber();
          ui.intSumPoint = ui.intSumPoint + Convert.ToInt32(blocks[i,j].GetCurrentNumber());
          ui.UpdateSumPoint();
          points.transform.DOLocalMove(new Vector3(points.transform.position.x, points.transform.position.y - 2, 0), 2).OnComplete(() => Destroy(points));
     }


     public IEnumerator CollapseBlocksAndUpdateCurrentNumber() { // этот метод работает неверно, блоки складываюся только вверх 
     ui.UpdateRecordPoint();
          foreach (var block in blocks) {
               if (block != null) {
                    Block targetBlock = block.GetTargetBlock();

                    if (targetBlock == null)
                         continue;

                    // GameObject blockOB = block.gameObject;

                    targetBlock.SetCurrentNumber(targetBlock.GetNextCurrentNumber());
                    // Destroy(blockOB);
                    // blocks[block.x, block.y] = null;
                    blocks[targetBlock.x, targetBlock.y] = null;


               //     targetBlock.transform.DOLocalMove(block.transform.localPosition, 0.25f).OnComplete(() => targetBlock.transform.DOScale(2f, 0.25f).OnRewin());
                    targetBlock.transform.DOLocalMove(block.transform.localPosition, 0.25f).OnComplete(() => targetBlock.transform.DOScale(1.5f, 0.1f).SetLoops(2,LoopType.Yoyo));
                    

                    GetNewColorBlock(targetBlock);
                    blocks[block.x, block.y] = targetBlock;
                    Destroy(block.gameObject);
                    
                    // blocks[block.x, block.y] =  targetBlock;
                    
                   // blocks[block.x, block.y] = null;
                    
                   
                   
                    // .OnComplete(() => Destroy(blockOB));
               }
          }

          yield return new WaitForSeconds(1f);

          yield break;
     }

     public void GetNewColorBlock(Block block){ //получаем ноый цвет блока при схлопывании
     int index = Array.IndexOf(scriptObject.GetComponent<ScriptableObjects>().square, block.square);
     if(index == scriptObject.GetComponent<ScriptableObjects>().square.Length -1){
     block.GetComponent<SpriteRenderer>().color = scriptObject.GetComponent<ScriptableObjects>().square[index].color;
     }
     else {
          block.GetComponent<SpriteRenderer>().color = scriptObject.GetComponent<ScriptableObjects>().square[index+1].color;
          }
     }

     public IEnumerator DropBlocks() {
          yield return new WaitForSeconds(0.13f);
          for (int x = 0; x < WIDTH; x++) {
               for (int y = HEIGHT - 1; y >= 0; y--) {
                    if (blocks[x, y] == null) {
                         for (int i = y - 1; i >= 0; i--) {
                              if (blocks[x, i] != null) {
                                   blocks[x, y] = blocks[x, i];
                                   blocks[x, y].x = x;
                                   blocks[x, y].y = y;
                                   blocks[x, y].drop = true;
                                   Tween tween = blocks[x, y].transform.DOLocalMove(Helper.TransformBlock(x, y), 15).SetSpeedBased().SetEase(Ease.Linear);
                                   // yield return tween.WaitForCompletion();
                                   blocks[x, i] = null;
                                   break;
                              }
                         }
                    }
               }
          }
          yield return new WaitForSeconds(0.13f);
     }

     public void TEST() {
          Debug.Log("HELL:");
     }

     #region PrivateMethods

     private Block InstantiateBlock(int x, int y) {
          Block _block = Instantiate(blockPrefab, transform).GetComponent<Block>();
          _block.transform.localPosition = Helper.TransformBlock(x, y);
          // _block.SetRandomCurrentNumber();
          return _block;
     }

      private void InstantiateNoGameBlock(int x, int y) {
          GameObject block = Instantiate(blackBlock, Helper.TransformBlock(x, y),Quaternion.identity);
          block.transform.SetParent(gameGrid.transform);          
          block.transform.position = new Vector3(block.transform.position.x,block.transform.position.y,15f);
          // _block.transform.localPosition = Helper.TransformBlock(x, y);
          // // _block.SetRandomCurrentNumber();
          // return _block;
     }

     // private Block InstantiateGridBlock(int x, int y) {
     //      Block _block = Instantiate(blockPrefab, transform).GetComponent<Block>();
     //      _block.transform.localPosition = Helper.TransformBlock(x, y);
     //      // _block.SetRandomCurrentNumber();
     //      return _block;
     // }

     private IEnumerator UpdateNormalMove(int x, int y) {
          yield return new WaitForSeconds(0.02f);
          Debug.Log("Отправляет блок в полет");
          yield return GetFlyableTween(x, y).WaitForCompletion();

          // Подкидываем поле.
          FoldBackGameGrid();

          // Обновляем логические значения.
          blocks[x, y] = currentPlayerBlock;
          blocks[x, y].drop = true;
          blocks[x, y].x = x;
          blocks[x, y].y = y;
          currentPlayerBlock = null;
          currentPlayerBlock = nextPlayerBlock;
          Tween tweens = nextPlayerBlock.transform.DOMove(positionPlayerBlock.transform.position, 30).SetSpeedBased().SetEase(Ease.Linear);
          yield return tweens.WaitForCompletion();
          nextPlayerBlock = CreateNextPlayerBlock();

          yield return new WaitForSeconds(0.02f);
          stateGame = StateGame.FindMatches;

          do {
               yield return StartCoroutine(FindMatches(blocks[x, y]));

               foreach (var block in blocks) {
                    if (block != null)
                         block.drop = false;
               }

               yield return StartCoroutine(CollapseBlocksAndUpdateCurrentNumber());
               yield return StartCoroutine(DropBlocks());
          } while (isMatchesFound == true);


          //do {
          //     yield return StartCoroutine(FindMatches(blocks[x, y]));
          //     yield return StartCoroutine(DropBlocks());
          //} while (isMatchesFound == true);


          stateGame = StateGame.Waiting;
     }

     private IEnumerator UpdateLoseMove() {
          // yield return new WaitForSeconds(3f);
          DOTween.Kill(gameGrid.transform);
          ui.EndGame();




          Debug.Log("Игрок проиграл");

          yield break;
     }

     private int GetYEmptyCell(int x) {
          int value = -1;
          for (int y = HEIGHT - 1; y >= 0; y--) {
               if (blocks[x, y] == null)
                    return value = y;
          }
          return -1;
     }

     private Tween GetFlyableTween(int x, int y) {
          Tween flyableTween = null;
          flyableTween = currentPlayerBlock.transform.DOLocalMove(Helper.TransformBlock(x, y), speedPlayerBlock).SetEase(Ease.Linear).SetSpeedBased();
          return flyableTween;
     }


     private void MoveDownGameGrid() {
          gameGrid.transform.DOMove(new Vector3(0, targetPositionYGameGrid, 0), speedDropDownGameGrid).SetId("GameGrid").SetSpeedBased().SetEase(Ease.Linear);
          //dropDownGameGridTween.Play(); это можно убрать, твин и так будет работать.
          Debug.Log("должно работать");
     }

     // Для отбрасывания создаем метод отдельный, который это будет делать.
     private void FoldBackGameGrid() {
          DOTween.Kill(gameGrid.transform);
          gameGrid.transform.DOMove(new Vector3(0, gameGrid.transform.position.y + offsetYFoldBack, 0), speedFoldBackGameGrid).SetId("GameGrid").SetSpeedBased().SetEase(Ease.Linear).OnComplete(MoveDownGameGrid);
     }

     /// <summary>
     /// Проверяет вышло ли поле за границу и если вышло, то не дает дальше его толкать.
     /// </summary>
     private void CheckBorder() {
          if (gameGrid.transform.position.y >= topBorder) {
               gameGrid.transform.position = new Vector3(gameGrid.transform.position.x, topBorder, gameGrid.transform.position.z);
          }
     }

     #endregion
}







// public class Basics : MonoBehaviour
// {
// 	public Transform redCube, greenCube, blueCube, purpleCube;

// 	IEnumerator Start()
// 	{
// 		// Start after one second delay (to ignore Unity hiccups when activating Play mode in Editor)
// 		yield return new WaitForSeconds(1);

// 		// Let's move the red cube TO 0,4,0 in 2 seconds
// 		redCube.DOMove(new Vector3(0,4,0), 2);

// 		// Let's move the green cube FROM 0,4,0 in 2 seconds
// 		greenCube.DOMove(new Vector3(0,4,0), 2).From();

// 		// Let's move the blue cube BY 0,4,0 in 2 seconds
// 		blueCube.DOMove(new Vector3(0,4,0), 2).SetRelative();

// 		// Let's move the purple cube BY 6,0,0 in 2 seconds
// 		// and also change its color to yellow.
// 		// To change its color, we'll have to use its material as a target (instead than its transform).
// 		purpleCube.DOMove(new Vector3(6,0,0), 2).SetRelative();
// 		// Also, let's set the color tween to loop infinitely forward and backwards
// 		purpleCube.GetComponent<Renderer>().material.DOColor(Color.yellow, 2).SetLoops(-1, LoopType.Yoyo);
// 	}
// }

// using UnityEngine;
// using System.Collections;
// using DG.Tweening;

// public class Sequences : MonoBehaviour
// {
// 	public Transform cube;
// 	public float duration = 4;

// 	IEnumerator Start()
// 	{
// 		// Start after one second delay (to ignore Unity hiccups when activating Play mode in Editor)
// 		yield return new WaitForSeconds(1);

// 		// Create a new Sequence.
// 		// We will set it so that the whole duration is 6
// 		Sequence s = DOTween.Sequence();
// 		// Add an horizontal relative move tween that will last the whole Sequence's duration
// 		s.Append(cube.DOMoveX(6, duration).SetRelative().SetEase(Ease.InOutQuad));
// 		// Insert a rotation tween which will last half the duration
// 		// and will loop forward and backward twice
// 		s.Insert(0, cube.DORotate(new Vector3(0, 45, 0), duration / 2).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo));
// 		// Add a color tween that will start at half the duration and last until the end
// 		s.Insert(duration / 2, cube.GetComponent<Renderer>().material.DOColor(Color.yellow, duration / 2));
// 		// Set the whole Sequence to loop infinitely forward and backwards
// 		s.SetLoops(-1, LoopType.Yoyo);
// 	}
// }