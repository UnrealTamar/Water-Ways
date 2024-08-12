using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    // Public fields to be set in the editor
    public Material materialToChange;
    public Color colorToApply = Color.blue;
    public Color defaultColor;

    void OnDisable()
    {
        materialToChange.color = defaultColor;
    }

    // Update is called once per frame
    void Start()
    {

        if (materialToChange != null)
        {
            // Apply the selected color to the material
            materialToChange.color = colorToApply;
        }
    }
}
