using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtonsController : MonoBehaviour
{
   [SerializeField] private Button _exitButton;
   [SerializeField] private Button _restartButton;

   private void Awake()
   {
      _exitButton.onClick.AddListener(ExitGame);
      _restartButton.onClick.AddListener(RestartGame);
   }

   private void ExitGame()
   {
      Application.Quit();
   }

   private void RestartGame()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
}
