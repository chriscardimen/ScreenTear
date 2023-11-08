using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
      using UnityEditor;
      using UnityEditor.AI;
#endif


public class DesignerAssist : MonoBehaviour
{
    #if UNITY_EDITOR
    [Button]
    public void AutomaticallyBakeNavmesh()
    {
        ElevatorFloor[] elevators = FindObjectsOfType<ElevatorFloor>();
        foreach (ElevatorFloor floor in elevators)
        {
            floor.GetComponent<MeshRenderer>().enabled = true;
        }
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        foreach (ElevatorFloor floor in elevators)
        {
            floor.GetComponent<MeshRenderer>().enabled = false;
        }
    }

        [Button]
        public void AutomaticallyAddPowerButtons()
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UI/PowerButton.prefab", typeof(GameObject));
            RoomState[] objs = FindObjectsOfType<RoomState>();

            
            GameObject cat2 = GameObject.Find("Environment/EnvUI");
            if (cat2 == null)
            {
                cat2 = Instantiate<GameObject>(new GameObject());
                cat2.transform.parent = GameObject.Find("Environment").transform;
            }
            cat2.name = "EnvUI";

            foreach (var room in objs)
            {
                GameObject newButton = (GameObject)Instantiate(prefab, room.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity, cat2.transform);
                newButton.GetComponent<PowerButton>().roomState = room;
            }
        }

        [Button]
        public void RefreshPowerButtons()
        {
            foreach(PowerButton button in FindObjectsOfType<PowerButton>())
            {
                DestroyImmediate(button.gameObject);
            }   
            AutomaticallyAddPowerButtons();
        }
    #endif
}
