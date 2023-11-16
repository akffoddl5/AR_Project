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
        // 마우스 클릭 감지
        if (Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log(Event.current.mousePosition);
            // Ray를 마우스 클릭 위치로 발사
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Ray가 충돌한 지점에 오브젝트 배치
                Instantiate(prefab, hit.point, Quaternion.identity);
            }
        }
    }


}
