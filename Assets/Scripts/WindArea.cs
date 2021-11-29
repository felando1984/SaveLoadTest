using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindArea : MonoBehaviour
{
    [Range(1,50)]
    public float WindSpeed;
    [Range(0, 360)]
    public float  WindDirect;
    //public Vector3 direction;
    public static float WindSpeedValue;
    public Material OceanMaterial;
    public Slider SliderWS, SliderWD;
    public TMP_InputField InputWS, InputWD;


    void Start()
    {
        WindSpeed = 5;
        //var tempRotation = Quaternion.identity;
        //var tempVector = Vector3.zero;
        //tempVector = tempRotation.eulerAngles;
        //tempVector.y = Random.Range(0, 359);
        //tempRotation.eulerAngles = tempVector;
        //transform.rotation = tempRotation;
    }
    void Update()
    {
        OceanMaterial.SetFloat("_Displacement", WindSpeed * 1.2f);
        WindSpeedValue = WindSpeed;
        this.transform.eulerAngles = new Vector3(0, WindDirect,0);
        if (Input.GetMouseButton(0) || Input.anyKey)
        {
            if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>())
            {
                Active_InputField();
            }
            else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
            {
                Active_sliders();
            }
        }
    }

    private void Active_InputField()
    {
        WindDirect = float.Parse(InputWD.text);
        SliderWD.value = WindDirect;
        WindSpeed = float.Parse(InputWS.text);
        SliderWS.value = WindSpeed;
    }

    private void Active_sliders()
    {
        WindDirect = SliderWD.value;
        InputWD.text = WindDirect.ToString();
        WindSpeed = SliderWS.value;
        InputWS.text = WindSpeed.ToString();
    }

}

