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
     SpriteRenderer spriteRenderer;
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

     private void Awake() {
          spriteRenderer = GetComponent<SpriteRenderer>();
     }

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
                    spriteRenderer.color = descriptionsBlocks.block2.color;
                    break;

               case 4:
                    spriteRenderer.color = descriptionsBlocks.block4.color;
                    break;

               case 8:
                    spriteRenderer.color = descriptionsBlocks.block8.color;
                    break;
               case 16:
                    spriteRenderer.color = descriptionsBlocks.block16.color;
                    break;
               case 32:
                    spriteRenderer.color = descriptionsBlocks.block32.color;
                    break;
               case 64:
                    spriteRenderer.color = descriptionsBlocks.block64.color;
                    break;
               case 128:
                    spriteRenderer.color = descriptionsBlocks.block128.color;
                    break;
               case 256:
                    spriteRenderer.color = descriptionsBlocks.block256.color;
                    break;
               case 512:
                    spriteRenderer.color = descriptionsBlocks.block512.color;
                    break;
               case 1024:
                    spriteRenderer.color = descriptionsBlocks.block1024.color;
                    break;
               case 2048:
                    spriteRenderer.color = descriptionsBlocks.block2048.color;
                    break;

          }
     }
}
