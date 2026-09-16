using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private int score = 0;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    [Min(0f)] public float maxSpeed = 5f;
    public GameObject boosterFlame;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    public GameObject explosionEffect;
    private Button restartButton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find the inactive child too, if its Inspector reference was cleared.
        if (boosterFlame == null)
        {
            Transform flameTransform = transform.Find("BoosterFlame");
            if (flameTransform != null)
                boosterFlame = flameTransform.gameObject;
        }

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        if (boosterFlame != null)
            boosterFlame.SetActive(false);
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void MovePlayer()
    {
        bool isThrusting = Mouse.current != null && Mouse.current.leftButton.isPressed;

        // Show the booster only while the left mouse button is held.
        if (boosterFlame != null)
            boosterFlame.SetActive(isThrusting);

        if (isThrusting)
        {
            // Calculate mouse direction.
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse.
            if (direction.sqrMagnitude > 0f)
                transform.up = direction;
            rb.AddForce(direction * thrustForce);
        }

        // Limit speed even when the ship is coasting after releasing the mouse.
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    void OnDisable()
    {
        if (boosterFlame != null)
            boosterFlame.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
    }
    
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
