using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    private bool switchMenusSig = true;


    public Menu _menu;
    public enum Menu {
        Loading = 1,
        Home,
        Register
    }
    
    public void ChangeMenu(int menu) {
        _menu = (Menu) menu;
        switchMenusSig = true;
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

        if (switchMenusSig) {
            switchMenusSig = false;
            switch (_menu) {
                case Menu.Loading:
                    SwitchMenu(loadingMenu);
                    break;
                case Menu.Home:
                    SwitchMenu(homeMenu);
                    break;
                case Menu.Register:
                    SwitchMenu(registerMenu);
                    break;
            }
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

    private void SwitchMenu(GameObject menuToEnable) {
        bool showLoadingMenu = this.loadingMenu == menuToEnable;
        bool showHomeMenu = this.homeMenu == menuToEnable;
        bool showRegisterMenu = this.registerMenu == menuToEnable;
        
        loadingMenu.SetActive(showLoadingMenu);
        homeMenu.SetActive(showHomeMenu);
        registerMenu.SetActive(showRegisterMenu);
    }
}
