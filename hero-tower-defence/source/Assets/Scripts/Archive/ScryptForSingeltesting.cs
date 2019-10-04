using UnityEngine;

public class ScryptForSingeltesting : MonoBehaviour
{
    void Update()
    {
        try
        {
            GameObject.Find("BaseA").tag = "Alpha";
            GameObject.Find("BaseO").tag = "Omega";
            GameObject.Find("Base(Clone)").tag = "Untagged";
            GameObject.Find("Base(Clone)").transform.position = new Vector3(0, -100, 0);

        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Null in Test");
        }
    }
}
