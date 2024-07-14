using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cat
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public CatController catController;
        [SerializeField] private TMP_Text gameOverText;
        public bool isGameOver { get; private set; }
        void Awake()
        {
            Instance = this;
        }

        public void GameOver()
        {
            isGameOver = true;
            gameOverText.gameObject.SetActive(true);
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

