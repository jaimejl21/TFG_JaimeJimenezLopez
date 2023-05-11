using System;
using System.Collections.Generic;
using System.IO;
using PixelCrushers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemoveBackground : MonoBehaviour
{
    public GameObject loadingObject;
    // private Texture2D _texture2D;
    //public Button button;
    // public Button buttonPreviousImage;
    // public Button buttonNextImage;
    // public List<Image> imageList;
    // public Image colorSelectedImage;
    // public int idx;
    //
    // [SerializeField] Color selectedColor;
    // [SerializeField] float threshold;
    // [SerializeField] RectTransform cursor;


    public Image imageToDeleteSelectedColor;
    public Image test;
    public Vector2 hotSpot = Vector2.zero;

    Image _imageToDeleteSelectedColorSprite;
    private RawImage auxRawImage;
    Texture2D _tex;
    Color32 _color32;
    Rect _r;
    Vector2 _localPoint;
    private int _px;
    private int _py;



    void Awake()
    {
        // idx = 0;
        //button.onClick.AddListener(Save);
        // buttonPreviousImage.onClick.AddListener(ChangeToPreviousImage);
        // buttonNextImage.onClick.AddListener(ChangeToNextImage);
        // imageList = new List<Image>();
        // buttonPreviousImage.enabled = false;
        // buttonNextImage.enabled = false;
    }

    // void applyFilterOnImage()
    // {
    //
    //     Image image = GameObject.Find("Image").GetComponent<Image>();
    //
    //     imageList.Add(image);
    //     _texture2D = image.sprite.texture;
    //     Color chromaColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    //
    //     // Itera a través de cada píxel de la textura
    //     Color32[] pixels = _texture2D.GetPixels32();
    //     for (int i = 0; i < pixels.Length; i++)
    //     {
    //         // Calcula la distancia entre el color de cromatismo y el píxel actual
    //         float distance = Mathf.Sqrt(
    //             Mathf.Pow(chromaColor.r - pixels[i].r, 2) +
    //             Mathf.Pow(chromaColor.g - pixels[i].g, 2) +
    //             Mathf.Pow(chromaColor.b - pixels[i].b, 2)
    //         );
    //
    //         // Comprueba si la distancia está dentro del umbral de cromatismo
    //         if (distance < threshold)
    //         {
    //             // Establece el píxel en transparente
    //             pixels[i].a = 0;
    //         }
    //     }
    //
    //     // Establece los píxeles modificados en la textura de destino
    //     _texture2D.SetPixels32(pixels);
    //
    //     // Aplica los cambios a la textura
    //     _texture2D.Apply();
    //     image.sprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height),
    //         new Vector2(0.5f, 0.5f));
    // }

    private void Update()
    {
        // if (loadingObject.activeSelf)
        // {
        //     // button.enabled = false;
        // }
        // else
        // {
            _imageToDeleteSelectedColorSprite = imageToDeleteSelectedColor;
            _tex = imageToDeleteSelectedColor.mainTexture as Texture2D;
            _r = imageToDeleteSelectedColor.rectTransform.rect;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToDeleteSelectedColor.rectTransform,
                Input.mousePosition, null, out _localPoint);


            if (_localPoint.x > _r.x && _localPoint.y > _r.y && _localPoint.x < (_r.width + _r.x) &&
                _localPoint.y < (_r.height + _r.y))
            {
                _px = Mathf.Clamp(0, (int)(((_localPoint.x - _r.x) * _tex.width) / _r.width), _tex.width);
                _px = Mathf.Clamp(0, (int)(((_localPoint.y - _r.y) * _tex.height) / _r.height), _tex.height);

                _color32 = _tex.GetPixel(_px, _py);
                test.color = _color32;
                Cursor.SetCursor(null, hotSpot, CursorMode.Auto);
            }
        //}


        // if (imageList.Count > 1 && idx > 0)
        // {
        //     buttonPreviousImage.enabled = true;
        // }
        // else
        // {
        //     buttonPreviousImage.enabled = false;
        // }
        //
        // if (imageList.Count > 1 && idx < imageList.Count - 1)
        // {
        //     buttonNextImage.enabled = true;
        // }
        // else
        // {
        //     buttonNextImage.enabled = false;
        // }
    }

    // void ChangeToPreviousImage()
    // {
    //     // Si hay mas de un elemento y el indice es mayor que cero , siempre podemos reducir de elemento
    //     if (imageList.Count > 1 && idx >= 1)
    //     {
    //         idx--;
    //         GameObject.Find("Image").GetComponent<Image>().sprite = imageList[idx].sprite;
    //         buttonNextImage.enabled = true;
    //     }
    // }
    //
    // void ChangeToNextImage()
    // {
    //     //Si hay mas de un elemento y el indice es mayor o igual que cero entonces podemos ir al siguiente siempre que sea menor que el numero de elementos
    //     if (imageList.Count > 1 && idx >= 0 && idx < imageList.Count - 1)
    //     {
    //         idx++;
    //         GameObject.Find("Image").GetComponent<Image>().sprite = imageList[idx].sprite;
    //     }
    // }
}