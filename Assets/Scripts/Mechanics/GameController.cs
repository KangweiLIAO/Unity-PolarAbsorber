using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public static float timer {get; private set; }  = 60;
        public static float powerTimer { get; private set; } = 0;
        [SerializeField] TMPro.TextMeshProUGUI tmpText;
        [SerializeField] TMPro.TextMeshProUGUI pwrText;
        [SerializeField] GameObject gameOverGroup; //drag gameobject into inspector
        [SerializeField] Button restartButton;
        [SerializeField] RectTransform flood;

        void Start()
        {
            flood = GetComponent<RectTransform>();
            restartButton.onClick.AddListener(Restart);
        }
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            CountdownTimer();
        }

        void CountdownTimer()
        {
            timer -= Time.deltaTime; //magic variable!!!!
            tmpText.text = "Timer: " + Mathf.Round(timer).ToString() + "s";
            pwrText.text = "Power Timer: " + Mathf.Round(powerTimer).ToString() + "s";
            if (powerTimer > 0)
            {
                powerTimer -= Time.deltaTime;
            }


            if (timer <= 0)
            {
                gameOverGroup.SetActive(true);
                Time.timeScale = 0; //pauses game
            }

        }

        public static void ReduceTimer(float time)
        {
            timer -= time;
        }

        public static void powerUp(float time)
        {
            powerTimer += time;
        }

        void Restart()
        {

            SceneManager.LoadScene(0);
            timer = 60;
            powerTimer = 0;

        }
    }
}