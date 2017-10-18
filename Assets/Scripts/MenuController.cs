using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public CanvasGroup faderCanvasGroup; // fade out and block ray cast
	public float fadeDuration = 1f;

	private SceneController sceneController;
	private bool isFading;

	// Use this for initialization
	void Start () {
		sceneController = FindObjectOfType<SceneController> ();
		StartCoroutine (Fade (1f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadGameplayScene(){
		StartCoroutine (Fade (0f));
		sceneController.FadeAndLoadScene ("GameplayScene");
	}

	private IEnumerator Fade(float finalAlpha){
		isFading = true;
		faderCanvasGroup.blocksRaycasts = false;

		float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

		while (!Mathf.Approximately(faderCanvasGroup.alpha,finalAlpha)) {
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			yield return null; // come back here next frame?
		}


		faderCanvasGroup.blocksRaycasts = true;
		isFading = false;
	}
}
