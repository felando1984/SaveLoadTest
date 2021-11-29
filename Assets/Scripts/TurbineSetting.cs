using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TurbineSetting : MonoBehaviour
{

    //public TextMeshProUGUI PowerText, RPMText;
    //public TMP_InputField InputWS, InputWD, InputBL, InputCp, InputWakeLoss, InputMechLoss, InputElecLoss, InputTimeOut;
    //public Slider SliderWS, SliderWD, SliderBL, SliderCp, SliderWakeLoss, SliderMechLoss, SliderElecLoss, SliderTimeOut;
    ////public bool inWindZone = false;
    //public GameObject wind;
    //Rigidbody rb;
    //private GameObject[] Yaw, Blades, Pitch;
    ////public float YawAngle;
    ////public float PitchAngle;
    //public float BladeLenStatic, WakeLossStatic, MechLossStatic, ElecLossStatic, TimeOutStatic, CpStatic;
    ////public Button yourButton;
    //public Transform GaugePanel;
    //public Toggle toggle;
    //public Material OceanMaterial;
    private GameObject WindTransform;
    private Transform YawObj, HubObj, BladeObj, BodyObj;

    [Range(1.0f, 100.0f)]
    public float BladeLength, HubLength;
    [Range(1.0f, 100.0f)]
    public float WakeLoss, TimeOut, MechLoss, ElecLoss, Cp;

    private float  WindSpeed;
    public float WindOutPut, RPM;

    private void Start()
    {

        YawObj = this.transform.Find("Yaw");
        HubObj = this.transform.Find("Tower");
        BladeObj = this.YawObj.Find("Blades");
        WindTransform = GameObject.FindGameObjectWithTag("WindArea");
        BladeLength = 30; HubLength = 1.0f; Cp = 4;
        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
        //rb = GetComponent<Rigidbody>();
        //WindSpeed = 5;
        //BladeLen = 30;
        //WakeLoss = MechLoss = ElecLoss = TimeOut = 0;
        //Active_InputField();
        //Active_sliders();
    }
    //void TaskOnClick()
    //{
    //    Debug.Log("It workssssssssssssssss");
    //    GaugePanel.gameObject.SetActive(true);
    //}
    void Update()
    {

        //if (Input.GetMouseButton(0) || Input.anyKey)
        //{
        //    if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>())
        //    {
        //        Active_InputField();
        //    }
        //    else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
        //    {
        //        Active_sliders();
        //    }
        //}
        //if (toggle.isOn)
        //{
        //    GaugePanel.gameObject.SetActive(true);
        //}
        //else { GaugePanel.gameObject.SetActive(false); }
        WindPowerCalculation(BladeLength, WindArea.WindSpeedValue);

        this.transform.eulerAngles = new Vector3(0, WindTransform.transform.eulerAngles.y , 0);
        YawObj.localScale = new Vector3((1+BladeLength * 0.02f), (1+BladeLength * 0.02f), (1+BladeLength * 0.01f));
        HubObj.localScale = new Vector3(1, HubLength, 1);
        BladeObj.Rotate(new Vector3(0, 0, (float)(RPM * 6.0f)) * Time.deltaTime);
        //Pitch.transform.eulerAngles = new Vector3(0, PitchAngle, 0);
        //transform.Rotate(Vector3.forward, (RPM * 6f) * Time.deltaTime);
    }

    //private void Active_InputField()
    //{
    //    WindDirect = float.Parse(InputWD.text);
    //    SliderWD.value = WindDirect;
    //    WindSpeed = float.Parse(InputWS.text);
    //    SliderWS.value = WindSpeed;
    //    BladeLen = float.Parse(InputBL.text);
    //    SliderBL.value = BladeLen;
    //    Cp = float.Parse(InputCp.text);
    //    SliderCp.value = Cp;
    //    WakeLoss = float.Parse(InputWakeLoss.text);
    //    SliderWakeLoss.value = WakeLoss;
    //    MechLoss = float.Parse(InputMechLoss.text);
    //    SliderMechLoss.value = MechLoss;
    //    ElecLoss = float.Parse(InputElecLoss.text);
    //    SliderElecLoss.value = ElecLoss;
    //    TimeOut = float.Parse(InputTimeOut.text);
    //    SliderTimeOut.value = TimeOut;
    //}

    //private void Active_sliders()
    //{
    //    WindDirect = SliderWD.value;
    //    InputWD.text = WindDirect.ToString();
    //    WindSpeed = SliderWS.value;
    //    InputWS.text = WindSpeed.ToString();
    //    BladeLen = SliderBL.value;
    //    InputBL.text = BladeLen.ToString();
    //    Cp = SliderCp.value;
    //    InputCp.text = Cp.ToString();
    //    WakeLoss = SliderWakeLoss.value;
    //    InputWakeLoss.text = WakeLoss.ToString();
    //    MechLoss = SliderMechLoss.value;
    //    InputMechLoss.text = MechLoss.ToString();
    //    ElecLoss = SliderElecLoss.value;
    //    InputElecLoss.text = ElecLoss.ToString();
    //    TimeOut = SliderTimeOut.value;
    //    InputTimeOut.text = TimeOut.ToString();
    //}

    private void  WindPowerCalculation(float BladeLength, float WindVelocity)
    {

        var rho = 1.23f;  //Air density (kg / m3)
        // Power Coefficient the real world limit is well below the Betz Limit with values of 0.35 - 0.45 common even in the best designed wind turbines
        var Radius = BladeLength;   //Blade Length(m)
        var Miu = (1 - (WakeLoss * 0.01)) * (1 - (MechLoss * 0.01)) * (1 - (TimeOut * 0.01)) * (1 - (ElecLoss * 0.01)) * (Cp * 0.01); //Real efficiency
        var SweptArea = (Math.PI * Math.Pow(Radius, 2));  //Swept Are (m2)
        var WindPower = (0.5f * rho * SweptArea * Math.Pow(WindVelocity, 3));   // PowerWind = 1/2 * rho * A * V**3 * Cp //(Wind Speed m/s) 
        WindOutPut = (float)(Miu * WindPower);
        var RotorDiameter = BladeLength * 2; //Rotor Diameter
        var TSR = 8;  //Tip Speed Ratio
        RPM = (float)((60 * WindVelocity * TSR) / (Math.PI * RotorDiameter));
        //PowerText.text = ("Wind Power: " + (WindOutPut / 1000000).ToString("0.00") + " MW");
        //RPMText.text = ("Rotational Speed: " + RPM.ToString("0.00") + " rpm");
    }
    //private void fixedupdate()
    //{
    //    if (inWindZone)
    //    {

    //        rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength);
    //    }
    //}

    //void ontriggerenter(Collider coll)
    //{
    //    if (coll.gameObject.tag == "windArea")
    //    {
    //        windZone = coll.gameObject;
    //        inWindZone = true;
    //    }
    //}

    //void ontriggerexit(Collider coll)
    //{
    //    if (coll.gameObject.tag == "windArea")
    //    {
    //        inWindZone = false;
    //    }
    //}
}
