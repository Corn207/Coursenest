import { Routes, Route, Outlet } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';

import Login from './components/Login/Login';
import Header from "./components/Header/Header";
import ManageUsers from "./components/ManageUsers/ManageUsers";
import DisplayAdminInfo from "./components/DisplayAdminInfo/DisplayAdminInfo";
import ListCategories from "./components/ListCategories/ListCategories";
import ManageCourses from "./components/ManageCourses/ManageCourses";

const setToken = (adminToken) => {
    localStorage.setItem('accessToken', adminToken);
}

function App() {

    let logged = false;

    localStorage.getItem("accessToken") ? logged=true : logged=false;

    return (
        <div>
            {!logged && <>
                <Routes>
                    <Route index element={<Login setAccessToken={setToken}/>} />
                    <Route path="login" element={<Login setAccessToken={setToken}/>} />
                </Routes>
            </>}
            {logged && <>
                <Header />
                <Outlet />
                <Routes>
                    <Route exact path="/" element={<ManageUsers />} />
                    <Route exact path="/profile" element={<DisplayAdminInfo/>} />
                    <Route exact path="/categories" element={<ListCategories />} />
                    <Route exact path="/courses" element={<ManageCourses />} />
                    <Route path="*" element={<p>Path not resolved</p>} />
                </Routes>
            </>}
        </div>
    );
}
export default App;