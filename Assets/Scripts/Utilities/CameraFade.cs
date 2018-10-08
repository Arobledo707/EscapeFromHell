using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    private GUIStyle m_backgroundStyle = new GUIStyle();        // Style for background tiling
    private Texture2D m_fadeTexture;                // 1x1 pixel texture used for fading
    private Color m_currentScreenOverlayColor = new Color(0, 0, 0, 0);  // default starting color: black and fully transparrent
    private Color m_targetScreenOverlayColor = new Color(0, 0, 0, 0);   // default target color: black and fully transparrent
    private Color m_deltaColor = new Color(0, 0, 0, 0);     // the delta-color is basically the "speed / second" at which the current color should change
    private int m_fadeGUIDepth = -1000;             // make sure this texture is drawn on top of everything

    public static CameraFade instance = null;

    // initialize the texture, background-style and initial color:
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }


        m_fadeTexture = new Texture2D(1, 1);
        m_backgroundStyle.normal.background = m_fadeTexture;
        SetScreenOverlayColor(m_currentScreenOverlayColor);

        // TEMP:
        // usage: use "SetScreenOverlayColor" to set the initial color, then use "StartFade" to set the desired color & fade duration and start the fade
       // SetScreenOverlayColor(new Color(0,0,0,1));
       // StartFade(new Color(0,0,0,0), 3);
    }


    // draw the texture and perform the fade:
    private void OnGUI()
    {
        // if the current color of the screen is not equal to the desired color: keep fading!
        if (m_currentScreenOverlayColor != m_targetScreenOverlayColor)
        {
            // if the difference between the current alpha and the desired alpha is smaller than delta-alpha * deltaTime, then we're pretty much done fading:
            if (Mathf.Abs(m_currentScreenOverlayColor.a - m_targetScreenOverlayColor.a) < Mathf.Abs(m_deltaColor.a) * Time.deltaTime)
            {
                m_currentScreenOverlayColor = m_targetScreenOverlayColor;
                SetScreenOverlayColor(m_currentScreenOverlayColor);
                m_deltaColor = new Color(0, 0, 0, 0);
            }
            else
            {
                // fade!
                SetScreenOverlayColor(m_currentScreenOverlayColor + m_deltaColor * Time.deltaTime);
            }
        }

        // only draw the texture when the alpha value is greater than 0:
        if (m_currentScreenOverlayColor.a > 0)
        {
            GUI.depth = m_fadeGUIDepth;
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), m_fadeTexture, m_backgroundStyle);
        }
    }


    // instantly set the current color of the screen-texture to "newScreenOverlayColor"
    // can be usefull if you want to start a scene fully black and then fade to opague
    public void SetScreenOverlayColor(Color newScreenOverlayColor)
    {
        m_currentScreenOverlayColor = newScreenOverlayColor;
        m_fadeTexture.SetPixel(0, 0, m_currentScreenOverlayColor);
        m_fadeTexture.Apply();
    }


    // initiate a fade from the current screen color (set using "SetScreenOverlayColor") towards "newScreenOverlayColor" taking "fadeDuration" seconds
    public void StartFade(Color newScreenOverlayColor, float fadeDuration)
    {
        if (fadeDuration <= 0.0f)       // can't have a fade last -2455.05 seconds!
        {
            SetScreenOverlayColor(newScreenOverlayColor);
        }
        else                    // initiate the fade: set the target-color and the delta-color
        {
            m_targetScreenOverlayColor = newScreenOverlayColor;
            m_deltaColor = (m_targetScreenOverlayColor - m_currentScreenOverlayColor) / fadeDuration;
        }
    }
}