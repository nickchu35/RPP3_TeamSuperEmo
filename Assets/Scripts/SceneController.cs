using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public event Action BeforeSceneUnload; // no return type
    public event Action AfterSceneLoad;

    public CanvasGroup faderCanvasGroup; // fade out and block ray cast
    public float fadeDuration = 1f;
    public string startingSceneName = "MainMenuScene";

	private bool isFading;

    private IEnumerator Start ()
    {
        faderCanvasGroup.alpha = 1f; // opaque

		yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));

		StartCoroutine (Fade (0f)); // at the end of the function, we don't care if we wait for it(Fade) to finish, so no yield
    }


	public void FadeAndLoadScene (string sceneName){
		if (!isFading) {
			StartCoroutine (FadeAndSwitchScenes(sceneName));
		}
    }

	private IEnumerator FadeAndSwitchScenes(string sceneName){
		yield return StartCoroutine (Fade (1f));

		if (BeforeSceneUnload != null) {
			BeforeSceneUnload (); // tell every subscriber that we are gonna unload the scene
		}

		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);

		yield return StartCoroutine (LoadSceneAndSetActive (sceneName));

		if (AfterSceneLoad != null) {
			AfterSceneLoad ();
		}

		yield return StartCoroutine (Fade (0f));
	}

	private IEnumerator LoadSceneAndSetActive(string sceneName){
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive); // add scene

		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1); // get the most recently loaded scene
		SceneManager.SetActiveScene(newlyLoadedScene);
	}

	private IEnumerator Fade(float finalAlpha){
		isFading = true;
		faderCanvasGroup.blocksRaycasts = true;

		float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

		while (!Mathf.Approximately(faderCanvasGroup.alpha,finalAlpha)) {
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			//small delay
			yield return null; // come back here next frame?
		}


		faderCanvasGroup.blocksRaycasts = false;
		isFading = false;
	}
}
