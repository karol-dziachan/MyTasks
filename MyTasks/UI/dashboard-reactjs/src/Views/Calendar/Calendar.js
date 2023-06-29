import React from "react";
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import "./Calendar.css"
import { useState, useEffect } from "react";
import CalendarComponent from "../../components/Calendar/Calendar";
import List from "../../components/List/List";
import useApiClient from "../../store/apiClient";
import paths from "../../store/paths";

export default function Calendar(){
    const [view, setView] = useState("list")
    const [tasks, setTasks] = useState([]);
    const { response, loading } = useApiClient(paths.tasks.path, paths.tasks.method, null);

    useEffect(() => {
        setTasks(response);
    }, [response]);

    console.log(tasks); 
    return <>
    <div className="calendar">
        <h1>Zadania</h1>
        <div className="calendar__view">
            Wybierz widok: <FormatListBulletedIcon fontSize="large" marginLeft="20px" onClick={() => { setView('list') }} />
        <CalendarMonthIcon fontSize="large" onClick={() => { setView('calendar') }} />
    </div>

    {loading || !tasks || tasks.length === 0 ?
     <div>Ładowanie...</div>
    :  
     view==='list' ? <List tasksProp={tasks}/> : <CalendarComponent tasks={tasks}/>
    }
  

        {(tasks.length === 0) ?
        <div>Brak zadań do wyświetlenia</div> : <></> }

        </div>
    </>
}

