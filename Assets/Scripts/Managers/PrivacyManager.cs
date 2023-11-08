using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class PrivacyManager : MonoBehaviour
{
    // Store whether opt in consent is required, and what consent ID to use
    string consentIdentifier;
    bool isOptInConsentRequired;

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            if (consentIdentifiers.Count > 0)
            {
                consentIdentifier = consentIdentifiers[0];
                isOptInConsentRequired = consentIdentifier == "pipl";
            }
        }
        catch (ConsentCheckException)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately
        }
    }

    public void CheckUserConsent()
    {
        try
        {
            if (isOptInConsentRequired)
            {
                // Show a PIPL consent flow
                // ...

                // If consent is provided for both use and export
                AnalyticsService.Instance.ProvideOptInConsent(consentIdentifier, true);

                // If consent is not provided
                AnalyticsService.Instance.ProvideOptInConsent(consentIdentifier, false);
            }
        }
        catch (ConsentCheckException)
        {
            // Handle the exception by checking e.Reason
        }
    }
}