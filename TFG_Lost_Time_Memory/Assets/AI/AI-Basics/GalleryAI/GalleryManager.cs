using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    public string spriteFolder = "Images/InGameImages";
    public Sprite[] generatedImages;
    private List<Sprite> _modifiedImages;
    public Image imageBeingDisplayed;
    public Sprite defaultGalleryImage;
    public Button rightButton;
    public Button leftButton;
    public Button convertToPngButton;
    public Button removeImageButton;
    public Button goToGenerateImageScene;
    public GameObject imageName;
    private Color _colorHolder;
    private string fullPath;
    private string[] guids;
    private int _index;
    
    private void Start()
    {
        _index = 0;
        
        rightButton.onClick.AddListener(NextImageRight);
        leftButton.onClick.AddListener(NextImageLeft);
        convertToPngButton.onClick.AddListener(RemoveBackground);
        removeImageButton.onClick.AddListener(RemoveImage);
        goToGenerateImageScene.onClick.AddListener(GenerateImageScene);
        _modifiedImages = new List<Sprite>();
        
        AssetDatabase.Refresh();
        OnValidate();
        
        if (generatedImages.Length > 0)
        {
            imageBeingDisplayed.sprite = generatedImages[_index];
            imageName.GetComponent<TextMeshProUGUI>().text = generatedImages[_index].name;
        }
        else
        {
            imageBeingDisplayed.sprite = defaultGalleryImage;
            imageName.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    void GenerateImageScene()
    {
        SceneManager.LoadScene(10);
    }
    
    void NextImageRight()
    {
        if (_index < generatedImages.Length - 1)
        {
            _index++;
            imageBeingDisplayed.sprite = generatedImages[_index];
            imageName.GetComponent<TextMeshProUGUI>().text = generatedImages[_index].name;
        }
    }
    
    void NextImageLeft()
    {
        if (_index > 0)
        {
            _index--;
            imageBeingDisplayed.sprite = generatedImages[_index];
            imageName.GetComponent<TextMeshProUGUI>().text = generatedImages[_index].name;
        }
    }

    void RemoveImage()
    {
        if (generatedImages.Length > 0)
        {
            //Obtenemos el path 
            var path = AssetDatabase.GUIDToAssetPath(guids[_index]);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
            OnValidate();
            if (generatedImages.Length > 0 && _index > 0)
            {
                _index--;
                imageBeingDisplayed.sprite = generatedImages[_index];
            }
            else
            {
                imageBeingDisplayed.sprite = defaultGalleryImage;
            }
        }
    }

    void OnValidate() {
        fullPath = $"{Application.dataPath}/{spriteFolder}";
        if (!Directory.Exists(fullPath)) {            
            return;
        }

        var folder = new string[]{$"Assets/{spriteFolder}"};
        guids = AssetDatabase.FindAssets("t:Sprite", folder);

        var newSprites = new Sprite[guids.Length];

        bool mismatch;
        if (generatedImages == null)
        {
            mismatch = true;
            generatedImages = newSprites;
        }
        else
        {
            mismatch = newSprites.Length != generatedImages.Length;
        }

        for (int i = 0; i < newSprites.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            newSprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            mismatch |= (i < generatedImages.Length && generatedImages[i] != newSprites[i]);
        }

        if (mismatch)
        {
            generatedImages = newSprites;
            Debug.Log($"{name} sprite list updated.");
        }
    }

    void RemoveBackground()
    {
        if (!imageBeingDisplayed.sprite.texture.isReadable ||
            !imageBeingDisplayed.sprite.texture.format.Equals(TextureImporterFormat.RGBA32))
        {
            MakeTextureReadable(imageBeingDisplayed.sprite.texture);
        }

        AssignPixelColor(imageBeingDisplayed.sprite);
        AssetDatabase.Refresh();
    }

    private void AssignPixelColor(Sprite sprite)
    {
        Color[] pixels = sprite.texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (ColorDistance(pixels[i], ColorManager.getKeyColor()) < ColorManager.getTolerance())
            {
                pixels[i] = Color.clear;
            }
        }

        sprite.texture.SetPixels(pixels);
        sprite.texture.Apply();
    }
    
    float ColorDistance(Color c1, Color c2)
    {
        float r = c1.r - c2.r;
        float g = c1.g - c2.g;
        float b = c1.b - c2.b;
        return Mathf.Sqrt(r * r + g * g + b * b);
    }

    void MakeTextureReadable(Texture2D texture2D)
    {
        string path = AssetDatabase.GetAssetPath(texture2D); 
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        textureImporter.isReadable = true;
        
        TextureImporterPlatformSettings settings = textureImporter.GetDefaultPlatformTextureSettings();
        settings.format = TextureImporterFormat.RGBA32; // Cambiar el formato de la textura
        
        textureImporter.SetPlatformTextureSettings(settings);
        textureImporter.SaveAndReimport();
        AssetDatabase.ImportAsset(path); 
    }
}
