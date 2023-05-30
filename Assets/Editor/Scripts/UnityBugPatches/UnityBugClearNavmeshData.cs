using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnityBugClearNavmeshData {
    [MenuItem("Tools/UnityFixes/Force Cleanup NavMesh")]
    public static void ForceCleanupNavMesh() {
        if (Application.isPlaying)
            return;

        NavMesh.RemoveAllNavMeshData();
    }
}
