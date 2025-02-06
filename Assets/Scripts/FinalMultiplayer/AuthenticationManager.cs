using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;

public class AuthenticationManager : MonoBehaviour
{
    public static async Task Authenticate()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Signed in! PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
    }
}
