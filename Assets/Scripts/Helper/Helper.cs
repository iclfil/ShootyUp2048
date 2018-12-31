using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {
     public static float POS_SCALE = 1.40f;
     public static float POS_OFFSET_X = 2;
     public static float POS_OFFSET_Y = 4;

     public static Vector3 TransformBlock(int x, int y) {
          return new Vector3(( x - POS_OFFSET_X ) * POS_SCALE, ( y - POS_OFFSET_Y ) * POS_SCALE, 0);
     }

     public static float TransformBlockX(int x) {
          return  (x - POS_OFFSET_X ) * POS_SCALE;
     }



}
