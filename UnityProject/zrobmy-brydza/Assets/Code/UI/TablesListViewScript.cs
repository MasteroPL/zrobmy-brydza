using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Code.UI;

public class TablesListViewScript : MonoBehaviour
{
    [SerializeField] GameObject ListElementTemplate;
    private List<GameObject> ListItems;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(0); });
        GameObject.Find("CreateNewTableButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Creating new button..."); });

        //FetchAndUpdateTables();
    }

    async void FetchAndUpdateTables()
    {
        ListItems = new List<GameObject>();
        // TODO fetching data

        string[] data = { "Bajlando 1", "Bajlando 2", "Bajlando 3", "Bajlando 4", "Bajlando 5", "Bajlando 6", "Bajlando 7", "Bajlando 8", "Bajlando 9", "Bajlando 10", "Bajlando 11", "Bajlando 12", "Bajlando 13", "Bajlando 14", "Bajlando 15", "Bajlando 16", "Bajlando 17", "Bajlando 18", "Bajlando 19", "Bajlando 20", "Bajlando 21", "Bajlando 22", "Bajlando 23", "Bajlando 24", "Bajlando 25", "Bajlando 26", "Bajlando 27", "Bajlando 28", "Bajlando 29", "Bajlando 30" };
        GameObject ListContent = GameObject.Find("TablesList/Viewport/Content");

        float height = ListElementTemplate.GetComponent<RectTransform>().rect.height;
        ListContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, data.Length * height + 5 * (data.Length - 1));

        for (int i = 0; i < data.Length; i++)
        {
            GameObject ListElement = Instantiate(ListElementTemplate);
            ListElement.SetActive(true);
            ListElement.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(1); });
            ListElement.GetComponent<Button>().GetComponentInChildren<Text>().text = data[i];

            ListElement.transform.SetParent(ListContent.transform, false);
            
            ListElement.transform.localPosition = new Vector3 (
                                                        ListElement.transform.localPosition.x,
                                                        ListElement.transform.localPosition.y - i * (height + 5), 
                                                        ListElement.transform.localPosition.z
                                                  );

            ListItems.Add(ListElement);
        }
    }

    void Destroy()
    {
        foreach(var item in ListItems)
        {
            item.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        string[] ComponentsNamesToReleaseListeners = {
            "BackButton",
            "CreateNewButton"
        };

        foreach(var ComponentName in ComponentsNamesToReleaseListeners)
        {
            GameObject.Find(ComponentName).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
