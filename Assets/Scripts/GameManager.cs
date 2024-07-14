using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrapTheCat
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public CatController catController;
        [SerializeField] private TMP_Text gameOverText;
        public bool isGameOver { get; private set; }
        void Awake()
        {
            if(Instance == null)
            Instance = this;
            else
                Destroy(Instance);
        }

        public void GameOver(string goText)
        {
            isGameOver = true;
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = goText;
        }

        public void UpdateCatPath()
        {
            catController.MoveCat();
        }

        public void RestartGame()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(currentSceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }


}

