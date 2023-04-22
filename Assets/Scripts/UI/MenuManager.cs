using System.Collections;
using System.Collections.Generic;
using Networking;
using Networking.MessageHandlers;
using Shared.DataClasses;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance;

    [Header("Menu GameObjects")] 
    public GameObject loadingMenu;
    public GameObject homeMenu;
    public GameObject registerMenu;

    private bool hideMenuSig = false;


    public Menu _menu;
    public enum Menu {
        Loading = 1,
        Home,
        Register
    }
    
    public void ChangeMenu(int menu) {
        _menu = (Menu) menu;
    }
    
    private void Awake() {
        instance = this;
        _menu = Menu.Loading;
        LoginSuccessfulHandler.Happened += HideOnLogin;
    }

    private void HideOnLogin(object sender, UserData e) {
        hideMenuSig = true;
    }

    private void Update() {
        if (hideMenuSig) {
            gameObject.SetActive(false);
            hideMenuSig = false;
        }
        switch (_menu) {
            case Menu.Loading:
                DisableMenus();
                loadingMenu.SetActive(true);
                break;
            case Menu.Home:
                DisableMenus();
                homeMenu.SetActive(true);
                break;
            case Menu.Register:
                DisableMenus();
                registerMenu.SetActive(true);
                break;
        }
    }

    public void SendRegisterTest() {
        ClientSendData.instance.SendAccount("test", "email@email.com", "password");
    }

    public void SendLoginTest() {
        ClientSendData.instance.SendLogin("test", "test123");
    }

    private void DisableMenus() {
        loadingMenu.SetActive(false);
        homeMenu.SetActive(false);
        registerMenu.SetActive(false);
    }
}
