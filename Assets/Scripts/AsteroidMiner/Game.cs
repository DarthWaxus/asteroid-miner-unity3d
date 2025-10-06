using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace AsteroidMiner
{
    public class Game : MonoBehaviour
    {
        public Stage stage;
        bool inputEnabled = false;

        public InputSystem_Actions controls;
        private InputAction attack;
        public TMP_Text gameOverText;
        public Button restartButton;
        public GameObject gameName;
        public GameObject helpText;
        public Image gameOverPanelBg;
        public RectTransform lowerCurtain;
        public RectTransform upperCurtain;
        bool curtainsOpened = false;
        public TMP_Text playerCollectedText;
        public TMP_Text rocketCollectedText;
        public GameObject uiHolder;
        private bool gameOvered = false;

        [Tooltip("Процент закрытия экрана шторками (0.0 до 1.0)")]
        public float curtainCoveragePercent = 0.5f;

        private void Awake()
        {
            controls = new InputSystem_Actions();
            EventManager.MissionCompleted.AddListener(OnMissionCompleted);
            EventManager.PlayerAmountChanged.AddListener(OnPlayerAmountChanged);
            EventManager.RocketAmountChanged.AddListener(OnRocketAmountChanged);
        }

        private void OnPlayerAmountChanged(float cur, float max)
        {
            playerCollectedText.text = $"{cur} / {max}";
        }

        private void OnRocketAmountChanged(float cur, float max)
        {
            rocketCollectedText.text = $"{cur} / {max}";
        }

        private void OnEnable()
        {
            attack = controls.Player.Attack;
            attack.Enable();
            attack.performed += OnAttack;
        }

        private void OnDisable()
        {
            attack.Disable();
        }

        private void Start()
        {
            gameOverPanelBg.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            restartButton.onClick.AddListener(Restart);
            restartButton.gameObject.SetActive(false);
            gameName.SetActive(true);
            helpText.SetActive(true);
            uiHolder.SetActive(false);
            stage.Init();
            FadeIn(1f);
        }

        public void GameOver()
        {
            if(gameOvered) return;
            gameOvered = true;
            inputEnabled = false;
            uiHolder.SetActive(false);
            gameOverPanelBg.gameObject.SetActive(true);

            Vector3 startGameOverTextPos = gameOverText.transform.position;
            gameOverText.transform.position = new Vector3(startGameOverTextPos.x + 300, startGameOverTextPos.y,
                startGameOverTextPos.z);
            gameOverText.gameObject.SetActive(true);
            gameOverText.transform.DOMoveX(startGameOverTextPos.x, 1f).SetEase(Ease.OutBack);

            Vector3 startRestartButtonPos = restartButton.transform.position;
            restartButton.transform.position = new Vector3(startRestartButtonPos.x + 300, startRestartButtonPos.y,
                startRestartButtonPos.z);
            restartButton.gameObject.SetActive(true);
            restartButton.transform.DOMoveX(startRestartButtonPos.x, 1f).SetDelay(1f).SetEase(Ease.OutBack);
        }

        public void OnMissionCompleted()
        {
            gameOverText.text = "Mission completed!";
            stage.player.transform.DOJump(stage.mainBase.rocket.position, 1f, 1, 1f)
                .OnComplete(() =>
                {
                    stage.player.gameObject.SetActive(false);
                    stage.mainBase.StartRocket();
                });
            GameOver();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (inputEnabled == false) return;
            if (!curtainsOpened)
            {
                // upperCurtain.DOAnchorMin(new Vector2(0, 1), 1);
                // lowerCurtain.DOAnchorMax(new Vector2(1, 0), 1);
                gameName.transform.DOMoveX(gameName.transform.position.x + 300, 1f)
                    .OnComplete(() => gameName.SetActive(false));
                helpText.transform.DOMoveX(helpText.transform.position.x + 300, 1f)
                    .OnComplete(() => helpText.SetActive(false));
                curtainsOpened = true;
                uiHolder.SetActive(true);
            }

            stage?.player?.Jump();
        }

        public void Restart()
        {
            restartButton.enabled = false;
            FadeOut(1f);
        }

        public void FadeOut(float duration)
        {
            upperCurtain.anchorMin = new Vector2(0, 1);
            lowerCurtain.anchorMax = new Vector2(1, 0);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, upperCurtain.DOAnchorMin(new Vector2(0, 0.5f), duration));
            sequence.Insert(0, lowerCurtain.DOAnchorMax(new Vector2(1, 0.5f), duration));
            sequence.OnComplete(OnFadedOut);
        }

        public void FadeIn(float duration)
        {
            upperCurtain.anchorMin = new Vector2(0, 0.5f);
            lowerCurtain.anchorMax = new Vector2(1, 0.5f);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, upperCurtain.DOAnchorMin(new Vector2(0, 1f), duration));
            sequence.Insert(0, lowerCurtain.DOAnchorMax(new Vector2(1, 0f), duration));
            sequence.OnComplete(() => inputEnabled = true);
        }

        public void OnFadedOut()
        {
            SceneManager.LoadScene(0);
        }
    }
}