using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour {

     #region PublicFields

     public int x, y;
     public TextMeshPro currentNumberText;
     public bool drop = false;

     [System.Serializable]
     public class DescriptionsBlocks {
          public DescriptionBlock block2;
          public DescriptionBlock block4;
          public DescriptionBlock block8;
          public DescriptionBlock block16;
          public DescriptionBlock block32;
          public DescriptionBlock block64;
          public DescriptionBlock block128;
          public DescriptionBlock block256;
          public DescriptionBlock block512;
          public DescriptionBlock block1024;
          public DescriptionBlock block2048;
     }

     public int CurrentNumber {
          get { return _currentNumber; }
          set {
               _currentNumber = value;
               InitBlock(_currentNumber);
          }
     }

     public DescriptionsBlocks descriptionsBlocks = new DescriptionsBlocks();


     #endregion

     #region PrivateFields
     [SerializeField]
     private int _currentNumber = 0;
     // private int currentNumber = 2;
     // private SpriteRenderer spriteRenderer; // при рестарте выбивало ошибку, типа не инициализоровано
     /// <summary>
     /// Блок, к которому будет лететь текущий блок, при схлопывании.
     /// </summary>
     [SerializeField]
     private Block targetBlock = null;
     /// <summary>
     /// 
     /// </summary>
     private int nextCurrentNumber = 0;

     #endregion

     // private void Awake() {
     //      spriteRenderer = GetComponent<SpriteRenderer>();     // Первоначально было Awake, при рестарте выбивало ошибку, типа не инициализоровано
     // }
     // void Start() {
     //      spriteRenderer = GetComponent<SpriteRenderer>(); // я пробовал так 
     // }

     // void OnEnable(){
     //      spriteRenderer = GetComponent<SpriteRenderer>(); // и так, но не работает
     // }

     public void SetTargetBlock(Block targetBlock) {
          this.targetBlock = targetBlock;
     }

     public Block GetTargetBlock() {
          return targetBlock;
     }


     public void SetNextCurrentNumber(int nextCurrentNumber) {
          this.nextCurrentNumber = nextCurrentNumber;
     }

     public int GetNextCurrentNumber() {
          return nextCurrentNumber;
     }

     private void InitBlock(int number) {
         

          currentNumberText.text = number.ToString();

          switch (number) {

               case 2:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block2.color; // так работает
                    break;

               case 4:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block4.color;
                    // spriteRenderer.color = descriptionsBlocks.block4.color; // было первоначально
                    break;

               case 8:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block8.color;
                    break;
               case 16:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block16.color;
                    break;
               case 32:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block32.color;
                    break;
               case 64:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block64.color;
                    break;
               case 128:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block128.color;
                    break;
               case 256:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block256.color;
                    break;
               case 512:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block512.color;
                    break;
               case 1024:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block1024.color;
                    break;
               case 2048:
                    gameObject.GetComponent<SpriteRenderer>().color = descriptionsBlocks.block2048.color;
                    break;

          }
     }
}
