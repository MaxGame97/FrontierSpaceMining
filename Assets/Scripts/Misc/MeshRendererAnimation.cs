using UnityEngine;
using System.Collections;

public class MeshRendererAnimation : MonoBehaviour {

	
    [SerializeField] private int animationRows = 1;     // The amount of rows in the animation
    [SerializeField] private int animationColumns = 1;  // The amount of columns in the animation

    [SerializeField] [Range(0.1f, 2f)] private float maxInterval = 0.5f; // The max interval (in seconds)

    private int maxIndex;           // The max cell index
    private int currentIndex;       // The current cell index
    
    private float currentInterval;  // The current inverval (in seconds)

    private Vector2 scale;

    void Start()
    {
        // Set the max and current index
        maxIndex = animationRows * animationColumns;
        currentIndex = 0;

        // Set the current interval to the max interval
        currentInterval = maxInterval;

        // Calculate the scale of the animation cells
        scale = new Vector2(1f / animationRows, 1f / animationColumns);
    }

    void Update() {
        // Decrease the interval
        currentInterval -= Time.timeScale;

        // If the interval has expired
        if(currentInterval < 0)
        {
            // Increase the current animation index
            currentIndex++;

            // If the animation index has reached the max index, reset it
            if (currentIndex == maxIndex)
                currentIndex = 0;

            // Reset the current interval
            currentInterval = maxInterval;
        }

        // Get the cell index based on the current index
        Vector2 cellIndex = new Vector2(currentIndex % animationRows, currentIndex % animationColumns);

        // Calculate the offset based on the index and scale, invert the y axis
        Vector2 offset = new Vector2(cellIndex.x * scale.x, 1f - scale.y - cellIndex.y * scale.y);

        // Set the line renderer's offset and scale based on the calculated values
        GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", offset);
        GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", scale);
    }
}
