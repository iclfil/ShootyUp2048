using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class GameGrid : MonoBehaviour {
     //public GameObject blackBlock;
     public GameObject scriptObject;


     #region PublicFields
     [Header("Prefabs")]
     //public GameObject dropPoints;
     //public GameObject blockPrefab;
     public GameObject gameGrid;
     public GameObject controlPanel;
     public GameEditor gameEditor;

     [Header("Settings")]
     public UIManager ui;
     public GameObject gamePanel;
     public GameObject topBorder;
     //public float speedPlayerBlock;
     //public float speedFoldBackGameGrid;
     //public float speedDropDownGameGrid;
     //public float offsetYFoldBack = 1f;
     //public float targetPositionYGameGrid = -10f;
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

     private bool firstBlockInGame;

     private int[] numbers = { 2, 4, 8, 16, 32, 64, 128 };

     #endregion

     // private void Start() {
     // }
     public void CreateNoGameBlockOnGrid() {
          for (int x = 0; x < WIDTH; x++) {
               for (int y = HEIGHT - 1; y >= HEIGHT / 2 + 2; y--) {
                    InstantiateNoGameBlock(x, y);
               }
          }
     }
     public void StartGame() {
          firstBlockInGame = true;
          InitGameGrid();
     }
 
     private void Update() {
          CheckBorder();
     }
     public void ClearGameGrid() {
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
          stateGame = StateGame.Waiting;
          MoveDownGameGrid();
          blocks = new Block[WIDTH, HEIGHT];
          //CreateNoGameBlockOnGrid();
          Debug.Log("HEIGHT =" + HEIGHT);  // создаем поле из блоков (высота в 2 раза больше)
          //HEIGHT = HEIGHT / 2;//приводим к первоначальному виду
          currentPlayerBlock = CreatePlayerBlock();
          nextPlayerBlock = CreateNextPlayerBlock();
     }

     public Block CreatePlayerBlock() {          // создается первый блок (самый первый)
          Block block = InstantiateBlock(0, 0);

          if(firstBlockInGame){
               block.transform.position = new Vector3(10f, positionPlayerBlock.transform.position.y, 0);
               block.transform.DOLocalMove(new Vector3(positionPlayerBlock.transform.position.x, positionPlayerBlock.transform.position.y, 0),0.5f);
          }
          else{
               block.transform.position = new Vector3(positionPlayerBlock.transform.position.x, positionPlayerBlock.transform.position.y, 0);
          }

          
          // block.transform.position = new Vector3(positionPlayerBlock.transform.position.x, positionPlayerBlock.transform.position.y, 0);
          int randItem = UnityEngine.Random.Range(0, numbers.Length+1);

          if(randItem == numbers.Length+1){

          }
          else block.CurrentNumber = numbers[randItem];
         
          block.transform.SetParent(controlPanel.transform);
          return block;
     }

     public Block CreateNextPlayerBlock() { //???????????????
          Block block = InstantiateBlock(0, 0);
          block.transform.localScale = new Vector3(0, 0, 0);
          int randItem = UnityEngine.Random.Range(0, numbers.Length);
          block.CurrentNumber = numbers[randItem];
          // block.transform.position = new Vector3(positionNextPlayerBlock.transform.position.x, positionNextPlayerBlock.transform.position.y, 0);
           
          if(firstBlockInGame){
               block.transform.position = new Vector3(-10f, positionNextPlayerBlock.transform.position.y, 0);
               block.transform.DOLocalMove(new Vector3(positionNextPlayerBlock.transform.position.x, positionNextPlayerBlock.transform.position.y, 0),0.5f);
               firstBlockInGame = false;
          }

           else{
               block.transform.position = new Vector3(positionNextPlayerBlock.transform.position.x, positionNextPlayerBlock.transform.position.y, 0);
          }
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

          Debug.Log("FIRE");

          stateGame = StateGame.Fly;

          currentPlayerBlock.transform.localPosition = new Vector3(Helper.TransformBlockX(x), currentPlayerBlock.transform.localPosition.y, currentPlayerBlock.transform.localPosition.z); //перемещает кубик в точку выстрела вверх

          Debug.Log("Игрок выстреливает блоком.");

          int y = GetYEmptyCell(x); // получает y пустой ячейку по вертикале по номеру нижней
          currentPlayerBlock.transform.SetParent(gameGrid.transform);
          currentPlayerBlock.transform.position = new Vector3(currentPlayerBlock.transform.position.x, currentPlayerBlock.transform.position.y, 15f);
          if (y != -1) {
               StartCoroutine(UpdateNormalMove(x, y));
          } else {
               StartCoroutine(UpdateLoseMove());
          }
     }

     public IEnumerator FindMatches(Block playerBlock) {
          isMatchesFound = false;
          // Потом смотрим матчи с блоками, которые упали(сюда так же попададает блок игрока).
          foreach (var block in blocks) {
               if (block != null) {
                    if (block.drop == true) {
                         int curNumber = GetSumNumbersBlocks(block.x, block.y);

                         if (curNumber > block.CurrentNumber) {
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

                              if (curNumber > blocks[x, y].CurrentNumber) {
                                   isMatchesFound = true;
                                   blocks[x, y].SetNextCurrentNumber(curNumber);
                              }
                         }
                    }
               }
          }

          yield break;
     }

     public int GetNumberAndUpdateBlocks(int x, int y) {
          int minX = Mathf.Max(0, x - 1);
          int maxX = Mathf.Min(WIDTH, x + 2);

          int minY = Mathf.Max(0, y - 1);
          int maxY = Mathf.Min(HEIGHT, y + 2);

          int curNumber = blocks[x, y].CurrentNumber;
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

                         if (blocks[i, j].CurrentNumber == blocks[x, y].CurrentNumber) {
                           
                              isMatchesFound = true;
                              curNumber += blocks[i, j].CurrentNumber;
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

          int sumNumbers = blocks[x, y].CurrentNumber;

          bool matchFoundUp = false;
          bool matchFoundLeft = false;
          bool matchFoundRight = false;

          int countFoundBlocks = 0;
          Block topBlock = null; // Блок выше нашего блока

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

                         if (blocks[i, j].CurrentNumber == blocks[x, y].CurrentNumber) {

                              if (j == y + 1){
                                   matchFoundUp = true;
                                   topBlock = blocks[i,j];
                              }

                              if(i == x - 1)
                                   matchFoundLeft = true;

                              if(i == x + 1)
                                   matchFoundRight = true;

                              countFoundBlocks++;

                              sumNumbers += blocks[i, j].CurrentNumber;
                              blocks[i, j].SetTargetBlock(blocks[x, y]);
                              // AnimationNumeralDown(i, j);
                         }
                    }
               }
          }

          if (countFoundBlocks == 2)
               sumNumbers += blocks[x, y].CurrentNumber;

          if (countFoundBlocks == 3) {
               sumNumbers += blocks[x, y].CurrentNumber;
               sumNumbers += blocks[x, y].CurrentNumber;
          }

          Debug.LogFormat("Left" + matchFoundLeft + "Right" + matchFoundRight + "Up" + matchFoundUp);

          // Установка таргет блока.
          if(matchFoundLeft == false && matchFoundRight == false && matchFoundUp == true){
               blocks[topBlock.x, topBlock.y].SetTargetBlock(null);
               blocks[x,y].SetTargetBlock(topBlock);
          }



          return sumNumbers;
     }

     public void AnimationNumeralDown(int i, int j) { //анимация выпадания очков из блоков
          GameObject points = Instantiate(gameEditor.pointsPrefab, new Vector3(blocks[i, j].gameObject.transform.position.x, blocks[i, j].gameObject.transform.position.y, 0), Quaternion.identity);
          points.GetComponent<TextMeshPro>().text = "+" + blocks[i, j].CurrentNumber;
          ui.intSumPoint = ui.intSumPoint + Convert.ToInt32(blocks[i, j].CurrentNumber);
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

                    GameObject gameObjectBlock = block.gameObject;
                    AnimationNumeralDown(block.x, block.y);
                    blocks[block.x, block.y] = null;
                    Tween scaleTween = targetBlock.transform.DOScale(1.5f, 0.2f).SetLoops(2, LoopType.Yoyo);
                    targetBlock.drop = true;
                    block.transform.DOLocalMove(targetBlock.transform.localPosition, 15).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() => Destroy(gameObjectBlock));
                    targetBlock.CurrentNumber = targetBlock.GetNextCurrentNumber();
               }
          }

          // Сбрасываем везде таргеты.
          foreach (var block in blocks) {
               if(block != null)
                    block.SetTargetBlock(null);
          }
          yield return new WaitForSeconds(gameEditor.timeWaitCollapseBlocks);

          yield break;
     }

     //public void GetNewColorBlock(Block block) { //получаем ноый цвет блока при схлопывании
     //     int index = Array.IndexOf(scriptObject.GetComponent<ScriptableObjects>().square, block.square);
     //     if (index == scriptObject.GetComponent<ScriptableObjects>().square.Length - 1) {
     //          block.GetComponent<SpriteRenderer>().color = scriptObject.GetComponent<ScriptableObjects>().square[index].color;
     //     } else {
     //          block.GetComponent<SpriteRenderer>().color = scriptObject.GetComponent<ScriptableObjects>().square[index + 1].color;
     //     }
     //}

     public IEnumerator DropBlocks() {
          yield return new WaitForSeconds(gameEditor.timeWaitDropBlocks);

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
                                   blocks[x, i] = null;
                                   break;
                              }
                         }
                    }
               }
          }
     }

     #region PrivateMethods

     private Block InstantiateBlock(int x, int y) {
          Block _block = Instantiate(gameEditor.blockPrefab, transform).GetComponent<Block>();
          _block.transform.localPosition = Helper.TransformBlock(x, y);
          return _block;
     }

     private void InstantiateNoGameBlock(int x, int y) {
          GameObject block = Instantiate(gameEditor.blackBlockPrefab, Helper.TransformBlock(x, y), Quaternion.identity);
          block.transform.SetParent(gameGrid.transform);
          block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, 15f);

     }

     private IEnumerator UpdateNormalMove(int x, int y) {
          // yield return new WaitForSeconds(0.02f);
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
         
          Tween tweens = nextPlayerBlock.transform.DOMove(positionPlayerBlock.transform.position, gameEditor.speedNextBlock).SetSpeedBased().SetEase(Ease.Linear);
          yield return tweens.WaitForCompletion();
          nextPlayerBlock = CreateNextPlayerBlock();

          yield return new WaitForSeconds(gameEditor.timeWaitBeforeFindMatches);
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
          flyableTween = currentPlayerBlock.transform.DOLocalMove(Helper.TransformBlock(x, y), gameEditor.speedPlayerBlock).SetEase(Ease.Linear).SetSpeedBased();
          return flyableTween;
     }


     private void MoveDownGameGrid() {
          gameGrid.transform.DOMove(new Vector3(0, gameEditor.targetPositionYGameGrid, 0), gameEditor.speedDownDropGameGrid).SetId("GameGrid").SetSpeedBased().SetEase(Ease.Linear);
     }

     // Для отбрасывания создаем метод отдельный, который это будет делать.
     private void FoldBackGameGrid() {
          DOTween.Kill(gameGrid.transform);
          gameGrid.transform.DOMove(new Vector3(0, gameGrid.transform.position.y + gameEditor.offsetYFoldBack, 0), gameEditor.speedFoldBackGameGrid).SetId("GameGrid").SetSpeedBased().SetEase(Ease.Linear).OnComplete(MoveDownGameGrid);
     }

     /// <summary>
     /// Проверяет вышло ли поле за границу и если вышло, то не дает дальше его толкать.
     /// </summary>
     private void CheckBorder() {
          if (gameGrid.transform.position.y >= topBorder.transform.position.y) {
               gameGrid.transform.position = new Vector3(gameGrid.transform.position.x, topBorder.transform.position.y, gameGrid.transform.position.z);
          }
     }

     #endregion
}
