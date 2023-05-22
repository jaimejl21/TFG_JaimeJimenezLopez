using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using PixelCrushers;

namespace OpenAI
{
    public class DallE : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Image resultImage;
        [SerializeField] private Button button;
        [SerializeField] private GameObject loadGameObject;
        
        private DallEServiceImpl _dallEServiceImpl;

        private void Start()
        {
            GameObject newGameObject = new GameObject("NuevoObjeto");
            _dallEServiceImpl = newGameObject.AddComponent<DallEServiceImpl>();
            button.onClick.AddListener(GetAnImageByPrompt);
        }

        /**
         * Llamada asociada al botón de la pantalla que realiza la petición
         */
        private async void GetAnImageByPrompt()
        {
            loadGameObject.SetActive(true);

            var dallEResponse = await _dallEServiceImpl.GetImage(new DallERequestDTO
            {
                Prompt = inputField.text,
                Size = SizeImage.SizeImage256X256
            });

            if (!dallEResponse.Data.Equals(null) && dallEResponse.Data.Count > 0)
            {
                using var request = new UnityWebRequest(dallEResponse.Data[0].Url);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                request.SendWebRequest();

                do
                {
                    await Task.Yield();
                } while (!request.isDone);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(request.downloadHandler.data);
                Save(texture);
                var sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.zero, 1f);
                resultImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("NO IMAGE WAS CREATED FOR THIS PROMPT");
            }

            loadGameObject.SetActive(false);
        }

        /*
         * Class to set the different images size
         */
        private static class SizeImage
        {
            public const string SizeImage256X256 = "256x256";
            public const string SizeImage512X512 = "512x512";
            public const string SizeImage1024X1024 = "1024x1024";
        }

        public void Save(Texture2D texture2D)
        {
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(
                Application.dataPath + "/Images/InGameImages/" + Regex.Replace(inputField.text, @"\s", "") + "_Image.png", bytes);
        }
    }
}