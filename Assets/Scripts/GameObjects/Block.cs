using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour {
     public GameObject scriptObject;
     public Squares square;

     #region PublicFields

     public int x, y;
     public TextMeshPro currentNumberText;
     public bool drop = false;
     // public Color color2;
     // public Color color4;
     // public Color color8;
     // public Color color16;
     // public Color color32;
     // public Color color64;
     // public Color color128;
     // public Color color256;
     // public Color color512;
     // public Color color1024;

     // Для моего режима
     //public int[] numbers = { 2,3, 4, 5, 6,7 };
     //private int[] numbersForPlayer = {1, 2, 3, 4, 5, 6, 7};

     // public int[] numbers = {2,4,8,16,32,64,128,256,512};
     // private int[] numbersForPlayer = { 2, 4, 8, 16, 32, 64, 128, 256, 512 };


     #endregion

     #region PrivateFields
     [SerializeField]
     public int currentNumber;
     // private int currentNumber = 2;
     SpriteRenderer spriteRenderer;
     /// <summary>
     /// Блок, к которому будет лететь текущий блок, при схлопывании.
     /// </summary>
     private Block targetBlock = null;
     /// <summary>
     /// 
     /// </summary>
     private int nextCurrentNumber = 0;

     #endregion

     private void Awake() {
          square = scriptObject.GetComponent<ScriptableObjects>().square[Random.Range(0, 9)];
          spriteRenderer = GetComponent<SpriteRenderer>();
          spriteRenderer.color = square.color;
          currentNumberText.text = square.number.ToString();
          currentNumber = square.number;
     }

     // public void SetRandomCurrentNumber() {
     //      int randItem = UnityEngine.Random.Range(0, numbersForPlayer.Length);
     //      currentNumber = numbersForPlayer[randItem];
     //      currentNumberText.text = currentNumber.ToString();
     //      SetColor(currentNumber);
     // }

     // public void SetRandomCurrentNumberGrid() {
     //      int randItem = UnityEngine.Random.Range(0, numbers.Length);
     //      currentNumber = numbers[randItem];
     //      currentNumberText.text = currentNumber.ToString();
     //      SetColor(currentNumber);
     // }

     public void SetCurrentNumber(int currentNumber) {
          this.currentNumber = currentNumber;
          currentNumberText.text = currentNumber.ToString();
          spriteRenderer.color = this.spriteRenderer.color; // Что это за строчка? // я понимаю это для проверки ?
          // SetColor(currentNumber);
     }

     public int GetCurrentNumber() {
          return currentNumber;
     }

     public void SetTargetBlock(Block targetBlock) {
          if (this.targetBlock != null)
               return;

          this.targetBlock = targetBlock;
     }

     public Block GetTargetBlock(){
          return targetBlock;
     }


     public void SetNextCurrentNumber(int nextCurrentNumber) {
          this.nextCurrentNumber = nextCurrentNumber;
     }

     public int GetNextCurrentNumber() {
          return nextCurrentNumber;
     }

     // public void SetColor(int number) {

     //      switch (number) {
     //           case 2:
     //                spriteRenderer.color = color2;
     //                break;
     //           case 4:
     //                spriteRenderer.color = color4;

     //                break;
     //           case 8:
     //                spriteRenderer.color = color8;

     //                break;
     //           case 16:
     //                spriteRenderer.color = color16;

     //                break;
     //           case 32:
     //                spriteRenderer.color = color32;

     //                break;
     //           case 64:
     //                spriteRenderer.color = color64;

     //                break;
     //           case 128:
     //                spriteRenderer.color = color128;

     //                break;
     //           case 256:
     //                spriteRenderer.color = color256;

     //                break;
     //           case 512:
     //                spriteRenderer.color = color512;
     //                break;

     //           case 1024:
     //                spriteRenderer.color = color1024;
     //                break;
     //      }
     // }
}
