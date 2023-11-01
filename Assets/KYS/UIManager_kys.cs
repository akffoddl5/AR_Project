using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager_kys : MonoBehaviour
{
	public GameObject start_ui;

	// UI러프로 움직이기
	IEnumerator CorLerp(GameObject _gameObject, Vector3 start_pos, Vector3 des_pos, bool _visual)
	{
		RectTransform RT = _gameObject.GetComponent<RectTransform>();
		RT.localPosition = start_pos;
		while (Vector3.Distance(RT.localPosition, des_pos) > 0.1f)
		{
			//Debug.Log(Vector3.Distance(RT.localPosition, des_pos));
			RT.localPosition = Vector3.Lerp(RT.localPosition, des_pos, 0.1f);
			//yield return new WaitForSeconds(0.5f);
			yield return null;
		}

		_gameObject.SetActive(_visual);

		yield break;
	}

	public void GameStart()
    {
		//Start UI 집어 넣고
		int screen_height = GameManager_kys.instance.deviceHeight + 1000;
		StartCoroutine(CorLerp(start_ui, Vector3.zero, new Vector3(0, screen_height, 0) , false));//
		
    }

	public void GameReset()
	{
		//Start UI 불러오고
		int screen_height = GameManager_kys.instance.deviceHeight + 1000;
		StartCoroutine(CorLerp(start_ui, new Vector3(0, screen_height, 0), Vector3.zero , true));//
	}


	//게임종료
    public void GameExit()
    {
        Application.Quit();
    }
    
    
}
