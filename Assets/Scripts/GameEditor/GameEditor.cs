using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEditor", menuName = "Create GameEditor")]
public class GameEditor : ScriptableObject {

     [Header("Prefabs")]
     public GameObject blockPrefab;
     public GameObject pointsPrefab;
     public GameObject blackBlockPrefab;

     [Header("Level Settings")]
     public float speedPlayerBlock;
     public float speedNextBlock;
     public float speedFoldBackGameGrid;
     public float speedDownDropGameGrid;
     public float offsetYFoldBack = 1f;
     public float targetPositionYGameGrid = -10f;
     public float topBorder = 2.5f;
     public float timeWaitCollapseBlocks = 0.15f;
     public float timeWaitDropBlocks = 0.15f;
     public float timeWaitBeforeFindMatches = 0.2f;
}
