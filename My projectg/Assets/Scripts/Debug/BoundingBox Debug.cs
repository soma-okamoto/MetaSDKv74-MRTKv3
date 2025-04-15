using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.Input; // ���K�v�ɉ����Ēǉ�
using UnityEngine.XR.Interaction.Toolkit;

public class BoundingBoxDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject BBox;



    public void DisActiveBBOX()
    {
        // BBox���A�N�e�B�u�ɂ��鏈��
        BBox.GetComponent<BoundsControl>().enabled = false;
        BBox.GetComponent<ObjectManipulator>().enabled = false;
        BBox.GetComponent<BoxCollider>().enabled = false;
        GameObject target = GameObject.Find("Origin/BoundingBox/BoundingBoxWithTraditionalHandles(Clone)");
        target.SetActive(false);

        Debug.Log("BBox false");
    }
    public void ActiveBBOX()
    {
        // BBox���A�N�e�B�u�ɂ��鏈��
        BBox.GetComponent<BoundsControl>().enabled = true;
        BBox.GetComponent<ObjectManipulator>().enabled = true;
        BBox.GetComponent<BoxCollider>().enabled = true;
        GameObject target = GameObject.Find("Origin/BoundingBox/BoundingBoxWithTraditionalHandles(Clone)");
        target.SetActive(true);
        Debug.Log("BBox true");


    }

}     

        
