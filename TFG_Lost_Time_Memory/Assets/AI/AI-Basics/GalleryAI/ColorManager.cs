using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Image imageColor;
    public Slider redSlider;
    private static float _redColor;
    public Slider blueSlider;
    private static float _blueColor;
    public Slider greenSlider;
    private static float _greenColor;
    public Slider toleranceSlider;
    private static float _tolerance;
    // Start is called before the first frame update
    void Start()
    {
        _redColor = redSlider.value;
        _blueColor = blueSlider.value;
        _greenColor = greenSlider.value;
        _tolerance = toleranceSlider.value;
        
        redSlider.onValueChanged.AddListener(SliderRedChangeValue);
        blueSlider.onValueChanged.AddListener(SliderBlueChangeValue);
        greenSlider.onValueChanged.AddListener(SliderGreenChangeValue);
        toleranceSlider.onValueChanged.AddListener(UpdateToleranceValue);
    }

    void SliderRedChangeValue(float newColor)
    {
        _redColor = newColor;
    }
    
    void SliderBlueChangeValue(float newColor)
    {
        _blueColor = newColor;
    }
    
    void SliderGreenChangeValue(float newColor)
    {
        _greenColor = newColor;
    }
    
    void UpdateToleranceValue(float newTolerance)
    {
        _tolerance = newTolerance;
    }

    public static Color getKeyColor()
    {
        return new Color(_redColor, _blueColor, _greenColor);
    }

    public static float getTolerance()
    {
        return _tolerance;
    }

    // Update is called once per frame
    void Update()
    {
        imageColor.color = getKeyColor();
    }
}
