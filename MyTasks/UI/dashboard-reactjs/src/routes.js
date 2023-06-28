/* eslint-disable */
import React from "react";
import HomeIcon from "@material-ui/icons/Home";
import ReceiptIcon from "@material-ui/icons/Receipt";
import NotificationsIcon from "@material-ui/icons/Notifications";
import DesktopWindowsIcon from "@material-ui/icons/DesktopWindows";
import SettingsIcon from "@material-ui/icons/Settings";

const Profile =React.lazy(()=>import('./Views/Profile/Profile'));
const Settings=React.lazy(()=> import('./Views/Settings/Settings'));

function onClick(e, item) {
  console.log(item);
}
  
export const items = [
    {
      name: "user",
      label: "Użytkownik",
      Icon: SettingsIcon,
      items: [
        {
            path: "/profile",
            name: "profile",
            label: "Szczegóły profilu",
            Icon: SettingsIcon,
            component: Profile,
            layout: "/admin",
            onClick , 
        },
        {
          path: "/",
          name: "login",
          label: "Zaloguj",
          layout: "/login",
          Icon: SettingsIcon,
          component: Profile,
          onClick , 
      }
      ]
    },
   {
      name: "data",
      label: "Dane",
      Icon: SettingsIcon,
      items: [
        false ? 
        {
            path: "/tasks",
            name: "tasks",
            label: "Zadania",
            Icon: SettingsIcon,
            component: Profile,
            layout: "/actions",
            onClick , 
        } : {},
      ]
    },

  ];