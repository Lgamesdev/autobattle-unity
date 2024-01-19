using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
using LGamesDev;
using LGamesDev.Core;
using LGamesDev.Core.Request;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
#if UNITY_ANDROID
    public async void LinkAccountWithGoogle()
    {
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            //Initialize PlayGamesPlatform
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Debug.Log("Login with Google Play games successful.");

                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, async (string code) =>
                    {
                        Debug.Log("Authorization code: " + code);

                        await LinkWithGooglePlayGamesAsync(code);
                    });
                }
                else
                {
                    StartManager.Instance.modalWindow.ShowAsTextPopup(
                        "Account link failed",
                        "Failed to retrieve Google play games authorization",
                        null,
                        "Ok",
                        null,
                        StartManager.Instance.modalWindow.Close
                    );
                    Debug.Log("Google login Unsuccessful");
                }
            });
            
        }
    }
#endif
    
    private async Task LinkWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            StartManager.Instance.modalWindow.ShowAsTextPopup(
                "Account already linked",
                "This user is already linked with another account. Log in instead.",
                null,
                "Ok",
                null,
                StartManager.Instance.modalWindow.Close
            );
        }

        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    
    public void Logout()
    {
        /*Authentication authentication = StartManager.Instance.GetAuthentication();

        StartCoroutine(AuthenticationHandler.Logout(
            this,
            authentication.refresh_token,
            () => {
                StartManager.Instance.Logout();
            }
        ));*/
    }
}
