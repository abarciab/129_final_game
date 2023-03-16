using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject wipe, mainMenu, instructions, credits, instructionsButton, creditsButton;
    bool canSwitch;

    private void Start() {
        canSwitch = true;
    }

    public void StartGame() {
        if (wipe.activeInHierarchy) return;
        wipe.SetActive(true);
        StartCoroutine(waitThenStart());
    }

    IEnumerator waitThenStart() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    private void Update() {
        if (Input.anyKeyDown && instructions.activeInHierarchy) SwitchToMain(instructionsButton);
        if (Input.anyKeyDown && credits.activeInHierarchy) SwitchToMain(creditsButton);
    }

    public void SwitchToCredits() {
        if (!canSwitch) return;

        mainMenu.SetActive(false);
        credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(DisableSwitching());
    }

    public void SwitchToInstructions() {
        if (!canSwitch) return;

        mainMenu.SetActive(false);
        instructions.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(DisableSwitching());
    }

    void SwitchToMain(GameObject previous) {
        if (!canSwitch) return;

        instructions.SetActive(false);
        credits.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(previous);
        StartCoroutine(DisableSwitching());
    }

    IEnumerator DisableSwitching() {
        canSwitch = false;
        yield return new WaitForSeconds(0.5f);
        canSwitch = true;
    }
}
