using UnityEngine;
using System.Collections;

public class ButtonTutorialBehaviour : MonoBehaviour {
    
    [SerializeField] [Range(0f, 50f)] private float duration = 30;  // How long the tooltips are shown

    [SerializeField] private float xPos = 320;                      // Position on the screen in X direction
    [SerializeField] private float yPos = 80;                       // Position on the screen in Y direction
    [SerializeField] private float width = 220;                     // Width of the textbox
    [SerializeField] private float height = 90;                     // Height of the textbox

    [SerializeField] private string text = "";                      // String of what the textbox should contain

    [SerializeField] private Texture texture;                       // Texture to show in the textbox
    [SerializeField] Texture2D background;                          // Background of the textbox

    private Texture2D RGBBackground;                                // Sliders for the RGB colors of the textbox
    [SerializeField] [Range(0f, 1f)] private float R;               // These are only used if we dont use a texture for the background
    [SerializeField] [Range(0f, 1f)] private float G;
    [SerializeField] [Range(0f, 1f)] private float B;

    private GUIContent content;                                     // Content holder of the textbox

    private GUIStyle style = new GUIStyle();                        // Style holder of the textbox

    private bool show = false;                                      // Is true while the textbox is shown

    void Start()
    {
        // Set the content of the textbox
        content = new GUIContent(text, texture);

        // Make sure the text and texture are placed in the middle of the textbox
        style.alignment = TextAnchor.MiddleCenter;

        // Place the texture above the textbox
        style.imagePosition = ImagePosition.ImageAbove;

        // If we dont have a background texture we create one of the values in the RGB sliders
        if (background == null)
        {
            RGBBackground = new Texture2D(128, 128);

            for (int y = 0; y < texture.height; ++y)
            {
                for (int x = 0; x < texture.width; ++x)
                {
                    Color color = new Color(R, G, B);
                    RGBBackground.SetPixel(x, y, color);
                }
            }
            RGBBackground.Apply();

            // Set the background to the newly created RGB color
            style.normal.background = RGBBackground;
        }
        // If we have a background texture then we use it
        else
            // Set the background to the texture
            style.normal.background = background;
    }

    // If the player collides with the trigger we show the textbox
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            show = true;
        }
    }

    // Create the textbox using the positions, width, height, content and style
    // Destroy the textbox after a set amount of time
    void OnGUI()
    {
        if (show)
        {
            GUI.Box(new Rect(xPos, yPos, width, height), content, style);

            Destroy(gameObject, duration);
        }
    }
}
