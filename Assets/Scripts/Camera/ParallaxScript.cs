using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {

    private Material material; // Contains the background's material

    [SerializeField] [Range(0.1f, 1f)]private float scrollSpeed = 0.5f; // Defines the parallax effect's speed
    
    [SerializeField] private bool usesCustomScale = true; // Whether or not the background should be stretched or correctly tiled

    [SerializeField] [Range(0.01f, 0.25f)] private float customMaterialScale = 0.1f; // Custom scale for the background

	// Use this for initialization
	void Start () {
        // Get the background's material
        material = GetComponent<MeshRenderer>().material;

        if (usesCustomScale)
        {
            // Get the background's scale factor
            Vector2 backgroundScale = GetComponent<BackgroundBehaviour>().BackgroundScale;

            // Get the material's texture scale
            Vector2 textureScale = material.mainTextureScale;

            // Scale the material so that it is no longer stretched, taking the custom scale into account
            textureScale = new Vector2(textureScale.x * (backgroundScale.x * customMaterialScale), textureScale.y * (backgroundScale.y * customMaterialScale));

            // Update the material's scale
            material.mainTextureScale = textureScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = material.mainTextureOffset;

        // Offset the material based on it's position and the parallax scroll speed
        offset.x = transform.position.x / transform.localScale.x * scrollSpeed;
        offset.y = transform.position.y / transform.localScale.y * scrollSpeed;

        // Update the material's offset
        material.mainTextureOffset = offset;
    }
}
