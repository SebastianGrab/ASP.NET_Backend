import { Routes, Route, Outlet, Link,  useNavigate  } from "react-router-dom";


export default function NavigationService() {
    const navigation = useNavigate();

    function toNewProtocol(){
        navigation('newProtocol');


    }


    return (
        <>

        </>
    )

}
