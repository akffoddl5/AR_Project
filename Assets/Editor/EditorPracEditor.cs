using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EditorPracEditor))]
public class EditorPracEditor : Editor
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("green" + Input.mousePosition);
    }


	private void OnSceneGUI()
	{
        Debug.Log(Event.current.mousePosition);
        // ���콺 Ŭ�� ����
        if (Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log(Event.current.mousePosition);
            // Ray�� ���콺 Ŭ�� ��ġ�� �߻�
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Ray�� �浹�� ������ ������Ʈ ��ġ
                Instantiate(prefab, hit.point, Quaternion.identity);
            }
        }
    }


}
