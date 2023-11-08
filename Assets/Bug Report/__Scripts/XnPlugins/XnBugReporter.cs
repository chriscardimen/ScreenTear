using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NaughtyAttributes;

/// <summary>
/// Reports bugs and other comments to a Google Sheet. MUST be attached to a GameObject to work.
/// The Google App Script and directions for the Google Sheet are in a long comment at the end of this script.
/// </summary>
public class XnBugReporter : MonoBehaviour { // SerializedMonoBehaviour {
    static private XnBugReporter S;
    [SerializeField]
    private string sheetsUrl = "You must replace this with the URL for your sheet script";

    //public Dictionary<string, string> formFields;

    private void Awake() {
        S = this;
    }

    IEnumerator POST_CoRo(string uri, WWWForm wForm, System.Action<bool, string> callback = null) {
        UnityWebRequest req = UnityWebRequest.Post(uri, wForm);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success) {
            Debug.Log("Error downloading: " + req.error);
            callback?.Invoke(false, req.error);
        } else {
            // show the info back from Google Sheets
            Debug.Log(req.downloadHandler.text);
            callback?.Invoke(true, req.downloadHandler.text);
        }
    }

    static public void POST(WWWForm wForm, System.Action<bool, string> callback = null) {
        if (S == null) {
            Debug.LogError("Attempt to call POST() without a singleton.");
            return;
        }
        S.StartCoroutine(S.POST_CoRo(S.sheetsUrl, wForm, callback));
    }

}


/* ---------------------- Google App Script for Google Sheets Spreadsheet ----------------------
/// 1. Open the Template here: https://docs.google.com/spreadsheets/d/14yVoua5SRlmlO0NYugA3iqvzNrkMXRQq3C3sZqaROvA/edit?usp=sharing
/// 2. Choose File > Make a Copy from the Google Sheets menu
/// 3. Open that new copy
/// 4. From the Sheets menu, choose Extensions > Apps Script
/// 5. Cut and paste this script into that window
/// 6. Click Save Project (the little diskette icon)
/// 7. Set the function to be run to "setup"
/// 8. Click "Run". This will take a while, and it will ask you to allow the script to access Google Sheets as you.
/// 9. Once it has run, the docID property will be set.
///10. Click "Deploy" and choose "New Deployment"
///11. Click "Select" and choose "Web App"
///12. Set the following:  Description: (Whatever you want), Execute as: "Me", Who has access: "Anyone"
///13. Click "Deploy". This will run for a bit.
///14. Copy the URL under the "Web app" heading by clicking the "Copy" button. This is the URL you need inside Unity.
///15. Paste the copied URL into the sheetsURL field of the XnBugReporter component in Unity.


// original from: http://mashe.hawksey.info/2014/07/google-sheets-as-a-database-insert-with-apps-script-using-postget-methods-with-ajax-example/
// original gist: https://gist.github.com/willpatera/ee41ae374d3c9839c2d6 

// function doGet(e){
//   return handleResponse(e);
// }

//  Enter sheet name where data is to be written below
var SHEET_NAME = "Sheet1";

var SCRIPT_PROP = PropertiesService.getScriptProperties(); // new property service

function handleResponse(e) {
  // shortly after my original solution Google announced the LockService[1]
  // this prevents concurrent access overwritting data
  // [1] http://googleappsdeveloper.blogspot.co.uk/2011/10/concurrency-and-google-apps-script.html
  // we want a public lock, one that locks for all invocations
  var lock = LockService.getPublicLock();
  lock.waitLock(30000);  // wait 30 seconds before conceding defeat.
  
  try {
    // next set where we write the data - you could write to multiple/alternate destinations
    var doc = SpreadsheetApp.openById(SCRIPT_PROP.getProperty("docID"));
    var sheet = doc.getSheetByName(SHEET_NAME);
    
    // we'll assume header is in row 1 but you can override with header_row in GET/POST data
    var headRow = e.parameter.header_row || 1;
    var headers = sheet.getRange(1, 1, 1, sheet.getLastColumn()).getValues()[0];

    // Check headers to make sure that there are no new headers
    var found = false;
    var headerLength = headers.length;
    var mustAppend = false;
    for (var key in e.parameter) {
      found = false;
      for (var i=0; i<headerLength; i++) {
        if (key == headers[i]) {
          found = true;
          break;
        }
      }
      if (!found) {
        headers.push(key);
        mustAppend = true;
      }
    }
    if (mustAppend) {
      sheet.getRange(1, 1, 1, headers.length).setValues([headers]);
    }

    var nextRow = sheet.getLastRow()+1; // get next row
    var row = []; 
    // loop through the header columns
    for (i in headers){
      if (headers[i] == "Timestamp"){ // special case if you include a 'Timestamp' column
        row.push(new Date());
      } else if (headers[i] == "Bug Status") { // special case if you include a 'Bug Status' column
        row.push("New");  
      } else { // else use header name to get data
        row.push(e.parameter[headers[i]]);
      }
    }
    // more efficient to set values as [][] array than individually
    sheet.getRange(nextRow, 1, 1, row.length).setValues([row]);
    // return json success results
    return ContentService
          .createTextOutput(JSON.stringify({"result":"success", "row": nextRow}))
          .setMimeType(ContentService.MimeType.JSON);
  } catch(e){
    // if error return this
    return ContentService
          .createTextOutput(JSON.stringify({"result":"error", "error": e}))
          .setMimeType(ContentService.MimeType.JSON);
  } finally { //release lock
    lock.releaseLock();
  }
}


function doPost(e) {
  return handleResponse(e);
}






function setup() {
    Logger.log("Running setup()");
    var doc = SpreadsheetApp.getActiveSpreadsheet();
    SCRIPT_PROP.setProperty("docID", doc.getId());
    Logger.log("docID set to: %s",SCRIPT_PROP.getProperty("docID"));
}

*/
