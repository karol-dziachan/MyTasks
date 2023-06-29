/* eslint-disable */
import React from "react";
import HomeIcon from "@material-ui/icons/Home";
import ReceiptIcon from "@material-ui/icons/Receipt";
import NotificationsIcon from "@material-ui/icons/Notifications";
import DesktopWindowsIcon from "@material-ui/icons/DesktopWindows";
import SettingsIcon from "@material-ui/icons/Settings";
import consumeIsUserAuthenticated from "./store/LoggedUserProvider";

const Profile =React.lazy(()=>import('./Views/Profile/Profile'));
const Settings=React.lazy(()=> import('./Views/Settings/Settings'));

const Calendar = React.lazy(() => import('./Views/Calendar/Calendar'))

function onClick(e, item) {
  console.log(item);
}


  
const items = () => {
  const isAuth = consumeIsUserAuthenticated();
   return [
    {
      name: "user",
      label: "Użytkownik",
      Icon: SettingsIcon,
      items: [
      isAuth ?  {
            path: "/profile",
            name: "profile",
            label: "Szczegóły profilu",
            Icon: SettingsIcon,
            component: Profile,
            layout: "/admin",
            onClick , 
        } : {},
        isAuth ?
         {
          path: "/",
          name: "logout",
          label: "Wyloguj",
          layout: "/logout",
          Icon: SettingsIcon,
          // component: Profile,
          onClick , 
      }
        :
        {
          path: "/",
          name: "login",
          label: "Zaloguj",
          layout: "/login",
          Icon: SettingsIcon,
          // component: Profile,
          onClick , 
      }
      ]
    },
 isAuth ?   {
      name: "data",
      label: "Kokpit",
      Icon: SettingsIcon,
      items: [
        
        {
            path: "/calendar",
            name: "tasks",
            label: "Kalendarz",
            Icon: SettingsIcon,
            component: Calendar,
            layout: "/actions",
            onClick , 
        } ,
      ]
    } : {},

  ];

};

export default items;