using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Red))]
public class RedEditor : Editor
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnSceneGUI()
    {
        Event currentEvent = Event.current;
        prefab = GameObject.Find("Cube");
        Debug.Log("position..");
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
        Debug.Log("button.." + Event.current.keyCode);

        }

        // ���콺 Ŭ�� ����
            if (Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log(Event.current.mousePosition);
            // Ray�� ���콺 Ŭ�� ��ġ�� �߻�
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(((Red)target).name);
                // Ray�� �浹�� ������ ������Ʈ ��ġ
                var a = Instantiate(prefab, hit.point, Quaternion.identity);
                
                //serializedObject.targetObject = (Red)target;
                //SerializedObject.targetObject = (Red)target;
            }

            // �̺�Ʈ�� �Һ��Ͽ� SceneView�� Ŭ�� �̺�Ʈ�� ó������ �ʵ��� �մϴ�.
        }
        currentEvent.Use();
        Selection.activeObject = ((Red)target).transform.gameObject;
    }
}
